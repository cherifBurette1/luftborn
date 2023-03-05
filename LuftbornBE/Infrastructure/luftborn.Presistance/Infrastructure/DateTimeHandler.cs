using Dapper;
using System;
using System.Data;

namespace luftborn.Presistance.Infrastructure
{
    public class DateTimeHandler : SqlMapper.TypeHandler<DateTime>
    {
        public override void SetValue(IDbDataParameter parameter, DateTime value)
        {
            parameter.Value = DateTime.SpecifyKind(value.ToUniversalTime(), DateTimeKind.Utc);
        }

        public override DateTime Parse(object value)
        {
            return DateTime.SpecifyKind((DateTime)value, DateTimeKind.Utc);
        }
    }
}
