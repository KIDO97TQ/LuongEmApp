namespace FistWeb.Data.DTOs
{
    public class UserOrderDto
    {
        public string Username { get; set; }
        public string phone { get; set; }
        public long OrderId { get; set; }
        public decimal TotalAmount { get; set; }
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
        public int? qty { get; set; }                  //qty as Số_Lượng,
        public decimal? totalamount { get; set; }      //totalamount as Tổng_Tiền,
        public decimal? priceperday { get; set; }      //priceperday as Giá_Thuê_1Ngày,
        public decimal? moneycoc { get; set; }         //moneycoc as Tiền_Cọc,
        public decimal? tienphatsinh { get; set; }     //tienphatsinh as Tiền_Phát_Sinh,
        public string? status { get; set; }            //status as Trạng_Thái

    }
}
