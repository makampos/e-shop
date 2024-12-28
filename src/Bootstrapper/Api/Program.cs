var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services
    .AddCarterWithAssemblies(typeof(CatalogModule).Assembly);

builder.Services
    .AddCatalogModule(builder.Configuration)
    .AddBasketModule(builder.Configuration)
    .AddCatalogModule(builder.Configuration);

// Register custom exception handler into the DI
builder.Services
    .AddExceptionHandler<CustomExceptionHandler>();

var app = builder.Build();

// Configure the HTTP request pipeline
app.MapCarter();

app
    .UseCatalogModule()
    .UseBasketModule()
    .UseOrderingModule();

app.UseExceptionHandler(options => { });

app.Run();