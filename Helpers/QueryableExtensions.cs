using MoviesApi.DTOs;
using System.Linq;

namespace MoviesApi.Helpers
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> Paginate<T>(this IQueryable<T> queryable, PaginatorDTO paginatorDTO)
        {
            return queryable
                .Skip((paginatorDTO.Page - 1) * paginatorDTO.CountPerPage)
                .Take(paginatorDTO.PerPage);
        }
    }
}
