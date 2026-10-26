using Bannerlord.UIExtenderEx.Attributes;
using Bannerlord.UIExtenderEx.ViewModels;
using TaleWorlds.CampaignSystem.ViewModelCollection.Encyclopedia;
using TaleWorlds.CampaignSystem.ViewModelCollection.Encyclopedia.List;
using TaleWorlds.Library;

namespace Bannerlord.EncyclopediaFilters.Columns;

/// <summary>
/// Gives the navigation bar the open list's sort controller, for the stand-in dropdown drawn under the search results.
/// </summary>
[ViewModelMixin]
public sealed class NavigatorSortMixin : BaseViewModelMixin<EncyclopediaNavigatorVM>
{
    public NavigatorSortMixin(EncyclopediaNavigatorVM vm) : base(vm)
    {
        SearchState.Changed += OnSearchStateChanged;
    }

    [DataSourceProperty]
    public EncyclopediaListSortControllerVM? CurrentSortController => SearchState.CurrentSortController;

    [DataSourceProperty]
    public bool IsSortStandInVisible => SearchState.IsSearchResultsShown && SearchState.CurrentSortController is not null;

    public override void OnFinalize()
    {
        SearchState.Changed -= OnSearchStateChanged;
        base.OnFinalize();
    }

    private void OnSearchStateChanged()
    {
        OnPropertyChanged(nameof(CurrentSortController));
        OnPropertyChanged(nameof(IsSortStandInVisible));
    }
}
