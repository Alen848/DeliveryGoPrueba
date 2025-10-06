using DeliveryGo.Core.Order;
using DeliveryGO.Core.Command;

namespace DeliveryGO.Core.Order;

public interface IPedidoBuilder
{
    IPedidoBuilder ConItems(IEnumerable<(string sku, string nombre, decimal precio, int cantidad)> items);
    IPedidoBuilder ConDireccion(string direccion);
    IPedidoBuilder ConMetodoPago(string tipoPago);
    IPedidoBuilder ConMonto(decimal monto);
    DeliveryGo.Core.Order.Pedido Build();
}

public class PedidoBuilder : IPedidoBuilder
{
    private readonly List<(string sku, string nombre, decimal precio, int cantidad)> _items = new();
    private string _direccion = string.Empty;
    private string _tipoPago = string.Empty;
    private decimal _monto;
    private static int _nextId = 1;

    public IPedidoBuilder ConItems(IEnumerable<(string sku, string nombre, decimal precio, int cantidad)> items)
    {
        _items.Clear();
        _items.AddRange(items);
        return this;
    }

    public IPedidoBuilder ConDireccion(string direccion)
    {
        _direccion = direccion;
        return this;
    }

    public IPedidoBuilder ConMetodoPago(string tipoPago)
    {
        _tipoPago = tipoPago;
        return this;
    }

    public IPedidoBuilder ConMonto(decimal monto)
    {
        _monto = monto;
        return this;
    }

    public Pedido Build()
    {
        if (!_items.Any())
            throw new InvalidOperationException("El pedido debe tener al menos un ítem");
        
        if (string.IsNullOrWhiteSpace(_direccion))
            throw new InvalidOperationException("El pedido debe tener una dirección");

        // Crear los items del pedido
        var itemsDelPedido = _items.Select(static i => new Item
        { 
            Sku = i.sku, 
            Nombre = i.nombre, 
            Precio = i.precio, 
            Cantidad = i.cantidad 
        }).ToList();

        return new Pedido
        {
            Id = _nextId++,
            Items = itemsDelPedido,
            Direccion = _direccion,
            TipoPago = _tipoPago,
            Monto = _monto,
            Estado = EstadoPedido.Recibido
        };
    }
}