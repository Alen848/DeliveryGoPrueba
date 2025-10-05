using System;


public class PagoTarjeta : IPago
{
    public string Nombre => "Tarjeta de Crédito/Débito";

    public bool Procesar(decimal monto)
    {
        Console.WriteLine($"Procesando pago con tarjeta por ${monto}");
        return true;
    }
}
