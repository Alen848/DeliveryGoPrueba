using System;


public class PagoTransfer : IPago
{
    public string Nombre => "Transferencia Bancaria";

    public bool Procesar(decimal monto)
    {
        Console.WriteLine($"Procesando transferencia por ${monto}");
        return true;
    }
}