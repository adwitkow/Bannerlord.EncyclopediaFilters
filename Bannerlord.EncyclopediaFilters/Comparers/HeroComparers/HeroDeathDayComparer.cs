using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Encyclopedia;
using TaleWorlds.CampaignSystem.Encyclopedia.Pages;

namespace Bannerlord.EncyclopediaFilters.Comparers.HeroComparers
{
    public sealed class HeroDeathDayComparer : DefaultEncyclopediaHeroPage.EncyclopediaListHeroComparer
    {
        public override int Compare(EncyclopediaListItem x, EncyclopediaListItem y)
        {
            return CompareHeroes(x, y, (hero1, hero2) => hero1.DeathDay.CompareTo(hero2.DeathDay));
        }

        public override string GetComparedValueText(EncyclopediaListItem item)
        {
            if (item.Object is not Hero hero)
            {
                return _emptyValue.ToString();
            }

#if !LOWER_THAN_1_1
            if (!Campaign.Current.Models.InformationRestrictionModel.DoesPlayerKnowDetailsOf(hero))
            {
                return _missingValue.ToString();
            }
#endif

            if (hero.IsAlive)
            {
                return _emptyValue.ToString();
            }

            return hero.DeathDay.ToString();
        }
    }
}
