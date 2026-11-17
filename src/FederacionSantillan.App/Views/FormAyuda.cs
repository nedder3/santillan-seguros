using FederacionSantillan.App.Helpers;

namespace FederacionSantillan.App.Views;

public partial class FormAyuda : Form
{
    public FormAyuda()
    {
        InitializeComponent();
        Icon = IconHelper.CrearIconoEscudo();
    }

    private void FormAyuda_Load(object sender, EventArgs e)
    {
        rtbContenido.Text =
            "GUÍA RÁPIDA DE FUNCIONALIDADES\n" +
            "==================================================\n\n" +
            "1. NOTIFICACIONES Y VENCIMIENTOS (Pestaña Principal)\n" +
            "   • Cuotas Vencidas (Morosos): Muestra cuotas impagas cuya fecha de vencimiento ya pasó.\n" +
            "   • Cuotas Por Vencer (15 días): Muestra vencimientos próximos para enviar recordatorio al asegurado.\n" +
            "   • Registrar Pago: Seleccione una fila, elija el medio de pago y confirme el cobro.\n\n" +
            "2. CARTERA DE ASEGURADOS\n" +
            "   • Alta y Edición: Complete DNI/CUIT, Nombre y Teléfono (obligatorios) y presione Guardar.\n" +
            "   • Búsqueda en vivo: Escriba en el filtro superior por nombre o documento.\n\n" +
            "3. PÓLIZAS Y PLAN DE CUOTAS\n" +
            "   • Al emitir una póliza, el sistema genera automáticamente sus N cuotas mensuales en MariaDB.\n" +
            "   • Haga clic sobre cualquier póliza de la lista izquierda para ver el detalle de sus cuotas.\n\n" +
            "ATAJOS DE TECLADO\n" +
            "   • TAB / SHIFT+TAB: Navegar entre campos en orden secuencial.\n" +
            "   • ENTER: Confirmar acción principal.\n" +
            "   • ESC: Cancelar / Cerrar ventana auxiliar.\n\n" +
            "SOPORTE Y ARQUITECTURA\n" +
            "   • Motor de Base de Datos: MariaDB (3FN) • Arquitectura: MVC en 3 Capas.";
    }
}
