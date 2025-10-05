using System;


public class PagoAdapterMp : IPago
{
    private readonly MpSdkFalsa _sdk;

    public PagoAdapterMp(MpSdkFalsa sdk)
    {
        _sdk = sdk;
    }

    public string Nombre => "Mercado Pago (Adapter)";

    public bool Procesar(decimal monto)
    {
        return _sdk.Cobrar(monto);
    }
}