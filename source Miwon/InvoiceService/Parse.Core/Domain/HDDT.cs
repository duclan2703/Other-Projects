using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parse.Core.Domain
{
    public class HDDT
    {
        public virtual int HDDT_Id { get; set; }
        public virtual string Fkey { get; set; }
        public virtual string Pattern { get; set; }
        public virtual string Serial { get; set; }
        public virtual string Buyer { get; set; }
        public virtual string CusCode { get; set; }
        public virtual string CusName { get; set; }
        public virtual string CusAddress { get; set; }
        public virtual string CusTaxCode { get; set; }
        public virtual string Currency { get; set; }
        public virtual decimal? Total { get; set; }
        public virtual decimal? TaxAmount { get; set; }
        public virtual decimal? Discount { get; set; }
        public virtual decimal? TotalAmount { get; set; }
        public virtual string CusEmail { get; set; }
        public virtual string Note { get; set; }
        /// <summary>
        /// 1: tạo mới, 2: điều chỉnh tăng, 3: điều chỉnh giảm, 4: chiết khấu, 5: hủy bỏ
        /// </summary>
        public virtual int Type { get; set; }
        public virtual string PreFkey { get; set; }
        public virtual string TaxCode { get; set; }
        public virtual DateTime CreatedDate { get; set; }
        public virtual string InvNo { get; set; }
        public virtual string ErrorCode { get; set; }
        public virtual string ErrorDesc { get; set; }
        public virtual int Status { get; set; }
        public virtual IList<HDDT_Detail> lstDetail { get; set; }
    }
}
