using Bannerlord.EncyclopediaFilters.Patches;
using Bannerlord.UIExtenderEx;
using HarmonyLib;
using System.Runtime.CompilerServices;
using TaleWorlds.ModuleManager;
using TaleWorlds.MountAndBlade;

namespace Bannerlord.EncyclopediaFilters;

public class SubModule : MBSubModuleBase
{
    protected override void OnSubModuleLoad()
    {
        var moduleId = typeof(SubModule).Namespace;

        var harmony = new Harmony(moduleId);
        DefaultEncyclopediaHeroPagePatch.Patch(harmony);
        DefaultEncyclopediaClanPagePatch.Patch(harmony);
        DefaultEncyclopediaUnitPagePatch.Patch(harmony);
        DefaultEncyclopediaSettlementPagePatch.Patch(harmony);

        TooltipRefresherCollectionPatch.Patch(harmony);

        if (ModuleHelper.IsModuleActive("Bannerlord.UIExtenderEx"))
        {
            EnableUiExtender(moduleId);
            EncyclopediaListVMPatch.Patch(harmony);
        }

        base.OnSubModuleLoad();
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private void EnableUiExtender(string moduleId)
    {
        var extender = UIExtender.Create(moduleId);
        extender.Register(typeof(SubModule).Assembly);
        extender.Enable();
    }
}