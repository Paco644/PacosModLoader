using PacosModLoader.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace PacosModLoader.interfaces
{
    public interface IModManagerMod
    {
        string ModName { get; }
        string ModVersion { get; }
        string ModAuthor { get; }
        string ModDescription { get; }
        bool IsActive { get; }

        void SetActive(bool isActive);

    }
}
