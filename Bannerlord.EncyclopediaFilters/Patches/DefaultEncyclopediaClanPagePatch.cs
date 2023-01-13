using System.Collections.Generic;
using System.Linq;
using HarmonyLib.BUTR.Extensions;
using HarmonyLib;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Encyclopedia;
using TaleWorlds.CampaignSystem.Encyclopedia.Pages;
using static Bannerlord.EncyclopediaFilters.EncyclopediaHelper;

namespace Bannerlord.EncyclopediaFilters.Patches
{
    public static class DefaultEncyclopediaClanPagePatch
    {
        public static void Patch(Harmony harmony)
        {
            harmony.TryPatch(AccessTools2.Method(typeof(DefaultEncyclopediaClanPage), "InitializeFilterItems"),
                postfix: AccessTools2.Method(typeof(DefaultEncyclopediaClanPagePatch), nameof(InitializeFilterItemsPostfix)));
        }

        public static void InitializeFilterItemsPostfix(ref IEnumerable<EncyclopediaFilterGroup> __result)
        {
            var groups = (List<EncyclopediaFilterGroup>)__result;

            RemoveCultureGroup(groups);
            AddMajorToTypeGroup(groups);
            AddKingdomGroup(groups);

            __result = groups;
        }

        private static void AddKingdomGroup(List<EncyclopediaFilterGroup> groups)
        {
            var kingdomFilters = Campaign.Current.Kingdoms.Where(kingdom => !kingdom.IsEliminated)
                .Select(kingdom => CreateFilterItem<Clan>(kingdom.Name, clan => clan.Kingdom == kingdom))
                .ToList();
            var kingdomGroup = CreateFilterGroup("Kingdom", kingdomFilters);
            groups.Add(kingdomGroup);
        }

        private static void AddMajorToTypeGroup(List<EncyclopediaFilterGroup> groups)
        {
            var typeGroup = groups.FirstOrDefault(group => "Type".Equals(group.Name.ToString()));
            if (typeGroup is not null)
            {
                var majorFilter = CreateFilterItem<IFaction>("Major", faction => !faction.IsMinorFaction);
                typeGroup.Filters.Add(majorFilter);
            }
        }

        private static void RemoveCultureGroup(List<EncyclopediaFilterGroup> groups)
        {
            var cultureGroup = groups.FirstOrDefault(group => "Culture".Equals(group.Name.ToString()));
            if (cultureGroup is not null)
            {
                groups.Remove(cultureGroup);
            }
        }
    }
}
