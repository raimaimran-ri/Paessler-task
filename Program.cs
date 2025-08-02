using dotenv.net;
using Microsoft.Extensions.FileProviders;
using Microsoft.Identity.Web;
using Paessler.Task.Model;
using Paessler.Task.Services.Repositories;
using Paessler.Task.Services.Repositories.IRepositories;
using Paessler.Task.Services.Mappers;
using Paessler.Task.Services.Validators;
using FluentValidation.AspNetCore;
using FluentValidation;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

DotEnv.Load();
builder.Services.AddCors(options =>
{
    options.AddPolicy("allowAny", policy =>
    {
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

Log.Logger = new LoggerConfiguration()
    .WriteTo.File("Logs/app_log.txt", rollingInterval: RollingInterval.Day)
    .Enrich.FromLogContext()
    .MinimumLevel.Warning()
    .CreateLogger();
    
builder.Host.UseSerilog();
builder.Services.AddControllers();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new()
    {
        Title = "paessler-task API",
        Version = "v1"
    });

});

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddSingleton<PostgresContext>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddDataProtection();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
//builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<OrderDTOValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<CustomerDTOValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<ProductOrderedDTOValidator>();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseHttpsRedirection();
}

app.UseCors("allowAny");
app.MapControllers();
app.Run();