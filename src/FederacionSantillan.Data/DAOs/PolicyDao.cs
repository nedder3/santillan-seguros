using FederacionSantillan.Data.Factories;
using FederacionSantillan.Data.Translators;
using FederacionSantillan.Model.DTOs.Requests;
using FederacionSantillan.Model.DTOs.Responses;
using FederacionSantillan.Model.Exceptions;
using MySqlConnector;

namespace FederacionSantillan.Data.DAOs;

public class PolicyDao
{
    public List<PolicyResponseDto> GetAll(string filter = "") =>
        DbExceptionTranslator.Execute(() =>
        {
            var list = new List<PolicyResponseDto>();
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
                list.Add(new PolicyResponseDto
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

    public string IssuePolicyWithInstallments(PolicyRequestDto dto) =>
        DbExceptionTranslator.Execute(() =>
        {
            if (ExistsPolicyNumber(dto.PolicyNumber))
                throw new DuplicateResourceException(SystemMessages.Format(SystemMessages.DuplicatePolicyNumber, dto.PolicyNumber));

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
                    cmd.Parameters.AddWithValue("@num", dto.PolicyNumber);
                    cmd.Parameters.AddWithValue("@insuredId", dto.InsuredId);
                    cmd.Parameters.AddWithValue("@branchId", dto.BranchId);
                    cmd.Parameters.AddWithValue("@company", dto.Company.Trim());
                    cmd.Parameters.AddWithValue("@asset", dto.InsuredAsset.Trim());
                    cmd.Parameters.AddWithValue("@premium", dto.TotalPremium);
                    cmd.Parameters.AddWithValue("@count", dto.InstallmentsCount);
                    cmd.Parameters.AddWithValue("@start", dto.StartDate.Date);
                    cmd.Parameters.AddWithValue("@end", dto.EndDate.Date);
                    policyId = Convert.ToInt32(cmd.ExecuteScalar());
                }

                decimal installmentAmount = Math.Round(dto.TotalPremium / dto.InstallmentsCount, 2);
                const string sqlInstallment = @"
                    INSERT INTO installments (policy_id, installment_number, amount, due_date, status)
                    VALUES (@policyId, @num, @amount, @dueDate, @status);";

                for (int i = 1; i <= dto.InstallmentsCount; i++)
                {
                    DateTime dueDate = dto.FirstDueDate.AddMonths(i - 1).Date;
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
                return $"Póliza #{dto.PolicyNumber} emitida con {dto.InstallmentsCount} cuotas generadas.";
            }
            catch
            {
                tx.Rollback();
                throw;
            }
        }, SystemMessages.Format(SystemMessages.DuplicatePolicyNumber, dto.PolicyNumber));
}
