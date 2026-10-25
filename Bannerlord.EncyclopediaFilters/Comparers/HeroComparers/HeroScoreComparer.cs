using Bannerlord.EncyclopediaFilters.Columns;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem;

namespace Bannerlord.EncyclopediaFilters.Comparers.HeroComparers;

/// <summary>
/// Sorts heroes by the average of the skills ticked in the 'Skills' filter group.
/// </summary>
public sealed class HeroScoreComparer : HeroComparerBase
{
    private readonly IReadOnlyList<SkillFilter> skillFilters;

    public HeroScoreComparer(IReadOnlyList<SkillFilter> skillFilters)
    {
        this.skillFilters = skillFilters;
    }

    protected override int CompareHeroes(Hero left, Hero right)
    {
        var leftScore = CalculateScore(left);
        var rightScore = CalculateScore(right);

        if (leftScore is null || rightScore is null)
        {
            if (leftScore is null && rightScore is null)
            {
                return 0;
            }

            // Heroes without a score go last in both sort directions.
            return (leftScore is null ? 1 : -1) * (IsAscending ? 1 : -1);
        }

        return leftScore.Value.CompareTo(rightScore.Value);
    }

    protected override string GetComparedValueText(Hero hero)
    {
        var score = CalculateScore(hero);
        return score is null ? _emptyValue.ToString() : score.Value.ToString("0.#");
    }

    private float? CalculateScore(Hero hero)
    {
        var total = 0;
        var count = 0;
        foreach (var skillFilter in skillFilters)
        {
            if (skillFilter.Filter.IsActive)
            {
                total += hero.GetSkillValue(skillFilter.Skill);
                count++;
            }
        }

        return count == 0 ? null : (float)total / count;
    }
}
