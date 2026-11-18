using System.Text;
using FederacionSantillan.Data.Factories;
using FederacionSantillan.Data.Translators;
using FederacionSantillan.Model.DTOs.Requests;
using FederacionSantillan.Model.DTOs.Responses;
using FederacionSantillan.Model.Entities;
using FederacionSantillan.Model.Exceptions;
using MySqlConnector;

namespace FederacionSantillan.Data.DAOs;

public class InstallmentDao
{
    public void SyncOverdueInstallments() =>
        DbExceptionTranslator.Execute(() =>
        {
            const string sql = @"
                UPDATE installments
                SET status = 'OVERDUE'
                WHERE status = 'PENDING' AND due_date < CURDATE();";

            using var conn = ConnectionFactory.CreateConnection();
            conn.Open();
            using var cmd = new MySqlCommand(sql, conn);
            cmd.ExecuteNonQuery();
        });

    public AlertsDashboardResponseDto LoadAlertsDashboard(int daysAhead = 15)
    {
        var overdue = GetOverdue();
        var upcoming = GetUpcoming(daysAhead);
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

        return new AlertsDashboardResponseDto
        {
            Overdue = overdue,
            Upcoming = upcoming,
            OverdueTotal = overdueSum,
            UpcomingTotal = upcomingSum,
            InstallmentIndex = index
        };
    }

    public List<Installment> GetByPolicy(int policyId) =>
        ExecuteInstallmentQuery(@"
            SELECT c.installment_id, c.policy_id, c.installment_number, c.amount, c.due_date,
                   c.status, c.payment_date, COALESCE(c.payment_method, '') AS payment_method,
                   COALESCE(c.notes, '') AS notes,
                   p.policy_number, i.full_name, i.phone, b.name AS branch_name
            FROM installments c
            INNER JOIN policies p ON c.policy_id = p.policy_id
            INNER JOIN insureds i ON p.insured_id = i.insured_id
            INNER JOIN branches b ON p.branch_id = b.branch_id
            WHERE c.policy_id = @policyId
            ORDER BY c.installment_number;",
            cmd => cmd.Parameters.AddWithValue("@policyId", policyId));

    public List<Installment> GetOverdue()
    {
        SyncOverdueInstallments();
        return ExecuteInstallmentQuery(@"
            SELECT c.installment_id, c.policy_id, c.installment_number, c.amount, c.due_date,
                   c.status, c.payment_date, COALESCE(c.payment_method, '') AS payment_method,
                   COALESCE(c.notes, '') AS notes,
                   p.policy_number, i.full_name, i.phone, b.name AS branch_name
            FROM installments c
            INNER JOIN policies p ON c.policy_id = p.policy_id
            INNER JOIN insureds i ON p.insured_id = i.insured_id
            INNER JOIN branches b ON p.branch_id = b.branch_id
            WHERE c.status = 'OVERDUE' AND p.status = 'ACTIVE' AND i.active = TRUE
            ORDER BY c.due_date ASC;");
    }

    public List<Installment> GetUpcoming(int daysAhead = 15) =>
        ExecuteInstallmentQuery(@"
            SELECT c.installment_id, c.policy_id, c.installment_number, c.amount, c.due_date,
                   c.status, c.payment_date, COALESCE(c.payment_method, '') AS payment_method,
                   COALESCE(c.notes, '') AS notes,
                   p.policy_number, i.full_name, i.phone, b.name AS branch_name
            FROM installments c
            INNER JOIN policies p ON c.policy_id = p.policy_id
            INNER JOIN insureds i ON p.insured_id = i.insured_id
            INNER JOIN branches b ON p.branch_id = b.branch_id
            WHERE c.status = 'PENDING'
              AND c.due_date BETWEEN CURDATE() AND DATE_ADD(CURDATE(), INTERVAL @days DAY)
              AND p.status = 'ACTIVE' AND i.active = TRUE
            ORDER BY c.due_date ASC;",
            cmd => cmd.Parameters.AddWithValue("@days", daysAhead));

    public string RegisterPayment(PaymentRequestDto dto) =>
        DbExceptionTranslator.Execute(() =>
        {
            const string sql = @"
                UPDATE installments
                SET status = 'PAID',
                    payment_date = NOW(),
                    payment_method = @method,
                    notes = @notes
                WHERE installment_id = @id AND status <> 'PAID';";

            using var conn = ConnectionFactory.CreateConnection();
            conn.Open();
            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", dto.InstallmentId);
            cmd.Parameters.AddWithValue("@method", dto.PaymentMethod);
            cmd.Parameters.AddWithValue("@notes", dto.Notes ?? string.Empty);

            if (cmd.ExecuteNonQuery() == 0)
                throw new ValidationException(SystemMessages.InstallmentAlreadyPaid);

            return "Pago registrado correctamente. La alerta fue saldada.";
        });

    public static string ExportToCsv(IReadOnlyList<Installment> installments)
    {
        var sb = new StringBuilder(installments.Count * 96 + 80);
        sb.AppendLine("Poliza;Asegurado;Telefono;Ramo;Cuota;Monto;Vencimiento;DiasAtraso");
        for (int i = 0; i < installments.Count; i++)
        {
            var c = installments[i];
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

    private List<Installment> ExecuteInstallmentQuery(string sql, Action<MySqlCommand> configureParams = null) =>
        DbExceptionTranslator.Execute(() =>
        {
            var list = new List<Installment>();
            using var conn = ConnectionFactory.CreateConnection();
            conn.Open();
            using var cmd = new MySqlCommand(sql, conn);
            configureParams?.Invoke(cmd);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var inst = new Installment(
                    reader.GetInt32("installment_id"),
                    reader.GetInt32("policy_id"),
                    reader.GetInt32("installment_number"),
                    reader.GetDecimal("amount"),
                    reader.GetDateTime("due_date"),
                    reader.GetString("status")
                )
                {
                    PaymentMethod = reader.GetString("payment_method"),
                    Notes = reader.GetString("notes"),
                    PolicyNumberRef = reader.GetInt32("policy_number"),
                    InsuredNameRef = reader.GetString("full_name"),
                    InsuredPhoneRef = reader.GetString("phone"),
                    BranchNameRef = reader.GetString("branch_name")
                };

                if (!reader.IsDBNull(reader.GetOrdinal("payment_date")))
                    inst.PaymentDate = reader.GetDateTime("payment_date");

                list.Add(inst);
            }
            return list;
        });
}
