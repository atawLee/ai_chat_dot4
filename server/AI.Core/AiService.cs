using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using ModelContextProtocol.Client;
#pragma warning disable SKEXP0001

namespace AI.Core;

public class AiService
{
    private readonly AzureAIConfiguration _configuration;
    private readonly Kernel _kernel;
    private readonly Task _initTask;
    private IMcpClient _mcpClient;

    public AiService(AzureAIConfiguration configuration)
    {
        _configuration = configuration;

        this._kernel = Kernel.CreateBuilder()
            .AddAzureOpenAIChatCompletion(
                _configuration.DeploymentName,
                _configuration.ApiUrl,
                _configuration.ApiKey)
            .Build();

        _initTask = InitPlugInAsync();
    }

    private async Task InitPlugInAsync()
    {
        // 추천 플러그인 등록
        await _kernel.ImportPluginFromOpenApiAsync("Recommender", new Uri("http://localhost:8082/swagger/v1/swagger.json"));

        var clientTransport = new SseClientTransport(new SseClientTransportOptions
        {
            Endpoint = new Uri("http://localhost:8083/sse"),
            ConnectionTimeout = TimeSpan.FromSeconds(15),
            Name = "ProjectManagerServer",
            TransportMode = HttpTransportMode.Sse,
        });

        _mcpClient = await McpClientFactory.CreateAsync(clientTransport);
        var tools = await _mcpClient.ListToolsAsync();

        // 프로젝트 매니저툴 등록
        var projectManagerFunctions = tools.Select(aiFunction => aiFunction.AsKernelFunction()).ToList();
        _kernel.Plugins.AddFromFunctions("ProjectManager", projectManagerFunctions);
    }
    
    public async Task<string> InvokePromptAsync(string prompt)
    {
        await _initTask;
        
        OpenAIPromptExecutionSettings promptExecutionSettings = new()
        {
            ToolCallBehavior = ToolCallBehavior.AutoInvokeKernelFunctions
        };

        Console.WriteLine("Invoking prompt...");
        var result = await _kernel.InvokePromptAsync(
            prompt,
            new(promptExecutionSettings));

        return result.GetValue<string>();
    }

    public async Task<string> InvokePromptWithUserConfirmationAsync(string prompt, Func<string, string, Task<bool>> userConfirmationCallback)
    {
        await _initTask;

        OpenAIPromptExecutionSettings promptExecutionSettings = new()
        {
            ToolCallBehavior = ToolCallBehavior.EnableKernelFunctions
        };

        Console.WriteLine("Invoking prompt...");

        var chatCompletion = _kernel.GetRequiredService<IChatCompletionService>();
        var chatHistory = new ChatHistory();
        chatHistory.AddUserMessage(prompt);

        while (true)
        {
            var result = await chatCompletion.GetChatMessageContentAsync(chatHistory, promptExecutionSettings, _kernel);

            // 툴 호출이 없는 경우 결과 반환
            if (result.Items.OfType<FunctionCallContent>().Count() == 0)
            {
                return result.Content ?? string.Empty;
            }

            // 툴 호출이 있는 경우 사용자에게 확인
            var functionCalls = result.Items.OfType<FunctionCallContent>();
            foreach (var functionCall in functionCalls)
            {
                string toolName = $"{functionCall.PluginName}.{functionCall.FunctionName}";
                string parameters = string.Join(", ", functionCall.Arguments.Select(kvp => $"{kvp.Key}: {kvp.Value}"));

                bool userApproved = await userConfirmationCallback(toolName, parameters);

                if (!userApproved)
                {
                    return "사용자가 툴 사용을 거부했습니다.";
                }
            }

            // 사용자가 승인한 경우 툴 실행
            chatHistory.Add(result);

            foreach (var functionCall in functionCalls)
            {
                try
                {
                    var function = _kernel.Plugins.GetFunction(functionCall.PluginName, functionCall.FunctionName);
                    var functionResult = await function.InvokeAsync(_kernel, functionCall.Arguments);
                    chatHistory.Add(new ChatMessageContent(AuthorRole.Tool, functionResult.GetValue<string>())
                    {
                        Items = [new FunctionResultContent(functionCall, functionResult.GetValue<string>())]
                    });
                }
                catch (Exception ex)
                {
                    chatHistory.Add(new ChatMessageContent(AuthorRole.Tool, $"Error: {ex.Message}")
                    {
                        Items = [new FunctionResultContent(functionCall, $"Error: {ex.Message}")]
                    });
                }
            }
        }
    }
}