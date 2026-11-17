using System.Text;
using FederacionSantillan.Data.DAOs;
using FederacionSantillan.Model;
using FederacionSantillan.Model.Exceptions;

namespace FederacionSantillan.App.Controllers;

public class AlertsDashboardData
{
    public List<Installment> Overdue { get; set; } = new();
    public List<Installment> Upcoming { get; set; } = new();
    public decimal OverdueTotal { get; set; }
    public decimal UpcomingTotal { get; set; }
    public Dictionary<int, Installment> InstallmentIndex { get; set; } = new();
}

public class NotificationController
{
    private readonly InstallmentDao _installmentDao = new InstallmentDao();
    private readonly NotificationDao _notificationDao = new NotificationDao();

    public AlertsDashboardData LoadAlertsDashboard(int daysAhead = 15)
    {
        var overdue = _installmentDao.GetOverdue();
        var upcoming = _installmentDao.GetUpcoming(daysAhead);
        var index = new Dictionary<int, Installment>(overdue.Count + upcoming.Count);

        decimal overdueSum = 0m;
        for (int i = 0; i < overdue.Count; i++)
        {
            overdueSum += overdue[i].Amount;
            index[overdue[i].InstallmentId] = overdue[i];
        }

        decimal upcomingSum = 0m;
        for (int i = 0; i < upcoming.Count; i++)
        {
            upcomingSum += upcoming[i].Amount;
            index[upcoming[i].InstallmentId] = upcoming[i];
        }

        return new AlertsDashboardData
        {
            Overdue = overdue,
            Upcoming = upcoming,
            OverdueTotal = overdueSum,
            UpcomingTotal = upcomingSum,
            InstallmentIndex = index
        };
    }

    public string RegisterInstallmentPayment(int installmentId, string paymentMethod, string notes = "")
    {
        if (installmentId <= 0 || string.IsNullOrWhiteSpace(paymentMethod))
            throw new BusinessRuleException(SystemMessages.SelectionRequired);

        bool paid = _installmentDao.RegisterPayment(installmentId, paymentMethod, notes);
        if (!paid)
            throw new BusinessRuleException(SystemMessages.InstallmentAlreadyPaid);

        return "Pago registrado correctamente. La alerta fue saldada.";
    }

    public string LogAndBuildWhatsAppReminder(Installment installment)
    {
        if (installment == null)
            throw new BusinessRuleException(SystemMessages.SelectionRequired);

        string message = installment.buildWhatsAppReminderText();
        string alertType = installment.isOverdue() ? "OVERDUE" : "UPCOMING";
        var notification = new Notification(0, installment, alertType, message);
        _notificationDao.LogNotification(notification);
        return message;
    }

    public string ExportOverdueToCsv(IReadOnlyList<Installment> overdueList)
    {
        var sb = new StringBuilder(overdueList.Count * 96 + 80);
        sb.AppendLine("Poliza;Asegurado;Telefono;Ramo;Cuota;Monto;Vencimiento;DiasAtraso");
        for (int i = 0; i < overdueList.Count; i++)
        {
            var c = overdueList[i];
            sb.Append(c.PolicyNumberRef).Append(';')
              .Append('"').Append(c.InsuredNameRef?.Replace("\"", "'")).Append("\";")
              .Append(c.InsuredPhoneRef).Append(';')
              .Append(c.BranchNameRef).Append(';')
              .Append(c.InstallmentNumber).Append(';')
              .Append(c.Amount.ToString("F2")).Append(';')
              .Append(c.DueDate.ToString("dd/MM/yyyy")).Append(';')
              .AppendLine(c.getDaysOverdue().ToString());
        }
        return sb.ToString();
    }
}
