using System.Text;
using Lumina.Text.ReadOnly;

namespace DutyTracker;

public static class Utils
{
    // From Ottermandias
    public static string ToTitleCaseExtended(this ReadOnlySeString s, sbyte article = 0)
    {
        if (article == 1)
            return s.ExtractText();

        var sb        = new StringBuilder(s.ExtractText());
        var lastSpace = true;
        for (var i = 0; i < sb.Length; ++i)
        {
            if (sb[i] == ' ')
            {
                lastSpace = true;
            }
            else if (lastSpace)
            {
                lastSpace = false;
                sb[i]     = char.ToUpperInvariant(sb[i]);
            }
        }

        return sb.ToString();
    }
}