using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceService.Models
{
    public class ProductObj
    {
        public string ProductCode { get; set; }
        public string StockingSKU { get; set; }
        public string Location { get; set; }
        public string ProductType { get; set; }
        public string ItemDescription { get; set; }
        public string ItemLocalDescription { get; set; }
        public string UOM { get; set; }
        public string Item_Type_Code { get; set; }
        public string Lot_Number { get; set; }
        public string QtyOrdered { get; set; }
        public string QtyReleased { get; set; }
        public string Past_Qty_Released { get; set; }
        public string SplitLineWarehouse { get; set; }
        public string Vat_Line_Tax { get; set; }
        public string Line_Tax { get; set; }
        public string UnitTax { get; set; }
        public string UnitPrice { get; set; }
        public string UnitDisc { get; set; }
        public string UnitPH { get; set; }
        public string UnitFr { get; set; }
        public string UnitLog { get; set; }
        public string UnitOth { get; set; }
        public string ExtendedPrice { get; set; }
        public string DiscountAmt { get; set; }
        public string DiscRetail { get; set; }
        public string TaxRate_1 { get; set; }
        public string TaxRate_2 { get; set; }
        public string TaxRate_3 { get; set; }
        public string UnitWeight { get; set; }
        public string Hazardous_Item_Flag { get; set; }
        public string NonInventory_Item_Flag { get; set; }
        public string VolumnPoints { get; set; }
        public string EarnBase { get; set; }
        public string AlchoholPercent { get; set; }
        public string CountryOfOrigin { get; set; }
        public string OrderLine_Id { get; set; }
        public string QuantityReleased { get; set; }
        public string QuantityOrdered { get; set; }
        public string Product_Type { get; set; }
        public string Unit_Price { get; set; }
        public string Extended_Price { get; set; }
        public string Unit_Tax_Rate { get; set; }
        public string PickList_Id { get; set; }
    }
}
