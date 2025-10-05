using System;

public class Item
{
    public string Sku { get; }
    public string Nombre { get; }
    public decimal Precio { get; }
    public int Cantidad { get; set; }

    public Item(string sku, string nombre, decimal precio, int cantidad = 1)
    {
        Sku = sku;
        Nombre = nombre;
        Precio = precio;
        Cantidad = cantidad;
    }
}

