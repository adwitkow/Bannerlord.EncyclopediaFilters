using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Encyclopedia;
using static TaleWorlds.CampaignSystem.Encyclopedia.Pages.DefaultEncyclopediaClanPage;

namespace Bannerlord.EncyclopediaFilters.Comparers.ClanComparers;

public class ClanRelationComparer : EncyclopediaListClanComparer
{
    public override int Compare(EncyclopediaListItem x, EncyclopediaListItem y)
    {
        return base.CompareClans(x, y, (left, right) => GetRelation(left).CompareTo(GetRelation(right)));
    }

    public override string GetComparedValueText(EncyclopediaListItem item)
    {
        if (item.Object is not Clan clan)
        {
            return _emptyValue.ToString();
        }

        var leader = clan?.Leader;
        if (leader is null)
        {
            return _emptyValue.ToString();
        }

#if !LOWER_THAN_1_1
        if (!Campaign.Current.Models.InformationRestrictionModel.DoesPlayerKnowDetailsOf(leader))
        {
            return _missingValue.ToString();
        }
#endif

        return GetRelation(clan).ToString();
    }

    private static float GetRelation(Clan? clan)
    {
        var leader = clan?.Leader;
        if (leader is null)
        {
            return float.MinValue;
        }

#if !LOWER_THAN_1_1
        if (!Campaign.Current.Models.InformationRestrictionModel.DoesPlayerKnowDetailsOf(leader))
        {
            return float.MinValue;
        }
#endif

        return leader.GetRelationWithPlayer();
    }
}
