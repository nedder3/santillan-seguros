using FederacionSantillan.Data.DAOs;

namespace FederacionSantillan.App.Controllers;

public class SettingsController
{
    private readonly DatabaseBackupDao _backupDao = new DatabaseBackupDao();

    public string GenerateDatabaseSqlDump() => _backupDao.GenerateSqlDump();
}
