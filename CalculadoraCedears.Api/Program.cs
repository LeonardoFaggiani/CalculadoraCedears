using CalculadoraCedears.Api.Infrastructure.Extensions;

using MediatR;

using System.Reflection;

var builder = WebApplication.CreateBuilder(args);


builder.AddSecretEnviroment();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAuthorization(builder.Configuration);
builder.Services.AddSwagger();
builder.Services.AddRepositories();
builder.Services.AddMediatR(Assembly.GetExecutingAssembly());
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
builder.Services.AddDomainEvents();
builder.Services.AddBuilders();
builder.Services.AddConnectionString(builder.Configuration);
builder.Services.AddHealthChecks();
builder.Services.AddBackgroundServices();
builder.Services.AddServices();
builder.Services.AddOptions(builder.Configuration);

var app = builder.Build();

app.UseSwagger(builder.Environment);

app.UseCors(x => x.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.UseHealthChecks();

app.UseWebSockets();

app.UseDefaultFiles();
app.UseStaticFiles();

app.UseHttpsRedirection();

app.Run();

