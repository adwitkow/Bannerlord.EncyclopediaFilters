using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Encyclopedia;
using TaleWorlds.CampaignSystem.Encyclopedia.Pages;

namespace Bannerlord.EncyclopediaFilters.Comparers.HeroComparers
{
    public sealed class HeroLevelComparer : DefaultEncyclopediaHeroPage.EncyclopediaListHeroComparer
    {
        public override int Compare(EncyclopediaListItem x, EncyclopediaListItem y)
        {
            return CompareHeroes(x, y, (hero1, hero2) => hero1.Level.CompareTo(hero2.Level));
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

            return hero.Level.ToString();
        }
    }
}
