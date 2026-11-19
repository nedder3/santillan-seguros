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
            "GUÍA DE USO Y FUNCIONALIDADES\n" +
            "==================================================\n\n" +
            "1. NOTIFICACIONES Y VENCIMIENTOS (Pestaña Principal)\n" +
            "   • Cuotas Vencidas (Morosos): Listado de cuotas impagas con fecha de vencimiento superada.\n" +
            "   • Cuotas Por Vencer: Vencimientos próximos dentro de la ventana de días seleccionada.\n" +
            "   • Registrar Pago: Seleccione la cuota en la grilla, elija el medio de cobro y confirme la operación.\n" +
            "   • Aviso WhatsApp: Genera automáticamente el mensaje de recordatorio y lo copia al portapapeles.\n" +
            "   • Exportar CSV: Descarga el listado consolidado de morosos para auditoría o planilla de caja.\n\n" +
            "2. CARTERA DE ASEGURADOS\n" +
            "   • Tipo de Documento: Seleccione DNI o CUIT (valida dígito verificador oficial de Argentina).\n" +
            "   • Alta y Edición: Complete los datos requeridos y presione 'Guardar Asegurado'.\n" +
            "   • Búsqueda instantánea: Filtre la cartera en tiempo real escribiendo nombre o documento.\n\n" +
            "3. PÓLIZAS Y PLAN DE CUOTAS\n" +
            "   • Emisión: Asocie la póliza al asegurado y ramo oficial; el sistema calcula y genera las cuotas mensuales.\n" +
            "   • Consulta de Cronograma: Al seleccionar una póliza, se desglosan sus cuotas y estados de pago.\n\n" +
            "ATAJOS DE TECLADO\n" +
            "   • TAB / SHIFT+TAB: Desplazarse secuencialmente entre campos y botones.\n" +
            "   • ENTER: Ejecutar la acción principal del formulario activo.\n" +
            "   • ESC: Cerrar ventanas emergentes o de ayuda.\n\n" +
            "SOPORTE AL USUARIO\n" +
            "   • Sistema de Gestión Federación Santillán Seguros • Versión 2026\n" +
            "   • Ante consultas técnicas o dudas operativas, comuníquese con el administrador del sistema.";
    }
}
