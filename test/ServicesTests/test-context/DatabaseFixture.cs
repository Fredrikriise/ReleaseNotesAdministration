namespace test.ServicesTests.test_context
{
    public class DatabaseFixture
    {
        public string ConnectionString { get; set; }

        public DatabaseFixture()
        {
            ConnectionString = "Server=tcp:hrmts-dev-releasenotesapp-sql.database.windows.net,1433;Initial Catalog=hrmts-dev-releasenotesapp-db;Persist Security Info=False;User ID=operations;Password=8f4XVncTkQsFDPlGCNEhXAhVA5AmXQ;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        }
    }
}
