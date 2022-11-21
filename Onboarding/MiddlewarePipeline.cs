using DbUp;
#nullable disable

namespace Onboarding
{/// <summary>
     ///  configuration of middleware  pipeline
     /// </summary>
    public static class MiddlewarePipeline
    {
        public static WebApplication SetupMiddleware(this WebApplication app, ConfigurationManager configuration)
        {
            //Configure Pipeline
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();

            TableMigrationScript(configuration);
            StoredProcedureMigrationScript(configuration);

            return app;
        }

        /// <summary>
        /// Sql migration for table Schema
        /// </summary>
        public static void TableMigrationScript(ConfigurationManager configuration)
        {
            string dbConnStr = configuration.GetConnectionString("Default");
            EnsureDatabase.For.SqlDatabase(dbConnStr);

            var upgrader = DeployChanges.To.SqlDatabase(dbConnStr)
            .WithScriptsFromFileSystem(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Sql", "Tables"))
            .WithTransactionPerScript()
            .JournalToSqlTable("dbo", "TableMigration")
             .LogToConsole()
            .WithVariablesDisabled()
            .Build();

            upgrader.PerformUpgrade();
        }

        /// <summary>
        /// Sql migration for stored procedure
        /// </summary>
        public static void StoredProcedureMigrationScript(ConfigurationManager configuration)
        {
            string dbConnStr = configuration.GetConnectionString("Default");
            EnsureDatabase.For.SqlDatabase(dbConnStr);

            var upgrader = DeployChanges.To.SqlDatabase(dbConnStr)
            .WithScriptsFromFileSystem(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Sql", "Sprocs"))
            .WithTransactionPerScript()
            .JournalToSqlTable("dbo", "SprocsMigration")
            //.LogTo(new SerilogDbUpLog(_logger))
            .LogToConsole()
            .Build();

            upgrader.PerformUpgrade();
        }

    }

}
