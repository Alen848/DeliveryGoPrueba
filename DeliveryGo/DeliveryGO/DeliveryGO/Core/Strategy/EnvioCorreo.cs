using System;
using DeliveryGo.Envios;
using DeliveryGo.Config;

namespace DeliveryGo.Envios
{
    public class EnvioCorreo : IEnvioStrategy
    {
        public string Nombre => "Correo";

        public decimal Calcular(decimal subtotal)
        {
            return subtotal >= ConfigManager.Instance.EnvioGratisDesde ? 0m : 3500m;
        }
    }
}

