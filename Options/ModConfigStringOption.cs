using BepInEx.Configuration;
using PacosModLoader.enums;
using PacosModLoader.interfaces;
using System;

namespace PacosModLoader.Options
{
    public class ModConfigStringOption : IModConfigOption<string>
    {
        private ConfigEntry<string> _configEntry;

        public string Id { get; }
        public string Name { get; }
        public string Description { get; }
        public ConfigOptionType Type => ConfigOptionType.InputFieldString;
        public string PlaceholderText { get; }

        public event EventHandler SettingChanged;

        public ModConfigStringOption(
            string id,
            string name,
            string description,
            ConfigEntry<string> configEntry,
            string placeholderText = ""
        )
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentNullException(nameof(id), "ModConfigStringOption 'id' cannot be null or empty.");
            Id = id;
            Name = name;
            Description = description;
            _configEntry = configEntry ?? throw new ArgumentNullException(nameof(configEntry), "ModConfigStringOption 'configEntry' cannot be null.");
            PlaceholderText = placeholderText;

            _configEntry.SettingChanged += (sender, args) =>
            {
                SettingChanged?.Invoke(this, EventArgs.Empty);
            };
        }

        public string GetValue() => _configEntry.Value;
        public void SetValue(string value) => _configEntry.Value = value;
    }
}