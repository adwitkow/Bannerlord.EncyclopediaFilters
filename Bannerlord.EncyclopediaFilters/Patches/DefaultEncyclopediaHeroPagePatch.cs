using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using HarmonyLib.BUTR.Extensions;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.Encyclopedia;
using TaleWorlds.CampaignSystem.Encyclopedia.Pages;
using TaleWorlds.CampaignSystem.Extensions;
using TaleWorlds.CampaignSystem.ViewModelCollection;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
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

        private static readonly int[] TraitLevels = new[]
        {
            0, 1, 3, 4
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
            AddClanLeaderOccupationFilter(groups);
            RemoveEmptyCultureFilters(groups);
            AddSkillFilters(groups);
            AddTraitFilters(groups);

            __result = groups;
        }

        public static void InitializeSortControllersPostfix(ref IEnumerable<EncyclopediaSortController> __result)
        {
            var controllers = (List<EncyclopediaSortController>)__result;

            AddHeroLevelSortController(controllers);
            AddDeathDaySortController(controllers);
            AddSkillSortControllers(controllers);

            __result = controllers;
        }

        private static void AddHeroLevelSortController(List<EncyclopediaSortController> controllers)
        {
            var title = GameTexts.FindText("str_level");
            controllers.Add(new EncyclopediaSortController(title, new HeroLevelComparer()));
        }

        private static void AddSkillSortControllers(List<EncyclopediaSortController> controllers)
        {
            foreach (var skill in AllSkills())
            {
                var controller = new EncyclopediaSortController(skill.Name, new SkillComparer(skill));
                controllers.Add(controller);
            }
        }

        private static void AddDeathDaySortController(List<EncyclopediaSortController> controllers)
        {
            var title = new TextObject("{=W73My5KO}Death");
            controllers.Add(new EncyclopediaSortController(title, new DeathDayComparer()));
        }

        private static void AddKingdomFilters(List<EncyclopediaFilterGroup> groups)
        {
            var kingdomFilters = Campaign.Current.Kingdoms.Where(kingdom => !kingdom.IsEliminated)
                .Select(kingdom => CreateFilterItem<Hero>(kingdom.Name, hero => hero.Clan != null && hero.Clan.Kingdom == kingdom))
                .ToList();
            var kingdomTextObject = GameTexts.FindText("str_kingdom");
            var kingdomGroup = CreateFilterGroup(kingdomTextObject, kingdomFilters);
            groups.Add(kingdomGroup);
        }

        private static void AddClanLeaderOccupationFilter(List<EncyclopediaFilterGroup> groups)
        {
            var occupationTextObject = new TextObject("{=GZxFIeiJ}Occupation");
            var occupationGroup = groups.FirstOrDefault(group => group.Name.HasSameValue(occupationTextObject));
            if (occupationGroup is not null)
            {
                var clanLeaderTextObject = GameTexts.FindText("role", "ClanLeader");
                var majorFilter = CreateFilterItem<Hero>(clanLeaderTextObject, hero => hero.Clan != null && hero.Clan.Leader == hero);
                occupationGroup.Filters.Add(majorFilter);
            }
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

                if (!Campaign.Current.AliveHeroes.Any(hero => hero.Culture.Name == cultureName))
                {
                    cultureGroup.Filters.RemoveAt(i);
                }
            }
        }

        private static void AddSkillFilters(List<EncyclopediaFilterGroup> groups)
        {
            var skillsTextObject = GameTexts.FindText("str_skills");
            var skillFilters = AllSkills().Reverse()
                .Select(skill => CreateFilterItem<Hero>(skill.Name, hero => hero.GetSkillValue(skill) > 50));
            var skillGroup = CreateFilterGroup(skillsTextObject, skillFilters);

            groups.Add(skillGroup);
        }

        private static void AddTraitFilters(List<EncyclopediaFilterGroup> groups)
        {
            var baseHeroTraits = CampaignUIHelper.GetHeroTraits();

            var allTraits = new List<TraitVariation>();
            foreach (var trait in baseHeroTraits)
            {
                foreach (var traitLevel in TraitLevels)
                {
                    var name = GameTexts.FindText("str_trait_name_" + trait.StringId.ToLower(), traitLevel.ToString());
                    var variation = new TraitVariation()
                    {
                        Underlying = trait,
                        Name = name,
                        Variation = traitLevel,
                    };

                    if (Hero.FindAll(hero => MatchesTrait(variation, hero)).Any())
                    {
                        allTraits.Add(variation);
                    }
                }
            }

            var traitFilters = allTraits.Select(trait => CreateFilterItem<Hero>(trait.Name, hero => MatchesTrait(trait, hero)));
            var traitsTextObject = GameTexts.FindText("str_traits_group");
            var traitsGroup = CreateFilterGroup(traitsTextObject, traitFilters);

            groups.Add(traitsGroup);
        }

        private static bool MatchesTrait(TraitVariation trait, Hero hero)
        {
            return hero.GetTraitLevel(trait.Underlying) + MathF.Abs(trait.Underlying.MinValue) == trait.Variation;
        }

        private static IEnumerable<SkillObject> AllSkills()
        {
            return Skills.All.OrderBy(skill => Array.IndexOf(SkillOrder, skill.StringId));
        }

        private sealed class HeroLevelComparer : DefaultEncyclopediaHeroPage.EncyclopediaListHeroComparer
        {
            public override int Compare(EncyclopediaListItem x, EncyclopediaListItem y)
            {
                return base.CompareHeroes(x, y, (Hero hero1, Hero hero2) => hero1.Level.CompareTo(hero2.Level));
            }

            public override string GetComparedValueText(EncyclopediaListItem item)
            {
                if (item.Object is not Hero hero)
                {
                    return base._emptyValue.ToString();
                }

#if !LOWER_THAN_1_1
                if (!Campaign.Current.Models.InformationRestrictionModel.DoesPlayerKnowDetailsOf(hero))
                {
                    return base._missingValue.ToString();
                }
#endif

                return hero.Level.ToString();
            }
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
                if (item.Object is not Hero hero)
                {
                    return base._emptyValue.ToString();
                }

#if !LOWER_THAN_1_1
                if (!Campaign.Current.Models.InformationRestrictionModel.DoesPlayerKnowDetailsOf(hero))
                {
                    return base._missingValue.ToString();
                }
#endif

                return hero.GetSkillValue(skill).ToString();
            }

            private int CompareSkills(Hero hero1, Hero hero2)
            {
                return hero1.GetSkillValue(skill).CompareTo(hero2.GetSkillValue(skill));
            }
        }

        private sealed class DeathDayComparer : DefaultEncyclopediaHeroPage.EncyclopediaListHeroComparer
        {
            public override int Compare(EncyclopediaListItem x, EncyclopediaListItem y)
            {
                return base.CompareHeroes(x, y, (Hero hero1, Hero hero2) => hero1.DeathDay.CompareTo(hero2.DeathDay));
            }

            public override string GetComparedValueText(EncyclopediaListItem item)
            {
                if (item.Object is not Hero hero)
                {
                    return base._emptyValue.ToString();
                }

#if !LOWER_THAN_1_1
                if (!Campaign.Current.Models.InformationRestrictionModel.DoesPlayerKnowDetailsOf(hero))
                {
                    return base._missingValue.ToString();
                }
#endif

                if (hero.IsAlive)
                {
                    return base._emptyValue.ToString();
                }

                return hero.DeathDay.ToString();
            }
        }
        
        private readonly struct TraitVariation
        {
            public TraitObject Underlying { get; init; }

            public TextObject Name { get; init; }

            public int Variation { get; init; }
        }
    }
}
