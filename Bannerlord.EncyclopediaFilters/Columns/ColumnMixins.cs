using Bannerlord.UIExtenderEx.Attributes;
using Bannerlord.UIExtenderEx.ViewModels;
using System.Runtime.CompilerServices;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.ViewModelCollection.Encyclopedia.List;
using TaleWorlds.Library;

namespace Bannerlord.EncyclopediaFilters.Columns;

// The prefab binds 7 extra columns; ColumnLayout decides how many of them fit.

[ViewModelMixin]
public sealed class ListItemColumnsMixin : BaseViewModelMixin<EncyclopediaListItemVM>
{
    private static readonly ConditionalWeakTable<EncyclopediaListItemVM, ListItemColumnsMixin> Mixins = new();

    public ListItemColumnsMixin(EncyclopediaListItemVM vm) : base(vm)
    {
        Mixins.Remove(vm);
        Mixins.Add(vm, this);
    }

    [DataSourceProperty]
    public float ColumnNameWidth => ColumnLayout.GetNameWidth(State);

    [DataSourceProperty]
    public float PrimaryColumnWidth => State?.PrimaryWidth ?? ColumnLayout.VanillaPrimaryWidth;

    [DataSourceProperty]
    public string ExtraColumn1Value => GetValue(0);

    [DataSourceProperty]
    public bool IsExtraColumn1Visible => IsVisible(0);

    [DataSourceProperty]
    public string ExtraColumn2Value => GetValue(1);

    [DataSourceProperty]
    public bool IsExtraColumn2Visible => IsVisible(1);

    [DataSourceProperty]
    public string ExtraColumn3Value => GetValue(2);

    [DataSourceProperty]
    public bool IsExtraColumn3Visible => IsVisible(2);

    [DataSourceProperty]
    public string ExtraColumn4Value => GetValue(3);

    [DataSourceProperty]
    public bool IsExtraColumn4Visible => IsVisible(3);

    [DataSourceProperty]
    public string ExtraColumn5Value => GetValue(4);

    [DataSourceProperty]
    public bool IsExtraColumn5Visible => IsVisible(4);

    [DataSourceProperty]
    public string ExtraColumn6Value => GetValue(5);

    [DataSourceProperty]
    public bool IsExtraColumn6Visible => IsVisible(5);

    [DataSourceProperty]
    public string ExtraColumn7Value => GetValue(6);

    [DataSourceProperty]
    public bool IsExtraColumn7Visible => IsVisible(6);

    private HeroListState? State => ViewModel?.Object is Hero ? HeroListState.ForHeroes() : null;

    public static void RefreshAll(MBBindingList<EncyclopediaListItemVM> items)
    {
        foreach (var item in items)
        {
            if (Mixins.TryGetValue(item, out var mixin))
            {
                mixin.Refresh();
            }
        }
    }

    public override void OnFinalize()
    {
        if (ViewModel is not null)
        {
            Mixins.Remove(ViewModel);
        }

        base.OnFinalize();
    }

    private void Refresh()
    {
        OnPropertyChanged(nameof(ColumnNameWidth));
        OnPropertyChanged(nameof(PrimaryColumnWidth));
        OnPropertyChanged(nameof(ExtraColumn1Value));
        OnPropertyChanged(nameof(IsExtraColumn1Visible));
        OnPropertyChanged(nameof(ExtraColumn2Value));
        OnPropertyChanged(nameof(IsExtraColumn2Visible));
        OnPropertyChanged(nameof(ExtraColumn3Value));
        OnPropertyChanged(nameof(IsExtraColumn3Visible));
        OnPropertyChanged(nameof(ExtraColumn4Value));
        OnPropertyChanged(nameof(IsExtraColumn4Visible));
        OnPropertyChanged(nameof(ExtraColumn5Value));
        OnPropertyChanged(nameof(IsExtraColumn5Visible));
        OnPropertyChanged(nameof(ExtraColumn6Value));
        OnPropertyChanged(nameof(IsExtraColumn6Visible));
        OnPropertyChanged(nameof(ExtraColumn7Value));
        OnPropertyChanged(nameof(IsExtraColumn7Visible));
    }

    private bool IsVisible(int index)
    {
        var state = State;
        return state is not null && index < state.Columns.Count;
    }

    private string GetValue(int index)
    {
        var state = State;
        if (state is null || ViewModel is null || index >= state.Columns.Count)
        {
            return string.Empty;
        }

        return state.Columns[index].Comparer.GetComparedValueText(ViewModel.ListItem);
    }
}

[ViewModelMixin]
public sealed class SortControllerColumnsMixin : BaseViewModelMixin<EncyclopediaListSortControllerVM>
{
    private static readonly ConditionalWeakTable<EncyclopediaListSortControllerVM, SortControllerColumnsMixin> Mixins = new();

    public SortControllerColumnsMixin(EncyclopediaListSortControllerVM vm) : base(vm)
    {
        Mixins.Remove(vm);
        Mixins.Add(vm, this);
        SearchState.Changed += OnSearchStateChanged;
    }

    [DataSourceProperty]
    public bool IsSortDropdownVisible => !SearchState.IsSearchResultsShown;

    [DataSourceProperty]
    public float ColumnNameHeaderWidth => ColumnLayout.GetNameWidth(State) - ColumnLayout.HeaderNameOffset;

    [DataSourceProperty]
    public float PrimaryColumnHeaderWidth => State?.PrimaryWidth ?? ColumnLayout.VanillaPrimaryHeaderWidth;

    [DataSourceProperty]
    public float FirstExtraHeaderMargin => State is { PrimaryWidth: 0f } ? ColumnLayout.HeaderPrimaryMargin : 0f;

    [DataSourceProperty]
    public float DividerMarginLeft => ColumnLayout.GetDividerMargin(State);

    [DataSourceProperty]
    public bool IsDividerVisible => ViewModel?.IsAlternativeSortVisible == true || ColumnLayout.GetFirstSkillColumnIndex(State) >= 0;

    [DataSourceProperty]
    public string ExtraColumn1Header => GetHeader(0);

    [DataSourceProperty]
    public bool IsExtraColumn1Visible => IsVisible(0);

    [DataSourceProperty]
    public int ExtraColumn1SortState => ColumnController.GetSortState(State, 0);

    [DataSourceMethod]
    public void ExecuteSortByExtraColumn1() => ColumnController.SortByColumn(ViewModel, 0);

    [DataSourceProperty]
    public string ExtraColumn2Header => GetHeader(1);

    [DataSourceProperty]
    public bool IsExtraColumn2Visible => IsVisible(1);

    [DataSourceProperty]
    public int ExtraColumn2SortState => ColumnController.GetSortState(State, 1);

    [DataSourceMethod]
    public void ExecuteSortByExtraColumn2() => ColumnController.SortByColumn(ViewModel, 1);

    [DataSourceProperty]
    public string ExtraColumn3Header => GetHeader(2);

    [DataSourceProperty]
    public bool IsExtraColumn3Visible => IsVisible(2);

    [DataSourceProperty]
    public int ExtraColumn3SortState => ColumnController.GetSortState(State, 2);

    [DataSourceMethod]
    public void ExecuteSortByExtraColumn3() => ColumnController.SortByColumn(ViewModel, 2);

    [DataSourceProperty]
    public string ExtraColumn4Header => GetHeader(3);

    [DataSourceProperty]
    public bool IsExtraColumn4Visible => IsVisible(3);

    [DataSourceProperty]
    public int ExtraColumn4SortState => ColumnController.GetSortState(State, 3);

    [DataSourceMethod]
    public void ExecuteSortByExtraColumn4() => ColumnController.SortByColumn(ViewModel, 3);

    [DataSourceProperty]
    public string ExtraColumn5Header => GetHeader(4);

    [DataSourceProperty]
    public bool IsExtraColumn5Visible => IsVisible(4);

    [DataSourceProperty]
    public int ExtraColumn5SortState => ColumnController.GetSortState(State, 4);

    [DataSourceMethod]
    public void ExecuteSortByExtraColumn5() => ColumnController.SortByColumn(ViewModel, 4);

    [DataSourceProperty]
    public string ExtraColumn6Header => GetHeader(5);

    [DataSourceProperty]
    public bool IsExtraColumn6Visible => IsVisible(5);

    [DataSourceProperty]
    public int ExtraColumn6SortState => ColumnController.GetSortState(State, 5);

    [DataSourceMethod]
    public void ExecuteSortByExtraColumn6() => ColumnController.SortByColumn(ViewModel, 5);

    [DataSourceProperty]
    public string ExtraColumn7Header => GetHeader(6);

    [DataSourceProperty]
    public bool IsExtraColumn7Visible => IsVisible(6);

    [DataSourceProperty]
    public int ExtraColumn7SortState => ColumnController.GetSortState(State, 6);

    [DataSourceMethod]
    public void ExecuteSortByExtraColumn7() => ColumnController.SortByColumn(ViewModel, 6);

    private HeroListState? State => ColumnController.GetState(ViewModel);

    public static void Refresh(EncyclopediaListSortControllerVM? vm)
    {
        if (vm is not null && Mixins.TryGetValue(vm, out var mixin))
        {
            mixin.Refresh();
        }
    }

    public override void OnFinalize()
    {
        SearchState.Changed -= OnSearchStateChanged;
        if (ViewModel is not null)
        {
            SearchState.OnSortControllerFinalized(ViewModel);
        }

        if (ViewModel is not null)
        {
            Mixins.Remove(ViewModel);
        }

        base.OnFinalize();
    }

    private void Refresh()
    {
        OnPropertyChanged(nameof(ColumnNameHeaderWidth));
        OnPropertyChanged(nameof(PrimaryColumnHeaderWidth));
        OnPropertyChanged(nameof(FirstExtraHeaderMargin));
        OnPropertyChanged(nameof(DividerMarginLeft));
        OnPropertyChanged(nameof(IsDividerVisible));
        OnPropertyChanged(nameof(ExtraColumn1Header));
        OnPropertyChanged(nameof(IsExtraColumn1Visible));
        OnPropertyChanged(nameof(ExtraColumn1SortState));
        OnPropertyChanged(nameof(ExtraColumn2Header));
        OnPropertyChanged(nameof(IsExtraColumn2Visible));
        OnPropertyChanged(nameof(ExtraColumn2SortState));
        OnPropertyChanged(nameof(ExtraColumn3Header));
        OnPropertyChanged(nameof(IsExtraColumn3Visible));
        OnPropertyChanged(nameof(ExtraColumn3SortState));
        OnPropertyChanged(nameof(ExtraColumn4Header));
        OnPropertyChanged(nameof(IsExtraColumn4Visible));
        OnPropertyChanged(nameof(ExtraColumn4SortState));
        OnPropertyChanged(nameof(ExtraColumn5Header));
        OnPropertyChanged(nameof(IsExtraColumn5Visible));
        OnPropertyChanged(nameof(ExtraColumn5SortState));
        OnPropertyChanged(nameof(ExtraColumn6Header));
        OnPropertyChanged(nameof(IsExtraColumn6Visible));
        OnPropertyChanged(nameof(ExtraColumn6SortState));
        OnPropertyChanged(nameof(ExtraColumn7Header));
        OnPropertyChanged(nameof(IsExtraColumn7Visible));
        OnPropertyChanged(nameof(ExtraColumn7SortState));
    }

    private bool IsVisible(int index)
    {
        var state = State;
        return state is not null && index < state.Columns.Count;
    }

    private void OnSearchStateChanged()
    {
        OnPropertyChanged(nameof(IsSortDropdownVisible));
    }

    private string GetHeader(int index)
    {
        var state = State;
        if (state is null || index >= state.Columns.Count)
        {
            return string.Empty;
        }

        return state.Columns[index].Name.ToString();
    }
}
