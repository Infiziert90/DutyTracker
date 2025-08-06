using Dalamud.Interface.Utility.Raii;
using Dalamud.Bindings.ImGui;

namespace DutyTracker.Windows.Config;

public partial class ConfigWindow
{
    private void General()
    {
        using var tabItem = ImRaii.TabItem("General");
        if (!tabItem.Success)
            return;

        var changed = false;
        changed |= ImGui.Checkbox("Include [DutyTracker] label", ref DutyTracker.Configuration.IncludeDutyTrackerLabel);
        changed |= ImGui.Checkbox("Suppress values that are zero", ref DutyTracker.Configuration.SuppressEmptyValues);

        if (changed)
            DutyTracker.Configuration.Save();
    }
}