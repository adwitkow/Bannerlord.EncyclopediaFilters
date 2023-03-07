using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem.Encyclopedia;
using TaleWorlds.Localization;

namespace Bannerlord.EncyclopediaFilters
{
    public static class EncyclopediaHelper
    {
        public static EncyclopediaFilterItem CreateFilterItem<T>(TextObject name, Func<T, bool> selector)
        {
            return new EncyclopediaFilterItem(name, obj => selector.Invoke((T)obj));
        }

        public static EncyclopediaFilterItem CreateFilterItem<T>(string name, Func<T, bool> selector)
        {
            return CreateFilterItem(new TextObject(name), selector);
        }

        public static EncyclopediaFilterGroup CreateFilterGroup(TextObject textObject, List<EncyclopediaFilterItem> filters)
        {
            return new EncyclopediaFilterGroup(filters, textObject);
        }

        public static EncyclopediaFilterGroup CreateFilterGroup(TextObject textObject, IEnumerable<EncyclopediaFilterItem> filters)
        {
            return new EncyclopediaFilterGroup(filters.ToList(), textObject);
        }

        public static EncyclopediaFilterGroup CreateFilterGroup(string name, List<EncyclopediaFilterItem> filters)
        {
            return CreateFilterGroup(new TextObject(name), filters);
        }

        public static EncyclopediaFilterGroup CreateFilterGroup(string name, IEnumerable<EncyclopediaFilterItem> filters)
        {
            return CreateFilterGroup(new TextObject(name), filters);
        }
    }
}
