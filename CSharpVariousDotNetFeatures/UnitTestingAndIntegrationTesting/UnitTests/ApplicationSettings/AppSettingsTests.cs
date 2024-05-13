using Microsoft.Extensions.Configuration;
using StackExchange.Redis;

namespace UnitTestingAndIntegrationTesting.UnitTests.ApplicationSettings
{
    public class AppSettingsTests
    {
        [Fact]
        public void Appsettings_northwind_connection_string_is_valid()
        {
            var sut = new ConfigurationBuilder().SetBasePath(AppDomain.CurrentDomain.BaseDirectory).AddJsonFile("appsettings.json").Build();

            var northwind_ConnectionString = sut.GetSection("ConnectionStrings").GetValue<string>("Northwind");

            Assert.Equal("Data Source=Michael-Gaming-\\SQLEXPRESS;Initial Catalog=Northwind;Integrated Security=true", northwind_ConnectionString);
        }

        [Fact]
        public void Appsettings_adventureworks_connection_string_is_valid()
        {
            var sut = new ConfigurationBuilder().SetBasePath(AppDomain.CurrentDomain.BaseDirectory).AddJsonFile("appsettings.json").Build();

            var adventureworks_ConnectionString = sut.GetSection("ConnectionStrings").GetValue<string>("AdventureWorks");

            Assert.Equal("Data Source=Michael-Gaming-\\SQLEXPRESS;Initial Catalog=AdventureWorks;Integrated Security=true", adventureworks_ConnectionString);
        }

        [Fact]
        public void Appsettings_development_northwind_connection_string_is_valid()
        {
            var sut = new ConfigurationBuilder().SetBasePath(AppDomain.CurrentDomain.BaseDirectory).AddJsonFile("appsettings.development.json").Build();

            var northwind_Dev_ConnectionString = sut.GetSection("ConnectionStrings").GetValue<string>("Northwind_Dev");

            Assert.Equal("Data Source=Michael-Gaming-\\SQLEXPRESS;Initial Catalog=Northwind_Dev;Integrated Security=true", northwind_Dev_ConnectionString);
        }

        [Fact]
        public void Appsettings_development_adventureworks_connection_string_is_valid()
        {
            var sut = new ConfigurationBuilder().SetBasePath(AppDomain.CurrentDomain.BaseDirectory).AddJsonFile("appsettings.development.json").Build();

            var adventureworks_Dev_ConnectionString = sut.GetSection("ConnectionStrings").GetValue<string>("AdventureWorks_Dev");

            Assert.Equal("Data Source=Michael-Gaming-\\SQLEXPRESS;Initial Catalog=AdventureWorks_Dev;Integrated Security=true", adventureworks_Dev_ConnectionString);
        }

        [Fact]
        public void Appsettings_featuretoggle_is_valid()
        {
            var sut = new ConfigurationBuilder().SetBasePath(AppDomain.CurrentDomain.BaseDirectory).AddJsonFile("appsettings.json").Build();

            var featuretoggle = sut.GetValue<bool>("FeatureToggle");

            Assert.False(featuretoggle);
        }

        [Fact]
        public void Appsettings_development_featuretoggle_is_valid()
        {
            var sut = new ConfigurationBuilder().SetBasePath(AppDomain.CurrentDomain.BaseDirectory).AddJsonFile("appsettings.development.json").Build();

            var featuretoggle_Dev = sut.GetValue<bool>("FeatureToggle");

            Assert.True(featuretoggle_Dev);
        }

        [Fact]
        public void Appsettings_deprecated_key_is_removed()
        {
            var sut = new ConfigurationBuilder().SetBasePath(AppDomain.CurrentDomain.BaseDirectory).AddJsonFile("appsettings.json").Build();

            Exception exception = Record.Exception(() => sut.GetRequiredSection("DeprecatedKey").Key);

            Assert.IsType<InvalidOperationException>(exception);
        }

#if DEBUG
        [Fact]
        public void Appsettings_development_azure_redis_cache_is_running_locally()
        {
            var sut = ConnectionMultiplexer.Connect("localhost:6379");

            var redisCache_IsRunningLocally = sut.IsConnected;

            Assert.True(redisCache_IsRunningLocally);
        }
#endif
    }
}
