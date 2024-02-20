var builder = DistributedApplication.CreateBuilder(args);
var redisCache = builder.AddRedisContainer("RedisCach");

var apiservice=builder.AddProject<Projects.Web_Api>("web.api").WithReference(redisCache);

builder.AddProject<Projects.BlazorApp>("blazorapp").WithReference(apiservice);

builder.Build().Run();
