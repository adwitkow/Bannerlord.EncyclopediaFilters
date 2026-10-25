// Offline check: loads the mod DLL next to the game assemblies, applies each patch class and lists what it hooked.
// Usage: PatchCheck.exe [path to Bannerlord.EncyclopediaFilters.v*.dll]
using System;
using System.IO;
using System.Linq;
using System.Reflection;

using HarmonyLib;

internal static class Program
{
    private const string Game = @"D:\Games\Steam\steamapps\common\Mount & Blade II Bannerlord";
    private const string Mods = @"D:\Games\Modding\Mount & Blade II Bannerlord\mods";

    private static readonly string[] SearchDirs =
    {
        Path.Combine(Game, @"bin\Win64_Shipping_Client"),
        Path.Combine(Game, @"Modules\Native\bin\Win64_Shipping_Client"),
        Path.Combine(Game, @"Modules\SandBoxCore\bin\Win64_Shipping_Client"),
        Path.Combine(Game, @"Modules\SandBox\bin\Win64_Shipping_Client"),
        Path.Combine(Mods, @"Harmony\Bannerlord.Harmony\bin\Win64_Shipping_Client"),
        Path.Combine(Mods, @"UIExtenderEx\Bannerlord.UIExtenderEx\bin\Win64_Shipping_Client"),
    };

    private static readonly string[] PatchClasses =
    {
        "Bannerlord.EncyclopediaFilters.Patches.DefaultEncyclopediaHeroPagePatch",
        "Bannerlord.EncyclopediaFilters.Patches.DefaultEncyclopediaClanPagePatch",
        "Bannerlord.EncyclopediaFilters.Patches.DefaultEncyclopediaUnitPagePatch",
        "Bannerlord.EncyclopediaFilters.Patches.DefaultEncyclopediaSettlementPagePatch",
        "Bannerlord.EncyclopediaFilters.Patches.EncyclopediaListVMPatch",
        "Bannerlord.EncyclopediaFilters.Patches.TooltipRefresherCollectionPatch",
    };

    private static int Main(string[] args)
    {
        string dll = args.Length > 0 ? args[0] : Directory.GetFiles(Path.GetFullPath(@"..\..\dist\Modules\Bannerlord.EncyclopediaFilters\bin\Win64_Shipping_Client"), "Bannerlord.EncyclopediaFilters.v*.dll").Single();
        AppDomain.CurrentDomain.AssemblyResolve += (_, e) =>
        {
            string name = new AssemblyName(e.Name).Name + ".dll";
            foreach (var dir in SearchDirs.Append(Path.GetDirectoryName(dll)!))
            {
                string path = Path.Combine(dir, name);
                if (File.Exists(path))
                {
                    return Assembly.LoadFrom(path);
                }
            }

            return null;
        };

        var asm = Assembly.LoadFrom(dll);
        int failures = 0;
        foreach (string className in PatchClasses)
        {
            var harmony = new Harmony("encyclopediafilters.patchcheck." + className);
            try
            {
                var patchClass = asm.GetType(className, true);
                patchClass.GetMethod("Patch", BindingFlags.Public | BindingFlags.Static)!.Invoke(null, new object[] { harmony });
                var hooked = harmony.GetPatchedMethods().ToList();
                foreach (var method in hooked)
                {
                    var info = Harmony.GetPatchInfo(method);
                    string kinds = string.Join(",", new[]
                    {
                        info.Prefixes.Any(p => p.owner == harmony.Id) ? "prefix" : null,
                        info.Postfixes.Any(p => p.owner == harmony.Id) ? "postfix" : null,
                        info.Transpilers.Any(p => p.owner == harmony.Id) ? "transpiler" : null,
                    }.Where(k => k != null));
                    Console.WriteLine($"{patchClass.Name}: {method.DeclaringType?.Name}.{method.Name} [{kinds}]");
                }

                if (hooked.Count == 0)
                {
                    Console.WriteLine($"{patchClass.Name}: nothing patched (may be expected for version-gated patches)");
                }
            }
            catch (Exception ex)
            {
                failures++;
                Console.WriteLine($"{className}: FAILED {ex.GetBaseException().Message}");
            }
        }

        Console.WriteLine(failures == 0 ? "All patches applied." : $"{failures} failure(s).");
        return failures;
    }
}
