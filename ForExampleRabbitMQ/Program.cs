using ForExampleRabbitMQ;
using ForExampleRabbitMQ.Consumer;
using ForExampleRabbitMQ.ForEmailConsumer;
using ForExampleRabbitMQ.ForPaymentConsumer;
using ForExampleRabbitMQ.Helper;
using ForExampleRabbitMQ.RabbitForEmail;
using ForExampleRabbitMQ.RabbitForOrder;
using ForExampleRabbitMQ.RabbitForPayment;
using ForExampleRabbitMQ.RabbitMQSender;
using ForExampleRabbitMQ.Repo;
using Microsoft.AspNetCore.Identity.UI.Services;
//using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddHostedService<RabbitMQConsumer>();
builder.Services.AddHostedService<RabbitMQPaymentConsumer>();
//builder.Services.AddHostedService<RabbitMQEmailConsumer>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddDbContext<DataContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IOrderServ, OrderServ>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<IEmailSender, EmailSender>();

var optionBuilder = new DbContextOptionsBuilder<DataContext>();
optionBuilder.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
builder.Services.AddSingleton<IRabbitMQMessageSender, RabbitMQMessageSender>();
builder.Services.AddSingleton<IRabbitMQOrderMessageSender, RabbitMQOrderMessageSender>();
builder.Services.AddSingleton<IRabbitMQPaymentMessageSender, RabbitMQPaymentMessageSender>();
builder.Services.AddSingleton<IRabbitMQEmailMessageSender, RabbitMQEmailMessageSender>();
builder.Services.AddSingleton(new OrderServ(optionBuilder.Options));
builder.Services.AddSingleton(new PaymentService(optionBuilder.Options));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
