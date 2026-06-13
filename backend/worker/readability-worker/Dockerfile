FROM golang:1.26 AS worker-builder

ARG SEM_VERSION

WORKDIR /build

COPY . .

RUN sed -e "s/\(SEM_VERSION\)=\"v[0-9]\.[0-9]\.[0-9]\"/\1=\"${SEM_VERSION}\"/" -i Makefile \
	&& sed -e "s/\(BUILD_DIR\)=\.\/[a-zA-Z]*/\1=\./" -i Makefile \ 
	&& make build-container

FROM alpine AS runtime
WORKDIR /app

RUN apk update && apk add bash && apk add gcompat

COPY --from=worker-builder /build/readability-worker .
SHELL [ "bash" ]

ENTRYPOINT [ "./", "readability-worker" ]
