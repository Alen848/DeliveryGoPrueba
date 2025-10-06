using System;

using DeliveryGO.Core.Singleton;


public class PagoConImpuesto : IPago
{
    private readonly IPago _pago;

    public PagoConImpuesto(IPago pago)
    {
        _pago = pago;
    }

    public string Nombre => $"{_pago.Nombre} + IVA";

    public bool Procesar(decimal monto)
    {
        var total = monto * (1 + ConfigManager.Instance.IVA);
        Console.WriteLine($"Aplicando IVA ({ConfigManager.Instance.IVA:P2}): ${monto} -> ${total}");
        return _pago.Procesar(total);
    }
}