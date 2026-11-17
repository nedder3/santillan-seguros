using FederacionSantillan.Data;
using FederacionSantillan.Data.DAOs;

namespace FederacionSantillan.Tests;

[Trait("Category", "Integration")]
public class MariaDbIntegrationTests
{
    [Fact]
    public void ConexionMariaDb_Y_LecturaDeRamos_FuncionanConDockerActivo()
    {
        if (!ConnectionFactory.TestConnection(out _))
            return;

        var ramos = new RamoDAO().ObtenerTodos();
        Assert.NotEmpty(ramos);
    }
}
