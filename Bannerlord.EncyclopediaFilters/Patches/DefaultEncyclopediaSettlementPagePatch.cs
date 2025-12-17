using Bannerlord.EncyclopediaFilters.Comparers.SettlementComparers;
using HarmonyLib;
using HarmonyLib.BUTR.Extensions;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Encyclopedia;
using TaleWorlds.CampaignSystem.Encyclopedia.Pages;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Localization;
using static Bannerlord.EncyclopediaFilters.EncyclopediaHelper;

namespace Bannerlord.EncyclopediaFilters.Patches
{
    public static class DefaultEncyclopediaSettlementPagePatch
    {
        public static void Patch(Harmony harmony)
        {
            harmony.TryPatch(AccessTools2.Method(typeof(DefaultEncyclopediaSettlementPage), "InitializeFilterItems"),
                postfix: AccessTools2.Method(typeof(DefaultEncyclopediaSettlementPagePatch), nameof(InitializeFilterItemsPostfix)));
            harmony.TryPatch(AccessTools2.Method(typeof(DefaultEncyclopediaSettlementPage), "InitializeSortControllers"),
                postfix: AccessTools2.Method(typeof(DefaultEncyclopediaSettlementPagePatch), nameof(InitializeSortControllersPostfix)));
        }

        public static void InitializeFilterItemsPostfix(ref IEnumerable<EncyclopediaFilterGroup> __result)
        {
            var groups = (List<EncyclopediaFilterGroup>)__result;

            RemoveEmptyCultureFilters(groups);
            AddKingdomGroup(groups);
            AddVisitedSettlements(groups);

            __result = groups;
        }

        public static void InitializeSortControllersPostfix(ref IEnumerable<EncyclopediaSortController> __result)
        {
            var controllers = (List<EncyclopediaSortController>)__result;

            AddVillagePrimaryProductionSortController(controllers);

            __result = controllers;
        }

        private static void AddVillagePrimaryProductionSortController(List<EncyclopediaSortController> controllers)
        {
            var title = GameTexts.FindText("str_primary_production");
            controllers.Add(new EncyclopediaSortController(title, new VillagePrimaryProductionComparer()));
        }

        private static void AddVisitedSettlements(List<EncyclopediaFilterGroup> groups)
        {
            var visitedFilter = CreateFilterItem<Settlement>("{=aeouhelq}Yes", settlement => settlement.HasVisited);
            var notVisitedFilter = CreateFilterItem<Settlement>("{=8OkPHu4f}No", settlement => !settlement.HasVisited);
            var visitedGroup = CreateFilterGroup("{=r2y3n7dR}Visited Settlements", notVisitedFilter, visitedFilter);

            groups.Add(visitedGroup);
        }

        private static void RemoveEmptyCultureFilters(List<EncyclopediaFilterGroup> groups)
        {
            var cultureTextObject = GameTexts.FindText("str_culture");
            var cultureGroup = groups.FirstOrDefault(group => group.Name.HasSameValue(cultureTextObject));

            if (cultureGroup is null)
            {
                return;
            }

            for (int i = cultureGroup.Filters.Count - 1; i >= 0; i--)
            {
                var cultureName = cultureGroup.Filters[i].Name;

                if (!Campaign.Current.Settlements.Any(settlement => IsValidSettlementOfCulture(settlement, cultureName)))
                {
                    cultureGroup.Filters.RemoveAt(i);
                }
            }
        }

        private static void AddKingdomGroup(List<EncyclopediaFilterGroup> groups)
        {
            var kingdomFilters = Campaign.Current.Kingdoms.Where(kingdom => !kingdom.IsEliminated)
                .Select(kingdom => CreateFilterItem<Settlement>(kingdom.Name, settlement => settlement.MapFaction == kingdom))
                .ToList();
            var kingdomTextObject = GameTexts.FindText("str_kingdom");
            var kingdomGroup = CreateFilterGroup(kingdomTextObject, kingdomFilters);
            groups.Add(kingdomGroup);
        }

        private static bool IsValidSettlementOfCulture(Settlement settlement, TextObject cultureName)
        {
            return IsValid(settlement) && settlement.Culture.Name.HasSameValue(cultureName);
        }

        private static bool IsValid(Settlement settlement)
        {
            return settlement.IsFortification || settlement.IsVillage;
        }
    }
}
