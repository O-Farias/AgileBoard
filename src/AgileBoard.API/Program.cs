using Microsoft.EntityFrameworkCore;
using AgileBoard.API.Data;
using AgileBoard.API.Services;
using AgileBoard.API.Middleware;
using AgileBoard.API.Hubs;
using FluentValidation;
using FluentValidation.AspNetCore;
using AgileBoard.API.Validators;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Host.UseSerilog((context, services, configuration) => configuration
    .ReadFrom.Configuration(context.Configuration)
    .ReadFrom.Services(services)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day));

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<BoardValidator>();


builder.Services.AddSignalR(options =>
{
    options.EnableDetailedErrors = true;
    options.MaximumReceiveMessageSize = 102400; // 100 KB
});

// Add services
builder.Services.AddScoped<IBoardService, BoardService>();
builder.Services.AddScoped<ICardService, CardService>();
builder.Services.AddScoped<IListService, ListService>();
builder.Services.AddScoped<IUserService, UserService>();

// Configure SQL Server
builder.Services.AddDbContext<AgileBoardContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Middleware pipeline configuration
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler("/error");
app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseRouting();

// Mapear endpoints SignalR
app.MapHub<BoardHub>("/hubs/board");
app.MapControllers();

app.Run();