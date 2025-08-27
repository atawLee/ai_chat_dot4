using Microsoft.Extensions.Configuration;

namespace AI.Core;

public class AzureAIConfiguration
{
    public AzureAIConfiguration(IConfiguration configuration)
    {
        configuration.GetSection("AzureAI").Bind(this);
    }

    public string ApiKey { get; set; } = string.Empty;
    public string ApiUrl { get; set; } = string.Empty;

    public string DeploymentName { get; set; } = string.Empty;
}