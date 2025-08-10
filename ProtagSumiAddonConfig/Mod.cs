using ProtagSumiAddonConfig.Configuration;
using ProtagSumiAddonConfig.Template;
using Reloaded.Hooks.ReloadedII.Interfaces;
using Reloaded.Mod.Interfaces;
using CriFs.V2.Hook.Interfaces;
using BF.File.Emulator.Interfaces;
using BMD.File.Emulator.Interfaces;
using PAK.Stream.Emulator.Interfaces;
using SPD.File.Emulator.Interfaces;
using P5R.CostumeFramework.Interfaces;
using CriExtensions;

namespace ProtagSumiAddonConfig
{
    public class Mod : ModBase
    {
        private readonly IModLoader _modLoader;
        private readonly IReloadedHooks? _hooks;
        private readonly ILogger _logger;
        private readonly IMod _owner;
        private Config _configuration;
        private readonly IModConfig _modConfig;

        public Mod(ModContext context)
        {
            _modLoader = context.ModLoader;
            _hooks = context.Hooks;
            _logger = context.Logger;
            _owner = context.Owner;
            _configuration = context.Configuration;
            _modConfig = context.ModConfig;

            string modDir = _modLoader.GetDirectoryForModId(_modConfig.ModId);
            string modId = _modConfig.ModId;

            // Initialize file emulator controllers
            var criFsCtl = _modLoader.GetController<ICriFsRedirectorApi>();
            var bfEmuCtl = _modLoader.GetController<IBfEmulator>();
            var bmdEmuCtl = _modLoader.GetController<IBmdEmulator>();
            var pakEmuCtl = _modLoader.GetController<IPakEmulator>();
            var spdEmuCtl = _modLoader.GetController<ISpdEmulator>();
            var costumeCtl = _modLoader.GetController<ICostumeApi>();

            if (criFsCtl == null || !criFsCtl.TryGetTarget(out var criFsApi)) { _logger.WriteLine("CRI FS missing → cpk and binds broken.", System.Drawing.Color.Red); return; }
            if (bfEmuCtl == null || !bfEmuCtl.TryGetTarget(out var bfEmu)) { _logger.WriteLine("BF Emu missing → BF merges broken.", System.Drawing.Color.Red); return; }
            if (bmdEmuCtl == null || !bmdEmuCtl.TryGetTarget(out var bmdEmu)) { _logger.WriteLine("BMD Emu missing → BMD merges broken.", System.Drawing.Color.Red); return; }
            if (pakEmuCtl == null || !pakEmuCtl.TryGetTarget(out var pakEmu)) { _logger.WriteLine("PAK Emu missing → PAK merges broken.", System.Drawing.Color.Red); return; }
            if (spdEmuCtl == null || !spdEmuCtl.TryGetTarget(out var spdEmu)) { _logger.WriteLine("SPD Emu missing → SPD merges broken.", System.Drawing.Color.Red); return; }
            if (costumeCtl == null || !costumeCtl.TryGetTarget(out var costumeApi)) { _logger.WriteLine("Costume API missing → Costumes broken.", System.Drawing.Color.Red); return; }

            // CBT Alts
            if (_configuration.CBTAlts)
                costumeApi.AddCostumesFolder(modDir, Path.Combine(modDir, "OptionalModFiles", "CostumesCBTAlts"));

            // Addon Costumes
            if (_configuration.MiscCostumesSumi)
                costumeApi.AddCostumesFolder(modDir, Path.Combine(modDir, "OptionalModFiles", "CostumesAddon"));

            // Equipment Patch Config
            if (_configuration.EquipmentPatchAddon == Config.EquipmentPatch.GunOverhaul || _configuration.EquipmentPatchAddon == Config.EquipmentPatch.ExtraRounds)
            {
                string folder = _configuration.EquipmentPatchAddon == Config.EquipmentPatch.GunOverhaul ? "EquipmentGunOverhaul" : "EquipmentGunRoundsExtra";
                criFsApi.AddProbingPath(Path.Combine(modDir, "OptionalModFiles", "Gameplay", folder));
                pakEmu.AddDirectory(Path.Combine(modDir, "OptionalModFiles", "Gameplay", folder, "FEmulator", "PAK"));
            }

            // Beta Icons (MortyBeta)
            if (_configuration.MortyBeta)
                BindAllFilesIn(Path.Combine("OptionalModFiles", "UI", "BetaIcons"), modDir, criFsApi, modId);

            // AoA Color
            if (_configuration.AoAColor)
                BindAllFilesIn(Path.Combine("OptionalModFiles", "UI", "ColoredAoABackgrounds"), modDir, criFsApi, modId);

            // Weapon Models: Expanded or Expanded_No_Lances
            if (_configuration.WeaponModelsConfigEnum == Config.WeaponsModels.Expanded || _configuration.WeaponModelsConfigEnum == Config.WeaponsModels.Expanded_No_Lances)
            {
                string folder = _configuration.WeaponModelsConfigEnum == Config.WeaponsModels.Expanded ? "ExpandedWeaponModels" : "ExpandedWeaponModels-NoLances";
                BindAllFilesIn(Path.Combine("OptionalModFiles", "Weapon", folder), modDir, criFsApi, modId);
            }

            // Sumire Overhaul
            if (_configuration.SumireOverhaul)
                BindAllFilesIn(Path.Combine("OptionalModFiles", "Overhaul", "SumireVersion"), modDir, criFsApi, modId);

            // Sumire Animations
            if (_configuration.SumireAnimations)
                BindAllFilesIn(Path.Combine("OptionalModFiles", "Overhaul", "SumireAnimations"), modDir, criFsApi, modId);

            // EPIC
            if (_configuration.EPICPP)
                spdEmu.AddDirectory(Path.Combine(modDir, "OptionalModFiles", "UI", "EPIC", "SPD"));

            // Colorful Pack
            if (_configuration.ColorfulPack)
                spdEmu.AddDirectory(Path.Combine(modDir, "OptionalModFiles", "UI", "Colorful", "SPD"));
                BindAllFilesIn(Path.Combine("OptionalModFiles", "UI", "Colorful", "Bind"), modDir, criFsApi, modId);

            // Loading Wipe (Pink or Gray)
            if (_configuration.LoadingWipeAlt == Config.LoadingWipeAltEnum.Pink || _configuration.LoadingWipeAlt == Config.LoadingWipeAltEnum.Gray)
            {
                string wipe = _configuration.LoadingWipeAlt == Config.LoadingWipeAltEnum.Pink ? "PinkLoadingWipe" : "GreyLoadingWipe";
                spdEmu.AddDirectory(Path.Combine(modDir, "OptionalModFiles", "UI", wipe, "SPD"));
            }
        }

        private static void BindAllFilesIn(string subPathRelativeToModDir, string modDir, ICriFsRedirectorApi criFsApi, string modId)
        {
            var absoluteFolder = Path.Combine(modDir, subPathRelativeToModDir);
            if (!Directory.Exists(absoluteFolder)) return;
            foreach (var file in Directory.EnumerateFiles(absoluteFolder, "*", SearchOption.AllDirectories))
                criFsApi.AddBind(file, Path.GetRelativePath(absoluteFolder, file).Replace(Path.DirectorySeparatorChar, '/'), modId);
        }

        public override void ConfigurationUpdated(Config configuration)
        {
            _configuration = configuration;
            _logger.WriteLine($"[{_modConfig.ModId}] Config Updated: Applying");
        }

#pragma warning disable CS8618
        public Mod() { }
#pragma warning restore CS8618
    }
}
