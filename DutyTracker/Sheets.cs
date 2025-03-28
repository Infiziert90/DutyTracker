using Lumina.Excel;
using Lumina.Excel.Sheets;

namespace DutyTracker;

public static class Sheets
{
    public static readonly ExcelSheet<TerritoryType> TerritorySheet;
    public static readonly ExcelSheet<ContentFinderCondition> ContentFinderSheet;

    static Sheets()
    {
        TerritorySheet = DutyTracker.Data.Excel.GetSheet<TerritoryType>();
        ContentFinderSheet = DutyTracker.Data.Excel.GetSheet<ContentFinderCondition>();
    }
}
