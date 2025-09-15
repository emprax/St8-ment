using System.Text.RegularExpressions;

namespace St8Ment.States.Models;

internal static partial class StateRegexes
{
    [GeneratedRegex("^[A-Z]+|(([A-Z]+_)+[A-Z]+)|(([A-Z]+-)+[A-Z]+)$")]
    internal static partial Regex StateId();
}
