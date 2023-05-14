namespace SportsBetting.BackofficeApi.DTO
{
    public class PagedResultDto<T>
    {
        public List<T> Items { get; set; }
        public int TotalItems { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
    }
}