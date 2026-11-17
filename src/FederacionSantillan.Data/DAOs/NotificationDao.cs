using FederacionSantillan.Model;
using MySqlConnector;

namespace FederacionSantillan.Data.DAOs;

public class NotificationDao
{
    public bool LogNotification(Notification notification) =>
        DbExceptionTranslator.Execute(() =>
        {
            const string sql = @"
                INSERT INTO notifications (installment_id, alert_type, message, generated_at, sent)
                VALUES (@installmentId, @alertType, @message, NOW(), TRUE);";

            using var conn = ConnectionFactory.CreateConnection();
            conn.Open();
            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@installmentId", notification.Installment.getInstallmentId());
            cmd.Parameters.AddWithValue("@alertType", notification.AlertType);
            cmd.Parameters.AddWithValue("@message", notification.Message);
            return cmd.ExecuteNonQuery() > 0;
        });
}
