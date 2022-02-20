using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parse.Core.Utils
{
    public class NumberUtil
    {
        public static string DocSoThanhChu(string number)
        {
            string[] part = new string[2];
            var lstSoTien = number.Split('.');
            if (lstSoTien.Length == 1 || lstSoTien[1] == "0")
                return DocCacSoRaChu(lstSoTien[0]) + " đồng";
            else
                return DocCacSoRaChu(lstSoTien[0]) + " phẩy " + DocCacSoRaChu(lstSoTien[1]).ToLower() + " đồng";
        }

        public static string DocCacSoRaChu(string number)
        {
            string strReturn = "";
            string s = number;
            while (s.Length > 0 && s.Substring(0, 1) == "0")
            {
                s = s.Substring(1);
            }
            string[] so = new string[] { "không", "một", "hai", "ba", "bốn", "năm", "sáu", "bảy", "tám", "chín" };
            string[] hang = new string[] { "", "nghìn", "triệu", "tỷ" };
            int i, j, donvi, chuc, tram;

            bool booAm = false;
            decimal decS = 0;

            try
            {
                decS = Convert.ToDecimal(s.ToString());
            }
            catch { }

            if (decS < 0)
            {
                decS = -decS;
                //s = decS.ToString();
                booAm = true;
            }
            i = s.Length;
            if (i == 0)
                strReturn = so[0] + strReturn;
            else
            {
                j = 0;
                while (i > 0)
                {
                    donvi = Convert.ToInt32(s.Substring(i - 1, 1));
                    i--;
                    if (i > 0)
                        chuc = Convert.ToInt32(s.Substring(i - 1, 1));
                    else
                        chuc = -1;
                    i--;
                    if (i > 0)
                        tram = Convert.ToInt32(s.Substring(i - 1, 1));
                    else
                        tram = -1;
                    i--;
                    if ((donvi > 0) || (chuc > 0) || (tram > 0) || (j == 3))
                        strReturn = hang[j] + strReturn;
                    j++;
                    if (j > 3) j = 1;   //Tránh lỗi, nếu dưới 13 số thì không có vấn đề.
                    //Hàm này chỉ dùng để đọc đến 9 số nên không phải bận tâm
                    if ((donvi == 1) && (chuc > 1))
                        strReturn = "mốt " + strReturn;
                    else
                    {
                        if ((donvi == 5) && (chuc > 0))
                            strReturn = "lăm " + strReturn;
                        else if (donvi > 0)
                            strReturn = so[donvi] + " " + strReturn;
                    }
                    if (chuc < 0) break;//Hết số
                    else
                    {
                        if ((chuc == 0) && (donvi > 0)) strReturn = "linh " + strReturn;
                        if (chuc == 1) strReturn = "mười " + strReturn;
                        if (chuc > 1) strReturn = so[chuc] + " mươi " + strReturn;
                    }
                    if (tram < 0) break;//Hết số
                    else
                    {
                        if ((tram > 0) || (chuc > 0) || (donvi > 0)) strReturn = so[tram] + " trăm " + strReturn;
                    }
                    strReturn = " " + strReturn;
                }
            }
            if (booAm) strReturn = "Âm " + strReturn;
            string result = strReturn.Trim().Substring(0, 1).ToUpper() + strReturn.Trim().Substring(1);
            return result;
        }
        public static long ConvertToUnixTime(DateTime datetime)
        {
            DateTime sTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var utcDateTime = datetime.ToUniversalTime();
            return (long)(utcDateTime - sTime).TotalMilliseconds;
        }
    }
}
