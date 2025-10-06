using DeliveryGO.Core.Command;
using DeliveryGO.Core.Strategy;
using DeliveryGO.Core.Order;
using DeliveryGO.Core.Order.Observers;
using DeliveryGO.Core.Facade;
using DeliveryGO.Core.Singleton;

class Program
{
    private static ICarritoPort _carrito;
    private static CheckoutFacade _facade;
    private static PedidoService _pedidoService;
    private static LogisticaObserver _logisticaObserver;
    private static bool _logisticaSuscripta = true;

    static void Main(string[] args)
    {
        InicializarSistema();
        PruebaRapida();
        MostrarMenuPrincipal();
    }

    static void InicializarSistema()
    {
        Console.WriteLine("=== INICIALIZANDO DELIVERYGO ===");

        // Configuración
        ConfigManager.Instance.EnvioGratisDesde = 50000m;
        ConfigManager.Instance.IVA = 0.21m;

        // Servicios principales
        _carrito = new CarritoPort();
        _pedidoService = new PedidoService();

        // Observadores
        var clienteObserver = new ClienteObserver("Cliente Demo");
        _logisticaObserver = new LogisticaObserver();
        var auditoriaObserver = new AuditoriaObserver();

        clienteObserver.Suscribir(_pedidoService);
        _logisticaObserver.Suscribir(_pedidoService);
        auditoriaObserver.Suscribir(_pedidoService);

        // Facade con estrategia inicial
        _facade = new CheckoutFacade(_carrito, new EnvioMoto(), _pedidoService);

        Console.WriteLine("Sistema inicializado correctamente.");
        Console.WriteLine($"Envío gratis desde: ${ConfigManager.Instance.EnvioGratisDesde}");
        Console.WriteLine($"IVA: {ConfigManager.Instance.IVA:P2}");
        Console.WriteLine();
    }

    static void PruebaRapida()
{
    Console.WriteLine("\n=== PRUEBA RÁPIDA DEL CARRITO ===");
    
    // Probar agregar item directamente
    Console.WriteLine("Agregando ítem de prueba...");
    _facade.AgregarItem("TEST001", "Producto Prueba", 1000m, 2);
    
    // Verificar inmediatamente
    var items = _carrito.GetItemsSnapshot();
    Console.WriteLine($"Ítems en carrito: {items.Count}");
    
    foreach (var item in items)
    {
        Console.WriteLine($" - {item.Nombre}: {item.Cantidad} x ${item.Precio}");
    }
    
    Console.WriteLine($"Subtotal: ${_carrito.Subtotal()}");
    Console.WriteLine("=== FIN PRUEBA ===\n");
}

    static void MostrarMenuPrincipal()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("=== DELIVERYGO - MENÚ PRINCIPAL ===");
            Console.WriteLine("1. Agregar ítem al carrito");
            Console.WriteLine("2. Cambiar cantidad de ítem");
            Console.WriteLine("3. Quitar ítem del carrito");
            Console.WriteLine("4. Ver resumen de compra");
            Console.WriteLine("5. Deshacer (Undo)");
            Console.WriteLine("6. Rehacer (Redo)");
            Console.WriteLine("7. Elegir método de envío");
            Console.WriteLine("8. Realizar pago");
            Console.WriteLine("9. Confirmar pedido");
            Console.WriteLine("10. " + (_logisticaSuscripta ? "Desuscribir" : "Suscribir") + " Logística");
            Console.WriteLine("0. Salir");
            Console.WriteLine("-----------------------------------");

            var opcion = PedirEntero("Seleccione una opción: ");

            try
            {
                switch (opcion)
                {
                    case 1: AgregarItem(); break;
                    case 2: CambiarCantidad(); break;
                    case 3: QuitarItem(); break;
                    case 4: MostrarResumen(); break;
                    case 5: Undo(); break;
                    case 6: Redo(); break;
                    case 7: ElegirEnvio(); break;
                    case 8: RealizarPago(); break;
                    case 9: ConfirmarPedido(); break;
                    case 10: ToggleLogistica(); break;
                    case 0: return;
                    default: MostrarError("Opción no válida"); break;
                }
            }
            catch (Exception ex)
            {
                MostrarError($"Error: {ex.Message}");
            }

            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }
    }

    static void AgregarItem()
    {
        Console.WriteLine("\n--- AGREGAR ÍTEM ---");
        var sku = PedirTexto("SKU: ");
        var nombre = PedirTexto("Nombre: ");
        var precio = PedirDecimal("Precio: ");
        var cantidad = PedirEntero("Cantidad: ");

        if (precio <= 0 || cantidad <= 0)
        {
            MostrarError("Precio y cantidad deben ser mayores a 0");
            return;
        }

        _facade.AgregarItem(sku, nombre, precio, cantidad);
        MostrarExito($"Ítem '{nombre}' agregado correctamente");
    }

    static void CambiarCantidad()
    {
        Console.WriteLine("\n--- CAMBIAR CANTIDAD ---");
        var sku = PedirTexto("SKU del ítem: ");
        var nuevaCantidad = PedirEntero("Nueva cantidad: ");

        if (nuevaCantidad <= 0)
        {
            MostrarError("La cantidad debe ser mayor a 0");
            return;
        }

        _facade.CambiarCantidad(sku, nuevaCantidad);
        MostrarExito($"Cantidad del ítem {sku} cambiada a {nuevaCantidad}");
    }

    static void QuitarItem()
    {
        Console.WriteLine("\n--- QUITAR ÍTEM ---");
        var sku = PedirTexto("SKU del ítem a quitar: ");

        _facade.QuitarItem(sku);
        MostrarExito($"Ítem {sku} quitado del carrito");
    }

    static void MostrarResumen()
{
    Console.WriteLine("\n--- RESUMEN DE COMPRA ---");
    
    var items = _carrito.GetItemsSnapshot();
    if (!items.Any())
    {
        Console.WriteLine("Carrito vacío");
        return;
    }

    Console.WriteLine("Ítems en carrito:");
    foreach (var item in items)
    {
        Console.WriteLine($"  {item.Nombre} (x{item.Cantidad}) - ${item.Precio * item.Cantidad}");
    }

    var subtotal = _carrito.Subtotal();
    var total = _facade.CalcularTotal();
    var costoEnvio = total - subtotal;

    Console.WriteLine($"\nSubtotal: ${subtotal}");
    Console.WriteLine($"Costo de envío: ${costoEnvio}");
    Console.WriteLine($"TOTAL: ${total}");
}
    static void Undo()
    {
        _carrito.Undo();
        MostrarExito("Operación deshecha (Undo)");
    }

    static void Redo()
    {
        _carrito.Redo();
        MostrarExito("Operación rehecha (Redo)");
    }

    static void ElegirEnvio()
    {
        Console.WriteLine("\n--- ELEGIR MÉTODO DE ENVÍO ---");
        Console.WriteLine("1. Moto ($1280)");
        Console.WriteLine("2. Correo ($3580 - Gratis desde $50000)");
        Console.WriteLine("3. Retiro en tienda (Gratis)");

        var opcion = PedirEntero("Seleccione método: ");

        IEnvioStrategy estrategia = opcion switch
        {
            1 => new EnvioMoto(),
            2 => new EnvioCorreo(),
            3 => new RetiroEnTienda(),
            _ => throw new ArgumentException("Opción no válida")
        };

        _facade.ElegirEnvio(estrategia);
        MostrarExito($"Método de envío cambiado a: {estrategia.Nombre}");
    }

    static void RealizarPago()
    {
        Console.WriteLine("\n--- REALIZAR PAGO ---");
        Console.WriteLine("Métodos de pago disponibles:");
        Console.WriteLine("1. Tarjeta");
        Console.WriteLine("2. Transferencia");
        Console.WriteLine("3. Mercado Pago");
        Console.WriteLine("4. Mercado Pago (Adapter)");

        var opcionPago = PedirEntero("Seleccione método de pago: ");
        var tipoPago = opcionPago switch
        {
            1 => "tarjeta",
            2 => "transf",
            3 => "mp",
            4 => "mp-adapter",
            _ => throw new ArgumentException("Método de pago no válido")
        };

        var aplicarIVA = PedirSiNo("¿Aplicar IVA? (s/n): ");
        var aplicarCupon = PedirSiNo("¿Aplicar cupón de descuento? (s/n): ");
        decimal? cupon = null;

        if (aplicarCupon)
        {
            cupon = PedirDecimal("Porcentaje de descuento (ej: 0.10 para 10%): ");
            if (cupon <= 0 || cupon >= 1)
            {
                MostrarError("El cupón debe estar entre 0 y 1 (ej: 0.10 para 10%)");
                return;
            }
        }

        var exito = _facade.Pagar(tipoPago, aplicarIVA, cupon);

        if (exito)
        {
            MostrarExito("¡Pago aprobado exitosamente!");
        }
        else
        {
            MostrarError("El pago fue rechazado");
        }
    }

    static void ConfirmarPedido()
    {
        Console.WriteLine("\n--- CONFIRMAR PEDIDO ---");

        var items = _carrito.GetItemsSnapshot();
        if (!items.Any())
        {
            MostrarError("No se puede confirmar pedido: carrito vacío");
            return;
        }

        var direccion = PedirTexto("Dirección de entrega: ");
        var tipoPagoRegistro = PedirTexto("Tipo de pago a registrar: ");

        Console.WriteLine("Confirmando pedido...");
        var pedido = _facade.ConfirmarPedido(direccion, tipoPagoRegistro);

        MostrarExito($"Pedido #{pedido.Id} confirmado exitosamente!");
        Console.WriteLine($"Estado final: {pedido.Estado}");
        Console.WriteLine($"Dirección: {pedido.Direccion}");
        Console.WriteLine($"Total: ${pedido.Monto}");
    }

    static void ToggleLogistica()
    {
        if (_logisticaSuscripta)
        {
            _logisticaObserver.Desuscribir(_pedidoService);
            _logisticaSuscripta = false;
            MostrarExito("Logística desuscrita - no recibirá más notificaciones");
        }
        else
        {
            _logisticaObserver.Suscribir(_pedidoService);
            _logisticaSuscripta = true;
            MostrarExito("Logística suscrita - recibirá notificaciones");
        }
    }

    #region Helpers de Entrada
    static string PedirTexto(string mensaje)
    {
        Console.Write(mensaje);
        return Console.ReadLine() ?? "";
    }

    static int PedirEntero(string mensaje)
    {
        while (true)
        {
            Console.Write(mensaje);
            if (int.TryParse(Console.ReadLine(), out int resultado) && resultado >= 0)
                return resultado;
            Console.WriteLine("Por favor ingrese un número entero válido.");
        }
    }

    static decimal PedirDecimal(string mensaje)
    {
        while (true)
        {
            Console.Write(mensaje);
            if (decimal.TryParse(Console.ReadLine(), out decimal resultado) && resultado >= 0)
                return resultado;
            Console.WriteLine("Por favor ingrese un número decimal válido.");
        }
    }

    static bool PedirSiNo(string mensaje)
    {
        while (true)
        {
            Console.Write(mensaje);
            var respuesta = Console.ReadLine()?.ToLower();
            if (respuesta == "s" || respuesta == "si") return true;
            if (respuesta == "n" || respuesta == "no") return false;
            Console.WriteLine("Por favor ingrese 's' para sí o 'n' para no.");
        }
    }

    static void MostrarExito(string mensaje)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"✓ {mensaje}");
        Console.ResetColor();
    }

    static void MostrarError(string mensaje)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"✗ {mensaje}");
        Console.ResetColor();
    }
    #endregion
}

