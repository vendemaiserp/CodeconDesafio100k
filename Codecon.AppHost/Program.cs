var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.Desafio100KUsers>("desafio100kusers");

builder.Build().Run();
