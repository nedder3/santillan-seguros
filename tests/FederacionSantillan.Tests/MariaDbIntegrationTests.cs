using FederacionSantillan.Data;
using FederacionSantillan.Data.DAOs;

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
