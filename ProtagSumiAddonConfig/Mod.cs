using ProtagSumiAddonConfig.Configuration;
using ProtagSumiAddonConfig.Template;
using Reloaded.Hooks.ReloadedII.Interfaces;
using Reloaded.Mod.Interfaces;
using CriFs.V2.Hook.Interfaces;
using CriExtensions;
using BF.File.Emulator.Interfaces;
using BMD.File.Emulator.Interfaces;
using PAK.Stream.Emulator.Interfaces;
using SPD.File.Emulator.Interfaces;
using P5R.CostumeFramework.Interfaces;

namespace ProtagSumiAddonConfig
{
    /// <summary>
    /// Your mod logic goes here.
    /// </summary>
    public class Mod : ModBase // <= Do not Remove.
    {
        /// <summary>
        /// Provides access to the mod loader API.
        /// </summary>
        private readonly IModLoader _modLoader;
    
        /// <summary>
        /// Provides access to the Reloaded.Hooks API.
        /// </summary>
        /// <remarks>This is null if you remove dependency on Reloaded.SharedLib.Hooks in your mod.</remarks>
        private readonly IReloadedHooks? _hooks;
    
        /// <summary>
        /// Provides access to the Reloaded logger.
        /// </summary>
        private readonly ILogger _logger;
    
        /// <summary>
        /// Entry point into the mod, instance that created this class.
        /// </summary>
        private readonly IMod _owner;
    
        /// <summary>
        /// Provides access to this mod's configuration.
        /// </summary>
        private Config _configuration;
    
        /// <summary>
        /// The configuration of the currently executing mod.
        /// </summary>
        private readonly IModConfig _modConfig;
    
        public Mod(ModContext context)
        {
            _modLoader = context.ModLoader;
            _hooks = context.Hooks;
            _logger = context.Logger;
            _owner = context.Owner;
            _configuration = context.Configuration;
            _modConfig = context.ModConfig;

            var modDir = _modLoader.GetDirectoryForModId(_modConfig.ModId); // modDir variable for file emulation

            // For more information about this template, please see
            // https://reloaded-project.github.io/Reloaded-II/ModTemplate/

            // If you want to implement e.g. unload support in your mod,
            // and some other neat features, override the methods in ModBase.

            // TODO: Implement some mod logic

            // Define controllers and other variables, set warning messages

            var criFsController = _modLoader.GetController<ICriFsRedirectorApi>();
            if (criFsController == null || !criFsController.TryGetTarget(out var criFsApi))
            {
                _logger.WriteLine($"Something in CriFS broke! Normal files will not load properly!", System.Drawing.Color.Red);
                return;
            }

            var BfEmulatorController = _modLoader.GetController<IBfEmulator>();
            if (BfEmulatorController == null || !BfEmulatorController.TryGetTarget(out var _BfEmulator))
            {
                _logger.WriteLine($"Something in BF Emulator broke! Files requiring bf merging will not load properly!", System.Drawing.Color.Red);
                return;
            }

            var BmdEmulatorController = _modLoader.GetController<IBmdEmulator>();
            if (BmdEmulatorController == null || !BmdEmulatorController.TryGetTarget(out var _BmdEmulator))
            {
                _logger.WriteLine($"Something in BMD Emulator broke! Files requiring msg merging will not load properly!", System.Drawing.Color.Red);
                return;
            }

            var PakEmulatorController = _modLoader.GetController<IPakEmulator>();
            if (PakEmulatorController == null || !PakEmulatorController.TryGetTarget(out var _PakEmulator))
            {
                _logger.WriteLine($"Something in PAK Emulator broke! Files requiring bin merging will not load properly!", System.Drawing.Color.Red);
                return;
            }

            var SPDEmulatorController = _modLoader.GetController<ISpdEmulator>();
            if (SPDEmulatorController == null || !SPDEmulatorController.TryGetTarget(out var _SpdEmulator))
            {
                _logger.WriteLine($"Something in SPD Emulator broke! Files requiring SPD merging will not load properly!", System.Drawing.Color.Red);
                return;
            }


            var CostumeFrameworkController = _modLoader.GetController<ICostumeApi>();
            if (CostumeFrameworkController == null || !CostumeFrameworkController.TryGetTarget(out var CostumeFrameworkAPI))
            {
                _logger.WriteLine($"Something in Costume Framework broke! Costumes will not load properly!", System.Drawing.Color.Red);
                return;
            }

            /* 
var BGMEController = _modLoader.GetController<IBgmeApi>();
           if (BGMEController == null || !BGMEController.TryGetTarget(out var _BGME))
           {
               _logger.WriteLine($"Something in BGME shit its pants! Files requiring bin merging will not load properly!", System.Drawing.Color.Red);
               return;
           }
               */

            // Set configuration options - obviously you don't need all of these, pick and choose what you need!





            // Grab Mod ID
            var mods = _modLoader.GetActiveMods();

            // CBT Alts
            if (_configuration.CBTAlts)
            {
                var CBTFolder = Path.Combine(modDir, "OptionalModFiles", "Costumes", "CBTAlts", "Costumes");
                CostumeFrameworkAPI.AddCostumesFolder(modDir, CBTFolder);
            }

            // Equipment Patch Config
            if (_configuration.EquipmentPatchAddon == Config.EquipmentPatch.GunOverhaul ||
                _configuration.EquipmentPatchAddon == Config.EquipmentPatch.ExtraRounds)
            {
                string? EquipmentFolder = _configuration.EquipmentPatchAddon switch
                {
                    Config.EquipmentPatch.GunOverhaul => "EquipmentGunOverhaul",
                    Config.EquipmentPatch.ExtraRounds => "EquipmentGunRoundsExtra",
                    _ => null
                };

                if (!string.IsNullOrEmpty(EquipmentFolder))
                {
                    criFsApi.AddProbingPath(Path.Combine(modDir, "OptionalModFiles", "Gameplay", EquipmentFolder));
                    _PakEmulator.AddDirectory(Path.Combine(modDir, "OptionalModFiles", "Gameplay", EquipmentFolder, "FEmulator", "PAK"));
                }
            }

            // Beta Icons
            if (_configuration.MortyBeta)
            {
                var assetFolder = Path.Combine(modDir, "OptionalModFiles", "UI", "BetaIcons", "Characters", "Joker", "1");

                if (Directory.Exists(assetFolder))
                {
                    foreach (var file in Directory.EnumerateFiles(assetFolder, "*", SearchOption.AllDirectories))
                    {
                        var relativePath = Path.GetRelativePath(assetFolder, file);
                        criFsApi.AddBind(file, relativePath, _modConfig.ModId);
                    }
                }
            }

            // AoA Color
            if (_configuration.AoAColor)
            {
                var assetFolder = Path.Combine(modDir, "OptionalModFiles", "UI", "ColoredAoABackgrounds", "Characters", "Joker", "1");

                if (Directory.Exists(assetFolder))
                {
                    foreach (var file in Directory.EnumerateFiles(assetFolder, "*", SearchOption.AllDirectories))
                    {
                        var relativePath = Path.GetRelativePath(assetFolder, file);
                        criFsApi.AddBind(file, relativePath, _modConfig.ModId);
                    }
                }
            }

            // Weapon Models
            if (_configuration.WeaponModelsConfigEnum == Config.WeaponsModels.Expanded ||
                _configuration.WeaponModelsConfigEnum == Config.WeaponsModels.Expanded_No_Lances)
            {
                string? WeaponsFolder = _configuration.WeaponModelsConfigEnum switch
                {
                    Config.WeaponsModels.Expanded => "ExpandedWeaponModels",
                    Config.WeaponsModels.Expanded_No_Lances => "ExpandedWeaponModels-NoLances",
                    _ => null
                };

                if (!string.IsNullOrEmpty(WeaponsFolder))
                {
                    var assetFolder = Path.Combine(modDir, "OptionalModFiles", "Weapon", WeaponsFolder, "Characters", "Joker", "1");

                    if (Directory.Exists(assetFolder))
                    {
                        foreach (var file in Directory.EnumerateFiles(assetFolder, "*", SearchOption.AllDirectories))
                        {
                            var relativePath = Path.GetRelativePath(assetFolder, file);
                            criFsApi.AddBind(file, relativePath, _modConfig.ModId);
                        }
                    }
                }
            }

            // Sumire Overhaul
            if (_configuration.SumireOverhaul)
            {
                var assetFolder = Path.Combine(modDir, "OptionalModFiles", "Overhaul", "SumireVersion", "Characters", "Joker", "1");

                if (Directory.Exists(assetFolder))
                {
                    foreach (var file in Directory.EnumerateFiles(assetFolder, "*", SearchOption.AllDirectories))
                    {
                        var relativePath = Path.GetRelativePath(assetFolder, file);
                        criFsApi.AddBind(file, relativePath, _modConfig.ModId);
                    }
                }
            }

            // Loading Wipe
            if (_configuration.LoadingWipeAlt == Config.LoadingWipeAltEnum.Pink ||
                _configuration.LoadingWipeAlt == Config.LoadingWipeAltEnum.Gray)
            {
                string? LoadingWipeFolder = _configuration.LoadingWipeAlt switch
                {
                    Config.LoadingWipeAltEnum.Pink => "PinkLoadingWipe",
                    Config.LoadingWipeAltEnum.Gray => "GreyLoadingWipe",
                    _ => null
                };

                if (!string.IsNullOrEmpty(LoadingWipeFolder))
                {
                    _SpdEmulator.AddDirectory(Path.Combine(modDir, "OptionalModFiles", "UI", LoadingWipeFolder, "SPD"));
                }
            }
        }

        #region Standard Overrides
        public override void ConfigurationUpdated(Config configuration)
        {
            // Apply settings from configuration.
            _configuration = configuration;
            _logger.WriteLine($"[{_modConfig.ModId}] Config Updated: Applying");
        }
        #endregion
    
        #region For Exports, Serialization etc.
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public Mod() { }
#pragma warning restore CS8618
        #endregion
    }
}
