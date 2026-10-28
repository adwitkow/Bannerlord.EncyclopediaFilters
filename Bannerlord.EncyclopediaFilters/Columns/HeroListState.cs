using Bannerlord.EncyclopediaFilters.Comparers.HeroComparers;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Encyclopedia;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace Bannerlord.EncyclopediaFilters.Columns;

public readonly struct SkillFilter
{
    public SkillFilter(SkillObject skill, EncyclopediaFilterItem filter)
    {
        Skill = skill;
        Filter = filter;
        Comparer = new HeroSkillComparer(skill);
    }

    public SkillObject Skill { get; }

    public EncyclopediaFilterItem Filter { get; }

    public HeroSkillComparer Comparer { get; }
}

public readonly struct ColumnOption
{
    public ColumnOption(TextObject name, EncyclopediaListItemComparerBase comparer)
    {
        Name = name;
        Comparer = comparer;
    }

    public TextObject Name { get; }

    public EncyclopediaListItemComparerBase Comparer { get; }
}

/// <summary>
/// Per-page state of the heroes list. Ticked skills become extra columns (in the order they were ticked),
/// preceded by a 'Score' column once two or more are ticked. One extra column at a time can sort the list.
/// </summary>
public sealed class HeroListState
{
    // Ticked skills beyond this still filter the list but get no column.
    public const int MaxSkillColumns = 6;

    private static readonly ConditionalWeakTable<EncyclopediaPage, HeroListState> States = new();

    private readonly List<SkillFilter> tickOrder = new();
    private List<ColumnOption> columns = new();

    public HeroListState()
    {
        ScoreComparer = new HeroScoreComparer(SkillFilters);
    }

    public List<SkillFilter> SkillFilters { get; } = new();

    public HeroScoreComparer ScoreComparer { get; }

    public IReadOnlyList<ColumnOption> Columns => columns;

    /// <summary>Width of the column showing the 'Sort By' value; 0 when sorting by 'None'.</summary>
    public float PrimaryWidth { get; private set; }

    /// <summary>The extra column the list is sorted by, or null when the 'Sort By' dropdown decides the order.</summary>
    public EncyclopediaListItemComparerBase? SortedComparer { get; set; }

    public static HeroListState GetOrCreate(EncyclopediaPage page)
    {
        return States.GetValue(page, _ => new HeroListState());
    }

    public static HeroListState? Get(EncyclopediaPage? page)
    {
        return page is not null && States.TryGetValue(page, out var state) ? state : null;
    }

    public static HeroListState? ForHeroes()
    {
        return Get(Campaign.Current?.EncyclopediaManager?.GetPageOf(typeof(Hero)));
    }

    /// <returns>True if the filter is one of the skill filters.</returns>
    public bool OnFilterChanged(EncyclopediaFilterItem filter)
    {
        var index = SkillFilters.FindIndex(skillFilter => skillFilter.Filter == filter);
        if (index < 0)
        {
            return false;
        }

        var skillFilter = SkillFilters[index];
        tickOrder.RemoveAll(ticked => ticked.Filter == filter);
        if (filter.IsActive)
        {
            tickOrder.Add(skillFilter);
        }

        UpdateColumns();
        return true;
    }

    public void SetPrimary(EncyclopediaListItemComparerBase? comparer)
    {
        PrimaryWidth = ColumnLayout.GetPrimaryWidth(comparer);
        UpdateColumns();
    }

    public void UpdateColumns()
    {
        // Skills ticked without us noticing (e.g. restored filters) go after the ones ticked in this list.
        var ticked = tickOrder.Where(skillFilter => skillFilter.Filter.IsActive)
            .Concat(SkillFilters.Where(skillFilter => skillFilter.Filter.IsActive && !tickOrder.Any(t => t.Filter == skillFilter.Filter)))
            .ToList();

        var newColumns = new List<ColumnOption>();
        if (ticked.Count >= 2)
        {
            newColumns.Add(new ColumnOption(new TextObject("{=EFE_Score}Score"), ScoreComparer));
        }

        newColumns.AddRange(ticked.Take(MaxSkillColumns).Select(skillFilter => new ColumnOption(skillFilter.Skill.Name, skillFilter.Comparer)));
        columns = newColumns.Take(ColumnLayout.GetMaxColumns(PrimaryWidth)).ToList();

        if (SortedComparer is not null && !columns.Any(column => column.Comparer == SortedComparer))
        {
            SortedComparer = null;
        }
    }
}
