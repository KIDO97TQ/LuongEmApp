using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace FistWeb.Data.Entities
{
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
}
