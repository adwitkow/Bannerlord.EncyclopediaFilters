using Bannerlord.EncyclopediaFilters.Patches;
using Bannerlord.UIExtenderEx;
using HarmonyLib;
#if LOWER_THAN_1_3
using System;
using System.Linq;
#endif
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

        if (IsModuleActive("Bannerlord.UIExtenderEx"))
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

    private static bool IsModuleActive(string moduleId)
    {
#if LOWER_THAN_1_3
        return TaleWorlds.Engine.Utilities.GetModulesNames()
             .Any(m => string.Equals(m, moduleId, StringComparison.OrdinalIgnoreCase));
#else
        return ModuleHelper.IsModuleActive(moduleId);
#endif
    }
}