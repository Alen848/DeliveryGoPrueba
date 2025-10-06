using System;
namespace DeliveryGO.Core.Singleton

{
    public sealed class ConfigManager
    {
        private static readonly Lazy<ConfigManager> _instance = new Lazy<ConfigManager>(() => new ConfigManager());

        public static ConfigManager Instance => _instance.Value;

        private ConfigManager() { }

        // Umbral para envío gratis
        public decimal EnvioGratisDesde { get; set; }
        public decimal IVA { get; internal set; }
    }
}

