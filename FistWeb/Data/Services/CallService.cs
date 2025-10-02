using FistWeb.Data;
using FistWeb.Data.DTOs;
using FistWeb.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using Npgsql;
using System;
using System.Data;
using System.Diagnostics;
using System.Text;

namespace FistWeb.Data.Services
{
    public class CallService : IThongKeService, GetListThueDo, SumGetListThueDo, IGetParaUserService, IGetParamaterService, IAddParaService, IGetUserIDService,
    IDeleteParaService, IInsertSPService, IGetSumWHService, IGetUserInfoService, IGetProductIDService, IStockQTYService, IInserUserService, IInsertOrdersService,
    IUpdateReturnOderService
    {
        private readonly AppDbContext _context;

        public CallService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<UserOrderDto>> GetUserInfo()
        {
            var users = await _context.Users
                .Select(user => new UserOrderDto
                {
                    Username = user.Username,
                    phone = user.Phone
                })
                .ToListAsync();

            return users;
        }

        public async Task<long> GetUserID(string ContactKH)
        {
            long userID = await _context.Users
                .Where(u => u.Phone == ContactKH)
                .Select(u => u.UserId)
                .FirstOrDefaultAsync();
            return userID;
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
                                                    status, orderid, b.productid
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

        public async Task<List<ListParamater>> GetParamater()
        {
            var query = from u in _context.Paramater
                        select new ListParamater
                        {
                            KeyPara = u.FunctionName,
                            keyData = u.item_key1
                        };

            return await query.ToListAsync();
        }

        public async Task<List<ListParaUser>> GetLoginUser(string fun, string user, string? pass)
        {
            List<NpgsqlParameter> parameters = new List<NpgsqlParameter>();

            StringBuilder sql = new StringBuilder(@" SELECT b.item_key1
                                             FROM clothings.paramater b
                                             WHERE b.function_name = @function_name ");

            parameters.Add(new NpgsqlParameter("function_name", fun));

            if (fun == "type")
            {
                sql.Append(" and UPPER(b.item_key1)=@taikhoan ");
                parameters.Add(new NpgsqlParameter("taikhoan", user));
            }
            else if (fun == "tocken")
            {
            }
            else
            {
                sql.Append(" and b.item_key1=@taikhoan ");
                parameters.Add(new NpgsqlParameter("taikhoan", user));
            }

            if (pass != null)
            {
                sql.Append(" and b.item_key2=@matkau ");
                parameters.Add(new NpgsqlParameter("matkau", pass));
            }

            return await _context.ListParaUsers
                           .FromSqlRaw(sql.ToString(), parameters.ToArray())
                           .ToListAsync();
        }

        public async Task<int> InsertParamaterRawAsync(string fun, string key1, string? key2)
        {
            var sql = new StringBuilder(@"INSERT INTO clothings.paramater (function_name, item_key1, item_key2)
                                               VALUES (@function_name, @item_key1, @item_key2)");

            var parameters = new List<NpgsqlParameter>
            {
                new NpgsqlParameter("function_name", fun),
                new NpgsqlParameter("item_key1", key1),
                new NpgsqlParameter("item_key2", (object?)key2 ?? DBNull.Value)
            };

            return await _context.Database.ExecuteSqlRawAsync(sql.ToString(), parameters.ToArray());
        }

        public async Task<int> DeleteParamaterRawAsync(string fun, string key1, string? key2 = null)
        {
            var sql = new StringBuilder(@"DELETE FROM clothings.paramater
                                                WHERE function_name = @function_name AND item_key1 = @item_key1");

            var parameters = new List<NpgsqlParameter>
            {
                new NpgsqlParameter("function_name", fun),
                new NpgsqlParameter("item_key1", key1)
            };

            if (!string.IsNullOrEmpty(key2))
            {
                sql.Append(" AND item_key2 = @item_key2");
                parameters.Add(new NpgsqlParameter("item_key2", key2));
            }

            return await _context.Database.ExecuteSqlRawAsync(sql.ToString(), parameters.ToArray());
        }

        public async Task<int> InserProduct(long productID, string nameSP, string? DescSP, decimal PriceSP, int QtySP, string sizeSP, string typeSP)
        {
            TimeZoneInfo vnZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            DateTime gioVN = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, vnZone);
            var sql = new StringBuilder(@"INSERT INTO clothings.products (ProductID, ProductName, Description, PricePerDay, StockQuantity, Size, Type_production, saveqty,createdate) 
                         VALUES (@productID, @productName, @description, @priceperday, @stockquantity, @size, @typeproduct, @qtysave,@createdate)");

            var parameters = new List<NpgsqlParameter>
            {
                new NpgsqlParameter("productID", productID),
                new NpgsqlParameter("productName", nameSP),
                new NpgsqlParameter("description", (object?)DescSP ?? DBNull.Value),
                new NpgsqlParameter("priceperday", PriceSP),
                new NpgsqlParameter("stockquantity", QtySP),
                new NpgsqlParameter("size", sizeSP),
                new NpgsqlParameter("typeproduct", typeSP),
                new NpgsqlParameter("qtysave", QtySP),
                new NpgsqlParameter("createdate", gioVN)
            };
            return await _context.Database.ExecuteSqlRawAsync(sql.ToString(), parameters.ToArray());
        }

        public async Task<List<ProductStock>> GetTotalWH(bool all, bool rdNotReturn, string? typeSP = null)
        {
            List<NpgsqlParameter> parameters = new List<NpgsqlParameter>();
            StringBuilder sql = new StringBuilder();

            // Chọn cột cần group
            string groupByColumn = (typeSP == null) ? "type_production" : "productname";
            string whereClause = (typeSP == null) ? "" : "WHERE type_production = :type_production";

            if (typeSP != null)
            {
                parameters.Add(new NpgsqlParameter("type_production", typeSP));
            }

            if (all)
            {
                sql.Append($@"SELECT {groupByColumn} AS type_production, SUM(saveqty) AS total_quantity
                              FROM clothings.products
                              {whereClause}
                              GROUP BY {groupByColumn}
                              ORDER BY {groupByColumn}");
            }
            else if (rdNotReturn)
            {
                sql.Append($@" SELECT {groupByColumn} AS type_production, SUM(stockquantity - saveqty) AS total_quantity
                                 FROM clothings.products
                                 {whereClause}
                                 GROUP BY {groupByColumn}
                                 ORDER BY {groupByColumn}");
            }
            else
            {
                sql.Append($@"SELECT {groupByColumn} AS type_production, SUM(stockquantity) AS total_quantity
                                FROM clothings.products
                                {whereClause}
                                GROUP BY {groupByColumn}
                                ORDER BY {groupByColumn}");
            }

            return await _context.ProductStock
                                       .FromSqlRaw(sql.ToString(), parameters.ToArray())
                                       .ToListAsync();
        }

        public async Task<List<ProductImageDto>> GetProductID(string typeProduction)
        {
            var products = await _context.Products
                .Where(p => p.type_production == typeProduction)
                .Select(p => new ProductImageDto
                {
                    ProductID = p.productid,
                    Price = p.priceperday,
                    Size = p.size,
                    Desc = p.description,
                    StockQTY = p.stockquantity,
                    SaveQTY = p.saveqty,
                    ImageUrl = p.productid + ".jpg"
                })
                .ToListAsync();
            return products;
        }

        public async Task<int> GetStockQTY(long idproduct)
        {
            int stock = await _context.Products
                .Where(p => p.productid == idproduct)
                .Select(p => p.saveqty)
                .FirstOrDefaultAsync();

            return stock;
        }

        public async Task<int> InsertUser(long id, string NameKach, string SdtKhach)
        {
            var sql = new StringBuilder(@"INSERT INTO clothings.users (userid, fullname, facebookphone) VALUES (@userid, @fullname, @facebookphone)");

            var parameters = new List<NpgsqlParameter>
            {
                new NpgsqlParameter("userid", id),
                new NpgsqlParameter("fullname", NameKach),
                new NpgsqlParameter("facebookphone", SdtKhach)
            };
            return await _context.Database.ExecuteSqlRawAsync(sql.ToString(), parameters.ToArray());
        }

        public async Task<int> InsertOrder(long userID, decimal TotalPrice, decimal Tiencoc, long productID, int QTYThue, string notes)
        {
            TimeZoneInfo vnZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            DateTime gioVN = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, vnZone);
            long orderId = long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss"));

            string sql = @"
                            INSERT INTO clothings.orders 
                                (orderid, userid, totalamount, status, moneycoc, productid, qty, notes, tienphatsinh, borrowdate) 
                            VALUES 
                                (@orderid, @userid, @totalamount, 'BORROW', @moneycoc, @productid, @qty, @notes, @tienphatsinh, @borrowdate);

                            UPDATE clothings.products 
                            SET saveqty = saveqty - @qty 
                            WHERE productid = @productid; ";

            var parameters = new[]
            {
                 new NpgsqlParameter("@orderid", orderId),
                 new NpgsqlParameter("@userid", userID),
                 new NpgsqlParameter("@totalamount", TotalPrice),
                 new NpgsqlParameter("@moneycoc", Tiencoc),
                 new NpgsqlParameter("@productid", productID),
                 new NpgsqlParameter("@qty", QTYThue),
                 new NpgsqlParameter("@notes", notes),
                 new NpgsqlParameter("@tienphatsinh", NpgsqlTypes.NpgsqlDbType.Numeric) { Value = 0m },
                 new NpgsqlParameter("@borrowdate", gioVN)
            };

            return await _context.Database.ExecuteSqlRawAsync(sql, parameters);
        }

        public async Task<int> UpdateReturnOrder(long orderId, decimal? lastmoney, long productid, int QTYThue, string status)
        {
            TimeZoneInfo vnZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            DateTime gioVN = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, vnZone);
            string sql = "";

            if (status == "RETURN")
                sql = @"UPDATE clothings.orders SET status='RETURN', returndate=@timereturn, lastmoney = totalamount 
                         WHERE orderid=@idorder;

                        UPDATE clothings.products SET  saveqty=saveqty + :sl where productid=:bookingid; ";
            else
                sql = @"UPDATE clothings.orders SET status='CANCEL', returndate=@timereturn, lastmoney = totalamount 
                             WHERE orderid=@idorder;

                            UPDATE clothings.products SET  saveqty=saveqty + :sl where productid=:bookingid; ";

            var parameters = new[]
            {
                 new NpgsqlParameter("@idorder", orderId),
                 new NpgsqlParameter("@lastmoney", NpgsqlTypes.NpgsqlDbType.Numeric) { Value = lastmoney ?? 0m },
                 new NpgsqlParameter("@timereturn", gioVN),
                 new NpgsqlParameter("@bookingid", productid),
                 new NpgsqlParameter("@sl", QTYThue)
            };

            return await _context.Database.ExecuteSqlRawAsync(sql, parameters);
        }
    }
}
