using FederacionSantillan.App.Advice;
using FederacionSantillan.App.Controllers;
using FederacionSantillan.App.Helpers;

namespace FederacionSantillan.App.Views;

public partial class FormOpciones : Form
{
    private readonly SettingsController _settingsController = new();

    public FormOpciones()
    {
        InitializeComponent();
        Icon = IconHelper.CrearIconoEscudo();
    }

    private void FormOpciones_Load(object sender, EventArgs e)
    {
        cmbModoVisual.SelectedIndex = (int)AccessibilityTheme.CurrentMode;
        chkTextoAmpliado.Checked = AccessibilityTheme.LargeTextEnabled;
    }

    private void btnExportarDump_Click(object sender, EventArgs e)
    {
        using var sfd = new SaveFileDialog
        {
            Filter = "Script SQL (*.sql)|*.sql",
            FileName = $"santillan_seguros_dump_{DateTime.Now:yyyyMMdd_HHmm}.sql"
        };

        if (sfd.ShowDialog(this) != DialogResult.OK) return;

        ControllerPipeline.Execute(() =>
        {
            string sqlDump = _settingsController.GenerateDatabaseSqlDump();
            File.WriteAllText(sfd.FileName, sqlDump, System.Text.Encoding.UTF8);
            MessageBox.Show(
                $"DUMP SQL exportado exitosamente con integridad referencial:\n{sfd.FileName}",
                "Respaldo Exitoso",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        });
    }

    private void btnRestablecer_Click(object sender, EventArgs e)
    {
        cmbModoVisual.SelectedIndex = 0;
        chkTextoAmpliado.Checked = false;
    }

    private void btnAplicarCerrar_Click(object sender, EventArgs e)
    {
        AccessibilityTheme.CurrentMode = (AccessibilityMode)cmbModoVisual.SelectedIndex;
        AccessibilityTheme.LargeTextEnabled = chkTextoAmpliado.Checked;
        DialogResult = DialogResult.OK;
        Close();
    }
}
