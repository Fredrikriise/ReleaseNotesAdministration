using DbUp;
using System;
using System.Reflection;

namespace test.ServicesTests.DbMigration
{
    public class DbMigrator
    {
        public static void Migrate(string connectionString)
        {
            try
            {
                EnsureDatabase.For.SqlDatabase(connectionString);
                var upgrader = DeployChanges.To
                    .SqlDatabase(connectionString)
                    .WithScriptsEmbeddedInAssembly(typeof(DbMigrator).GetTypeInfo().Assembly)
                    .WithTransaction()
                    .LogToConsole()
                    .Build();
                var result = upgrader.PerformUpgrade();

                if(result.Successful == false)
                {
                    throw result.Error;
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
