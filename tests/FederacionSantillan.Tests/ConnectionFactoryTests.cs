using FederacionSantillan.Data.Factories;

namespace FederacionSantillan.Tests;

[Trait("Category", "Unit")]
public class ConnectionFactoryTests
{
    [Fact]
    public void Given_EnvironmentVariableConnectionStringsMariaDb_When_GettingConnectionString_Then_PrioritizesEnvVar()
    {
        const string customConnStr = "Server=mariadb-container;Port=3306;Database=santillan_test;User ID=test;Password=test123;";
        string originalEnv = Environment.GetEnvironmentVariable("ConnectionStrings__MariaDb");

        try
        {
            Environment.SetEnvironmentVariable("ConnectionStrings__MariaDb", customConnStr);

            string actual = ConnectionFactory.GetConnectionString();

            Assert.Equal(customConnStr, actual);
        }
        finally
        {
            Environment.SetEnvironmentVariable("ConnectionStrings__MariaDb", originalEnv);
        }
    }

    [Fact]
    public void Given_EnvironmentVariableDbConnectionString_When_GettingConnectionString_Then_PrioritizesEnvVar()
    {
        const string customConnStr = "Server=mariadb-alt;Port=3306;Database=santillan_alt;User ID=alt;Password=alt123;";
        string originalPrimary = Environment.GetEnvironmentVariable("ConnectionStrings__MariaDb");
        string originalAlt = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");

        try
        {
            Environment.SetEnvironmentVariable("ConnectionStrings__MariaDb", null);
            Environment.SetEnvironmentVariable("DB_CONNECTION_STRING", customConnStr);

            string actual = ConnectionFactory.GetConnectionString();

            Assert.Equal(customConnStr, actual);
        }
        finally
        {
            Environment.SetEnvironmentVariable("ConnectionStrings__MariaDb", originalPrimary);
            Environment.SetEnvironmentVariable("DB_CONNECTION_STRING", originalAlt);
        }
    }
}
