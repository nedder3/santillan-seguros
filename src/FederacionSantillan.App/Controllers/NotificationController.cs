using FederacionSantillan.Data.DAOs;
using FederacionSantillan.Model.DTOs.Requests;
using FederacionSantillan.Model.DTOs.Responses;
using FederacionSantillan.Model.Entities;

namespace FederacionSantillan.App.Controllers;

public class NotificationController(
    InstallmentDao installmentDao = null,
    NotificationDao notificationDao = null)
{
    private readonly InstallmentDao _installmentDao = installmentDao ?? new();
    private readonly NotificationDao _notificationDao = notificationDao ?? new();

    public AlertsDashboardResponseDto LoadAlertsDashboard(int daysAhead = 15) =>
        _installmentDao.LoadAlertsDashboard(daysAhead);

    public string RegisterInstallmentPayment(PaymentRequestDto dto) =>
        _installmentDao.RegisterPayment(dto);

    public string LogAndBuildWhatsAppReminder(Installment installment) =>
        _notificationDao.LogAndBuildWhatsAppReminder(installment);

    public string ExportOverdueToCsv(IReadOnlyList<Installment> overdueList) =>
        InstallmentDao.ExportToCsv(overdueList);
}
