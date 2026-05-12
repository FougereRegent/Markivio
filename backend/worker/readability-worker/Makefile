BINARY_NAME=readability-worker
BUILD_DIR=./build

.PHONY: build-container build-local clean run-container run-local

build-container:
	go build -tags container -o $(BUILD_DIR)/$(BINARY_NAME) ./cmd/

build-local:
	go build -tags local -o $(BUILD_DIR)/$(BINARY_NAME) ./cmd/

run-container: build-container
	./$(BUILD_DIR)/$(BINARY_NAME)

run-local: build-local
	./$(BUILD_DIR)/$(BINARY_NAME)

clean:
	rm -rf $(BUILD_DIR)
