using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using HarmonyLib.BUTR.Extensions;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Encyclopedia;
using TaleWorlds.CampaignSystem.Encyclopedia.Pages;
using TaleWorlds.CampaignSystem.Extensions;
using TaleWorlds.Core;
using static Bannerlord.EncyclopediaFilters.EncyclopediaHelper;

namespace Bannerlord.EncyclopediaFilters.Patches
{
    public static class DefaultEncyclopediaHeroPagePatch
    {
        private static readonly string[] SkillOrder = new[]
        {
            "OneHanded", "TwoHanded", "Polearm",
            "Bow", "Crossbow", "Throwing",
            "Riding", "Athletics", "Crafting",
            "Scouting", "Tactics", "Roguery",
            "Charm", "Leadership", "Trade",
            "Steward", "Medicine", "Engineering"
        };

        public static void Patch(Harmony harmony)
        {
            harmony.TryPatch(AccessTools2.Method(typeof(DefaultEncyclopediaHeroPage), "InitializeFilterItems"),
                postfix: AccessTools2.Method(typeof(DefaultEncyclopediaHeroPagePatch), nameof(InitializeFilterItemsPostfix)));
            harmony.TryPatch(AccessTools2.Method(typeof(DefaultEncyclopediaHeroPage), "InitializeSortControllers"),
                postfix: AccessTools2.Method(typeof(DefaultEncyclopediaHeroPagePatch), nameof(InitializeSortControllersPostfix)));
        }

        public static void InitializeFilterItemsPostfix(ref IEnumerable<EncyclopediaFilterGroup> __result)
        {
            var groups = (List<EncyclopediaFilterGroup>)__result;

            AddKingdomFilters(groups);
            AddClanLeaderOccupationFilters(groups);
            RemoveEmptyCultureFilters(groups);
            AddSkillFilters(groups);

            __result = groups;
        }

        public static void InitializeSortControllersPostfix(ref IEnumerable<EncyclopediaSortController> __result)
        {
            var controllers = (List<EncyclopediaSortController>)__result;

            AddSkillSortControllers(controllers);

            __result = controllers;
        }

        private static void AddSkillSortControllers(List<EncyclopediaSortController> controllers)
        {
            foreach (var skill in AllSkills())
            {
                var controller = new EncyclopediaSortController(skill.Name, new SkillComparer(skill));
                controllers.Add(controller);
            }
        }

        private static void AddKingdomFilters(List<EncyclopediaFilterGroup> groups)
        {
            var kingdomFilters = Campaign.Current.Kingdoms.Where(kingdom => !kingdom.IsEliminated)
                .Select(kingdom => CreateFilterItem<Hero>(kingdom.Name, hero => hero.Clan != null && hero.Clan.Kingdom == kingdom))
                .ToList();
            var kingdomGroup = CreateFilterGroup("Kingdom", kingdomFilters);
            groups.Add(kingdomGroup);
        }

        private static void AddClanLeaderOccupationFilters(List<EncyclopediaFilterGroup> groups)
        {
            var occupationGroup = groups.FirstOrDefault(group => "Occupation".Equals(group.Name.ToString()));
            if (occupationGroup is not null)
            {
                var majorFilter = CreateFilterItem<Hero>("Clan Leader", hero => hero.Clan != null && hero.Clan.Leader == hero);
                occupationGroup.Filters.Add(majorFilter);
            }
        }

        private static void RemoveEmptyCultureFilters(List<EncyclopediaFilterGroup> groups)
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

        private static void AddSkillFilters(List<EncyclopediaFilterGroup> groups)
        {
            var skillFilters = AllSkills().Reverse()
                .Select(skill => CreateFilterItem<Hero>(skill.Name, hero => hero.GetSkillValue(skill) > 50));
            var skillGroup = CreateFilterGroup("Skills", skillFilters);

            groups.Add(skillGroup);
        }

        private static IEnumerable<SkillObject> AllSkills()
        {
            return Skills.All.OrderBy(skill => Array.IndexOf(SkillOrder, skill.StringId));
        }

        private sealed class SkillComparer : DefaultEncyclopediaHeroPage.EncyclopediaListHeroComparer
        {
            private readonly SkillObject skill;

            public SkillComparer(SkillObject skill)
            {
                this.skill = skill;
            }

            public override int Compare(EncyclopediaListItem x, EncyclopediaListItem y)
            {
                return base.CompareHeroes(x, y, (Hero hero1, Hero hero2) => CompareSkills(hero1, hero2));
            }

            public override string GetComparedValueText(EncyclopediaListItem item)
            {
                var hero = item.Object as Hero;

                if (hero is null)
                {
                    return string.Empty;
                }

                return hero.GetSkillValue(skill).ToString();
            }

            private int CompareSkills(Hero hero1, Hero hero2)
            {
                return hero1.GetSkillValue(skill).CompareTo(hero2.GetSkillValue(skill));
            }
        }
    }
}
