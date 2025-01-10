using GanitShop.Infrastructure;
using GanitShop.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//https://stackoverflow.com/questions/43058497/dependency-injection-with-netcore-for-dal-and-connection-string
// https://stackoverflow.com/questions/39083372/how-to-read-connection-string-in-net-core
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
 
builder.Services.AddSingleton(new DbProperties() { ConnString = connectionString });

builder.Services.AddTransient<IProductManager, ProductManager>();

builder.Services.AddTransient<IProductDbManager, ProductDbManager>();

builder.Services.AddTransient<IProductFileUploadManager, ProductFileUploadManager>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();
