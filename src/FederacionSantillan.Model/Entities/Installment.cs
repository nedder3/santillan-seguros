using FederacionSantillan.Model.Interfaces;

namespace FederacionSantillan.Model.Entities;

public class Installment(int id, int policyId, int number, decimal amount, DateTime dueDate, string status)
    : ICopiable<Installment>, IClonable<Installment>, IEquatableDomain<Installment>, IIdentifiable<int>
{
    private int installmentId = id, policyId = policyId, installmentNumber = number;
    private decimal amount = amount;
    private DateTime dueDate = dueDate;
    private string status = status, paymentMethod, notes = string.Empty;
    private DateTime? paymentDate;

    public int Id => installmentId;
    public int InstallmentId { get => installmentId; set => installmentId = value; }
    public int PolicyId { get => policyId; set => policyId = value; }
    public int InstallmentNumber { get => installmentNumber; set => installmentNumber = value; }
    public decimal Amount { get => amount; set => amount = value; }
    public DateTime DueDate { get => dueDate; set => dueDate = value; }
    public string Status { get => status; set => status = value; }
    public DateTime? PaymentDate { get => paymentDate; set => paymentDate = value; }
    public string PaymentMethod { get => paymentMethod; set => paymentMethod = value; }
    public string Notes { get => notes; set => notes = value; }

    public int PolicyNumberRef { get; set; }
    public string InsuredNameRef { get; set; }
    public string InsuredPhoneRef { get; set; }
    public string BranchNameRef { get; set; }

    public int getInstallmentId() => installmentId;
    public void setInstallmentId(int id) => installmentId = id;
    public int getInstallmentNumber() => installmentNumber;
    public void setInstallmentNumber(int n) => installmentNumber = n;
    public decimal getAmount() => amount;
    public void setAmount(decimal a) => amount = a;
    public string getStatus() => status;
    public void setStatus(string s) => status = s;

    public bool isOverdue() => status != "PAID" && dueDate.Date < DateTime.Today;

    public bool isUpcoming(int daysAhead) =>
        status == "PENDING" &&
        dueDate.Date >= DateTime.Today &&
        dueDate.Date <= DateTime.Today.AddDays(daysAhead);

    public int getDaysOverdue() =>
        isOverdue() ? (DateTime.Today - dueDate.Date).Days : 0;

    public int getDaysUntilDue() =>
        (dueDate.Date - DateTime.Today).Days;

    public bool registerPayment(string method)
    {
        if (status == "PAID" || string.IsNullOrWhiteSpace(method)) return false;
        (status, paymentDate, paymentMethod) = ("PAID", DateTime.Now, method);
        return true;
    }

    public string buildWhatsAppReminderText()
    {
        string stateText = isOverdue() ? "VENCIDA" : "próxima a vencer";
        return $"Hola {InsuredNameRef}, te escribimos de Federación Santillán Seguros (Federación Patronal). " +
               $"Te recordamos que la cuota #{installmentNumber} de tu póliza #{PolicyNumberRef} ({BranchNameRef}) " +
               $"por {amount:C2} se encuentra {stateText} (Vto: {dueDate:dd/MM/yyyy}). ¡Saludos!";
    }

    public void copy(Installment other)
    {
        if (other == null) return;
        (installmentId, policyId, installmentNumber, amount, dueDate, status, paymentDate, paymentMethod, notes) =
            (other.installmentId, other.policyId, other.installmentNumber, other.amount, other.dueDate, other.status, other.paymentDate, other.paymentMethod, other.notes);
    }

    public Installment clone() =>
        new(installmentId, policyId, installmentNumber, amount, dueDate, status)
        {
            paymentDate = paymentDate,
            paymentMethod = paymentMethod,
            notes = notes
        };

    public bool equals(Installment other) =>
        other != null &&
        (installmentId, policyId, installmentNumber, amount) ==
        (other.installmentId, other.policyId, other.installmentNumber, other.amount);

    public override string ToString() => $"Installment #{installmentNumber} - {amount:C2} ({status} - Due: {dueDate:dd/MM/yyyy})";
}
