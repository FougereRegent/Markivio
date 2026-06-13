BINARY_NAME=readability-worker
BUILD_DIR=./build

SEM_VERSION="v1.0.0"
.PHONY: build-container build-local clean run-container run-local download-deps

ARGS=-ldflags "-X main.WORKER_VERSION=$(SEM_VERSION)"

build-container:download-deps
	echo "##### Build readability-worker in container mode"
	go build -tags container -o $(BUILD_DIR)/$(BINARY_NAME) $(ARGS) ./cmd/

build-local: download-deps
	echo "##### Build readability-worker in local mode"
	go build -tags local -o $(BUILD_DIR)/$(BINARY_NAME) $(ARGS) ./cmd/

run-container: build-container
	./$(BUILD_DIR)/$(BINARY_NAME)

run-local: build-local
	./$(BUILD_DIR)/$(BINARY_NAME)

test: download-deps
	echo "##### Executing test"
	go test ./... -v

download-deps:
	echo "##### Downloading deps ..."
	go mod download

clean:
	rm -rf $(BUILD_DIR)
