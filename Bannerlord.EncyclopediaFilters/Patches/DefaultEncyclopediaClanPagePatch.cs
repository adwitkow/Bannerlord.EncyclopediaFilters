using System.Collections.Generic;
using System.Linq;
using HarmonyLib.BUTR.Extensions;
using HarmonyLib;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Encyclopedia;
using TaleWorlds.CampaignSystem.Encyclopedia.Pages;
using static Bannerlord.EncyclopediaFilters.EncyclopediaHelper;
using TaleWorlds.Core;

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
            var kingdomTextObject = GameTexts.FindText("str_kingdom");
            var kingdomGroup = CreateFilterGroup(kingdomTextObject, kingdomFilters);
            groups.Add(kingdomGroup);
        }

        private static void AddMajorToTypeGroup(List<EncyclopediaFilterGroup> groups)
        {
            var typeTextObject = GameTexts.FindText("str_sort_by_type_label");
            var typeGroup = groups.FirstOrDefault(group => group.Name.HasSameValue(typeTextObject));
            if (typeGroup is not null)
            {
                var nobleTextObject = GameTexts.FindText("str_noble");
                var majorFilter = CreateFilterItem<IFaction>(nobleTextObject, faction => !faction.IsMinorFaction);
                typeGroup.Filters.Add(majorFilter);
            }
        }

        private static void RemoveCultureGroup(List<EncyclopediaFilterGroup> groups)
        {
            var cultureTextObject = GameTexts.FindText("str_culture");
            var cultureGroup = groups.FirstOrDefault(group => group.Name.HasSameValue(cultureTextObject));
            if (cultureGroup is not null)
            {
                groups.Remove(cultureGroup);
            }
        }
    }
}
