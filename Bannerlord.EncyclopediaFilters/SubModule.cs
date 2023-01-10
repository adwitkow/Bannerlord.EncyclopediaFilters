using TaleWorlds.MountAndBlade;
using HarmonyLib;

namespace Bannerlord.EncyclopediaFilters
{
    public class SubModule : MBSubModuleBase
    {
        private static Harmony Harmony { get; set; } = default!;

        protected override void OnBeforeInitialModuleScreenSetAsRoot()
        {
            base.OnBeforeInitialModuleScreenSetAsRoot();

            Harmony = new Harmony("me.adwitkow.encyclopedia");
            Harmony.PatchAll();
        }
    }
}