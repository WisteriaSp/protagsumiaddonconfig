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
            Disabled,
            GunOverhaul,
            ExtraRounds,
        }

        public enum LoadingWipeAltEnum
        {
            Disabled,
            Pink,
            Gray,
        }

        [Category("Gameplay")]
        [DisplayName("Equipment Patch - Variants")]
        [Description("Variants of Equipment Patch. Check README in mod directory.")]
        [DefaultValue(EquipmentPatch.Disabled)]
        [Display(Order = 1)]
        public EquipmentPatch EquipmentPatchAddon { get; set; }

        [Category("UI")]
        [DisplayName("Alternate Loading Silhouette")]
        [Description("Adds an alternate loading screen icon based on Sumi's gymnast performance.")]
        [DefaultValue(LoadingWipeAltEnum.Disabled)]
        [Display(Order = 2)]
        public LoadingWipeAltEnum LoadingWipeAlt { get; set; }

        [Category("UI")]
        [DisplayName("2016 Beta Icons")]
        [Description("Replaces the icons from the main mod with Morty's 2016 beta icons.")]
        [DefaultValue(false)]
        [Display(Order = 3)]
        public bool MortyBeta { get; set; } = false;

        [Category("UI")]
        [DisplayName("Colorful AoA Portrait")]
        [Description("Adds a colorful AoA Portrait, recreation is based off of Haalyle's mod.")]
        [DefaultValue(false)]
        [Display(Order = 4)]
        public bool AoAColor { get; set; } = false;


        /*
                [Category("Costumes")]
                [DisplayName("Custom Bonus Tweaks Alts")]
                [Description("Adds Sumi's hair down alt outfits from Custom Bonus Tweaks.")]
                [DefaultValue(true)]
                [Display(Order = 1)]
                public bool CBTAlts { get; set; } = true;

                [Category("Costumes")]
                [DisplayName("Misc Costumes")]
                [Description("Adds random misc costumes.")]
                [DefaultValue(false)]
                [Display(Order = 2)]
                public bool MiscCostumesSumi { get; set; } = false;
         */
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