using FederacionSantillan.Model.Abstractions;

namespace FederacionSantillan.Model;

public class Notification : ICopiable<Notification>, IClonable<Notification>, IEquatableDomain<Notification>, IIdentifiable<int>
{
    private int notificationId;
    private Installment installment;
    private string alertType, message;
    private DateTime generatedAt;
    private bool sent;

    public int Id => notificationId;

    public Notification(int id, Installment installment, string alertType, string message)
    {
        notificationId = id;
        this.installment = installment;
        this.alertType = alertType;
        this.message = message;
        generatedAt = DateTime.Now;
        sent = true;
    }

    public int NotificationId { get => notificationId; set => notificationId = value; }
    public Installment Installment { get => installment; set => installment = value; }
    public string AlertType { get => alertType; set => alertType = value; }
    public string Message { get => message; set => message = value; }
    public DateTime GeneratedAt { get => generatedAt; set => generatedAt = value; }
    public bool Sent { get => sent; set => sent = value; }

    public void copy(Notification other)
    {
        if (other == null) return;
        notificationId = other.notificationId;
        alertType = other.alertType;
        message = other.message;
        generatedAt = other.generatedAt;
        sent = other.sent;
        if (installment != null && other.installment != null) installment.copy(other.installment);
    }

    public Notification clone() =>
        new Notification(notificationId, installment != null ? installment.clone() : null, alertType, message)
        {
            generatedAt = generatedAt,
            sent = sent
        };

    public bool equals(Notification other) =>
        other != null &&
        notificationId == other.notificationId &&
        alertType == other.alertType &&
        (installment == null ? other.installment == null : installment.equals(other.installment));

    public override string ToString() => $"[{alertType}] {message}";
}
