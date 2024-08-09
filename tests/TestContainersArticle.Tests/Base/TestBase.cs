using Microsoft.EntityFrameworkCore;
using Npgsql;
using Respawn;
using Testcontainers.PostgreSql;
using TestContainersArticle.Main.Data;
using TestContainersArticle.Main.Data.Repositories.UoW;

namespace TestContainersArticle.Tests.Base
{
    internal abstract class TestBase
    {
        private static PostgreSqlContainer _container = null!;
        private static string _connectionString = null!;
        private DataContext _context = null!;
        protected IUnitOfWork unitOfWork = null!;

        [OneTimeSetUp]
        public static async Task OneTimeSetUp()
        {
            _container = new PostgreSqlBuilder().WithImage("postgres:16.1").Build();
            await _container.StartAsync();

            _connectionString = _container.GetConnectionString();
        }

        [OneTimeTearDown]
        public static async Task OneTimeTearDown()
        {
            await _container.StopAsync();
            await _container.DisposeAsync();
        }

        [SetUp]
        public async Task Setup()
        {
            var options = new DbContextOptionsBuilder<DataContext>().UseNpgsql(_connectionString).Options;
            _context = new DataContext(options);

            await _context.Database.EnsureCreatedAsync();
            unitOfWork = new UnitOfWork(_context);
        }

        [TearDown]
        protected async Task TearDown()
        {
            var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            var respawner = await Respawner.CreateAsync(
                conn,
                new RespawnerOptions { SchemasToInclude = ["public", "postgres"], DbAdapter = DbAdapter.Postgres }
            );
            await respawner.ResetAsync(conn);

            conn.Dispose();
            await _context.DisposeAsync();
            unitOfWork.Dispose();
        }
    }
}
