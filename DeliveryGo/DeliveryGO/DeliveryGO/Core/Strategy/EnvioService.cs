namespace DeliveryGO.Core.Strategy;

public class EnvioService
{
    private IEnvioStrategy _actual;

    // Constructor corregido - inicializar _actual
    public EnvioService(IEnvioStrategy estrategiaInicial)
    {
        _actual = estrategiaInicial;
    }

    public void SetStrategy(IEnvioStrategy estrategia)
    {
        _actual = estrategia;
    }

    public decimal Calcular(decimal subtotal) => _actual?.Calcular(subtotal) ?? 0m;

    public string NombreActual => _actual?.Nombre ?? "No definido";
}