using System.Reflection;

namespace Markivio.Presentation.Config;

[AttributeUsage(
    AttributeTargets.Property | AttributeTargets.Parameter,
    AllowMultiple = true, Inherited = false)]
public class EnvironmentVariable : Attribute
{
    public string VariableName { get; init; }
    public EnvironmentVariable(string variableName) =>
        VariableName = variableName;
}

public static class ConfigurationConfig
{
    public static void Config(this IConfigurationBuilder config)
    {
        config.AddEnvironmentVariables();
    }

    public static T? BindEnvVariables<T>(this IConfiguration config) where T : class
    {
        Type modelType = typeof(T);
        Console.WriteLine(modelType.IsClass);
        if (modelType.IsClass)
        {
            return config.RecordStrategy<T>(modelType);
        }
        else
        {
            return config.ClasStrategy<T>(modelType);
        }
    }

    private static T? RecordStrategy<T>(this IConfiguration config, Type modelType) where T : class
    {
        ConstructorInfo ctrInfo = modelType.GetConstructors().First();
        ParameterInfo[] paramInfos = ctrInfo.GetParameters();

        object[] values = new object[paramInfos.Count()];
        int index = 0;
        foreach (ParameterInfo parameter in paramInfos)
        {
            EnvironmentVariable? envAttr = parameter.GetCustomAttribute(typeof(EnvironmentVariable)) as EnvironmentVariable;
            if (envAttr is null) throw new InvalidCastException("Cannot map env variables to the record");


            values[index++] = config[envAttr.VariableName] ?? string.Empty;
        }
        return (T?)ctrInfo.Invoke(values);
    }

    private static T? ClasStrategy<T>(this IConfiguration config, Type modelType) where T : class
    {
        PropertyInfo[]? properties = modelType.GetProperties(BindingFlags.Instance | BindingFlags.Instance);
        T? result = Activator.CreateInstance(modelType, true) as T;
        string attributeName = typeof(EnvironmentVariable).FullName ?? string.Empty;
        foreach (PropertyInfo property in properties)
        {
            EnvironmentVariable? attr = property.GetCustomAttributes()
              .FirstOrDefault(pre => pre.GetType().Name == attributeName) as EnvironmentVariable;

            if (attr is null) continue;

            string? val = config[attr.VariableName];
            property.SetValue(result, val);
        }
        return result;
    }
}
