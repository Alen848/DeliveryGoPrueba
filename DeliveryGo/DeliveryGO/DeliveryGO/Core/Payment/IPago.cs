using System;


public interface IPago
{
    string Nombre { get; }
    bool Procesar(decimal monto);
}