using System;
using DutyTracker.Enums;
using DutyTracker.Extensions;
using Lumina.Excel.Sheets;

namespace DutyTracker.Services.DutyEvent;

public class DutyStartedEventArgs(TerritoryType territoryType) : EventArgs
{
    public readonly TerritoryType TerritoryType = territoryType;
    public TerritoryIntendedUseEnum IntendedUse => TerritoryType.GetIntendedUseEnum();
}