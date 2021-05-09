namespace MoviesApi.DTOs
{
    public class PaginatorDTO
    {
        public int Page { get; set; } = 1;
        public int PerPage = 10;
        private readonly int MaxPerPage = 50;

        public int CountPerPage 
        {
            get => PerPage;

            set
            {
                PerPage = (value > MaxPerPage) ? MaxPerPage: value;
            }
        }
    }
}
