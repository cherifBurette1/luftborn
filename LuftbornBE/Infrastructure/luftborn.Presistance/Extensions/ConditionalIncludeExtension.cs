using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;

namespace luftborn.Presistance.Extensions
{
    public static class ConditionalIncludeExtention
    {
        public static IQueryable<T> If<T, P>(
            this IIncludableQueryable<T, P> source,
            bool condition,
            Func<IIncludableQueryable<T, P>, IQueryable<T>> transform
        )
            where T : class
        {

            return condition ? transform(source) : source;
        }

        public static IQueryable<T> If<T, P>(
            this IIncludableQueryable<T, IEnumerable<P>> source,
            bool condition,
            Func<IIncludableQueryable<T, IEnumerable<P>>, IQueryable<T>> transform
        )
            where T : class
        {
            return condition ? transform(source) : source;
        }
    }
}
