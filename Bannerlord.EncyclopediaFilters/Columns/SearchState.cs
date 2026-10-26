using System;
using TaleWorlds.CampaignSystem.ViewModelCollection.Encyclopedia;
using TaleWorlds.CampaignSystem.ViewModelCollection.Encyclopedia.List;

namespace Bannerlord.EncyclopediaFilters.Columns;

/// <summary>
/// Tracks the encyclopedia's search results popup and the open list's sort controller.
/// The bar and the page share a layer, and the page is drawn on top, so the raised 'Sort By' dropdown would cover
/// the search results. While they are shown, the page's dropdown hides and a non-interactive copy on the bar,
/// drawn under the results, takes its place.
/// </summary>
internal static class SearchState
{
    private static WeakReference<EncyclopediaNavigatorVM>? navigator;
    private static WeakReference<EncyclopediaListSortControllerVM>? sortController;

    public static event Action? Changed;

    public static bool IsSearchResultsShown =>
        navigator is not null && navigator.TryGetTarget(out var current) && current.IsSearchResultsShown;

    public static EncyclopediaListSortControllerVM? CurrentSortController =>
        sortController is not null && sortController.TryGetTarget(out var current) ? current : null;

    public static void OnNavigatorCreated(EncyclopediaNavigatorVM created)
    {
        navigator = new WeakReference<EncyclopediaNavigatorVM>(created);
        Changed?.Invoke();
    }

    public static void OnSearchResultsShownChanged(EncyclopediaNavigatorVM source)
    {
        if (navigator is null || !navigator.TryGetTarget(out var current) || current != source)
        {
            navigator = new WeakReference<EncyclopediaNavigatorVM>(source);
        }

        Changed?.Invoke();
    }

    public static void OnSortControllerCreated(EncyclopediaListSortControllerVM created)
    {
        sortController = new WeakReference<EncyclopediaListSortControllerVM>(created);
        Changed?.Invoke();
    }

    public static void OnSortControllerFinalized(EncyclopediaListSortControllerVM finalized)
    {
        if (CurrentSortController == finalized)
        {
            sortController = null;
            Changed?.Invoke();
        }
    }
}
