using Bannerlord.EncyclopediaFilters.Comparers.HeroComparers;
using HarmonyLib;
using System;
using TaleWorlds.CampaignSystem.Encyclopedia;
using TaleWorlds.CampaignSystem.ViewModelCollection;
using TaleWorlds.CampaignSystem.ViewModelCollection.Encyclopedia.List;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace Bannerlord.EncyclopediaFilters.Columns;

/// <summary>
/// Layout of the heroes list, in the list prefab's units:
/// [name][sorted value][extra columns...], all of it ending before the scrollbar.
/// </summary>
internal static class ColumnLayout
{
    public const float VanillaNameWidth = 500f;
    public const float VanillaPrimaryWidth = 400f;
    public const float VanillaPrimaryHeaderWidth = 240f;

    // The header's name label is 10 units narrower than the row's name so the columns line up.
    public const float HeaderNameOffset = 10f;

    // The row's sorted value has a 30 margin, the header's sort button a 40 one. Without a sorted value,
    // the first extra header column takes over that margin.
    public const float HeaderPrimaryMargin = 40f;

    public const float ExtraWidth = 95f;

    // From the end of the name margin to the scrollbar. Values are short, so the last column may reach under its edge.
    private const float AvailableWidth = 1000f;
    private const float MinNameWidth = 220f;

    // The page's vertical divider. Vanilla puts it between the name and the sorted value;
    // with skill columns it moves in front of the first one.
    public const float VanillaDividerMargin = 900f;
    private const float RowColumnsStart = 425f; // Name start (395) + the sorted value's 30 margin
    private const float DividerGap = 25f;

    public static float GetPrimaryWidth(EncyclopediaListItemComparerBase? comparer)
    {
        return comparer switch
        {
            null => 0f,
            HeroDeathDayComparer => 200f, // Dates
            _ => ExtraWidth,
        };
    }

    public static int GetMaxColumns(float primaryWidth)
    {
        return (int)((AvailableWidth - primaryWidth - MinNameWidth) / ExtraWidth);
    }

    public static float GetDividerMargin(HeroListState? state)
    {
        var firstSkillIndex = GetFirstSkillColumnIndex(state);
        if (state is null || firstSkillIndex < 0)
        {
            return VanillaDividerMargin;
        }

        return RowColumnsStart + GetNameWidth(state) + state.PrimaryWidth + (firstSkillIndex * ExtraWidth) - DividerGap;
    }

    public static int GetFirstSkillColumnIndex(HeroListState? state)
    {
        if (state is null)
        {
            return -1;
        }

        for (var i = 0; i < state.Columns.Count; i++)
        {
            if (state.Columns[i].Comparer is not HeroScoreComparer)
            {
                return i;
            }
        }

        return -1;
    }

    public static float GetNameWidth(HeroListState? state)
    {
        if (state is null)
        {
            return VanillaNameWidth;
        }

        return Math.Min(VanillaNameWidth, AvailableWidth - state.PrimaryWidth - (state.Columns.Count * ExtraWidth));
    }
}

/// <summary>
/// Keeps the extra columns, their values and the list order in sync with the filters and the 'Sort By' dropdown.
/// </summary>
internal static class ColumnController
{
    private static readonly AccessTools.FieldRef<EncyclopediaListSortControllerVM, EncyclopediaPage> PageField =
        AccessTools.FieldRefAccess<EncyclopediaListSortControllerVM, EncyclopediaPage>("_page");

    private static readonly AccessTools.FieldRef<EncyclopediaListSortControllerVM, MBBindingList<EncyclopediaListItemVM>> ItemsField =
        AccessTools.FieldRefAccess<EncyclopediaListSortControllerVM, MBBindingList<EncyclopediaListItemVM>>("_items");

    public static HeroListState? GetState(EncyclopediaListSortControllerVM? sortController)
    {
        return sortController is null ? null : HeroListState.Get(PageField(sortController));
    }

    /// <summary>A new list opens unsorted; the 'Sort By' selection is applied right after.</summary>
    public static void OnListCreated(EncyclopediaListSortControllerVM sortController)
    {
        var state = GetState(sortController);
        if (state is null)
        {
            return;
        }

        state.SortedComparer = null;
        state.SetPrimary(null);
    }

    /// <summary>The 'Sort By' selection or its direction changed, so the dropdown decides the order again.</summary>
    public static void OnPrimarySortChanged(EncyclopediaListSortControllerVM sortController)
    {
        var state = GetState(sortController);
        if (state is null)
        {
            return;
        }

        state.SortedComparer = null;
        var comparer = sortController.IsAlternativeSortVisible ? sortController.SortSelection?.SelectedItem?.Comparer : null;
        state.SetPrimary(comparer?.SortController.Comparer);
        Refresh(sortController);
    }

    public static void OnFilterChanged(EncyclopediaListVM list, EncyclopediaFilterItem filter)
    {
        var sortController = list.SortController;
        var state = GetState(sortController);
        if (state is null)
        {
            return;
        }

        var previouslySorted = state.SortedComparer;
        if (!state.OnFilterChanged(filter))
        {
            return;
        }

        if (state.SortedComparer is HeroScoreComparer)
        {
            // The score depends on the ticked skills.
            Sort(sortController, state.SortedComparer);
        }
        else if (previouslySorted is not null && state.SortedComparer is null)
        {
            // The column the list was sorted by went away.
            SortByPrimary(sortController);
        }

        Refresh(sortController);
    }

    public static void SortByColumn(EncyclopediaListSortControllerVM? sortController, int index)
    {
        var state = GetState(sortController);
        if (sortController is null || state is null || index >= state.Columns.Count)
        {
            return;
        }

        var comparer = state.Columns[index].Comparer;
        if (state.SortedComparer == comparer)
        {
            comparer.SwitchSortOrder();
        }
        else
        {
            comparer.SetDefaultSortOrder();
            state.SortedComparer = comparer;
        }

        Sort(sortController, comparer);
        sortController.AlternativeSortState = (int)CampaignUIHelper.SortState.Default;
        Refresh(sortController);
    }

    public static int GetSortState(HeroListState? state, int index)
    {
        if (state is null || index >= state.Columns.Count || state.Columns[index].Comparer != state.SortedComparer)
        {
            return (int)CampaignUIHelper.SortState.Default;
        }

        return (int)(state.SortedComparer!.IsAscending ? CampaignUIHelper.SortState.Ascending : CampaignUIHelper.SortState.Descending);
    }

    private static void Sort(EncyclopediaListSortControllerVM sortController, EncyclopediaListItemComparerBase comparer)
    {
        var listComparer = new EncyclopediaListItemComparer(new EncyclopediaSortController(TextObject.GetEmpty(), comparer));
        ItemsField(sortController).Sort(listComparer);
    }

    private static void SortByPrimary(EncyclopediaListSortControllerVM sortController)
    {
        var comparer = sortController.SortSelection?.SelectedItem?.Comparer;
        if (comparer is null)
        {
            return;
        }

        ItemsField(sortController).Sort(comparer);
        var isAscending = comparer.SortController.Comparer.IsAscending;
        sortController.AlternativeSortState = (int)(isAscending ? CampaignUIHelper.SortState.Ascending : CampaignUIHelper.SortState.Descending);
    }

    private static void Refresh(EncyclopediaListSortControllerVM sortController)
    {
        SortControllerColumnsMixin.Refresh(sortController);
        ListItemColumnsMixin.RefreshAll(ItemsField(sortController));
    }
}
