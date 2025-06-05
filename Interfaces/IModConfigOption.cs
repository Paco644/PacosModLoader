using PacosModLoader.enums;
using PacosModLoader.Interfaces;
using System;

namespace PacosModLoader.interfaces
{
    public interface IModConfigOption<T> : IModConfigOptionBase
    {
        T GetValue();

        void SetValue(T value);

        /// <summary>
        /// Event that fires when the value of this configuration option changes.
        /// Any mod (or the UI) with a reference to this option can subscribe to it.
        /// </summary>
        event EventHandler SettingChanged;
    }
}
