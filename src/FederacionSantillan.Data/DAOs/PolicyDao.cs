using FederacionSantillan.Model.Exceptions;
using MySqlConnector;

namespace FederacionSantillan.Data.DAOs;

public class PolicySummaryDto
{
    public int PolicyId { get; set; }
    public int PolicyNumber { get; set; }
    public int InsuredId { get; set; }
    public string InsuredName { get; set; }
    public string BranchName { get; set; }
    public string Company { get; set; }
    public string InsuredAsset { get; set; }
    public decimal TotalPremium { get; set; }
    public int InstallmentsCount { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Status { get; set; }
}

public class PolicyDao
{
    public List<PolicySummaryDto> GetAll(string filter = "") =>
        DbExceptionTranslator.Execute(() =>
        {
            var list = new List<PolicySummaryDto>();
            const string sql = @"
                SELECT p.policy_id, p.policy_number, p.insured_id,
                       i.full_name AS insured_name,
                       b.name AS branch_name,
                       p.company, p.insured_asset, p.total_premium,
                       p.installments_count, p.start_date, p.end_date, p.status
                FROM policies p
                INNER JOIN insureds i ON p.insured_id = i.insured_id
                INNER JOIN branches b ON p.branch_id = b.branch_id
                WHERE (@filter = '' OR CAST(p.policy_number AS CHAR) LIKE @likeFilter
                       OR i.full_name LIKE @likeFilter OR p.insured_asset LIKE @likeFilter)
                ORDER BY p.policy_number DESC;";

            using var conn = ConnectionFactory.CreateConnection();
            conn.Open();
            using var cmd = new MySqlCommand(sql, conn);
            string f = filter?.Trim() ?? string.Empty;
            cmd.Parameters.AddWithValue("@filter", f);
            cmd.Parameters.AddWithValue("@likeFilter", $"%{f}%");

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new PolicySummaryDto
                {
                    PolicyId = reader.GetInt32("policy_id"),
                    PolicyNumber = reader.GetInt32("policy_number"),
                    InsuredId = reader.GetInt32("insured_id"),
                    InsuredName = reader.GetString("insured_name"),
                    BranchName = reader.GetString("branch_name"),
                    Company = reader.GetString("company"),
                    InsuredAsset = reader.GetString("insured_asset"),
                    TotalPremium = reader.GetDecimal("total_premium"),
                    InstallmentsCount = reader.GetInt32("installments_count"),
                    StartDate = reader.GetDateTime("start_date"),
                    EndDate = reader.GetDateTime("end_date"),
                    Status = reader.GetString("status")
                });
            }
            return list;
        });

    public bool ExistsPolicyNumber(int policyNumber) =>
        DbExceptionTranslator.Execute(() =>
        {
            const string sql = "SELECT COUNT(1) FROM policies WHERE policy_number = @num;";
            using var conn = ConnectionFactory.CreateConnection();
            conn.Open();
            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@num", policyNumber);
            return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
        });

    public int IssuePolicyWithInstallments(
        int policyNumber, int insuredId, int branchId, string company,
        string insuredAsset, decimal totalPremium, int installmentsCount,
        DateTime startDate, DateTime endDate, DateTime firstDueDate) =>
        DbExceptionTranslator.Execute(() =>
        {
            using var conn = ConnectionFactory.CreateConnection();
            conn.Open();
            using var tx = conn.BeginTransaction();
            try
            {
                const string sqlPolicy = @"
                    INSERT INTO policies (policy_number, insured_id, branch_id, company, insured_asset,
                                          total_premium, installments_count, start_date, end_date, status)
                    VALUES (@num, @insuredId, @branchId, @company, @asset, @premium, @count, @start, @end, 'ACTIVE');
                    SELECT LAST_INSERT_ID();";

                int policyId;
                using (var cmd = new MySqlCommand(sqlPolicy, conn, tx))
                {
                    cmd.Parameters.AddWithValue("@num", policyNumber);
                    cmd.Parameters.AddWithValue("@insuredId", insuredId);
                    cmd.Parameters.AddWithValue("@branchId", branchId);
                    cmd.Parameters.AddWithValue("@company", company);
                    cmd.Parameters.AddWithValue("@asset", insuredAsset);
                    cmd.Parameters.AddWithValue("@premium", totalPremium);
                    cmd.Parameters.AddWithValue("@count", installmentsCount);
                    cmd.Parameters.AddWithValue("@start", startDate.Date);
                    cmd.Parameters.AddWithValue("@end", endDate.Date);
                    policyId = Convert.ToInt32(cmd.ExecuteScalar());
                }

                decimal installmentAmount = Math.Round(totalPremium / installmentsCount, 2);
                const string sqlInstallment = @"
                    INSERT INTO installments (policy_id, installment_number, amount, due_date, status)
                    VALUES (@policyId, @num, @amount, @dueDate, @status);";

                for (int i = 1; i <= installmentsCount; i++)
                {
                    DateTime dueDate = firstDueDate.AddMonths(i - 1).Date;
                    string status = dueDate < DateTime.Today ? "OVERDUE" : "PENDING";

                    using var cmdInst = new MySqlCommand(sqlInstallment, conn, tx);
                    cmdInst.Parameters.AddWithValue("@policyId", policyId);
                    cmdInst.Parameters.AddWithValue("@num", i);
                    cmdInst.Parameters.AddWithValue("@amount", installmentAmount);
                    cmdInst.Parameters.AddWithValue("@dueDate", dueDate);
                    cmdInst.Parameters.AddWithValue("@status", status);
                    cmdInst.ExecuteNonQuery();
                }

                tx.Commit();
                return policyId;
            }
            catch
            {
                tx.Rollback();
                throw;
            }
        }, SystemMessages.Format(SystemMessages.DuplicatePolicyNumber, policyNumber));
}
