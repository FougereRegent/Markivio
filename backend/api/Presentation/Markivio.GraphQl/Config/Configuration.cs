using System.Reflection;

namespace Markivio.Presentation.Config;

public static class ConfigurationConfig
{
    public static void Config(this IConfigurationBuilder config)
    {
        config.AddEnvironmentVariables();
    }
}
