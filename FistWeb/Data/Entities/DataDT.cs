using System.ComponentModel.DataAnnotations.Schema;

namespace FistWeb.Data.Entities
{
    [Table("users", Schema = "clothings")]
    public class users
    {
        [Column("userid")]
        public long UserId { get; set; }

        [Column("fullname")]
        public string Username { get; set; }

        [Column("facebookphone")]
        public string Phone { get; set; }

        [Column("CrateDate")]
        public DateTime CreateDate { get; set; }
    }

    [Table("orders", Schema = "clothings")]
    public class Order
    {
        [Column("orderid")]
        public long OrderId { get; set; }
        [Column("userid")]
        public long UserId { get; set; }
        [Column("totalamount")]
        public decimal TotalAmount { get; set; }
        [Column("borrowdate")]
        public DateTime BorrowDate { get; set; }
        [Column("returndate")]
        public DateTime ReturnDate { get; set; }
        [Column("status")]
        public string Status { get; set; }
        [Column("moneycoc")]
        public decimal MoneyCoc { get; set; }
        [Column("productid")]
        public long ProductId { get; set; }
        [Column("qty")]
        public int Qty { get; set; }
        [Column("note")]
        public string Note { get; set; }
        [Column("tienphatsinh")]
        public decimal TienPhatSinh { get; set; }
        [Column("lastmoney")]
        public decimal LastMoney { get; set; }
    }

    [Table("paramate", Schema = "clothings")]
    public class Paramater
    {
        [Column("id")]
        public int id { get; set; }

        [Column("function_name")]
        public string FunctionName { get; set; }

        [Column("item_key1")]
        public string item_key1 { get; set; }

        [Column("item_key2")]
        public string item_key2 { get; set; }

        [Column("item_key3")]
        public string? item_key3 { get; set; }

        [Column("item_key4")]
        public string? item_key4 { get; set; }

        [Column("imageid")]
        public long? imageid { get; set; }
    }

    [Table("products", Schema = "clothings")]
    public class Products
    {
        [Column("productid")]
        public long productid { get; set; }
        [Column("productname")]
        public string productname { get; set; }
        [Column("description")]
        public string? description { get; set; }
        [Column("priceperday")]
        public decimal priceperday { get; set; }
        [Column("stockquantity")]
        public int stockquantity { get; set; }
        [Column("createdate")]
        public DateTime createdate { get; set; }
        [Column("size")]
        public string size { get; set; }
        [Column("type_production")]
        public string type_production { get; set; }
        [Column("saveqty")]
        public int saveqty { get; set; }
    }

    public class ProductsAmy
    {
        [Column("productid")]
        public long productid { get; set; }
        [Column("productname")]
        public string productname { get; set; }
        [Column("description")]
        public string? description { get; set; }
        [Column("priceperday")]
        public decimal priceperday { get; set; }
        [Column("stockquantity")]
        public int stockquantity { get; set; }
        [Column("createdate")]
        public DateTime createdate { get; set; }
        [Column("size")]
        public string size { get; set; }
        [Column("type_production")]
        public string type_production { get; set; }
        [Column("saveqty")]
        public int saveqty { get; set; }
    }

    [Table("ordersamy", Schema = "clothings")]
    public class OrderAmy
    {
        [Column("orderid")]
        public long OrderId { get; set; }
        [Column("userid")]
        public long UserId { get; set; }
        [Column("totalamount")]
        public decimal TotalAmount { get; set; }
        [Column("borrowdate")]
        public DateTime BorrowDate { get; set; }
        [Column("returndate")]
        public DateTime ReturnDate { get; set; }
        [Column("status")]
        public string Status { get; set; }
        [Column("moneycoc")]
        public decimal MoneyCoc { get; set; }
        [Column("productid")]
        public long ProductId { get; set; }
        [Column("qty")]
        public int Qty { get; set; }
        [Column("note")]
        public string Note { get; set; }
        [Column("tienphatsinh")]
        public decimal TienPhatSinh { get; set; }
        [Column("lastmoney")]
        public decimal LastMoney { get; set; }
    }
}