using FistWeb.Data;
using FistWeb.Data.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Npgsql;
using System.Data;
using System.Text;

namespace FistWeb.Data.Services
{
    public class CallService : IUserService, IThongKeService, GetListThueDo, SumGetListThueDo
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

        public async Task<List<RentalSummary>> SumGetListThueDo(string status, int year, int? month = null)
        {
            List<NpgsqlParameter> parameters = new List<NpgsqlParameter>();
            StringBuilder sql = new StringBuilder();

            try
            {
                sql.Append(@" SELECT 
                               DATE(b.borrowdate) AS Date,
                               p.type_production as Type,
                               SUM(b.qty) AS Quantity
                           FROM clothings.orders b
                           JOIN clothings.products p ON b.productid = p.productid
                           WHERE EXTRACT(YEAR FROM b.borrowdate) = :year ");

            parameters.Add(new NpgsqlParameter("year", year));

            if (month != null)
            {
                sql.Append(" AND EXTRACT(MONTH FROM b.borrowdate) = :month ");
                parameters.Add(new NpgsqlParameter("month", month));
            }

            if (status != "ALL")
            {
                sql.Append(" AND b.status = :status ");
                parameters.Add(new NpgsqlParameter("status", status));
            }

            sql.Append(" GROUP BY rental_date, p.type_production ORDER BY rental_date");

            return await _context.Set<RentalSummary>()
                    .FromSqlRaw(sql.ToString(), parameters.ToArray())
                    .ToListAsync();
            }
            catch (Exception ex) { }
            return new List<RentalSummary>();
        }

        public async Task<List<InfoThueDoDto>> GetListThueDo(string status, int year, int? month = null)
        {
            List<NpgsqlParameter> parameters = new List<NpgsqlParameter>();

            StringBuilder sql = new StringBuilder(@" SELECT fullname,
                                                    facebookphone,
                                                    borrowdate,
                                                    returndate,
                                                    type_production,
                                                    size,
                                                    qty,
                                                    totalamount,
                                                    priceperday,
                                                    moneycoc,
                                                    tienphatsinh,
                                                    status
                                             FROM clothings.orders b
                                             JOIN clothings.products p ON b.productid = p.productid
                                             JOIN clothings.users u ON u.userid = b.userid
                                             WHERE EXTRACT(YEAR FROM b.borrowdate) = @year");

            parameters.Add(new NpgsqlParameter("year", year));

            if (month != null)
            {
                sql.Append(" AND EXTRACT(MONTH FROM b.borrowdate) = @month ");
                parameters.Add(new NpgsqlParameter("month", month));
            }

            if (status != "ALL")
            {
                sql.Append(" AND b.status = @status ");
                parameters.Add(new NpgsqlParameter("status", status));
            }

            return await _context.Set<InfoThueDoDto>()
                    .FromSqlRaw(sql.ToString(), parameters.ToArray())
                    .ToListAsync();
        }

    }
}
