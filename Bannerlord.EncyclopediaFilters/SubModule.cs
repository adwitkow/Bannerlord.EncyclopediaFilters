using Bannerlord.EncyclopediaFilters.Patches;
using Bannerlord.UIExtenderEx;
using HarmonyLib;
using TaleWorlds.MountAndBlade;

namespace Bannerlord.EncyclopediaFilters;

public class SubModule : MBSubModuleBase
{
    private readonly Harmony _harmony = new("me.adwitkow.encyclopedia");
    private readonly UIExtender _extender = UIExtender.Create("Bannerlord.EncyclopediaFilters");

    protected override void OnSubModuleLoad()
    {
        DefaultEncyclopediaHeroPagePatch.Patch(_harmony);
        DefaultEncyclopediaClanPagePatch.Patch(_harmony);
        DefaultEncyclopediaUnitPagePatch.Patch(_harmony);
        DefaultEncyclopediaSettlementPagePatch.Patch(_harmony);
        EncyclopediaListVMPatch.Patch(_harmony);

        TooltipRefresherCollectionPatch.Patch(_harmony);

        _extender.Register(typeof(SubModule).Assembly);
        _extender.Enable();

        base.OnSubModuleLoad();
    }
}