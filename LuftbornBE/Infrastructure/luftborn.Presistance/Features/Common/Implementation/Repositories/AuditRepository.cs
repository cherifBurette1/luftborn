using Dapper;
using luftborn.Data.AuditEntites;
using luftborn.Service.Features.Common.Interfaces;
using luftborn.Service.Features.Common.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using System.Transactions;

namespace luftborn.Presistance.Features.Common.Implementation.Repositories
{
    public class AuditRepository : IAuditRepository
    {
        private readonly IAuditDbConnectionFactory _auditDbConnectionFactory;

        public AuditRepository(IAuditDbConnectionFactory auditDbConnectionFactory)
        {
            _auditDbConnectionFactory = auditDbConnectionFactory;
        }
        public void LogDbCommandDetails(DbCommand command)
        {
            if (command == null)
                return;

            RunAsync(async () =>
            {
                if (command.CommandType == CommandType.StoredProcedure)
                {
                    //get parameters
                    var parameters = new Dictionary<string, object>();
                    foreach (DbParameter p in command.Parameters)
                    {
                        parameters.Add(p.ParameterName, p.Value);
                    }
                    using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Suppress, TimeSpan.FromMinutes(5), TransactionScopeAsyncFlowOption.Enabled))
                    {
                        using (var connection = _auditDbConnectionFactory.GetAuditDbConnection())
                        {
                            string sql = @"INSERT INTO ProcedureAuditLogs 
                                                            (AuditDate, ProcedureName, Parameters, UserId, UserName, RemoteUserIp, LocalUserIp, UserAgent, 
                                                            BrowserName, OsName, HostName) 
                                                            VALUES (@AuditDate, @ProcedureName, @Parameters, @UserId, @UserName, @RemoteUserIp, @LocalUserIp, @UserAgent, 
                                                            @BrowserName, @OsName, @HostName) ";
                            await connection.ExecuteAsync(sql, commandType: CommandType.Text);
                        }
                        transactionScope.Complete();
                    }
                }
            }, callingMethodName: "LogDbCommandDetails");
        }

        public void LogEntityChanges(List<EntityAuditLog> auditLogs)
        {
            if (auditLogs == null || auditLogs.Count == 0)
                return;
        }

        /// <summary>
        /// helper method to run the code asynchronously and handle the exceptions
        /// </summary>
        /// <param name="func">the code needed to run</param>
        /// <param name="callingMethodName">the name of calling method</param>
        private void RunAsync(Func<Task> func, string callingMethodName)
        {
            return;

            if (func == null)
                return;

            Task.Run(async () =>
            {
                //uncomment this line to test thread safety errors
                //await Task.Delay(20000);
                await func();

            });
        }
    }
}
