using System;


public static class PagoFactory
{
    public static IPago Create(string tipo)
    {
        return tipo.ToLower() switch
        {
            "tarjeta" => new PagoTarjeta(),
            "transf" => new PagoTransfer(),
            "mp" => new PagoMp(),
            _ => throw new ArgumentException($"Tipo de pago no soportado: {tipo}")
        };
    }
}