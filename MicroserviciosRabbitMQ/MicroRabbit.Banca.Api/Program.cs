using MicroRabbit.Banca.Data.Contexto;
using MicroRabbit.Infra.IoC;
using Microsoft.EntityFrameworkCore;
using System.Net.NetworkInformation;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Ini Codigo mio
builder.Services.AddDbContext<BancaDbConext>(opciones => 
{
    opciones.UseSqlServer(builder.Configuration.GetConnectionString("BancaDbConexion"));
});

// For the latest MediatR version 12.x February 2023
// Linea para solucionar errores en MediatR a partir de la version 12
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Ping).Assembly));

// Crear referencias desde API hacia Infra.IoC y los demas
ContenedorDependencias.RegistrarServicios(builder.Services, builder.Configuration);

// Fin codigo mio
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
