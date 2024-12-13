namespace BlogSystem.BLL.helpers
{
    public class Pagination<T>
    {
        public int PageIndex { get; set; }
        public int TotalPages { get; set; }
        public int Count { get; set; }
        public List<T> Items { get; set; }
        public Pagination(List<T> items, int count, int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            Count = count;
            Items = items;
        }

        public static Pagination<T> GetPagination(IQueryable<T> source , int pageIndex , int pageSize)
        {
            var count = source.Count();
            var items = source.Skip((pageIndex - 1)*pageSize).Take(pageSize).ToList();
            return new Pagination<T>(items,count,pageIndex,pageSize);
        }



    }
}
