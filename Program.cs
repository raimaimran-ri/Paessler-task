using dotenv.net;
using Microsoft.Extensions.FileProviders;
using Microsoft.Identity.Web;

var builder = WebApplication.CreateBuilder(args);

DotEnv.Load();
builder.Services.AddCors(options =>
{
    options.AddPolicy("allowAny", policy =>
    {
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

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