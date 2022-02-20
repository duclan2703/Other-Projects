using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parse.Core.Models
{
    public class DataInfo
    {
        public List<ChiTiet> MauHoaDon { get; set; }
    }

    public class ChiTiet
    {
        public string Ten { get; set; }
        public string ProcessTime { get; set; }
        public string MauSo { get; set; }
        public string KyHieu { get; set; }
        public string KyHieuLuiNgay { get; set; }
    }
}
