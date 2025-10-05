using System;

using DeliveryGO.Core.Order;

namespace DeliveryGO.Core.Order.Observers;

public class AuditoriaObserver
{
    public void Suscribir(PedidoService servicio)
    {
        servicio.EstadoCambiado += OnEstadoCambiado;
        Console.WriteLine("[AuditoriaObserver] Sistema de auditoría suscrito");
    }

    public void Desuscribir(PedidoService servicio)
    {
        servicio.EstadoCambiado -= OnEstadoCambiado;
        Console.WriteLine("[AuditoriaObserver] Sistema de auditoría desuscrito");
    }

    private void OnEstadoCambiado(object? sender, PedidoChangedEventArgs e)
    {
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine($"[AUDITORÍA] [{e.Cuando:yyyy-MM-dd HH:mm:ss}] Pedido #{e.PedidoId} -> {e.NuevoEstado}");
        Console.ResetColor();
    }
}