using FistWeb.Components.DTOs;
using FistWeb.Data;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System.Text;

namespace FistWeb.Components.Services
{
    public class CallService : IUserService, IThongKeService
    {
        private readonly AppDbContext _context;

        public CallService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<UserOrderDto>> GetUserOrdersAsync()
        {
            var query = from u in _context.Users
                        join o in _context.Order
                            on u.UserId equals o.UserId
                        select new UserOrderDto
                        {
                            Username = u.Username,
                            phone = u.Phone,
                            OrderId = o.OrderId,
                            TotalAmount = o.TotalAmount
                        };

            return await query.ToListAsync();
        }

        public async Task<List<DoanhThuThueDoDto>> GetDoanhThuThueDoUocTinhAsync(string typesp, int year, int? month = null, int? day = null)
        {
            try
            {
                var parameters = new List<NpgsqlParameter>
                {
                    new NpgsqlParameter("nam", year)
                };

                var sql = new StringBuilder(@" SELECT o.borrowdate::date AS rental_date,
                                                  p.type_production AS product_type,
                                                  SUM(o.totalamount) AS revenue
                                           FROM clothings.orders o 
                                           JOIN clothings.products p ON p.productid = o.productid
                                           WHERE (:nam IS NULL OR EXTRACT(YEAR FROM o.borrowdate) = :nam)");

                if (day.HasValue)
                {
                    sql.Append(" AND (:ngay IS NULL OR EXTRACT(DAY FROM o.borrowdate) = :ngay) ");
                    parameters.Add(new NpgsqlParameter("ngay", day));
                }

                if (month.HasValue)
                {
                    sql.Append(" AND (:thang IS NULL OR EXTRACT(MONTH FROM o.borrowdate) = :thang) ");
                    parameters.Add(new NpgsqlParameter("thang", month));
                }

                if (!string.IsNullOrWhiteSpace(typesp))
                {
                    sql.Append(" AND p.type_production = :typesp ");
                    parameters.Add(new NpgsqlParameter("typesp", typesp));
                }
                sql.Append(" GROUP BY rental_date, product_type ORDER BY rental_date ");

                return await _context.Set<DoanhThuThueDoDto>()
                    .FromSqlRaw(sql.ToString(), parameters.ToArray())
                    .ToListAsync();
            }
            catch (Exception ex) { }
            return new List<DoanhThuThueDoDto>();

        }

    }
}
