using System;
using System.Collections.Generic;
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

        public static EncyclopediaFilterGroup CreateFilterGroup(string name, List<EncyclopediaFilterItem> filters)
        {
            return new EncyclopediaFilterGroup(filters, new TextObject(name));
        }
    }
}
