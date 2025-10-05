using System;

namespace DeliveryGO.Core.Order;

public class PedidoService
{
    public event EventHandler<PedidoChangedEventArgs>? EstadoCambiado;

    public void CambiarEstado(int pedidoId, EstadoPedido nuevoEstado)
    {
        Console.WriteLine($"\n[PedidoService] Cambiando estado del pedido #{pedidoId} a {nuevoEstado}...");

        // Simular algún procesamiento
        Thread.Sleep(300);

        // Disparar el evento
        EstadoCambiado?.Invoke(this, new PedidoChangedEventArgs(pedidoId, nuevoEstado, DateTime.Now));
    }
}
