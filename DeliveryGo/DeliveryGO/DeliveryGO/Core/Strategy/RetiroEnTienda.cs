using System;
using DeliveryGo.Envios;
namespace DeliveryGo.Envios
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

