using FederacionSantillan.Model.Interfaces;

namespace FederacionSantillan.Model.Entities;

public class Notification(int id, Installment installment, string alertType, string message)
    : ICopiable<Notification>, IClonable<Notification>, IEquatableDomain<Notification>, IIdentifiable<int>
{
    private int notificationId = id;
    private Installment installment = installment;
    private string alertType = alertType, message = message;
    private DateTime generatedAt = DateTime.Now;
    private bool sent = true;

    public int Id => notificationId;
    public int NotificationId { get => notificationId; set => notificationId = value; }
    public Installment Installment { get => installment; set => installment = value; }
    public string AlertType { get => alertType; set => alertType = value; }
    public string Message { get => message; set => message = value; }
    public DateTime GeneratedAt { get => generatedAt; set => generatedAt = value; }
    public bool Sent { get => sent; set => sent = value; }

    public void copy(Notification other)
    {
        if (other == null) return;
        (notificationId, alertType, message, generatedAt, sent) =
            (other.notificationId, other.alertType, other.message, other.generatedAt, other.sent);
        if (installment != null && other.installment != null) installment.copy(other.installment);
    }

    public Notification clone() =>
        new(notificationId, installment?.clone(), alertType, message)
        {
            generatedAt = generatedAt,
            sent = sent
        };

    public bool equals(Notification other) =>
        other != null &&
        (notificationId, alertType) == (other.notificationId, other.alertType) &&
        (installment == null ? other.installment == null : installment.equals(other.installment));

    public override string ToString() => $"[{alertType}] {message}";
}
