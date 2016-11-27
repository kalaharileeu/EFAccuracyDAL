using AccuracyDAL.Models;
using System.Data.Entity.Infrastructure.Interception;

namespace AccuracyDAL
{
    using MySql.Data.Entity;
    using System.Data.Entity;

    //context is going to become a instance of accuracy entities
    [DbConfigurationType(typeof(MySqlEFConfiguration))]
    public class AccuracyEntities : DbContext
    {
        //Add databaselogger text, to append texts ("sqllog.txt", true)
        static readonly DatabaseLogger DatabaseLogger = new DatabaseLogger("sqllog.txt", false);

        // Your context has been configured to use a 'AccuracyEntities' connection string from the application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'AccuracyDAL.AccuracyEntities' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'AccuracyEntities' 
        // connection string in the application configuration file.
        /// <summary>
        /// pass the name of the connection string found in App.Config to the base class
        /// </summary>
        public AccuracyEntities() : base("name=AccuracyConnection")
        {
          //  DbInterception.Add(new ConsoleWriterInterceptor());//For custom logger
            DatabaseLogger.StartLogging();
            DbInterception.Add(DatabaseLogger);

        }

        // Add a DbSet for each entity type that you want to include in your model. For more information 
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.
        // public virtual DbSet<MyEntity> MyEntities { get; set; }
        //Add Dbset for each of the models
        public virtual DbSet<Testpoint> Testpoints { get; set; }
        public virtual DbSet<Testrun> Testruns { get; set; }

        protected override void Dispose(bool disposing)
        {
            DbInterception.Remove(DatabaseLogger);
            DatabaseLogger.StopLogging();
            base.Dispose(disposing);
        }
    }
}