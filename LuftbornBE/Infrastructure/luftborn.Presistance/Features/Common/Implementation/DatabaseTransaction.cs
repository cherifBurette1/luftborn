using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;
using luftborn.Service.Features.Common.Interfaces;

namespace luftborn.Presistance.Features.Common.Implementation
{
    public class DatabaseTransaction : IDatabaseTransaction
    {
        private IDbContextTransaction _transaction;
        public DatabaseTransaction(IluftbornEntities context)
        {
            _transaction = context.Database.BeginTransaction();
        }

        public async Task CommitAsync()
        {
            await _transaction.CommitAsync();
        }
        public async Task RollbackAsync()
        {
            await _transaction.RollbackAsync();
        }
        public async ValueTask DisposeAsync()
        {
            await _transaction.DisposeAsync();
        }

    }
}
