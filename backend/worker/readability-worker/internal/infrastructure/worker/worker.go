package worker

import (
	"context"
	"fmt"
	"log"
	"sync"

	rmq "github.com/rabbitmq/rabbitmq-amqp-go-client/pkg/rabbitmqamqp"
)

type Worker struct {
	poolSize int
	uri string
	qeueuName string
	*log.Logger

	env *rmq.Environment
	conn *rmq.AmqpConnection

	wg *sync.WaitGroup
}

type WorkerOpts struct {
	PoolSize int
	RabbitMqUri string
	QeueuName string
}

type UnitProcess func(data string, ctx context.Context) error

func NewWorker(logger *log.Logger, opts WorkerOpts) (*Worker, error) {
	w := &Worker{
		poolSize: opts.PoolSize,
		uri: opts.RabbitMqUri,
		qeueuName: opts.QeueuName,
		wg: &sync.WaitGroup{},
	}
	if err := w.init(); err != nil {
		return nil, err
	}

	return w, nil
}

func (w *Worker) Run(unitProcess UnitProcess) error {
	ctx := context.Background()
	for id := 0; id < w.poolSize; id++ {
		workerFunc, err := w.createConsumer(unitProcess, fmt.Sprintf("consumer_%d", id), ctx)
		if err != nil {
			return err
		}
		go workerFunc()
	}
	return nil
}

func (w *Worker) init() (error) {
	var err error
	w.env = rmq.NewEnvironment(w.uri, nil)
	w.conn, err = w.env.NewConnection(context.Background())
	if err != nil {
		return err
	}

	_, err = w.conn.Management().DeclareQueue(context.Background(), &rmq.QuorumQueueSpecification{
		Name: w.qeueuName,
	})
	if err != nil {
		return err
	}
	return nil
}

func (w *Worker) Close() {
	_ = w.env.CloseConnections(context.Background())
}

func (w *Worker) createConsumer(unitProcess UnitProcess, consumerName string, context context.Context) (func(), error) {
	consumer, err := w.conn.NewConsumer(context, w.qeueuName, &rmq.ConsumerOptions{
		Id: consumerName,
	})
	if err != nil  {
		return nil, err
	}

	w.wg.Add(1)
	return func() {
		defer w.wg.Done()
		for {
			delivery, err := consumer.Receive(context)
			if err != nil {
				continue
			}
			fmt.Printf("Consumer %s received data \n", consumerName)
			message := delivery.Message()
			data := message.GetData()
			if data == nil {
				continue
			}

			err = unitProcess(string(data), context)

			if err != nil {
				delivery.Requeue(context)
			} else {
				delivery.Accept(context)
			}
		}
	}, nil
}
