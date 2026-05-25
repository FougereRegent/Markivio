package worker

import (
	"context"
	"errors"
	"fmt"
	"runtime/debug"
	"sync"

	"github.com/FougereRegent/Markivio/backend/worker/readability-worker/internal/interfaces/logger"
	rmq "github.com/rabbitmq/rabbitmq-amqp-go-client/pkg/rabbitmqamqp"
)

type RMQClient interface {
	NewConnection(ctx context.Context) (RMQConnection, error)
	Close(ctx context.Context) error
}

type RMQConnection interface {
	DeclareQueue(ctx context.Context, name string) error
	NewConsumer(ctx context.Context, queueName string, consumerName string) (RMQConsumer, error)
}

type RMQConsumer interface {
	Receive(ctx context.Context) (RMQDelivery, error)
}

type RMQDelivery interface {
	Data() []byte
	Accept(ctx context.Context) error
	Requeue(ctx context.Context) error
}

type Worker struct {
	poolSize  int
	queueName string
	logger    logger.ILog

	client RMQClient
	conn   RMQConnection
	wg     *sync.WaitGroup
}

type WorkerOpts struct {
	PoolSize    int
	RabbitMqUri string
	QeueuName   string
}

type UnitProcess func(data string, ctx context.Context) error

func NewWorker(logger logger.ILog, opts WorkerOpts) (*Worker, error) {
	env := rmq.NewEnvironment(opts.RabbitMqUri, nil)
	return newWorkerWithClient(logger, opts, &rmqClient{env: env})
}

func newWorkerWithClient(logger logger.ILog, opts WorkerOpts, client RMQClient) (*Worker, error) {
	w := &Worker{
		poolSize:  opts.PoolSize,
		queueName: opts.QeueuName,
		wg:        &sync.WaitGroup{},
		client:    client,
		logger:    logger,
	}
	if err := w.init(); err != nil {
		return nil, fmt.Errorf("worker init: %w", err)
	}
	return w, nil
}

func (w *Worker) init() error {
	var err error
	w.conn, err = w.client.NewConnection(context.Background())
	if err != nil {
		return fmt.Errorf("new connection: %w", err)
	}
	if err := w.conn.DeclareQueue(context.Background(), w.queueName); err != nil {
		return fmt.Errorf("declare queue %s: %w", w.queueName, err)
	}
	return nil
}

func (w *Worker) Run(unitProcess UnitProcess) error {
	ctx := context.Background()
	for id := 0; id < w.poolSize; id++ {
		workerFunc, err := w.createConsumer(unitProcess, fmt.Sprintf("consumer_%d", id), ctx)
		if err != nil {
			return fmt.Errorf("create consumer %d: %w", id, err)
		}
		go w.safeRun(workerFunc, id)
	}
	return nil
}

func (w *Worker) safeRun(fn func(), id int) {
	defer func() {
		if r := recover(); r != nil {
			w.logger.Error("worker goroutine panicked",
				"consumer", id,
				"panic", r,
				"stack", string(debug.Stack()),
			)
		}
	}()
	fn()
}

func (w *Worker) Close() {
	_ = w.client.Close(context.Background())
}

func (w *Worker) createConsumer(unitProcess UnitProcess, consumerName string, ctx context.Context) (func(), error) {
	consumer, err := w.conn.NewConsumer(ctx, w.queueName, consumerName)
	if err != nil {
		w.logger.Error("failed to create consumer",
			"consumerName", consumerName,
			"error", err,
		)
		return nil, fmt.Errorf("new consumer %s: %w", consumerName, err)
	}

	w.wg.Add(1)
	return func() {
		defer w.wg.Done()
		for {
			delivery, err := consumer.Receive(ctx)
			if err != nil {
				if errors.Is(err, context.Canceled) {
					w.logger.Info("consumer stopped (context cancelled)", "consumerName", consumerName)
					return
				}
				w.logger.Error("consumer receive error, skipping",
					"consumerName", consumerName,
					"error", err,
				)
				continue
			}

			data := delivery.Data()
			if data == nil {
				w.logger.Warn("received nil message data, skipping",
					"consumerName", consumerName,
				)
				delivery.Accept(ctx)
				continue
			}

			w.logger.Info("processing message",
				"consumerName", consumerName,
				"dataLength", len(data),
			)

			err = unitProcess(string(data), ctx)

			if err != nil {
				w.logger.Warn("message processing failed, requeueing",
					"consumerName", consumerName,
					"error", err,
				)
				if requeueErr := delivery.Requeue(ctx); requeueErr != nil {
					w.logger.Error("failed to requeue message",
						"consumerName", consumerName,
						"error", requeueErr,
					)
				}
			} else {
				w.logger.Info("message processed successfully, acking",
					"consumerName", consumerName,
				)
				if ackErr := delivery.Accept(ctx); ackErr != nil {
					w.logger.Error("failed to ack message",
						"consumerName", consumerName,
						"error", ackErr,
					)
				}
			}
		}
	}, nil
}

type rmqClient struct {
	env *rmq.Environment
}

func (c *rmqClient) NewConnection(ctx context.Context) (RMQConnection, error) {
	conn, err := c.env.NewConnection(ctx)
	if err != nil {
		return nil, fmt.Errorf("rmq new connection: %w", err)
	}
	return &rmqConnection{conn: conn}, nil
}

func (c *rmqClient) Close(ctx context.Context) error {
	return c.env.CloseConnections(ctx)
}

type rmqConnection struct {
	conn *rmq.AmqpConnection
}

func (c *rmqConnection) DeclareQueue(ctx context.Context, name string) error {
	_, err := c.conn.Management().DeclareQueue(ctx, &rmq.QuorumQueueSpecification{
		Name: name,
	})
	if err != nil {
		return fmt.Errorf("rmq declare queue %s: %w", name, err)
	}
	return nil
}

func (c *rmqConnection) NewConsumer(ctx context.Context, queueName string, consumerName string) (RMQConsumer, error) {
	consumer, err := c.conn.NewConsumer(ctx, queueName, &rmq.ConsumerOptions{
		Id: consumerName,
	})
	if err != nil {
		return nil, fmt.Errorf("rmq new consumer %s on %s: %w", consumerName, queueName, err)
	}
	return &rmqConsumer{consumer: consumer}, nil
}

type rmqConsumer struct {
	consumer *rmq.Consumer
}

func (c *rmqConsumer) Receive(ctx context.Context) (RMQDelivery, error) {
	delivery, err := c.consumer.Receive(ctx)
	if err != nil {
		return nil, fmt.Errorf("rmq receive: %w", err)
	}
	return &rmqDelivery{delivery: delivery}, nil
}

type rmqDelivery struct {
	delivery rmq.IDeliveryContext
}

func (d *rmqDelivery) Data() []byte {
	return d.delivery.Message().GetData()
}

func (d *rmqDelivery) Accept(ctx context.Context) error {
	return d.delivery.Accept(ctx)
}

func (d *rmqDelivery) Requeue(ctx context.Context) error {
	return d.delivery.Requeue(ctx)
}
