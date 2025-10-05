using System;
namespace DeliveryGo.Envios
{
    public interface IEnvioStrategy
    {
        decimal Calcular(decimal subtotal);
        string Nombre { get; }
    }
}
