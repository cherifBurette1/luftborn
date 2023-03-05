using System.Data;

namespace luftborn.Service.Features.Common.Interfaces
{
    public interface IDbConnectionFactory
    {
        IDbConnection GetDbConnection();
    }
}
