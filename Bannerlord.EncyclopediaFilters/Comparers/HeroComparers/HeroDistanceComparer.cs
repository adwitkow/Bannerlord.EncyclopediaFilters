using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Library;

namespace Bannerlord.EncyclopediaFilters.Comparers.HeroComparers
{
    internal class HeroDistanceComparer : HeroComparerBase
    {
        protected override int CompareHeroes(Hero left, Hero right)
        {
            var leftParty = left.PartyBelongedTo;
            var rightParty = right.PartyBelongedTo;
            if (CompareNullability(leftParty, rightParty, out int result))
            {
                return result;
            }

            var leftDistanceSquared = CalculateDistanceSquaredToMain(leftParty);
            var rightDistanceSquared = CalculateDistanceSquaredToMain(rightParty);

            return leftDistanceSquared.CompareTo(rightDistanceSquared);
        }

        protected override string GetComparedValueText(Hero hero)
        {
            if (hero.PartyBelongedTo is null)
            {
                return _emptyValue.ToString();
            }

            var distanceSquared = CalculateDistanceSquaredToMain(hero.PartyBelongedTo);
            var distance = (int)MathF.Sqrt(distanceSquared);

            return distance.ToString();
        }

        private static float CalculateDistanceSquaredToMain(MobileParty party)
        {
            return party.Position.DistanceSquared(Hero.MainHero.PartyBelongedTo.Position);
        }

        private bool CompareNullability<T>(T left, T right, out int comparisonResult)
            where T : class
        {
            if (left is null && right is null)
            {
                comparisonResult = 0;
                return true;
            }
            else if (left is null)
            {
                comparisonResult = base.IsAscending ? 1 : -1;
                return true;
            }
            else if (right is null)
            {
                comparisonResult = base.IsAscending ? -1 : 1;
                return true;
            }
            else
            {
                comparisonResult = 0;
                return false;
            }
        }
    }
}
