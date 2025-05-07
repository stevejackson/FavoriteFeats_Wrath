using System;
using HarmonyLib;
using UnityModManagerNet;

namespace FavoriteFeats_Wrath
{
#if DEBUG
    [EnableReloading]
#endif

    static class Main
    {
        internal static UnityModManagerNet.UnityModManager.ModEntry.ModLogger Logger;
        public static bool Enabled;

        static bool Load(UnityModManager.ModEntry modEntry)
        {
#if DEBUG
            modEntry.OnUnload = Unload;
#endif

            modEntry.OnToggle = OnToggle;
            Logger = modEntry.Logger;

            try
            {
                var harmony = new Harmony(modEntry.Info.Id);
                harmony.PatchAll();
                Main.Logger.Log("Harmony patched successfully.");

                StateManager.ClearFavoritesCache();
            }
            catch (Exception e)
            {
                Logger.LogException("Failed to patch", e);
            }
            return true;
        }


#if DEBUG
        static bool Unload(UnityModManager.ModEntry modEntry)
        {
            var harmony = new Harmony(modEntry.Info.Id);
            harmony.UnpatchAll();

            StateManager.ClearFavoritesCache();

            return true;
        }
#endif


        static bool OnToggle(UnityModManager.ModEntry modEntry, bool value)
        {
            Enabled = value;

            return true;
        }
    }

}