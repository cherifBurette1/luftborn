using luftborn.Service.Features.Common.Interfaces.Repositories;
using System.Data;
using System.Data.Common;

namespace luftborn.Presistance.Infrastructure
{
    public class WrappedDbConnection : DbConnection
    {
        private DbConnection _connection;
        private readonly IAuditRepository _auditRepository;

        public WrappedDbConnection(DbConnection connection, IAuditRepository auditRepository)
        {
            _connection = connection;
            _auditRepository = auditRepository;
        }

        public IAuditRepository AuditRepository => _auditRepository;

        public DbConnection DbConnection { get => _connection; set => _connection = value; }

        public override string ConnectionString { get => _connection.ConnectionString; set => _connection.ConnectionString = value; }

        public override string Database => _connection.Database;

        public override string DataSource => _connection.DataSource;

        public override string ServerVersion => _connection.ServerVersion;

        public override ConnectionState State => _connection.State;

        public override void ChangeDatabase(string databaseName)
        {
            _connection.ChangeDatabase(databaseName);
        }

        public override void Close()
        {
            _connection.Close();
        }

        public override void Open()
        {
            _connection.Open();
        }

        protected override DbTransaction BeginDbTransaction(IsolationLevel isolationLevel)
        {
            return _connection.BeginTransaction(isolationLevel);
        }

        protected override DbCommand CreateDbCommand()
        {
            return new WrappedDbCommand(_connection.CreateCommand(), this);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                _connection?.Dispose();
            base.Dispose(disposing);
        }
    }
}
