var builder = DistributedApplication.CreateBuilder(args);

// Добавляем сервер Postgres
var postgresServer = builder.AddPostgres("postgres")
    .WithImage("ankane/pgvector")
    .WithImageTag("latest")
    .WithLifetime(ContainerLifetime.Persistent)
    .WithPgAdmin(); // Опционально для администрирования

// Создаем базу данных
var identityDb = postgresServer.AddDatabase("identity-db");
var todolistDb = postgresServer.AddDatabase("todo-list-db");

// Связываем с веб-проектом
builder.AddProject<Projects.TodoListAPI_Mvc>("todolistapi-mvc")
    .WithReference(identityDb)
    .WithReference(todolistDb);

builder.Build().Run();
