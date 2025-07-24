using RVAltModels.Template.Configuration;
using Reloaded.Mod.Interfaces.Structs;
using System.ComponentModel;
using CriFs.V2.Hook;
using CriFs.V2.Hook.Interfaces;
using System.Reflection;
using System.ComponentModel.DataAnnotations;

namespace RVAltModels.Configuration
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
        public enum PhantomSuit
        {
            [Display(Name = "Default")]
            Default,

            [Display(Name = "Pure White")]
            PureWhite,

            [Display(Name = "Red and White")]
            RedGold,
        }

        public enum BustupRV
        {
            [Display(Name = "Default")]
            Default,

            [Display(Name = "L7M3")]
            L7M3RV,
        }


        [Category("Bustup")]
        [DisplayName("Dialogue Portrait")]
        [Description("Select your preferred dialogue bustups.")]
        [DefaultValue(BustupRV.Default)]
        [Display(Order = 0)]
        public BustupRV BustupValue { get; set; }

        [Category("Model")]
        [DisplayName("Black Leotard Overhaul")]
        [Description("Replaces the Black Leotard phantom suit with a recoloured Leotard.")]
        [DefaultValue(PhantomSuit.Default)]
        [Display(Order = 1)]
        public PhantomSuit PhantomSuitValue { get; set; }

        [Category("Model")]
        [DisplayName("Golden Rapiers")]
        [Description("For usage with the Phantom Suit Overhaul, disable if you want regular Rapiers or other weapon model mods to work.")]
        [DefaultValue(false)]
        [Display(Order = 2)]
        public bool GoldRapiers { get; set; } = false;

        [Category("Model")]
        [DisplayName("Blue Dress over Winter Casual (R&V)")]
        [Description("Replaces the player's winter casual outfit with the blue dress from Sumire's SL.")]
        [DefaultValue(false)]
        [Display(Order = 3)]
        public bool BlueDressRV { get; set; } = false;
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