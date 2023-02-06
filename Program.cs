using hc_vo_filtering.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddGraphQLServer()
    .AddIdConverters()
    .AddTypes()
    .AddFiltering();

var app = builder.Build();
app.MapGraphQL();
app.Run();
