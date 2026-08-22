using FistWeb.Data.Entities;
using Microsoft.AspNetCore.Components.Forms;
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
    public class ListParamaterMakeup
    {
        public string KeyPara { get; set; }
        public string keyData1 { get; set; }
        public string? keyData2 { get; set; }
        public long? imageid { get; set; }
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

    public class RentalSummaryMakeup
    {
        public DateTime Date { get; set; }
        public string Type { get; set; }
        public decimal Reverue { get; set; }
    }

    public class TotalDoanhThu
    {
        public DateTime Date { get; set; }
        public string Type { get; set; }
        public decimal Reverue { get; set; }
    }

    public class InfoMakeUp
    {
        public string namekh { get; set; }           
        public string type { get; set; }               
        public decimal price { get; set; }
        public DateTime createdate { get; set; }
    }

    public class RentalSummaryChup
    {
        public DateTime Date { get; set; }
        public string Type { get; set; }
        public decimal Reverue { get; set; }
    }

    public class ListInfoGoiChup
    {
        public int id { get; set; }
        public string idorder { get; set; }
        public string namekh { get; set; }
        //public string type { get; set; }
        //public string Photograper { get; set; }
        public decimal price { get; set; }
        public DateTime dateChup { get; set; }
        public DateTime dateTraFile { get; set; }
        //public DateTime? dateCuoi { get; set; }
        public string note { get; set; }
        public DateTime createdate { get; set; }
        public long imageid { get; set; }
        public int qty { get; set; }
        public string? NameThoMake { get; set; }
        public string? NameThoToc { get; set; }
        public string? NVNhanJob { get; set; }
        public string? ImageUrl { get; set; }
    }

    public class ImageUploadResult
    {
        public IBrowserFile File { get; set; }
        public string Base64 { get; set; }
        public string PreviewUrl { get; set; }
    }

    public class UpdateProductWithImage
    {
        public Data.DTOs.ListInfoGoiChup? Product { get; set; }
        public byte[] FileBytes { get; set; }
    }

    public class WeddingDataResponse
    {
        public List<ListInfoGoiChup> Items { get; set; } = new();
        public int CountMakeup { get; set; }
        public int CountHair { get; set; }
        public int CountJob { get; set; }
    }

    #region Quan li chi tieu
    public class CategoriesInfo
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = "";
        public string Icon { get; set; } = "";
        public bool IsActive { get; set; }
        public string? Description { get; set; }
    }

    public class ExpenseInfo
    {
        public Guid Id { get; set; }
        public string User { get; set; }
        public DateTime ExpenseDate { get; set; }
        public decimal Amount { get; set; }
        public Guid CategoryId { get; set; }
        public string? Description { get; set; }
        public string? Note { get; set; }
    }

    public class DashboardCategoryInfo
    {
        public Guid CategoryId { get; set; }
        public string Name { get; set; } = "";
        public string Icon { get; set; } = "";
        public decimal Amount { get; set; }
        public int Count { get; set; }
    }

    public class ReportCategoryInfo
    {
        public Guid CategoryId { get; set; }
        public string Name { get; set; } = "";
        public string Icon { get; set; } = "";
        public decimal Amount { get; set; }
        public int Count { get; set; }
        public decimal Percent { get; set; }
    }
    #endregion
}
