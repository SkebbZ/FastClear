using BepInEx;
using EFT.InventoryLogic;
using HarmonyLib;
using System.Reflection;
using SPT.Reflection.Patching;


namespace FastClear
{
    // BepInPlugin attribute signifies this is a BepInEx plugin.
    // The GUID should be unique to your mod.
    [BepInPlugin("com.SkebbZ.FastClear", "FastClear", "1.0.0")]
    public class Plugin : BaseUnityPlugin
    {
        // The Awake() method is the entry point for BepInEx plugins.
        private void Awake()
        {
            // Create a new instance of our patch class and apply it.
            new KnowMalfPatch().Enable();

            // Log a message to the BepInEx console to confirm the mod is loaded.
            Logger.LogInfo("Plugin: Inspectionless Malfunctions is loaded and active!");
        }
    }

    // This is your original patch class, now named with a "Patch" suffix for clarity.
    // It inherits from ModulePatch, which is the SPT-standard way to handle Harmony patches.
    public class KnowMalfPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            // The target method remains the same in the new version of the game.
            // We get the type for the weapon's malfunction state and find the method named "IsKnownMalfType".
            Type malfStateType = typeof(Weapon.WeaponMalfunctionStateClass);
            return AccessTools.Method(malfStateType, nameof(Weapon.WeaponMalfunctionStateClass.IsKnownMalfType));
        }

        // A Postfix patch runs after the original method is complete.
        [PatchPostfix]
        private static void PatchPostfix(ref bool __result)
        {
            // __result is a special Harmony parameter that holds the return value of the original method.
            // We are forcing the result to always be 'true', making the game think you always know the malf type.
            __result = true;
        }
    }
}