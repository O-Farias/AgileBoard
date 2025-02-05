using Microsoft.EntityFrameworkCore;
using AgileBoard.API.Data;
using AgileBoard.API.Services;
using AgileBoard.API.Middleware;
using FluentValidation;
using FluentValidation.AspNetCore;
using AgileBoard.API.Validators;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Configurar Serilog
builder.Host.UseSerilog((context, services, configuration) => configuration
    .ReadFrom.Configuration(context.Configuration)
    .ReadFrom.Services(services)
    .WriteTo.Console());

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<BoardValidator>();

// Configure SQL Server
builder.Services.AddDbContext<AgileBoardContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IBoardService, BoardService>();

var app = builder.Build();

// Middleware pipeline
app.UseExceptionHandler("/error"); 
app.UseMiddleware<ErrorHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication(); 
app.UseAuthorization();
app.MapControllers();

app.Run();