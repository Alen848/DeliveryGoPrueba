using System;

namespace DeliveryGO.Core.Command;

public class CarritoPort : ICarritoPort
{
    private readonly Carrito _carrito = new();
    private readonly EditorCarrito _editor = new();

    public decimal Subtotal() => _carrito.Subtotal();

    public void Run(ICommand cmd)
    {
        _editor.Run(cmd);
        // Sincronizar el estado del carrito interno con los commands
        // En una implementación real, los commands trabajarían directamente con este carrito
    }

    public void Undo() => _editor.Undo();

    public void Redo() => _editor.Redo();

    public List<Item> GetItemsSnapshot() => _carrito.GetItemsSnapshot();
}