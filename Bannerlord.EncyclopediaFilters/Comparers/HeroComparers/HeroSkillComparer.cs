using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;

namespace Bannerlord.EncyclopediaFilters.Comparers.HeroComparers
{
    public sealed class HeroSkillComparer : HeroComparerBase
    {
        private readonly SkillObject skill;

        public HeroSkillComparer(SkillObject skill)
        {
            this.skill = skill;
        }

        protected override int CompareHeroes(Hero left, Hero right)
        {
            return left.GetSkillValue(skill).CompareTo(right.GetSkillValue(skill));
        }

        protected override string GetComparedValueText(Hero hero)
        {
            return hero.GetSkillValue(skill).ToString();
        }
    }
}