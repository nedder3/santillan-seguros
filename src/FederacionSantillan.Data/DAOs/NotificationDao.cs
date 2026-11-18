using FederacionSantillan.Data.Factories;
using FederacionSantillan.Data.Translators;
using FederacionSantillan.Model.Entities;
using FederacionSantillan.Model.Exceptions;
using MySqlConnector;

namespace FederacionSantillan.Data.DAOs;

public class NotificationDao
{
    public string LogAndBuildWhatsAppReminder(Installment installment) =>
        DbExceptionTranslator.Execute(() =>
        {
            if (installment == null)
                throw new ValidationException(SystemMessages.SelectionRequired);

            string message = installment.buildWhatsAppReminderText();
            string alertType = installment.isOverdue() ? "OVERDUE" : "UPCOMING";

            const string sql = @"
                INSERT INTO notifications (installment_id, alert_type, message, generated_at, sent)
                VALUES (@installmentId, @alertType, @message, NOW(), TRUE);";

            using var conn = ConnectionFactory.CreateConnection();
            conn.Open();
            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@installmentId", installment.getInstallmentId());
            cmd.Parameters.AddWithValue("@alertType", alertType);
            cmd.Parameters.AddWithValue("@message", message);
            cmd.ExecuteNonQuery();

            return message;
        });
}
