var builder = DistributedApplication.CreateBuilder(args);
builder.AddProject<Projects.UpSkills_Api>("API");
builder.AddProject<Projects.UpSkills_BlazorClient>("Blazor-Client");
builder.Build().Run();
