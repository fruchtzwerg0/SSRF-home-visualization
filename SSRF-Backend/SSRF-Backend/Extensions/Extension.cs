using System.Globalization;
using System.Text;

namespace SSRF_Backend.Extensions;

public static class Extension 
{
    // Validating free-form Unicode text
    public static string RemoveDiacritics(this string strThis)
    {
        if (strThis == null)
            return null!;

        var sb = new StringBuilder();

        foreach (char c in strThis.Normalize(NormalizationForm.FormD))
        {
            if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                sb.Append(c);
         }

        return sb.ToString();
    }

    // Check the URL
    public static bool CheckIfUrlIsValid(this string url, List<string> notAllowedUrls)
    {
        if (notAllowedUrls.Contains(url) && Uri.IsWellFormedUriString(url, UriKind.Absolute))
            return false;
        else
            return true;
    }
}








