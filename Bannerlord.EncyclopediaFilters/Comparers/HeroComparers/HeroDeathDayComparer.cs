using TaleWorlds.CampaignSystem;

namespace Bannerlord.EncyclopediaFilters.Comparers.HeroComparers
{
    public sealed class HeroDeathDayComparer : HeroComparerBase
    {
        protected override int CompareHeroes(Hero left, Hero right)
        {
            return left.DeathDay.CompareTo(right.DeathDay);
        }

        protected override string GetComparedValueText(Hero hero)
        {
            if (hero.IsAlive)
            {
                return _emptyValue.ToString();
            }

            return hero.DeathDay.ToString();
        }
    }
}
