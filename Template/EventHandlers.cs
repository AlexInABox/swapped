using LabApi.Features.Wrappers;
using UserSettings.ServerSpecific;

namespace Template;

public static class EventHandlers
{
    public static void RegisterEvents()
    {
        ServerSpecificSettingsSync.ServerOnSettingValueReceived += OnSSSReceived;
        Utils.RegisterKeybinds();

        // Feel free to add more event registrations here
    }

    public static void UnregisterEvents()
    {
        ServerSpecificSettingsSync.ServerOnSettingValueReceived -= OnSSSReceived;
    }

    private static void OnSSSReceived(ReferenceHub hub, ServerSpecificSettingBase ev)
    {
        // Make sure the player actually exists and stuff
        if (!Player.TryGet(hub.networkIdentity, out Player player))
            return;

        // Check if the user actually pressed OUR plugins keybind
        if (ev is not SSKeybindSetting keybindSetting ||
            keybindSetting.SettingId != Plugin.Instance.Config!.KeybindId ||
            !keybindSetting.SyncIsPressed) return;

        // Do something funny
    }
}