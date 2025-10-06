namespace DeliveryGO.Core.Command;

public class CarritoPort : ICarritoPort
{
    private readonly Carrito _carrito = new();
    private readonly EditorCarrito _editor = new();

    // Propiedad pública para acceder al carrito real
    public Carrito CarritoInterno => _carrito;

    public decimal Subtotal() => _carrito.Subtotal();

    public void Run(ICommand cmd) 
    { 
        _editor.Run(cmd);
        // Los commands ahora trabajan con este mismo carrito
    }

    public void Undo() => _editor.Undo();

    public void Redo() => _editor.Redo();

    public List<Item> GetItemsSnapshot() => _carrito.GetItemsSnapshot();
}