FROM mcr.microsoft.com/dotnet/sdk:10.0 AS api-builder

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

FROM node:22.17.0 AS front-builder
WORKDIR /build
ARG SEM_VERSION=v0.0.0
ARG VITE_MARKIVIO_GRAPHQL_API="/graphql"
ARG VITE_MARKIVIO_AUTH_AUDIENCE="https://localhost:8080/"
ARG VITE_MARKIVIO_AUTH_DOMAIN="markivio.eu.auth0.com"
ARG VITE_MARKIVIO_AUTH_CLIENT_ID="TWlbVAgnOznEURGzFbvAru15iGY2CbAW"

#install pnpm
RUN npm install -g pnpm@latest-10

COPY ./frontend/markivio-frontend/. .

RUN pnpm install \
	&& pnpm build

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runner

WORKDIR /app

COPY --from=api-builder /build/publish .
COPY --from=front-builder /build/dist/. ./wwwroot/.

EXPOSE 8080
ENTRYPOINT ["./Markivio.GraphQl"]
