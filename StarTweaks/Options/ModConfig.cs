namespace StarTweaks.Options {

    /// <summary>
    /// This class is used to generate and work with the SMAPI config system.
    /// Each property represents a key within the config file.
    /// The default value of each property is used when creating the config file.
    /// </summary>
    public class ModConfig {
        public bool ToggleToolbarSwitch { get; set; } = true;
    }
}
