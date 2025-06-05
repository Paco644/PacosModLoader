using PacosModLoader.enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace PacosModLoader.Interfaces
{
    /// <summary>
    /// Base interface for all Mod Config Options. This is non-generic
    /// so that different specific option types can be grouped in a single collection.
    /// </summary>
    public interface IModConfigOptionBase
    {
        string Id { get; }
        string Name { get; }
        string Description { get; }
        ConfigOptionType Type { get; }

    }
}
