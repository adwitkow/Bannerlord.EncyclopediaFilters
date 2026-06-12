using Bannerlord.EncyclopediaFilters.Patches;
using HarmonyLib;
using TaleWorlds.MountAndBlade;

namespace Bannerlord.EncyclopediaFilters;

public class SubModule : MBSubModuleBase
{
    private readonly Harmony _harmony = new("me.adwitkow.encyclopedia");

    protected override void OnSubModuleLoad()
    {
        DefaultEncyclopediaHeroPagePatch.Patch(_harmony);
        DefaultEncyclopediaClanPagePatch.Patch(_harmony);
        DefaultEncyclopediaUnitPagePatch.Patch(_harmony);
        DefaultEncyclopediaSettlementPagePatch.Patch(_harmony);

        TooltipRefresherCollectionPatch.Patch(_harmony);

        base.OnSubModuleLoad();
    }
}