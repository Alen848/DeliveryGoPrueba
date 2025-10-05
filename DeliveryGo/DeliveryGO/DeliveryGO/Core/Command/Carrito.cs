using System;

namespace DeliveryGO.Core.Command;

public class Carrito
{
    private readonly Dictionary<string, Item> _items = new();

    public void Agregar(Item item)
    {
        if (_items.ContainsKey(item.Sku))
        {
            _items[item.Sku].Cantidad += item.Cantidad;
        }
        else
        {
            _items[item.Sku] = item;
        }
    }

    public Item? Quitar(string sku)
    {
        if (_items.ContainsKey(sku))
        {
            var item = _items[sku];
            _items.Remove(sku);
            return item;
        }
        return null;
    }

    public bool SetCantidad(string sku, int nuevaCantidad)
    {
        if (_items.ContainsKey(sku) && nuevaCantidad > 0)
        {
            _items[sku].Cantidad = nuevaCantidad;
            return true;
        }
        return false;
    }

    public decimal Subtotal()
    {
        return _items.Values.Sum(item => item.Precio * item.Cantidad);
    }

    public List<Item> GetItemsSnapshot()
    {
        // Devolver una copia para evitar modificaciones externas
        return _items.Values.Select(item => new Item
        {
            Sku = item.Sku,
            Nombre = item.Nombre,
            Precio = item.Precio,
            Cantidad = item.Cantidad
        }).ToList();
    }
}

