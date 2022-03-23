using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parse.Core.Utils
{
    public class PagingComponent
    {
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        public int? Total { get; set; }
        public int? TotalPage
        {
            get
            {
                if (this.Total.HasValue)
                {
                    if (this.Total.Value % PageSize != 0)
                        return this.Total.Value / PageSize + 1;
                    else
                        return this.Total.Value / PageSize;
                }
                else
                    return null;
            }
        }

        public PagingComponent()
        {

        }
        public PagingComponent(int page, int size) : this()
        {
            this.PageIndex = page;
            this.PageSize = size;
        }
        public PagingComponent(int page, int size, int total) : this(page, size)
        {
            this.Total = total;
        }

        public bool HasNext()
        {
            if (TotalPage.HasValue)
                return this.PageIndex < TotalPage;
            else
                return false;
        }

        public bool HasPrev()
        {
            return this.PageIndex > 1;
        }
    }
}
