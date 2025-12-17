using TaleWorlds.CampaignSystem.Settlements;

namespace Bannerlord.EncyclopediaFilters.Comparers.SettlementComparers
{
    public class VillagePrimaryProductionComparer : VillageComparerBase
    {
        protected override int CompareVillages(Village left, Village right)
        {
            var leftPrimaryProduction = left.VillageType.PrimaryProduction.Name.ToString();
            var rightPrimaryProduction = right.VillageType.PrimaryProduction.Name.ToString();
            return rightPrimaryProduction.CompareTo(leftPrimaryProduction);
        }

        protected override string GetComparedValueText(Village village)
        {
            var primaryProduction = village.VillageType.PrimaryProduction.Name;

            return primaryProduction.ToString();
        }
    }
}
