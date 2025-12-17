using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Encyclopedia;
using TaleWorlds.CampaignSystem.Encyclopedia.Pages;
using TaleWorlds.CampaignSystem.Settlements;

namespace Bannerlord.EncyclopediaFilters.Comparers.SettlementComparers
{
    public abstract class VillageComparerBase : DefaultEncyclopediaSettlementPage.EncyclopediaListSettlementComparer
    {
        public override int Compare(EncyclopediaListItem x, EncyclopediaListItem y)
        {
            return CompareVillages(x, y, CompareVillages);
        }

        public override string GetComparedValueText(EncyclopediaListItem item)
        {
            if (item.Object is not Settlement settlement)
            {
                return _emptyValue.ToString();
            }

#if !LOWER_THAN_1_1
            if (!Campaign.Current.Models.InformationRestrictionModel.DoesPlayerKnowDetailsOf(settlement))
            {
                return _missingValue.ToString();
            }
#endif

            if (!settlement.IsVillage)
            {
                return _emptyValue.ToString();
            }

            return GetComparedValueText(settlement.Village);
        }

        protected abstract int CompareVillages(Village left, Village right);

        protected abstract string GetComparedValueText(Village village);

        private int CompareVillages(EncyclopediaListItem x, EncyclopediaListItem y, Func<Village, Village, int> comparison)
        {
            if (x.Object is not Settlement leftSettlement || y.Object is not Settlement rightSettlement)
            {
                return 0;
            }

            var ascendingMultiplier = base.IsAscending ? 1 : -1;

            if (CompareVisibility(leftSettlement, rightSettlement, out int result))
            {
                if (result == 0)
                {
                    return base.ResolveEquality(x, y);
                }

                return result * ascendingMultiplier;
            }

            result = leftSettlement.IsVillage.CompareTo(rightSettlement.IsVillage);
            if (result != 0)
            {
                return -result;
            }

            if (!leftSettlement.IsVillage && !rightSettlement.IsVillage)
            {
                return base.ResolveEquality(x, y);
            }

            result = comparison(leftSettlement.Village, rightSettlement.Village);
            if (result == 0)
            {
                return base.ResolveEquality(x, y);
            }

            return result * ascendingMultiplier;
        }
    }
}
