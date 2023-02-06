using hc_vo_filtering.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddGraphQLServer()
    .ModifyRequestOptions(
        options => options.IncludeExceptionDetails = builder.Environment.IsDevelopment())
    .AddTypes()
    .AddFiltering()
    .AddIdConverters();

var app = builder.Build();
app.MapGraphQL();
app.Run();
