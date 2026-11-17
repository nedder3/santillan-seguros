using FederacionSantillan.Model;
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

    public bool RegisterPayment(int installmentId, string paymentMethod, string notes = "") =>
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
            cmd.Parameters.AddWithValue("@id", installmentId);
            cmd.Parameters.AddWithValue("@method", paymentMethod);
            cmd.Parameters.AddWithValue("@notes", notes ?? string.Empty);
            return cmd.ExecuteNonQuery() > 0;
        });

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
