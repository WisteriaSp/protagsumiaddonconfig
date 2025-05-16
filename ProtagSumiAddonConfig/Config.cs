using ProtagSumiAddonConfig.Template.Configuration;
using Reloaded.Mod.Interfaces.Structs;
using System.ComponentModel;
using CriFs.V2.Hook;
using CriFs.V2.Hook.Interfaces;
using System.Reflection;
using System.ComponentModel.DataAnnotations;

namespace ProtagSumiAddonConfig.Configuration
{
	public class Config : Configurable<Config>
	{
        /*
            User Properties:
                - Please put all of your configurable properties here.

            By default, configuration saves as "Config.json" in mod user config folder.    
            Need more config files/classes? See Configuration.cs

            Available Attributes:
            - Category
            - DisplayName
            - Description
            - DefaultValue

            // Technically Supported but not Useful
            - Browsable
            - Localizable

            The `DefaultValue` attribute is used as part of the `Reset` button in Reloaded-Launcher.
        */

        public enum EquipmentPatch
        {
            [Display(Name = "Disabled")]
            Disabled,

            [Display(Name = "Gun Overhaul")]
            GunOverhaul,

            [Display(Name = "Extra Rounds")]
            ExtraRounds,
        }

        public enum LoadingWipeAltEnum
        {
            Disabled,
            Pink,
            Gray,
        }

        public enum WeaponsModels
        {
            [Display(Name = "Default")]
            Default,

            [Display(Name = "Expanded")]
            Expanded,

            [Display(Name = "Expanded (No Lances)")]
            Expanded_No_Lances,
        }

        public enum SumireOverhaulEnum
        {
            [Display(Name = "Disabled")]
            Disabled,

            [Display(Name = "Models Only")]
            Models_Only,

            [Display(Name = "Models and Animations")]
            Models_and_Animations,
        }

        [Category("Model")]
        [DisplayName("Sumire Overhaul")]
        [Description("Replaces all models with Sumire's glasses and hair down models (expect bugs!)")]
        [DefaultValue(SumireOverhaulEnum.Disabled)]
        [Display(Order = 1)]
        public SumireOverhaulEnum SumireOverhaul { get; set; }

        [Category("Model")]
        [DisplayName("Expanded Weapon Models")]
        [Description("Choose between the default weapon models or an expanded selection of weapon models by JustAdam.")]
        [DefaultValue(WeaponsModels.Default)]
        [Display(Order = 2)]
        public WeaponsModels WeaponModelsConfigEnum { get; set; }

        [Category("Costumes")]
        [DisplayName("Custom Bonus Tweaks Alts")]
        [Description("Adds Sumi's hair down alt outfits from Custom Bonus Tweaks.")]
        [DefaultValue(true)]
        [Display(Order = 3)]
        public bool CBTAlts { get; set; } = true;

        [Category("Costumes")]
        [DisplayName("Misc Costumes")]
        [Description("Adds random misc costumes.")]
        [DefaultValue(false)]
        [Display(Order = 4)]
        public bool MiscCostumesSumi { get; set; } = false;

        [Category("Gameplay")]
        [DisplayName("Equipment Patch - Variants")]
        [Description("Variants of Equipment Patch. Check README in mod directory.")]
        [DefaultValue(EquipmentPatch.Disabled)]
        [Display(Order = 5)]
        public EquipmentPatch EquipmentPatchAddon { get; set; }

        [Category("UI")]
        [DisplayName("Alternate Loading Silhouette")]
        [Description("Adds an alternate loading screen icon based on Sumi's gymnast performance.")]
        [DefaultValue(LoadingWipeAltEnum.Disabled)]
        [Display(Order = 6)]
        public LoadingWipeAltEnum LoadingWipeAlt { get; set; }

        [Category("UI")]
        [DisplayName("2016 Beta Icons")]
        [Description("Replaces the icons from the main mod with Morty's 2016 beta icons.")]
        [DefaultValue(false)]
        [Display(Order = 7)]
        public bool MortyBeta { get; set; } = false;

        [Category("UI")]
        [DisplayName("Colorful AoA Portrait")]
        [Description("Adds a colorful AoA Portrait, recreation is based off of Haalyle's mod.")]
        [DefaultValue(false)]
        [Display(Order = 8)]
        public bool AoAColor { get; set; } = false;
    }

    /// <summary>
    /// Allows you to override certain aspects of the configuration creation process (e.g. create multiple configurations).
    /// Override elements in <see cref="ConfiguratorMixinBase"/> for finer control.
    /// </summary>
	public class ConfiguratorMixin : ConfiguratorMixinBase
	{
		// 
	}
}