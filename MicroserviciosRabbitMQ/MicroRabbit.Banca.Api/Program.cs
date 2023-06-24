using MicroRabbit.Banca.Data.Contexto;
using MicroRabbit.Infra.IoC;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Codigo mio
builder.Services.AddDbContext<BancaDbConext>(opciones => 
{
    opciones.UseSqlServer(builder.Configuration.GetConnectionString("BancaDbConexion"));
});

// Crear referencias desde API hacia Infra.IoC y los demas
ContenedorDependencias.RegistrarServicios(builder.Services, builder.Configuration);

//
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
