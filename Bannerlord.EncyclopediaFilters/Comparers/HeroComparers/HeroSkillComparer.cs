using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Encyclopedia;
using TaleWorlds.CampaignSystem.Encyclopedia.Pages;
using TaleWorlds.Core;

namespace Bannerlord.EncyclopediaFilters.Comparers.HeroComparers
{
    public sealed class HeroSkillComparer : DefaultEncyclopediaHeroPage.EncyclopediaListHeroComparer
    {
        private readonly SkillObject skill;

        public HeroSkillComparer(SkillObject skill)
        {
            this.skill = skill;
        }

        public override int Compare(EncyclopediaListItem x, EncyclopediaListItem y)
        {
            return CompareHeroes(x, y, (hero1, hero2) => CompareSkills(hero1, hero2));
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

            return hero.GetSkillValue(skill).ToString();
        }

        private int CompareSkills(Hero hero1, Hero hero2)
        {
            return hero1.GetSkillValue(skill).CompareTo(hero2.GetSkillValue(skill));
        }
    }
}