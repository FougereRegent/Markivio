using System.Reflection;

namespace Markivio.UnitTests.Helper;

internal static class PrivateFieldHelper
{
    internal static T1? GetPrivateField<T, T1>(this T source, string name)
    {
        FieldInfo field = GetField<T>(name);
        return (T1?)field.GetValue(source);
    }

    internal static void SetPrivateField<T>(this T source, string name, T value)
    {
        FieldInfo field = GetField<T>(name);
        field.SetValue(source, value);
    }

    private static FieldInfo GetField<T>(string name)
    {
        Type objectType = typeof(T);
        FieldInfo? field = objectType.GetFields(BindingFlags.NonPublic | BindingFlags.Instance)
          .FirstOrDefault(pre => pre.Name == name);

        if (field is null)
            throw new ArgumentException($"The field name {name} cannot exist");

        return field;
    }
}
