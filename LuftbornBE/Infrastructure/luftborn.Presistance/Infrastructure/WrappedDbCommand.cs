using System.Data;
using System.Data.Common;

namespace luftborn.Presistance.Infrastructure
{
    public class WrappedDbCommand : DbCommand
    {
        private readonly DbCommand _command;
        private readonly WrappedDbConnection _wrappedConnection;

        public WrappedDbCommand(DbCommand command, WrappedDbConnection wrappedConnection)
        {
            _command = command;
            _wrappedConnection = wrappedConnection;
        }
        public override string CommandText { get => _command.CommandText; set => _command.CommandText = value; }
        public override int CommandTimeout { get => _command.CommandTimeout; set => _command.CommandTimeout = value; }
        public override CommandType CommandType { get => _command.CommandType; set => _command.CommandType = value; }
        public override bool DesignTimeVisible { get => _command.DesignTimeVisible; set => _command.DesignTimeVisible = value; }
        public override UpdateRowSource UpdatedRowSource { get => _command.UpdatedRowSource; set => _command.UpdatedRowSource = value; }
        protected override DbConnection DbConnection { get => _wrappedConnection.DbConnection; set => _wrappedConnection.DbConnection = value; }

        protected override DbParameterCollection DbParameterCollection => _command.Parameters;

        protected override DbTransaction DbTransaction { get => _command.Transaction; set => _command.Transaction = value; }

        public override void Cancel()
        {
            _command.Cancel();
        }

        public override int ExecuteNonQuery()
        {
            _wrappedConnection.AuditRepository.LogDbCommandDetails(_command);
            return _command.ExecuteNonQuery();
        }

        public override object ExecuteScalar()
        {
            return _command.ExecuteScalar();
        }

        public override void Prepare()
        {
            _command.Prepare();
        }

        protected override DbParameter CreateDbParameter()
        {
            return _command.CreateParameter();
        }

        protected override DbDataReader ExecuteDbDataReader(CommandBehavior behavior)
        {
            return _command.ExecuteReader();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                _command?.Dispose();
            base.Dispose(disposing);
        }
    }
}
