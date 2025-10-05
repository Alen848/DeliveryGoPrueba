using System;


public class MpSdkFalsa
{
    public bool Cobrar(decimal monto)
    {
        Console.WriteLine($"[SDK MercadoPago] Cobrando ${monto}");
        return true;
    }
}