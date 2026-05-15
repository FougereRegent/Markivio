package worker

import (
	"context"
	"errors"
	"sync"
	"testing"
	"time"

	"github.com/FougereRegent/Markivio/backend/worker/readability-worker/internal/interfaces/logger"
)

type mockClient struct {
	conn        *mockConnection
	closeCalled bool
	closeErr    error
}

func (m *mockClient) NewConnection(ctx context.Context) (RMQConnection, error) {
	if m.conn.err != nil {
		return nil, m.conn.err
	}
	return m.conn, nil
}

func (m *mockClient) Close(ctx context.Context) error {
	m.closeCalled = true
	return m.closeErr
}

type mockConnection struct {
	mu                  sync.Mutex
	err                 error
	declareErr          error
	consumer            *mockConsumer
	consumerErr         error
	newConsumerCallCount int
	createdConsumers    []string
}

func (m *mockConnection) DeclareQueue(ctx context.Context, name string) error {
	return m.declareErr
}

func (m *mockConnection) NewConsumer(ctx context.Context, queueName string, consumerName string) (RMQConsumer, error) {
	m.mu.Lock()
	defer m.mu.Unlock()
	m.newConsumerCallCount++
	m.createdConsumers = append(m.createdConsumers, consumerName)
	if m.consumerErr != nil {
		return nil, m.consumerErr
	}
	return m.consumer, nil
}

type mockConsumer struct {
	deliveries chan RMQDelivery
}

func (m *mockConsumer) Receive(ctx context.Context) (RMQDelivery, error) {
	select {
	case d, ok := <-m.deliveries:
		if !ok {
			return nil, context.Canceled
		}
		return d, nil
	case <-ctx.Done():
		return nil, ctx.Err()
	}
}

type mockDelivery struct {
	data       []byte
	accepted   chan struct{}
	requeued   chan struct{}
	acceptErr  error
	requeueErr error
}

func newMockDelivery(data []byte) *mockDelivery {
	return &mockDelivery{
		data:     data,
		accepted: make(chan struct{}),
		requeued: make(chan struct{}),
	}
}

func (m *mockDelivery) Data() []byte {
	return m.data
}

func (m *mockDelivery) Accept(ctx context.Context) error {
	select {
	case <-m.accepted:
	default:
		close(m.accepted)
	}
	return m.acceptErr
}

func (m *mockDelivery) Requeue(ctx context.Context) error {
	select {
	case <-m.requeued:
	default:
		close(m.requeued)
	}
	return m.requeueErr
}

type mockLogger struct{}

func (m *mockLogger) Debug(msg string, args ...any) {}
func (m *mockLogger) Info(msg string, args ...any)  {}
func (m *mockLogger) Warn(msg string, args ...any)  {}
func (m *mockLogger) Error(msg string, args ...any)   {}

func newLogger() logger.ILog {
	return &mockLogger{}
}

func TestNewWorker_Success(t *testing.T) {
	client := &mockClient{
		conn: &mockConnection{
			consumer: &mockConsumer{
				deliveries: make(chan RMQDelivery),
			},
		},
	}

	w, err := newWorkerWithClient(newLogger(), WorkerOpts{
		PoolSize:  3,
		QeueuName: "test-queue",
	}, client)
	if err != nil {
		t.Fatalf("expected no error, got: %v", err)
	}
	if w == nil {
		t.Fatal("expected non-nil worker")
	}
	if w.poolSize != 3 {
		t.Fatalf("expected poolSize 3, got %d", w.poolSize)
	}
	if w.queueName != "test-queue" {
		t.Fatalf("expected queueName 'test-queue', got %s", w.queueName)
	}
}

func TestNewWorker_ConnectionError(t *testing.T) {
	expectedErr := errors.New("connection failed")
	client := &mockClient{
		conn: &mockConnection{
			err: expectedErr,
		},
	}

	_, err := newWorkerWithClient(newLogger(), WorkerOpts{
		PoolSize:  1,
		QeueuName: "test-queue",
	}, client)
	if err == nil {
		t.Fatal("expected error, got nil")
	}
	if !errors.Is(err, expectedErr) {
		t.Fatalf("expected %v, got %v", expectedErr, err)
	}
}

func TestNewWorker_DeclareQueueError(t *testing.T) {
	expectedErr := errors.New("declare queue failed")
	client := &mockClient{
		conn: &mockConnection{
			declareErr: expectedErr,
			consumer: &mockConsumer{
				deliveries: make(chan RMQDelivery),
			},
		},
	}

	_, err := newWorkerWithClient(newLogger(), WorkerOpts{
		PoolSize:  1,
		QeueuName: "test-queue",
	}, client)
	if err == nil {
		t.Fatal("expected error, got nil")
	}
	if !errors.Is(err, expectedErr) {
		t.Fatalf("expected %v, got %v", expectedErr, err)
	}
}

func TestRun_Success(t *testing.T) {
	deliveries := make(chan RMQDelivery)
	defer close(deliveries)

	conn := &mockConnection{
		consumer: &mockConsumer{deliveries: deliveries},
	}
	client := &mockClient{conn: conn}

	w, err := newWorkerWithClient(newLogger(), WorkerOpts{
		PoolSize:  3,
		QeueuName: "test-queue",
	}, client)
	if err != nil {
		t.Fatalf("expected no error, got: %v", err)
	}

	err = w.Run(func(data string, ctx context.Context) error {
		return nil
	})
	if err != nil {
		t.Fatalf("expected no error, got: %v", err)
	}

	if conn.newConsumerCallCount != 3 {
		t.Fatalf("expected 3 consumer creations, got %d", conn.newConsumerCallCount)
	}

	if len(conn.createdConsumers) != 3 {
		t.Fatalf("expected 3 created consumers, got %d", len(conn.createdConsumers))
	}

	expectedNames := []string{"consumer_0", "consumer_1", "consumer_2"}
	for i, name := range conn.createdConsumers {
		if name != expectedNames[i] {
			t.Fatalf("expected consumer name %q, got %q", expectedNames[i], name)
		}
	}
}

func TestRun_ConsumerCreationError(t *testing.T) {
	deliveries := make(chan RMQDelivery)
	defer close(deliveries)

	conn := &mockConnection{
		consumerErr: errors.New("create consumer failed"),
		consumer:    &mockConsumer{deliveries: deliveries},
	}
	client := &mockClient{conn: conn}

	w, err := newWorkerWithClient(newLogger(), WorkerOpts{
		PoolSize:  3,
		QeueuName: "test-queue",
	}, client)
	if err != nil {
		t.Fatalf("expected no error at init, got: %v", err)
	}

	err = w.Run(func(data string, ctx context.Context) error {
		return nil
	})
	if err == nil {
		t.Fatal("expected error from Run, got nil")
	}
}

func TestCreateConsumer_ProcessSuccess(t *testing.T) {
	deliveries := make(chan RMQDelivery, 1)
	consumer := &mockConsumer{deliveries: deliveries}
	conn := &mockConnection{consumer: consumer}
	client := &mockClient{conn: conn}

	w, err := newWorkerWithClient(newLogger(), WorkerOpts{
		PoolSize:  1,
		QeueuName: "test-queue",
	}, client)
	if err != nil {
		t.Fatalf("expected no error, got: %v", err)
	}

	ctx, cancel := context.WithCancel(context.Background())
	defer cancel()

	processed := make(chan struct{})
	fn, err := w.createConsumer(func(data string, ctx context.Context) error {
		close(processed)
		return nil
	}, "test-consumer", ctx)
	if err != nil {
		t.Fatalf("expected no error, got: %v", err)
	}

	delivery := newMockDelivery([]byte("test message"))

	go fn()
	deliveries <- delivery

	select {
	case <-processed:
	case <-time.After(time.Second):
		t.Fatal("timeout waiting for unitProcess to be called")
	}

	select {
	case <-delivery.accepted:
	case <-time.After(time.Second):
		t.Fatal("timeout waiting for delivery to be accepted")
	}

	select {
	case <-delivery.requeued:
		t.Error("delivery should NOT have been requeued")
	default:
	}

	close(deliveries)
}

func TestCreateConsumer_ProcessError(t *testing.T) {
	deliveries := make(chan RMQDelivery, 1)
	consumer := &mockConsumer{deliveries: deliveries}
	conn := &mockConnection{consumer: consumer}
	client := &mockClient{conn: conn}

	w, err := newWorkerWithClient(newLogger(), WorkerOpts{
		PoolSize:  1,
		QeueuName: "test-queue",
	}, client)
	if err != nil {
		t.Fatalf("expected no error, got: %v", err)
	}

	ctx, cancel := context.WithCancel(context.Background())
	defer cancel()

	processed := make(chan struct{})
	fn, err := w.createConsumer(func(data string, ctx context.Context) error {
		close(processed)
		return errors.New("process error")
	}, "test-consumer", ctx)
	if err != nil {
		t.Fatalf("expected no error, got: %v", err)
	}

	delivery := newMockDelivery([]byte("test message"))

	go fn()
	deliveries <- delivery

	select {
	case <-processed:
	case <-time.After(time.Second):
		t.Fatal("timeout waiting for unitProcess to be called")
	}

	select {
	case <-delivery.requeued:
	case <-time.After(time.Second):
		t.Fatal("timeout waiting for delivery to be requeued")
	}

	select {
	case <-delivery.accepted:
		t.Error("delivery should NOT have been accepted")
	default:
	}

	close(deliveries)
}

func TestCreateConsumer_NilData(t *testing.T) {
	deliveries := make(chan RMQDelivery, 1)
	consumer := &mockConsumer{deliveries: deliveries}
	conn := &mockConnection{consumer: consumer}
	client := &mockClient{conn: conn}

	w, err := newWorkerWithClient(newLogger(), WorkerOpts{
		PoolSize:  1,
		QeueuName: "test-queue",
	}, client)
	if err != nil {
		t.Fatalf("expected no error, got: %v", err)
	}

	ctx, cancel := context.WithCancel(context.Background())
	defer cancel()

	fn, err := w.createConsumer(func(data string, ctx context.Context) error {
		return nil
	}, "test-consumer", ctx)
	if err != nil {
		t.Fatalf("expected no error, got: %v", err)
	}

	delivery := newMockDelivery(nil)

	go fn()
	deliveries <- delivery

	time.Sleep(50 * time.Millisecond)

	select {
	case <-delivery.accepted:
		t.Error("delivery should NOT have been accepted for nil data")
	default:
	}

	select {
	case <-delivery.requeued:
		t.Error("delivery should NOT have been requeued for nil data")
	default:
	}

	close(deliveries)
}

func TestCreateConsumer_ReceiveError(t *testing.T) {
	deliveries := make(chan RMQDelivery, 1)
	consumer := &mockConsumer{deliveries: deliveries}
	conn := &mockConnection{consumer: consumer}
	client := &mockClient{conn: conn}

	w, err := newWorkerWithClient(newLogger(), WorkerOpts{
		PoolSize:  1,
		QeueuName: "test-queue",
	}, client)
	if err != nil {
		t.Fatalf("expected no error, got: %v", err)
	}

	ctx, cancel := context.WithCancel(context.Background())
	defer cancel()

	fn, err := w.createConsumer(func(data string, ctx context.Context) error {
		return nil
	}, "test-consumer", ctx)
	if err != nil {
		t.Fatalf("expected no error, got: %v", err)
	}

	delivery := newMockDelivery([]byte("valid data"))

	go fn()

	close(deliveries)

	time.Sleep(50 * time.Millisecond)

	select {
	case <-delivery.accepted:
		t.Error("delivery should NOT have been accepted after Receive error")
	default:
	}

	select {
	case <-delivery.requeued:
		t.Error("delivery should NOT have been requeued after Receive error")
	default:
	}
}

func TestClose(t *testing.T) {
	client := &mockClient{
		conn: &mockConnection{
			consumer: &mockConsumer{
				deliveries: make(chan RMQDelivery),
			},
		},
	}

	w, err := newWorkerWithClient(newLogger(), WorkerOpts{
		PoolSize:  1,
		QeueuName: "test-queue",
	}, client)
	if err != nil {
		t.Fatalf("expected no error, got: %v", err)
	}

	w.Close()

	if !client.closeCalled {
		t.Error("expected client.Close to be called")
	}
}

func TestNewWorker_CustomLogger(t *testing.T) {
	logger := &mockLogger{}
	client := &mockClient{
		conn: &mockConnection{
			consumer: &mockConsumer{
				deliveries: make(chan RMQDelivery),
			},
		},
	}

	w, err := newWorkerWithClient(logger, WorkerOpts{
		PoolSize:  5,
		QeueuName: "custom-queue",
	}, client)
	if err != nil {
		t.Fatalf("expected no error, got: %v", err)
	}
	if w.poolSize != 5 {
		t.Fatalf("expected poolSize 5, got %d", w.poolSize)
	}
	if w.queueName != "custom-queue" {
		t.Fatalf("expected queueName 'custom-queue', got %s", w.queueName)
	}
	if w.logger == nil {
		t.Fatal("expected logger to be set")
	}
}
