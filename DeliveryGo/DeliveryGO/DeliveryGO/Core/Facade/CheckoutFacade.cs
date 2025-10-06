using DeliveryGo.Core.Order;
using DeliveryGO.Core.Command;
using DeliveryGO.Core.Order;
using DeliveryGO.Core.Strategy;

namespace DeliveryGO.Core.Facade;

public class CheckoutFacade
{
    private readonly ICarritoPort _carrito;
    private IEnvioStrategy _envioActual;
    private readonly PedidoService _pedidoService;

    public CheckoutFacade(ICarritoPort carrito, IEnvioStrategy envioInicial, PedidoService pedidoService)
    {
        _carrito = carrito;
        _envioActual = envioInicial;
        _pedidoService = pedidoService;
    }

    public void AgregarItem(string sku, string nombre, decimal precio, int cantidad)
    {
        // Obtener el carrito real del CarritoPort
        var carritoReal = (_carrito as CarritoPort)?.CarritoInterno;
        if (carritoReal == null) return;

        var item = new Item(sku, nombre, precio, cantidad);
        var command = new AgregarItemCommand(carritoReal, item);
        _carrito.Run(command);
    }

    public void CambiarCantidad(string sku, int cantidad)
    {
        var carritoReal = (_carrito as CarritoPort)?.CarritoInterno;
        if (carritoReal == null) return;

        var command = new SetCantidadCommand(carritoReal, sku, cantidad);
        _carrito.Run(command);
    }

    public void QuitarItem(string sku)
    {
        var carritoReal = (_carrito as CarritoPort)?.CarritoInterno;
        if (carritoReal == null) return;

        var command = new QuitarItemCommand(carritoReal, sku);
        _carrito.Run(command);
    }

    public void ElegirEnvio(IEnvioStrategy estrategia)
    {
        _envioActual = estrategia;
    }

    public decimal CalcularTotal()
    {
        var subtotal = _carrito.Subtotal();
        var costoEnvio = _envioActual.Calcular(subtotal);
        return subtotal + costoEnvio;
    }

    // ... (los demás métodos se mantienen igual)
    public bool Pagar(string tipoPago, bool aplicarIVA, decimal? cupon = null)
    {
        var monto = CalcularTotal();
        
        if (monto <= 0)
        {
            Console.WriteLine("Monto a pagar es 0, no se requiere pago");
            return true;
        }

        IPago pago;

        if (tipoPago == "mp-adapter")
        {
            pago = new PagoAdapterMp(new MpSdkFalsa());
        }
        else
        {
            pago = PagoFactory.Create(tipoPago);
        }

        if (aplicarIVA)
        {
            pago = new PagoConImpuesto(pago);
        }

        if (cupon.HasValue)
        {
            pago = new PagoConCupon(pago, cupon.Value);
        }

        Console.WriteLine($"Procesando pago con: {pago.Nombre}");
        return pago.Procesar(monto);
    }

    public Pedido ConfirmarPedido(string direccion, string tipoPago)
    {
        var items = _carrito.GetItemsSnapshot();
        
        if (!items.Any())
        {
            throw new InvalidOperationException("No se puede confirmar pedido: carrito vacío");
        }

        var builder = new PedidoBuilder();
        
        var pedido = builder
            .ConItems(items.Select(i => (i.Sku, i.Nombre, i.Precio, i.Cantidad)))
            .ConDireccion(direccion)
            .ConMetodoPago(tipoPago)
            .ConMonto(CalcularTotal())
            .Build();

        Console.WriteLine($"\nPedido #{pedido.Id} creado exitosamente:");
        Console.WriteLine($"- Ítems: {pedido.Items.Count}");
        Console.WriteLine($"- Total: ${pedido.Monto}");
        Console.WriteLine($"- Estado inicial: {pedido.Estado}");

        SimularWorkflowPedido(pedido.Id);

        return pedido;
    }

    private void SimularWorkflowPedido(int pedidoId)
    {
        Thread.Sleep(800);
        _pedidoService.CambiarEstado(pedidoId, DeliveryGO.Core.Order.EstadoPedido.Preparando);
        
        Thread.Sleep(1000);
        _pedidoService.CambiarEstado(pedidoId, DeliveryGO.Core.Order.EstadoPedido.Enviado);
        
        Thread.Sleep(1200);
        _pedidoService.CambiarEstado(pedidoId, DeliveryGO.Core.Order.EstadoPedido.Entregado);
    }
}