using System;


public class PagoConCupon : IPago
{
    private readonly IPago _pago;
    private readonly decimal _porcentaje;

    public PagoConCupon(IPago pago, decimal porcentaje)
    {
        _pago = pago;
        _porcentaje = porcentaje;
    }

    public string Nombre => $"{_pago.Nombre} - Cupón {_porcentaje:P2}";

    public bool Procesar(decimal monto)
    {
        var total = monto * (1 - _porcentaje);
        Console.WriteLine($"Aplicando cupón ({_porcentaje:P2}): ${monto} -> ${total}");
        return _pago.Procesar(total);
    }
}