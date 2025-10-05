using System;

using DeliveryGO.Core.Order;

namespace DeliveryGO.Core.Order.Observers;

public class LogisticaObserver
{
    public void Suscribir(PedidoService servicio)
    {
        servicio.EstadoCambiado += OnEstadoCambiado;
        Console.WriteLine("[LogisticaObserver] Departamento de logística suscrito");
    }

    public void Desuscribir(PedidoService servicio)
    {
        servicio.EstadoCambiado -= OnEstadoCambiado;
        Console.WriteLine("[LogisticaObserver] Departamento de logística desuscrito");
    }

    private void OnEstadoCambiado(object? sender, PedidoChangedEventArgs e)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"[LOGÍSTICA] Pedido #{e.PedidoId} cambió a {e.NuevoEstado}. Actualizando tablero de control...");
        Console.ResetColor();
    }
}