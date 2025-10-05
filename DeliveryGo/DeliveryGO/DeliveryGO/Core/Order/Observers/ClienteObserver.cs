using System;

using DeliveryGO.Core.Order;

namespace DeliveryGO.Core.Order.Observers;

public class ClienteObserver
{
    private readonly string _nombre;

    public ClienteObserver(string nombre)
    {
        _nombre = nombre;
    }

    public void Suscribir(PedidoService servicio)
    {
        servicio.EstadoCambiado += OnEstadoCambiado;
        Console.WriteLine($"[ClienteObserver] {_nombre} suscrito a notificaciones");
    }

    public void Desuscribir(PedidoService servicio)
    {
        servicio.EstadoCambiado -= OnEstadoCambiado;
        Console.WriteLine($"[ClienteObserver] {_nombre} desuscrito de notificaciones");
    }

    private void OnEstadoCambiado(object? sender, PedidoChangedEventArgs e)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine($"[CLIENTE {_nombre}] ¡Tu pedido #{e.PedidoId} ahora está {e.NuevoEstado}! ({e.Cuando:HH:mm:ss})");
        Console.ResetColor();
    }
}