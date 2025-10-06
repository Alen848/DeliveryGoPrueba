using System;

using DeliveryGO.Core.Command;
using DeliveryGO.Core.Order;

namespace DeliveryGo.Core.Order;

public class Pedido
{
    public int Id { get; set; }
    public List<Item> Items { get; set; } = new();
    public string Direccion { get; set; } = string.Empty;
    public string TipoPago { get; set; } = string.Empty;
    public EstadoPedido Estado { get; set; }
    public decimal Monto { get; set; }
}

