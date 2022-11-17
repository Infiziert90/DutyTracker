﻿using System;
using System.Numerics;
using Dalamud.Interface.Windowing;
using DutyTracker.Duty_Events;
using DutyTracker.Formatting;
using ImGuiNET;
using ImGuiScene;

namespace DutyTracker.Windows;

public sealed class MainWindow : Window, IDisposable
{
    private DutyTracker   dutyTracker;
    private DutyManager   dutyManager;
    private Configuration configuration;

    private static ImGuiTableFlags TableFlags = ImGuiTableFlags.BordersV | 
                                                ImGuiTableFlags.BordersOuterH | 
                                                ImGuiTableFlags.RowBg;

    public MainWindow(DutyTracker dutyTracker, DutyManager dutyManager, Configuration configuration) : base(
        "Duty Tracker", ImGuiWindowFlags.None)
    {
        this.SizeConstraints = new WindowSizeConstraints
        {
            MinimumSize = new Vector2(375, 330),
            MaximumSize = new Vector2(float.MaxValue, float.MaxValue),
        };

        this.dutyTracker   = dutyTracker;
        this.dutyManager   = dutyManager;
        this.configuration = configuration;
    }

    public void Dispose()
    {
    }

    public override void Draw()
    {
        if(ImGui.BeginTabBar("MainWindowTabBar"))
        {
            DisplayStatus();
            DisplayOptions();
        }
        ImGui.EndTabBar();
    }

    private void DisplayStatus()
    {
        if (!ImGui.BeginTabItem("Status"))
            return;

        ImGui.Text($"Start Time: {dutyManager.Duty.StartOfDuty:hh\\:mm\\:ss tt}");
        ImGui.Text($"Start of Current Run: {dutyManager.Duty.StartOfCurrentRun:hh\\:mm\\:ss tt}");
        ImGui.Text($"End Time: {dutyManager.Duty.EndOfDuty:hh\\:mm\\:ss tt}");
        ImGui.Text($"Elapsed Time: {TimeFormat.MinutesAndSeconds(dutyManager.TotalDutyTime)}");
        ImGui.Text($"Current Run Time: {TimeFormat.MinutesAndSeconds(dutyManager.CurrentRunTime)}");
        ImGui.Text($"Duty Status: {dutyManager.DutyActive}");
        ImGui.Text($"Deaths: {dutyManager.Duty.DeathEvents.Count}");
        ImGui.Text($"Wipes: {dutyManager.Duty.WipeEvents.Count}");
        
        if (dutyManager.Duty.DeathEvents.Count > 0)
        {
            if (ImGui.BeginTable("deaths", 2, TableFlags))
            {
                ImGui.TableSetupColumn("Player Name");
                ImGui.TableSetupColumn("Time of Death");
                ImGui.TableHeadersRow();
                
                foreach (var deathEvent in dutyManager.Duty.DeathEvents)
                {
                    ImGui.TableNextRow();
                    ImGui.TableSetColumnIndex(0);
                    ImGui.TextUnformatted($"{deathEvent.PlayerName}");
                    ImGui.TableSetColumnIndex(1);
                    ImGui.TextUnformatted($"{deathEvent.TimeOfDeath:hh\\:mm\\:ss tt}");
                }
            }
            ImGui.EndTable();
        }

        if (dutyManager.Duty.WipeEvents.Count > 0)
        {
            if (ImGui.BeginTable("wipes", 2, TableFlags))
            {
                ImGui.TableSetupColumn("Run Duration");
                ImGui.TableSetupColumn("Time of Wipe");

                ImGui.TableHeadersRow();

                foreach (var wipeEvent in dutyManager.Duty.WipeEvents)
                {
                    ImGui.TableNextRow();
                    ImGui.TableSetColumnIndex(0);
                    ImGui.TextUnformatted($"{TimeFormat.MinutesAndSeconds(wipeEvent.Duration)}");
                    ImGui.TableSetColumnIndex(1);
                    ImGui.TextUnformatted($"{wipeEvent.TimeOfWipe:hh\\:mm\\:ss tt}");
                }
            }
            ImGui.EndTable();
        }
        

        ImGui.EndTabItem();
    }

    private void DisplayOptions()
    {
        if (!ImGui.BeginTabItem("Options"))
            return;

        var includeDutyTrackerLabel = configuration.IncludeDutyTrackerLabel;
        if (ImGui.Checkbox("Include [DutyTracker] label", ref includeDutyTrackerLabel))
            configuration.IncludeDutyTrackerLabel = includeDutyTrackerLabel;
        
        var suppressEmptyValues = configuration.SuppressEmptyValues;
        if (ImGui.Checkbox("Suppress values that are zero", ref suppressEmptyValues))
            configuration.SuppressEmptyValues = suppressEmptyValues;
        
        if(ImGui.Button("Save"))
            configuration.Save();
        
        ImGui.EndTabItem();
    }
}
