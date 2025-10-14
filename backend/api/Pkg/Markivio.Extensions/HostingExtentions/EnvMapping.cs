using System.Reflection;
using Microsoft.Extensions.Configuration;

namespace Markivio.Extensions.HostingExtensions;

[AttributeUsage(
    AttributeTargets.Property | AttributeTargets.Parameter,
    AllowMultiple = true, Inherited = false)]
public class EnvironmentVariable : Attribute
{
    public string VariableName { get; init; }
    public EnvironmentVariable(string variableName) =>
        VariableName = variableName;
}

public static class EnvMapping
{
    public static T? BindEnvVariables<T>(this IConfiguration config) where T : class
    {
        Type modelType = typeof(T);
        bool hasConstructorWithParameter = modelType.GetConstructors().Any(pre => pre.GetParameters().Any());

        if (hasConstructorWithParameter)
            return config.ConstructorStrategy<T>(modelType);
        else
            return config.PropertyStrategy<T>(modelType);
    }

    private static T? ConstructorStrategy<T>(this IConfiguration config, Type modelType) where T : class
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

    private static T? PropertyStrategy<T>(this IConfiguration config, Type modelType) where T : class
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
