using TaleWorlds.MountAndBlade;
using HarmonyLib;
using Bannerlord.EncyclopediaFilters.Patches;

namespace Bannerlord.EncyclopediaFilters
{
    public class SubModule : MBSubModuleBase
    {
        private readonly Harmony _harmony = new("me.adwitkow.encyclopedia");

        protected override void OnSubModuleLoad()
        {
            DefaultEncyclopediaHeroPagePatch.Patch(_harmony);
            DefaultEncyclopediaClanPagePatch.Patch(_harmony);

            base.OnSubModuleLoad();
        }
    }
}