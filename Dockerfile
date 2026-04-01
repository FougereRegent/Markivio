FROM mcr.microsoft.com/dotnet/sdk:10.0 AS builder

ARG COMPIL_MODE=Release
ARG SEM_VERSION=v0.0.0

WORKDIR /build

COPY ./backend/api/. .

RUN dotnet restore \
  && mkdir publish \
  && ls -al \
  && dotnet publish \
    --no-restore \
    --configuration $COMPIL_MODE \
	-p:InformationaVersion=${SEM_VERSION} \
	-p:Version=$(echo "${SEM_VERSION}" | grep -o "[0-9]\+\.[0-9]\+\.[0-9]\+$") \
    --output ./publish \
    ./Presentation/Markivio.GraphQl/Markivio.GraphQl.csproj

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runner

WORKDIR /app

COPY --from=builder /build/publish ./markivio

EXPOSE 8080
ENTRYPOINT ["./markivio/Markivio.GraphQl"]

