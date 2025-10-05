using System;
using DeliveryGo.Envios;
namespace DeliveryGo.Envios;

public class EnvioService
{
    private IEnvioStrategy _actual;

    public void SetStrategy(IEnvioStrategy strategy)
    {
        _actual = strategy;
    }

    public decimal Calcular(decimal subtotal)
    {
        if (_actual == null)
            throw new InvalidOperationException("No se ha definido una estrategia de envío.");
        return _actual.Calcular(subtotal);
    }

    public string NombreActual => _actual?.Nombre;
}
