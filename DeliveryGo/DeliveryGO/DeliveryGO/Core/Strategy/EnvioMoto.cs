using System;
using DeliveryGo.Envios;

namespace Core.Strategy
{
    public class EnvioMoto : IEnvioStrategy
    {
        public string Nombre => "Moto";

        public decimal Calcular(decimal subtotal)
        {
            return 1220m;
        }
    }
}
