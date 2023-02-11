using HarmonyLib.BUTR.Extensions;
using HarmonyLib;
using TaleWorlds.CampaignSystem.Encyclopedia.Pages;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.Encyclopedia;
using System;
using TaleWorlds.Core;
using System.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Localization;
using static Bannerlord.EncyclopediaFilters.EncyclopediaHelper;
using TaleWorlds.CampaignSystem.Extensions;

namespace Bannerlord.EncyclopediaFilters.Patches
{
    public class DefaultEncyclopediaUnitPagePatch
    {
        public static void Patch(Harmony harmony)
        {
            harmony.TryPatch(AccessTools2.Method(typeof(DefaultEncyclopediaUnitPage), "InitializeFilterItems"),
                postfix: AccessTools2.Method(typeof(DefaultEncyclopediaUnitPagePatch), nameof(InitializeFilterItemsPostfix)));
        }

        private static void InitializeFilterItemsPostfix(ref DefaultEncyclopediaUnitPage __instance, ref IEnumerable<EncyclopediaFilterGroup> __result)
        {
            var groups = (List<EncyclopediaFilterGroup>)__result;
            var instance = __instance;

            AddWeaponTypes(instance, groups);

            __result = groups;
        }

        private static void AddWeaponTypes(DefaultEncyclopediaUnitPage instance, List<EncyclopediaFilterGroup> groups)
        {
            var weaponTypes = Enum.GetValues(typeof(WeaponClass))
                .Cast<WeaponClass>()
                .Except(new[] { WeaponClass.Arrow, WeaponClass.Bolt })
                .ToList();

            var validWeaponTypes = FindValidWeaponTypes(instance, weaponTypes);
            weaponTypes.RemoveAll(type => !validWeaponTypes.Contains(type));

            var translationMap = weaponTypes.ToDictionary(type => type, type => GetWeaponTypeTranslation(type));

            var weaponTypeFilters = translationMap.Select(pair =>
            {
                var type = pair.Key;
                var textObject = pair.Value;
                return CreateFilterItem<CharacterObject>(textObject, troop => troop.Equipment.HasWeaponOfClass(type));
            });

            groups.Add(CreateFilterGroup("{=2RIyK1bp}Weapons", weaponTypeFilters));
        }

        private static TextObject GetWeaponTypeTranslation(WeaponClass type)
        {
            return GameTexts.FindText("str_inventory_weapon", ((int)type).ToString());
        }

        private static ISet<WeaponClass> FindValidWeaponTypes(DefaultEncyclopediaUnitPage instance, ICollection<WeaponClass> weaponTypes)
        {
            var validWeaponTypes = new HashSet<WeaponClass>();
            var validCharacters = CharacterObject.All.Where(character => instance.IsValidEncyclopediaItem(character)).ToList();
            foreach (var character in validCharacters)
            {
                var usedWeaponTypes = weaponTypes.Where(weaponType => character.Equipment.HasWeaponOfClass(weaponType));
                foreach (var weaponType in usedWeaponTypes)
                {
                    validWeaponTypes.Add(weaponType);
                }
            }

            return validWeaponTypes;
        }
    }
}
