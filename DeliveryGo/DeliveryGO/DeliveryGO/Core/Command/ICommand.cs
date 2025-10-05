using System;

namespace DeliveryGO.Core.Command;

public interface ICommand
{
    void Execute();
    void Undo();
}