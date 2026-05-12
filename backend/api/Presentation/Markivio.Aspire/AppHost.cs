var builder = DistributedApplication.CreateBuilder(args);

var env = builder.Configuration.GetSection("environmentVariables");

var postgres = builder.AddPostgres("postgres")
                        .WithPgAdmin(configureContainer: opts =>
                        {
                            opts.WithEndpoint("http", endpoint => endpoint.Port = 8085);
                        })
                        .WithDataVolume("markivio-db");

var db = postgres.AddDatabase("markivio");

var username = builder.AddParameter("username", secret: true);
var password = builder.AddParameter("password", secret: true);

var rabbitmq = builder.AddRabbitMQ("broker", username, password)
				.WithManagementPlugin();

var graphqlApi = builder.AddProject<Projects.Markivio_GraphQl>("graphql-api")
                    .WithOtlpExporter()
                    .WaitFor(db)
                    .WithReference(db)
					.WithReference(rabbitmq)
                    .WithEnvironment("MARKIVIO_AUTHORITY", env["MARKIVIO_AUTHORITY"])
                    .WithEnvironment("MARKIVIO_AUDIENCE", env["MARKIVIO_AUDIENCE"])
                    .WithEnvironment("MARKIVIO_AUTH_ID", env["MARKIVIO_AUTH_CLIENT_ID"])
                    .WithEnvironment("MARKIVIO_AUTH_DOMAIN", env["MARKIVIO_AUTH_DOMAIN"])
                    .WithEnvironment("MARKIVIO_AUTH_AUDIENCE", env["MARKIVIO_AUTH_AUDIENCE"])
                    .WithUrl("/scalar")
                    .WithUrl("/graphql");

var frontend = builder.AddViteApp("frontend", "../../../../frontend/markivio-frontend")
                    .WithPnpm()
                    .WithRunScript("dev:custom")
                    .WithReference(graphqlApi)
                    .WithEndpoint("http", endpoint => endpoint.Port = 5173)
                    .WaitFor(graphqlApi)
                    .WithEnvironment("VITE_DEV", "true")
                    .WithEnvironment("VITE_MARKIVIO_AUTH_CLIENT_ID", env["MARKIVIO_AUTH_CLIENT_ID"])
                    .WithEnvironment("VITE_MARKIVIO_AUTH_DOMAIN", env["MARKIVIO_AUTH_DOMAIN"])
                    .WithEnvironment("VITE_MARKIVIO_AUTH_AUDIENCE", env["MARKIVIO_AUTH_AUDIENCE"])
                    .WithEnvironment("VITE_MARKIVIO_GRAPHQL_API", "https://localhost:8080/graphql");

var worker = builder.AddGolangApp(name: "worker", 
		workingDirectory: "../../../worker/readability-worker/",
		executable: "./...",
		buildTags: ["dev"])
		.WaitFor(db)
		.WaitFor(rabbitmq)
		.WithEnvironment(context => 
		{
			context.EnvironmentVariables["WORKER_PG_USERNAME"] = postgres.Resource.UserNameParameter!;
			context.EnvironmentVariables["WORKER_PG_PASSWORD"] = postgres.Resource.PasswordParameter!;
			context.EnvironmentVariables["WORKER_PG_HOST"] = postgres.Resource.PrimaryEndpoint.Property(EndpointProperty.Host);
			context.EnvironmentVariables["WORKER_PG_PORT"] = postgres.Resource.PrimaryEndpoint.Property(EndpointProperty.Port);
			context.EnvironmentVariables["WORKER_PG_DB"] = postgres.Resource.Databases!;
			context.EnvironmentVariables["WORKER_MQ_USER"] = rabbitmq.Resource.UserNameParameter!;
			context.EnvironmentVariables["WORKER_MQ_PASSWORD"] = rabbitmq.Resource.PasswordParameter!;
		});

builder.Build().Run();
