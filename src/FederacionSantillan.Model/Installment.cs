using FederacionSantillan.Model.Abstractions;

namespace FederacionSantillan.Model;

public class Installment : ICopiable<Installment>, IClonable<Installment>, IEquatableDomain<Installment>, IIdentifiable<int>
{
    private int installmentId, policyId, installmentNumber;
    private decimal amount;
    private DateTime dueDate;
    private string status, paymentMethod, notes;
    private DateTime? paymentDate;

    private int policyNumberRef;
    private string insuredNameRef, insuredPhoneRef, branchNameRef;

    public int Id => installmentId;

    public Installment(int id, int policyId, int number, decimal amount, DateTime dueDate, string status)
    {
        installmentId = id;
        this.policyId = policyId;
        installmentNumber = number;
        this.amount = amount;
        this.dueDate = dueDate;
        this.status = status;
        paymentDate = null;
        paymentMethod = null;
        notes = string.Empty;
    }

    public int InstallmentId { get => installmentId; set => installmentId = value; }
    public int PolicyId { get => policyId; set => policyId = value; }
    public int InstallmentNumber { get => installmentNumber; set => installmentNumber = value; }
    public decimal Amount { get => amount; set => amount = value; }
    public DateTime DueDate { get => dueDate; set => dueDate = value; }
    public string Status { get => status; set => status = value; }
    public DateTime? PaymentDate { get => paymentDate; set => paymentDate = value; }
    public string PaymentMethod { get => paymentMethod; set => paymentMethod = value; }
    public string Notes { get => notes; set => notes = value; }

    public int PolicyNumberRef { get => policyNumberRef; set => policyNumberRef = value; }
    public string InsuredNameRef { get => insuredNameRef; set => insuredNameRef = value; }
    public string InsuredPhoneRef { get => insuredPhoneRef; set => insuredPhoneRef = value; }
    public string BranchNameRef { get => branchNameRef; set => branchNameRef = value; }

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
        status = "PAID";
        paymentDate = DateTime.Now;
        paymentMethod = method;
        return true;
    }

    public string buildWhatsAppReminderText()
    {
        string prefix = isOverdue() ? "VENCIDA" : "próxima a vencer";
        return $"Hola {insuredNameRef}, te escribimos de Federación Santillán Seguros (Federación Patronal). " +
               $"Te recordamos que la cuota #{installmentNumber} de tu póliza #{policyNumberRef} ({branchNameRef}) " +
               $"por {amount:C2} se encuentra {prefix} (Vto: {dueDate:dd/MM/yyyy}). ¡Saludos!";
    }

    public void copy(Installment other)
    {
        if (other == null) return;
        installmentId = other.installmentId;
        policyId = other.policyId;
        installmentNumber = other.installmentNumber;
        amount = other.amount;
        dueDate = other.dueDate;
        status = other.status;
        paymentDate = other.paymentDate;
        paymentMethod = other.paymentMethod;
        notes = other.notes;
    }

    public Installment clone() =>
        new Installment(installmentId, policyId, installmentNumber, amount, dueDate, status)
        {
            paymentDate = paymentDate,
            paymentMethod = paymentMethod,
            notes = notes
        };

    public bool equals(Installment other) =>
        other != null &&
        installmentId == other.installmentId &&
        policyId == other.policyId &&
        installmentNumber == other.installmentNumber &&
        amount == other.amount;

    public override string ToString() => $"Installment #{installmentNumber} - {amount:C2} ({status} - Due: {dueDate:dd/MM/yyyy})";
}
