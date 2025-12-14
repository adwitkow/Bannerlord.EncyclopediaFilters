using TaleWorlds.CampaignSystem;

namespace Bannerlord.EncyclopediaFilters.Comparers.HeroComparers
{
    public sealed class HeroLevelComparer : HeroComparerBase
    {
        protected override int CompareHeroes(Hero left, Hero right)
        {
            return left.Level.CompareTo(right.Level);
        }

        protected override string GetComparedValueText(Hero hero)
        {
            return hero.Level.ToString();
        }
    }
}
