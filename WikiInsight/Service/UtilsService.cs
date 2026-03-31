using System.Text.RegularExpressions;

namespace WikiInsight.Service;

public static class UtilsService
{
    public static string GetEnvironmentVariable(string key)
    {
        var value = Environment.GetEnvironmentVariable(key);
        if (string.IsNullOrWhiteSpace(value))
            throw new Exception($"Missing Environment Variable: {key}");
        return value!;
    }

    public static string ToUrlSafeId(string? title)
    {
        if (string.IsNullOrWhiteSpace(title))
            return string.Empty;

        var s = title!.Trim();
        s = Regex.Replace(s, @"[^\w\-]+", "_");
        s = Regex.Replace(s, "_{2,}", "_");
        s = s.Trim('_');

        if (string.IsNullOrEmpty(s))
            return Uri.EscapeDataString(title);
        return s;
    }
}