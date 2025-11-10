using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
namespace Markivio.Extensions;

public static class RegexExt
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsNotMatch(ReadOnlySpan<char> source, string pattern) =>
          !Regex.IsMatch(source, pattern);
}
