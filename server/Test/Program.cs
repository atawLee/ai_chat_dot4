using AI.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder();
builder.Services.AddSingleton<AiService>();
builder.Services.AddSingleton<AzureAIConfiguration>();

var config = builder.Configuration;
var host = builder.Build();


//var chat = host.Services.GetRequiredService<IChatClient>();

//var text = await chat.GetResponseAsync("점메추");
var aiService = host.Services.GetRequiredService<AiService>();
var text = await aiService.InvokePromptAsync("신규 프로젝트를 등록해줘. 이번프로젝트는 우주배경 mmo rpg를 만드는 프로젝트고 기간은 2025년08월28일부터 2026년02월01일까지야 ");
//var text = await aiService.InvokePromptAsync("전체 프로젝트 목록을 가져다줘");

Console.WriteLine(text);


var text2 = await aiService.InvokePromptAsync("전체 프로젝트 목록을 가져와줘");
Console.WriteLine(text2);