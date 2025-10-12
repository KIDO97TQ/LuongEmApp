using FistWeb.Data.Entities;
using Newtonsoft.Json.Linq;

namespace FistWeb.Data.DTOs
{
    public class UserOrderDto
    {
        public string Username { get; set; }
        public string phone { get; set; }
        public long userid { get; set; }
    }

    public class DoanhThuThueDoDto
    {
        public DateTime rental_date { get; set; }
        public string product_type { get; set; }
        public decimal revenue { get; set; }
    }

    public class RentalSummary
    {
        public DateTime Date { get; set; }
        public string Type { get; set; }
        public int Quantity { get; set; }
    }

    public class InfoThueDoDto
    {
        public string fullname { get; set; }          //fullname as Tên_Người_Thuê,
        public string facebookphone { get; set; }     //facebookphone as Liên_Hệ,
        public DateTime borrowdate { get; set; }      //borrowdate as Ngày_Thuê,
        public DateTime? returndate { get; set; }      //returndate as Ngày_Trả,
        public string? type_production { get; set; }   //type_production as Loại_Đồ,
        public string? size { get; set; }                 //size,
        public int qty { get; set; }                  //qty as Số_Lượng,
        public decimal totalamount { get; set; }      //totalamount as Tổng_Tiền,
        public decimal priceperday { get; set; }      //priceperday as Giá_Thuê_1Ngày,
        public decimal moneycoc { get; set; }         //moneycoc as Tiền_Cọc,
        public decimal tienphatsinh { get; set; }     //tienphatsinh as Tiền_Phát_Sinh,
        public string? status { get; set; }            //status as Trạng_Thái
        public long orderid { get; set; }          //orderid as Mã_Đơn_Hàng
        public long productid { get; set; }
    }

    public class ListParamater
    {
        public string KeyPara { get; set; }
        public string keyData { get; set; }
    }

    public class ListParaUser
    {
        public string keyUsername { get; set; }
    }

    public class ListParaSP
    {
        public string KeyPara { get; set; }
        public string keyUsername { get; set; }
        public string keyUserpass { get; set; }
    }

    public class ProductStock
    {
        public string type_production { get; set; }
        public int total_quantity { get; set; }
    }

    public class ProductItem
    {
        public string TypeSP { get; set; }
        public decimal TienCoc { get; set; }
        public int QTYThue { get; set; }
        public string Notes { get; set; }
        public decimal PricePerDay { get; set; }
        public string Size { get; set; }
        public long ProductID { get; set; }
        public decimal TongTienThue { get; set; }
        public decimal TongTienThueNonCoc { get; set; }
    }

    public class ProductImageDto
    {
        public string ImageUrl { get; set; }
        public string TypeSP { get; set; }
        public string NameSP { get; set; }
        public long ProductID { get; set; }
        public decimal Price { get; set; }
        public string Size { get; set; }
        public string? Desc { get; set; }
        public int StockQTY { get; set; }
        public int SaveQTY { get; set; }
    }

    public class OrderDetailDto
    {
        public long idorder { get; set; }
        public long bookingid { get; set; }
        public decimal lastmoney { get; set; }
        public int QTYThue { get; set; }
    }
}
