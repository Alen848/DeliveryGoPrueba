namespace DeliveryGO.Core.Command;

public class Item
{
    public string Sku { get; set; } = "";           
    public string Nombre { get; set; } = "";        
    public decimal Precio { get; set; }             
    public int Cantidad { get; set; }

    
    public Item() { }

    public Item(string sku, string nombre, decimal precio, int cantidad)
    {
        Sku = sku;
        Nombre = nombre;
        Precio = precio;
        Cantidad = cantidad;
    }
}