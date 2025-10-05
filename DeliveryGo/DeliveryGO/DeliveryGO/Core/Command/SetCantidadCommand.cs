using System;

namespace DeliveryGO.Core.Command;

public class SetCantidadCommand : ICommand
{
    private readonly Carrito _carrito;
    private readonly string _sku;
    private readonly int _nuevaCantidad;
    private int _anteriorCantidad;
    private bool _ejecutado = false;

    public SetCantidadCommand(Carrito carrito, string sku, int nuevaCantidad)
    {
        _carrito = carrito;
        _sku = sku;
        _nuevaCantidad = nuevaCantidad;
    }

    public void Execute()
    {
        var items = _carrito.GetItemsSnapshot();
        var item = items.FirstOrDefault(i => i.Sku == _sku);

        if (item != null)
        {
            _anteriorCantidad = item.Cantidad;
            _carrito.SetCantidad(_sku, _nuevaCantidad);
            _ejecutado = true;
        }
    }

    public void Undo()
    {
        if (_ejecutado)
        {
            _carrito.SetCantidad(_sku, _anteriorCantidad);
        }
    }
}