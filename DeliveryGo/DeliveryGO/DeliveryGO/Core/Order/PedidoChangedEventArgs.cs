using DeliveryGO.Core.Order;
using System;
namespace DeliveryGO.Core.Order;

public class PedidoChangedEventArgs : EventArgs
{
    public int PedidoId { get; }
    public EstadoPedido NuevoEstado { get; }
    public DateTime Cuando { get; }

    public PedidoChangedEventArgs(int id, EstadoPedido estado, DateTime cuando)
    {
        PedidoId = id;
        NuevoEstado = estado;
        Cuando = cuando;
    }
}