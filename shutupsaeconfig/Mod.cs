using shutupsae.Configuration;
using shutupsae.Template;
using Reloaded.Hooks.ReloadedII.Interfaces;
using Reloaded.Mod.Interfaces;
using CriFs.V2.Hook.Interfaces;
using CriExtensions;

namespace shutupsae
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

            if (criFsCtl == null || !criFsCtl.TryGetTarget(out var criFsApi)) { _logger.WriteLine("CRI FS missing → cpk and binds broken.", System.Drawing.Color.Red); return; }


            var active = _modLoader.GetActiveMods().Select(x => x.Generic.ModId).ToHashSet();
            // check for rose and violet
            bool isRoseViolet = active.Contains("p5rpc.kasumi.roseandviolet");

            if (isRoseViolet)
            {
                _logger.WriteLine("Shut Up Sae: Rose & Violet detected, enabling compatibility", System.Drawing.Color.Purple);
                BindAllFilesIn(Path.Combine("RoseViolet"), modDir, criFsApi, modId);
            }

            // Legacy
            if (!_configuration.LegacyShutUp)
                criFsApi.AddProbingPath(Path.Combine(modDir, "LegacyToggle"));
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
