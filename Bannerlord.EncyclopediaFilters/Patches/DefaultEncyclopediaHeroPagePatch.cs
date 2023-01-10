using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Encyclopedia;
using TaleWorlds.CampaignSystem.Encyclopedia.Pages;
using TaleWorlds.Core;
using static Bannerlord.EncyclopediaFilters.EncyclopediaHelper;

namespace Bannerlord.EncyclopediaFilters.Patches
{
    [HarmonyPatch(typeof(DefaultEncyclopediaHeroPage), "InitializeFilterItems")]
    public static class DefaultEncyclopediaHeroPagePatch
    {
        public static void Postfix(ref IEnumerable<EncyclopediaFilterGroup> __result)
        {
            var groups = (List<EncyclopediaFilterGroup>)__result;

            AddKingdomGroup(groups);
            AddClanLeaderOccupation(groups);
            RemoveEmptyCultures(groups);

            __result = groups;
        }

        private static void AddKingdomGroup(List<EncyclopediaFilterGroup> groups)
        {
            var kingdomFilters = Campaign.Current.Kingdoms.Where(kingdom => !kingdom.IsEliminated)
                .Select(kingdom => CreateFilterItem<Hero>(kingdom.Name, hero => hero.Clan != null && hero.Clan.Kingdom == kingdom))
                .ToList();
            var kingdomGroup = CreateFilterGroup("Kingdom", kingdomFilters);
            groups.Add(kingdomGroup);
        }

        private static void AddClanLeaderOccupation(List<EncyclopediaFilterGroup> groups)
        {
            var occupationGroup = groups.FirstOrDefault(group => "Occupation".Equals(group.Name.ToString()));
            if (occupationGroup is not null)
            {
                var majorFilter = CreateFilterItem<Hero>("Clan Leader", hero => hero.Clan != null && hero.Clan.Leader == hero);
                occupationGroup.Filters.Add(majorFilter);
            }
        }

        private static void RemoveEmptyCultures(List<EncyclopediaFilterGroup> groups)
        {
            var cultureGroup = groups.FirstOrDefault(group => "Culture".Equals(group.Name.ToString()));

            for (int i = cultureGroup.Filters.Count - 1; i >= 0; i--)
            {
                var cultureName = cultureGroup.Filters[i].Name;

                if (!Campaign.Current.AliveHeroes.Any(hero => hero.Culture.Name == cultureName))
                {
                    cultureGroup.Filters.RemoveAt(i);
                }
            }
        }
    }
}
