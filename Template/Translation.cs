using System.ComponentModel;

namespace Template;

public class Translation
{
    [Description("The label for the keybind setting")]
    public string KeybindSettingLabel { get; set; } = "Template";

    [Description("The hint description for the keybind setting")]
    public string KeybindSettingHintDescription { get; set; } =
        "Press this key to TEMPLATE!!";

    [Description("Header text for the spray settings group")]
    public string TemplateGroupHeader { get; set; } = "Template Plugin Settings";
}