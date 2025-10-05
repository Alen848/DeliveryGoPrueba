using System;


public class PagoMp : IPago
{
    public string Nombre => "Mercado Pago";

    public bool Procesar(decimal monto)
    {
        Console.WriteLine($"Procesando pago con Mercado Pago por ${monto}");
        return true;
    }
}