using Microsoft.Extensions.Configuration;

namespace TestProject1
{
    public class TestConfiguration
    {
        public IConfiguration Configuration { get; }
        public string SqlServerConnectionString { get; }
        
        public TestConfiguration()
        {
            Configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.tests.json")
                .AddEnvironmentVariables()
                .Build();

            SqlServerConnectionString = Configuration["SqlServer:ConnectionString"];
        }
    }
}