using System.Security.Authentication;
using Microsoft.EntityFrameworkCore;
using PlatformService.AsyncDataServices;
using PlatformService.Data;
using PlatformService.SyncDataServices.Http;

var builder = WebApplication.CreateBuilder(args);

 // Add services to the container.
//  if(builder.Environment.IsProduction())
//  {
//     Console.WriteLine("--> Using SqlServer Db");
//     builder.Services.AddDbContext<AppDbContext>(opt => 
//         opt.UseSqlServer(builder.Configuration.GetConnectionString("PlatformsConn")));
//  }
//  else
//  {
    Console.WriteLine("--> Using InMem Db");
    builder.Services.AddDbContext<AppDbContext>(opt => 
        opt.UseInMemoryDatabase("InMem"));
//  }

builder.Services.AddScoped<IPlatformRepo, PlatformRepo>();

builder.Services.AddHttpClient("myServiceClient")
    .ConfigurePrimaryHttpMessageHandler(
        () => new HttpClientHandler(){
            ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; },
            SslProtocols=  SslProtocols.None
        }
    );

builder.Services.AddHttpClient<ICommandDataClient, HttpCommandDataClient>("myServiceClient");

builder.Services.AddSingleton<IMessageBusClient, MessageBusClient>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddSwaggerGen();

Console.WriteLine($"--> CommandService EndPoint { builder.Configuration.GetValue<string>("CommandService") }");

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

PrepDb.PrepPopulation(app, app.Environment.IsProduction());

app.Run();