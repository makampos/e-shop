using Carter;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddCarter(configurator: config =>
{
    var catalogModules = typeof(CatalogModule).Assembly.GetTypes()
        .Where(t => t.IsAssignableTo(typeof(ICarterModule))).ToArray();

    config.WithModules(catalogModules);
});

builder.Services
    .AddCatalogModule(builder.Configuration)
    .AddBasketModule(builder.Configuration)
    .AddCatalogModule(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline
app.MapCarter();

app
    .UseCatalogModule()
    .UseBasketModule()
    .UseOrderingModule();

app.Run();