using Bannerlord.EncyclopediaFilters.Columns;
using HarmonyLib;
using HarmonyLib.PatchBuilder;
using TaleWorlds.CampaignSystem.Encyclopedia;
using TaleWorlds.CampaignSystem.ViewModelCollection.Encyclopedia.List;
using TaleWorlds.Library;

namespace Bannerlord.EncyclopediaFilters.Patches;

public static class EncyclopediaListVMPatch
{
    public static void Patch(Harmony harmony)
    {
        harmony.Patch<EncyclopediaListVM>()
            .Method("UpdateFilters")
                .Postfix(UpdateFiltersPostfix);

        harmony.Patch<EncyclopediaListSortControllerVM>()
            .Method("OnSortSelectionChanged")
                .Postfix(PrimarySortChangedPostfix)
            .Method(nameof(EncyclopediaListSortControllerVM.ExecuteSwitchSortOrder))
                .Postfix(PrimarySortChangedPostfix)
            .Method(nameof(EncyclopediaListSortControllerVM.SetSortOrder))
                .Postfix(PrimarySortChangedPostfix);

        var constructor = AccessTools.Constructor(
            typeof(EncyclopediaListSortControllerVM),
            new[] { typeof(EncyclopediaPage), typeof(MBBindingList<EncyclopediaListItemVM>) });
        harmony.Patch(constructor, postfix: new HarmonyMethod(typeof(EncyclopediaListVMPatch), nameof(SortControllerConstructorPostfix)));
    }

    public static void UpdateFiltersPostfix(EncyclopediaListVM __instance, EncyclopediaListFilterVM filterVM)
    {
        if (filterVM?.Filter is not null)
        {
            ColumnController.OnFilterChanged(__instance, filterVM.Filter);
        }
    }

    public static void PrimarySortChangedPostfix(EncyclopediaListSortControllerVM __instance)
    {
        ColumnController.OnPrimarySortChanged(__instance);
    }

    public static void SortControllerConstructorPostfix(EncyclopediaListSortControllerVM __instance)
    {
        ColumnController.OnListCreated(__instance);
    }
}
