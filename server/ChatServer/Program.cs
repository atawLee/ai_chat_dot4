using AI.Core;
using ChatServer;
using ChatServer.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

builder.Services.AddSignalR();
builder.Services.AddSingleton<ChatLobbyService>();
builder.Services.AddSingleton<AiService>();
builder.Services.AddSingleton<AzureAIConfiguration>();
builder.Services.AddCors(o => o.AddPolicy("AllowAll", p => p
    .AllowAnyOrigin()     // 모든 Origin
    .AllowAnyMethod()     // GET, POST, PUT, DELETE …
    .AllowAnyHeader()));


builder.Services.AddSwaggerGen();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
app.UseRouting();
app.MapHub<ChatHub>("/chatHub");

app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/openapi/v1.json", "My API v1"); 
    
});

app.UseCors("AllowAll");
app.MapControllers();
app.Run("http://*:8080");
