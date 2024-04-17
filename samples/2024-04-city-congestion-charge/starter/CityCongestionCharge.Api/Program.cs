using CityCongestionCharge.Api;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors();

// Ignore cycles in JSON
builder.Services.Configure<JsonOptions>(options=> options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

// Add Open API (aka Swagger)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "City Congestion Charge API",
        Description = "An API for managing the CCC data",
        Contact = new OpenApiContact
        {
            Name = "HTBLA Leonding",
            Url = new Uri("https://www.htl-leonding.at/")
        }
    });
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

// Add Entity Framework including health checks
var context = builder.Services.AddDbContext<CccDataContext>(
    options => options.UseSqlServer(builder.Configuration["ConnectionStrings:DefaultConnection"]))
    .AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddHealthChecks().AddDbContextCheck<CccDataContext>();
builder.Services.AddScoped<DemoDataGenerator>();
builder.Services.AddScoped<DemoDataWriter>();

// Configure the HTTP request pipeline.
var app = builder.Build();
app.UseHttpsRedirection();
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

// Add CORS to support Single Page Apps (SPAs)
app.UseCors(b => b.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

// Add swagger
if (app.Environment.IsDevelopment())
{ 
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Add health checks
app.UseHealthChecks("/health");

// Add APIs
app.MapCarsApi();
app.MapPaymentsApi();

// Start web server
app.Run();

