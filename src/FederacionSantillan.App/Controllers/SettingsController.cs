using FederacionSantillan.Data.DAOs;

namespace FederacionSantillan.App.Controllers;

public class SettingsController(DatabaseBackupDao backupDao = null)
{
    private readonly DatabaseBackupDao _backupDao = backupDao ?? new();

    public string GenerateDatabaseSqlDump() => _backupDao.GenerateSqlDump();
}
