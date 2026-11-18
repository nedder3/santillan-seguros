using FederacionSantillan.Data.DAOs;
using FederacionSantillan.Data.Factories;

namespace FederacionSantillan.Tests;

[Trait("Category", "Integration")]
public class MariaDbIntegrationTests
{
    [Fact]
    public void Given_RunningMariaDbContainer_When_QueryingBranches_Then_ReturnsSeededCatalog()
    {
        if (!ConnectionFactory.TestConnection(out _))
            return;

        var branches = new BranchDao().GetAll();

        Assert.NotEmpty(branches);
    }
}
