using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MoviesApi.Helpers
{
    public static class HttpContextExtensions
    {
        public async static Task InsertPaginationParameters<T>(this HttpContext httpContext, IQueryable<T> queryable, int perPage)
        {
            double count = await queryable.CountAsync();
            double pages = Math.Ceiling(count / perPage);
            httpContext.Response.Headers.Add("pages", pages.ToString());
        }
    }
}
