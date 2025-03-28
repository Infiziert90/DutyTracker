using System;
using DutyTracker.Enums;
using DutyTracker.Extensions;
using Lumina.Excel.Sheets;

namespace DutyTracker.Services.DutyEvent;

public class DutyStartedEventArgs(TerritoryType territoryType, ContentFinderCondition content) : EventArgs
{
    public readonly TerritoryType TerritoryType = territoryType;
    public readonly ContentFinderCondition Content = content;

    public TerritoryIntendedUseEnum IntendedUse => TerritoryType.GetIntendedUseEnum();
}