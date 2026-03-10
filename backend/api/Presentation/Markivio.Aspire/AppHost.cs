var builder = DistributedApplication.CreateBuilder(args);

var env = builder.Configuration.GetSection("environmentVariables");

var postgres = builder.AddPostgres("postgres")
						.WithPgWeb();

var db = postgres.AddDatabase("markivio");

var graphqlApi = builder.AddProject<Projects.Markivio_GraphQl>("graphql-api")
					.WaitFor(db)
					.WithReference(db)
					.WithEnvironment("MARKIVIO_AUTHORITY", env["MARKIVIO_AUTHORITY"])
					.WithEnvironment("MARKIVIO_AUDIENCE", env["MARKIVIO_AUDIENCE"]);

builder.Build().Run();
