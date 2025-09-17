namespace FistWeb.Components.DTOs
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
}
