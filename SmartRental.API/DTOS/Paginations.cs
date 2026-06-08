using System.ComponentModel.DataAnnotations;

namespace SmartRental.API.DTOS
{
    public class Paginations<T> where T : class
    {
        public Paginations(int pageIndex, int pageSize, int count, IReadOnlyList<T> data)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            Count = count;
            Data = data;
        }

        [Range(1, int.MaxValue)]
      public  int PageIndex { get; set; }
        [Range(1, int.MaxValue)]
        public int PageSize { get; set; }
        [Range(1, int.MaxValue)]
        public int Count { get; set; }
        public IReadOnlyList<T> Data { get; set; }
    }
}
