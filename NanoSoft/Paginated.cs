using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace NanoSoft
{
    public struct Paginated<TItem>
    {
        [JsonConstructor]
        public Paginated(List<TItem> data, int currentPage, int pageSize, int total)
        {
            Check.NotNull(data, nameof(data));
            Check.ZeroOrMore(currentPage, nameof(currentPage));
            Check.ZeroOrMore(pageSize, nameof(pageSize));
            Check.ZeroOrMore(total, nameof(total));

            Data = data;
            CurrentPage = currentPage;
            PageSize = pageSize;
            Total = total;
        }

        public List<TItem> Data { get; }
        public int CurrentPage { get; }
        public int PageSize { get; }
        public int Total { get; }

        public int ShowingFrom
        {
            get
            {
                if (CurrentPage == 0)
                    return 0;

                return (CurrentPage - 1) * PageSize + 1;
            }
        }

        public int ShowingTo
        {
            get
            {
                if (CurrentPage == 0)
                    return 0;

                if (CurrentPage == LastPage)
                    return Total;

                return CurrentPage * PageSize;
            }
        }


        public int LastPage
        {
            get
            {
                if (PageSize == 0)
                    return 0;

                var result = Total / PageSize;

                return (Total % PageSize) > 0 ? result + 1 : result;
            }
        }

        public IEnumerable<int> Pages
        {
            get
            {
                var pages = new[] { 1, 2 }
                    .Concat(Enumerable.Range(CurrentPage - 2, 5))
                    .Concat(new[] { LastPage - 1, LastPage });

                var lastPage = LastPage;
                return pages.Where(n => n >= 1 && n <= lastPage).Distinct();
            }
        }
    }
}
