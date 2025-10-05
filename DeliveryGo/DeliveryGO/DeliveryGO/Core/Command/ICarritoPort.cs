using System;

namespace DeliveryGO.Core.Command;

public interface ICarritoPort
{
    decimal Subtotal();
    void Run(ICommand cmd);
    void Undo();
    void Redo();
    List<Item> GetItemsSnapshot();
}
