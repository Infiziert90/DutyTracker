using System;
using System.Collections.Generic;
using System.Linq;
using DutyTracker.Enums;
using DutyTracker.Extensions;
using DutyTracker.Services.DutyEvent;
using Lumina.Excel.Sheets;

namespace DutyTracker.DutyEvents;

public class Duty(DutyStartedEventArgs eventArgs)
{
    private uint TerritoryRow { get; } = eventArgs.TerritoryType.RowId;
    private uint ContentRow { get; } = eventArgs.Content.RowId;

    public readonly DateTime StartTime = DateTime.Now;
    public DateTime EndTime = DateTime.MinValue;
    public TimeSpan Duration => EndTime == DateTime.MinValue ? DateTime.Now - StartTime : EndTime - StartTime;
    public readonly List<Run> RunList = [];
    public readonly AllianceType AllianceType = eventArgs.IntendedUse.GetAllianceType();

    public int TotalDeaths => RunList.Sum(run => run.DeathList.Count);
    public int TotalWipes => int.Max(RunList.Count - 1, 0);

    public List<Death> AllDeaths
    {
        get { return RunList.Aggregate(new List<Death>(), (x, y) => x.Concat(y.DeathList).ToList()); }
    }

    public TerritoryType GetTerritory => Sheets.TerritorySheet.GetRow(TerritoryRow);
    public ContentFinderCondition GetContent => Sheets.ContentFinderSheet.GetRow(ContentRow);
}