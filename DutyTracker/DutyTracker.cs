using Dalamud.Game.Command;
using Dalamud.Interface.Windowing;
using Dalamud.IoC;
using Dalamud.Plugin;
using Dalamud.Plugin.Services;
using DutyTracker.DutyEvents;
using DutyTracker.Services.DutyEvent;
using DutyTracker.Services.PlayerCharacter;
using DutyTracker.Windows;
using DutyTracker.Windows.Config;

namespace DutyTracker;

public sealed class DutyTracker : IDalamudPlugin
{
    [PluginService] public static IDataManager Data { get; private set; } = null!;
    [PluginService] public static IFramework Framework { get; private set; } = null!;
    [PluginService] public static ICommandManager Commands { get; private set; } = null!;
    [PluginService] public static IDalamudPluginInterface PluginInterface { get; private set; } = null!;
    [PluginService] public static IClientState ClientState { get; private set; } = null!;
    [PluginService] public static IChatGui ChatGui { get; private set; } = null!;
    [PluginService] public static IPluginLog Log { get; private set; } = null!;
    [PluginService] public static IDutyState DutyState { get; private set; } = null!;

    private const string CommandName = "/dt";

    private readonly WindowSystem WindowSystem = new("DutyTracker");
    public readonly MainWindow MainWindow;
    private readonly ConfigWindow ConfigWindow;
    private readonly DebugWindow DebugWindow;

    public readonly Configuration Configuration;
    public readonly DutyManager DutyManager;

    internal static DutyEventService DutyEventService = null!;
    internal static PlayerCharacterState PlayerCharacterState = null!;

    public DutyTracker()
    {
        DutyEventService = new DutyEventService();
        PlayerCharacterState = new PlayerCharacterState();

        Configuration = PluginInterface.GetPluginConfig() as Configuration ?? new Configuration();

        DutyManager = new DutyManager(this);

        MainWindow = new MainWindow(this);
        ConfigWindow = new ConfigWindow(this);
        DebugWindow = new DebugWindow();
        WindowSystem.AddWindow(MainWindow);
        WindowSystem.AddWindow(ConfigWindow);
        WindowSystem.AddWindow(DebugWindow);

        Commands.AddHandler(CommandName, new CommandInfo(OnCommand)
        {
            HelpMessage = "Open the Duty Tracker menu",
        });

        PluginInterface.UiBuilder.Draw += DrawUi;
        PluginInterface.UiBuilder.OpenMainUi += OpenMain;
        PluginInterface.UiBuilder.OpenConfigUi += OpenSettings;
    }

    public void Dispose()
    {
        Commands.RemoveHandler(CommandName);
        WindowSystem.RemoveAllWindows();

        PlayerCharacterState.Dispose();
        DutyEventService.Dispose();
    }

    private void OnCommand(string command, string args)
    {
        switch (args)
        {
            case "debug":
                DebugWindow.Toggle();
                break;
            case "config":
                ConfigWindow.Toggle();
                break;
            default:
                MainWindow.Toggle();
                break;
        }
    }

    private void OpenMain() => MainWindow.Toggle();
    private void OpenSettings() => ConfigWindow.Toggle();

    private void DrawUi()
    {
        WindowSystem.Draw();
    }
}