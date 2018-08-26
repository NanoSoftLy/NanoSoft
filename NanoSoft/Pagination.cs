using System.Collections.Generic;
using System.Linq;

namespace NanoSoft
{
    public class Pagination<TItem>
    {
        public List<TItem> Data { get; set; } = new List<TItem>();
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int LastPage { get; set; }
        public IEnumerable<int> Pages
        {
            get
            {
                var pages = new[] { 1, 2 }
                    .Concat(Enumerable.Range(CurrentPage - 2, 5))
                    .Concat(new[] { LastPage - 1, LastPage });

                return pages.Where(n => n >= 1 && n <= LastPage).Distinct();
            }
        }
    }
}
