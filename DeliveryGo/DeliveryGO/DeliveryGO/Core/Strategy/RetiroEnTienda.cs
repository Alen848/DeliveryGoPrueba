using System;

namespace DeliveryGO.Core.Strategy
{
    public class RetiroEnTienda : IEnvioStrategy
    {
        public string Nombre => "Retiro";

        public decimal Calcular(decimal subtotal)
        {
            return 0m;
        }
    }
}

