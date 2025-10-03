var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.TodoListAPI_Mvc>("todolistapi-mvc");

builder.Build().Run();
