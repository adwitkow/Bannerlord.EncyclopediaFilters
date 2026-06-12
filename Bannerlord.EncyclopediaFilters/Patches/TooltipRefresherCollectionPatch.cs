using HarmonyLib;
using HarmonyLib.PatchBuilder;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.ViewModelCollection;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Information;

namespace Bannerlord.EncyclopediaFilters.Patches;

public static class TooltipRefresherCollectionPatch
{
    private static readonly MethodInfo WealthMethod =
        AccessTools.Method(
            typeof(CampaignUIHelper),
            nameof(CampaignUIHelper.GetClanWealthStatusText));

    private static readonly MethodInfo InjectMethod =
        AccessTools.Method(
            typeof(TooltipRefresherCollectionPatch),
            nameof(AddCustomProperty));

    public static void Patch(Harmony harmony)
    {
#if !LOWER_THAN_1_3
        harmony.Patch()
            .Method(() => TooltipRefresherCollection.RefreshClanTooltip(default, default))
                .Transpiler(RefreshClanTooltipTranspiler);
#endif
    }

    private static IEnumerable<CodeInstruction> RefreshClanTooltipTranspiler(
        IEnumerable<CodeInstruction> instructions)
    {
        var matcher = new CodeMatcher(instructions)
            .MatchStartForward(CodeMatch.Calls(WealthMethod));

        if (!matcher.IsValid)
        {
            return instructions; // Not worth a crash
        }

        return matcher.Insert(
            new CodeInstruction(OpCodes.Ldarg_0),
            new CodeInstruction(OpCodes.Ldarg_1),
            new CodeInstruction(OpCodes.Call, InjectMethod))
            .InstructionEnumeration();
    }

    private static void AddCustomProperty(
        PropertyBasedTooltipVM tooltip,
        object[] args)
    {
        var clan = args[0] as Clan;

        if (clan is null)
        {
            return;
        }

        var leader = clan.Leader;

        if (leader is null)
        {
            return;
        }

        tooltip.AddProperty(
            GameTexts.FindText("str_tooltip_label_relation").ToString(),
            leader.GetRelationWithPlayer().ToString(),
            0,
            TooltipProperty.TooltipPropertyFlags.None);
    }
}
