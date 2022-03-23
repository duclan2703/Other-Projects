using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parse.Core
{
    public static class NumberToLeter
    {
        static string[] ChuSo = new string[10] { " không", " một", " hai", " ba", " bốn", " năm", " sáu", " bảy", " tám", " chín" };
        static string[] Tien = new string[6] { "", " nghìn", " triệu", " tỷ", " nghìn tỷ", " triệu tỷ" };
        // Hàm đọc số thành chữ
        public static string DocTienBangChu(decimal SoTien)
        {
            string[] part = new string[2];
            var lstSoTien = SoTien.ToString().Split('.').Select(decimal.Parse).ToList<decimal>();

            if (lstSoTien.Count == 1 || lstSoTien[1] == 0)
                return DocCacSoRaChu(lstSoTien[0]) + " đồng";
            else
                return DocCacSoRaChu(lstSoTien[0]) + " phẩy " + DocCacSoRaChu(lstSoTien[1]).ToLower() + " đồng";
        }

        public static string DocCacSoRaChu(decimal SoTien)
        {
            int lan, i;
            decimal so;
            string KetQua = "", tmp = "";
            int[] ViTri = new int[6];
            if (SoTien < 0) return "Số tiền âm.";
            if (SoTien == 0) return "Không";
            if (SoTien > 0)
            {
                so = SoTien;
            }
            else
            {
                so = -SoTien;
            }
            //Kiểm tra số quá lớn
            if (SoTien > 8999999999999999)
            {
                SoTien = 0;
                return "";
            }
            ViTri[5] = (int)(so / 1000000000000000);
            so = so - long.Parse(ViTri[5].ToString()) * 1000000000000000;
            ViTri[4] = (int)(so / 1000000000000);
            so = so - long.Parse(ViTri[4].ToString()) * +1000000000000;
            ViTri[3] = (int)(so / 1000000000);
            so = so - long.Parse(ViTri[3].ToString()) * 1000000000;
            ViTri[2] = (int)(so / 1000000);
            ViTri[1] = (int)((so % 1000000) / 1000);
            ViTri[0] = (int)(so % 1000);
            if (ViTri[5] > 0)
            {
                lan = 5;
            }
            else if (ViTri[4] > 0)
            {
                lan = 4;
            }
            else if (ViTri[3] > 0)
            {
                lan = 3;
            }
            else if (ViTri[2] > 0)
            {
                lan = 2;
            }
            else if (ViTri[1] > 0)
            {
                lan = 1;
            }
            else
            {
                lan = 0;
            }
            for (i = lan; i >= 0; i--)
            {
                bool isDoc = false;
                if (ViTri[i].ToString().Length < 3 && i < lan)
                {
                    string testing = ViTri[i].ToString();
                    isDoc = true;
                    //ViTri[i] = AddZero(ViTri[i]);
                }
                tmp = DocSo3ChuSo(ViTri[i], isDoc);
                isDoc = false;
                KetQua += tmp;
                if (ViTri[i] != 0) KetQua += Tien[i];
                if ((i > 0) && (!string.IsNullOrEmpty(tmp))) KetQua += "";//&& (!string.IsNullOrEmpty(tmp))
            }
            if (KetQua.Substring(KetQua.Length - 1, 1) == ",") KetQua = KetQua.Substring(0, KetQua.Length - 1);
            KetQua = KetQua.Trim();
            return KetQua.Substring(0, 1).ToUpper() + KetQua.Substring(1);
        }
        static string AddZero(string str)
        {
            if (str.Length == 2)
                str = "0" + str;
            else if (str.Length == 1)
                str = "00" + str;
            return str;

        }

        // Hàm đọc số có 3 chữ số
        static string DocSo3ChuSo(int baso, bool isDoc0)
        {
            int tram, chuc, donvi;
            string KetQua = "";
            tram = (int)(baso / 100);
            chuc = (int)((baso % 100) / 10);
            donvi = baso % 10;
            if ((tram == 0) && (chuc == 0) && (donvi == 0)) return "";
            if (tram != 0 || isDoc0)
            {
                KetQua += ChuSo[tram] + " trăm";
                if ((chuc == 0) && (donvi != 0)) KetQua += " linh";
            }
            if ((chuc != 0) && (chuc != 1))
            {
                KetQua += ChuSo[chuc] + " mươi";
                if ((chuc == 0) && (donvi != 0)) KetQua = KetQua + " linh";
            }
            if (chuc == 1) KetQua += " mười";
            switch (donvi)
            {
                case 1:
                    if ((chuc != 0) && (chuc != 1))
                    {
                        KetQua += " mốt";
                    }
                    else
                    {
                        KetQua += ChuSo[donvi];
                    }
                    break;
                case 5:
                    if (chuc == 0)
                    {
                        KetQua += ChuSo[donvi];
                    }
                    else
                    {
                        KetQua += " lăm";
                    }
                    break;
                default:
                    if (donvi != 0)
                    {
                        KetQua += ChuSo[donvi];
                    }
                    break;
            }
            return KetQua;
        }
    }
}
