using FistWeb.Data;
using FistWeb.Data.DTOs;
using FistWeb.Data.Entities;
using Google.Apis.Drive.v3.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using Npgsql;
using System;
using System.Data;
using System.Diagnostics;
using System.IO.Pipelines;
using System.Text;

namespace FistWeb.Data.Services
{
    public class CallService : IThongKeService, GetListThueDo, SumGetListThueDo, IGetParaUserService, IGetParamaterService, IAddParaService, IGetUserIDService,
    IDeleteParaService, IInsertSPService, IGetSumWHService, IGetUserInfoService, IGetProductIDService, IStockQTYService, IInserUserService, IInsertOrdersService,
    IUpdateReturnOderService, IUpdatePWService, IDeleteProductService, IUpdateProductByIdService, IUpdateReturnAllOrderService, IGetUserInfo1Service, IUpdateUserService,
    UpdateReturnAllOrder1, IGetParamaterMakeupService, IInsertRevenueService, IGetSumRevenueService, IGetListMakeupService, IGetTotalDoanhThuService, IGetSumWHAmyService,
    IInsertSPAmyService, IGetProductIDAmyService, IUpdateProductByIdAmyService, IDeleteProductAmyService, IInsertRevenueAmyService, IGetSumRevenueAmyService, IGetListMakeupAmyService,
    IGetTotalDoanhThuAmyService, GetListThueDoAmy, IUpdateReturnOderAmyService, IUpdateReturnAllOrderAmyService, UpdateReturnAllOrder1Amy, IInsertOrdersAmyService,
    IStockQTYAmyService, IInsertRevenueWeddingService, IGetListWeddingService, IGetSumRevenueWeddingService, IAddParaWeddingService, IUpdateLichChupWedding, IUpdateOrderWeddingByIdService,
        IGetProductID1Service, IGetProductID1AmyService, GetQTYListThueDoAmy
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
                    phone = user.Phone,
                    userid = user.UserId
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

        public async Task<List<UserOrderDto>> GetUserInfo1(string ContactKH)
        {
            var users = await _context.Users
                .Where(user => user.Phone == ContactKH)
                .Select(user => new UserOrderDto
                {
                    Username = user.Username,
                    phone = user.Phone,
                    userid = user.UserId
                })
                .ToListAsync();

            return users;
        }
        #region Luong
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
            sql.Append(" order by borrowdate desc ");
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

        public async Task<List<ListParamaterMakeup>> GetParamaterMakeUp()
        {
            var query = from u in _context.Paramater
                        select new ListParamaterMakeup
                        {
                            KeyPara = u.FunctionName,
                            keyData1 = u.item_key1,
                            keyData2 = u.item_key2 ?? "",
                            imageid = u.imageid
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

            if (fun == "type" || fun == "typeAmy" || fun == "goiChupWedding")
            {
                sql.Append(" and UPPER(b.item_key1)=@taikhoan ");
                parameters.Add(new NpgsqlParameter("taikhoan", user));
            }
            else if (fun == "tocken" || fun == "background")
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
                new NpgsqlParameter("item_key2", (object?)key2 ?? " ")
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
                new NpgsqlParameter("description", (object?)DescSP ?? ""),
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
                .Where(p => p.type_production == typeProduction && p.saveqty > 0)
                .OrderByDescending(p => p.createdate)
                .Select(p => new ProductImageDto
                {
                    ProductID = p.productid,
                    Price = p.priceperday,
                    Size = p.size,
                    Desc = p.description ?? "",
                    StockQTY = p.stockquantity,
                    SaveQTY = p.saveqty,
                    ImageUrl = p.productid + ".jpg",
                    TypeSP = p.type_production,
                    NameSP = p.productname
                })
                .ToListAsync();
            return products;
        }

        public async Task<List<ProductImageDto>> GetProductID1(string typeProduction)
        {
            var products = await _context.Products
                .Where(p =>
                    p.saveqty > 0 &&
                    (string.IsNullOrEmpty(typeProduction) || p.type_production == typeProduction)
                )
                .OrderByDescending(p => p.createdate)
                .Select(p => new ProductImageDto
                {
                    ProductID = p.productid,
                    Price = p.priceperday,
                    Size = p.size,
                    Desc = p.description ?? "",
                    StockQTY = p.stockquantity,
                    SaveQTY = p.saveqty,
                    ImageUrl = p.productid + ".jpg",
                    TypeSP = p.type_production,
                    NameSP = p.productname
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

        public async Task<int> InsertOrder(long UserID, List<Data.DTOs.ProductItem> products)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                int totalInserted = 0;

                TimeZoneInfo vnZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
                DateTime gioVN = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, vnZone);

                string sql = @"INSERT INTO clothings.orders 
                                   (orderid, userid, totalamount, status, moneycoc, productid, qty, notes, tienphatsinh, borrowdate) 
                               VALUES 
                                   (@orderid, @userid, @totalamount, 'BORROW', @moneycoc, @productid, @qty, @notes, @tienphatsinh, @borrowdate);

                               UPDATE clothings.products 
                               SET saveqty = saveqty - @qty 
                               WHERE productid = @productid;";

                foreach (var order in products)
                {
                    long orderId = long.Parse(DateTime.Now.ToString("yyyyMMddHHmmssfff"));

                    var parameters = new[]
                    {
                        new NpgsqlParameter("@orderid", orderId),
                        new NpgsqlParameter("@userid", UserID),
                        new NpgsqlParameter("@totalamount", order.TongTienThueNonCoc),
                        new NpgsqlParameter("@moneycoc", order.TienCoc),
                        new NpgsqlParameter("@productid", order.ProductID),
                        new NpgsqlParameter("@qty", order.QTYThue),
                        new NpgsqlParameter("@notes", NpgsqlTypes.NpgsqlDbType.Text) { Value = (object?)order.Notes ?? "" },
                        new NpgsqlParameter("@tienphatsinh", NpgsqlTypes.NpgsqlDbType.Numeric) { Value = 0m },
                        new NpgsqlParameter("@borrowdate", gioVN)
                    };

                    totalInserted += await _context.Database.ExecuteSqlRawAsync(sql, parameters);
                }

                await transaction.CommitAsync();
                return totalInserted;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
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

        public async Task<int> UpdateReturnAllOrder(string sdt, string status)
        {

            var products = await (from o in _context.Order
                                  join u in _context.Users on o.UserId equals u.UserId
                                  where u.Phone == sdt && o.Status == "BORROW"
                                  select new Data.DTOs.OrderDetailDto
                                  {
                                      idorder = o.OrderId,
                                      QTYThue = o.Qty,
                                      bookingid = o.ProductId,
                                      lastmoney = o.TotalAmount
                                  }).ToListAsync();

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                int totalInserted = 0;

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

                foreach (var order in products)
                {
                    var parameters = new[]
                    {
                       new NpgsqlParameter("@idorder", order.idorder),
                       new NpgsqlParameter("@lastmoney", order.lastmoney),
                       new NpgsqlParameter("@timereturn", gioVN),
                       new NpgsqlParameter("@bookingid", order.bookingid),
                       new NpgsqlParameter("@sl", order.QTYThue)
                    };

                    totalInserted += await _context.Database.ExecuteSqlRawAsync(sql, parameters);
                }

                await transaction.CommitAsync();
                return totalInserted;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<bool> UpdatePasswordAsync(string newPassword)
        {
            try
            {
                var user = await _context.Paramater
                                .FirstOrDefaultAsync(u => u.FunctionName == "admin");

                user.item_key2 = newPassword;

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        public async Task<int> DeleteProductById(long productId)
        {
            string sql = @"DELETE from clothings.products WHERE productID=@productId";

            var parameters = new[]
            {
                 new NpgsqlParameter("@productId", productId)
            };

            return await _context.Database.ExecuteSqlRawAsync(sql, parameters);
        }

        public async Task<int> UpdateProductById(ProductImageDto updatedProduct)
        {
            string sql = @"update clothings.products set  productId=@productId, productname=@productname, description=@description, priceperday=@priceperday,
                            stockquantity=@stockquantity, size=@size, type_production=@type_production, saveqty=@saveqty
                            WHERE productID=@productId";

            var parameters = new[]
            {
                 new NpgsqlParameter("@productId", updatedProduct.ProductID),
                 new NpgsqlParameter("@productname", updatedProduct.NameSP),
                 new NpgsqlParameter("@description", updatedProduct.Desc),
                 new NpgsqlParameter("@priceperday", updatedProduct.Price),
                 new NpgsqlParameter("@stockquantity",  updatedProduct.StockQTY),
                 new NpgsqlParameter("@size", updatedProduct.Size),
                 new NpgsqlParameter("@type_production", updatedProduct.TypeSP),
                 new NpgsqlParameter("@saveqty",  updatedProduct.StockQTY)
            };
            return await _context.Database.ExecuteSqlRawAsync(sql, parameters);
        }

        public async Task<int> UpdateUser(List<Data.DTOs.UserOrderDto> userInfo, string NewNameKH)
        {
            string sql = @"update clothings.users set  fullname=@fullname
                            WHERE userid=@userid and  facebookphone=@facebookphone ";

            var parameters = new[]
            {
                 new NpgsqlParameter("@fullname", NewNameKH),
                 new NpgsqlParameter("@userid", userInfo[0].userid),
                 new NpgsqlParameter("@facebookphone", userInfo[0].phone)
            };
            return await _context.Database.ExecuteSqlRawAsync(sql, parameters);
        }

        public async Task<int> UpdateReturnAllOrder1()
        {

            var products = await (from o in _context.Order
                                  where o.Status == "BORROW"
                                  select new Data.DTOs.OrderDetailDto
                                  {
                                      idorder = o.OrderId,
                                      QTYThue = o.Qty,
                                      bookingid = o.ProductId,
                                      lastmoney = o.TotalAmount
                                  }).ToListAsync();

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                int totalInserted = 0;

                TimeZoneInfo vnZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
                DateTime gioVN = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, vnZone);

                string sql = @"UPDATE clothings.orders SET status='RETURN', returndate=@timereturn, lastmoney = totalamount 
                            WHERE orderid=@idorder;
 
                            UPDATE clothings.products SET  saveqty=saveqty + :sl where productid=:bookingid; ";

                foreach (var order in products)
                {
                    var parameters = new[]
                    {
                       new NpgsqlParameter("@idorder", order.idorder),
                       new NpgsqlParameter("@lastmoney", order.lastmoney),
                       new NpgsqlParameter("@timereturn", gioVN),
                       new NpgsqlParameter("@bookingid", order.bookingid),
                       new NpgsqlParameter("@sl", order.QTYThue)
                    };

                    totalInserted += await _context.Database.ExecuteSqlRawAsync(sql, parameters);
                }

                await transaction.CommitAsync();
                return totalInserted;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<int> InsertRevenue(string id, string NameKach, decimal price)
        {
            var sql = new StringBuilder(@"INSERT INTO clothings.revenue (idorder, namekh, priremake) VALUES (@idorder, @namekh, @pricemake)");

            var parameters = new List<NpgsqlParameter>
            {
                new NpgsqlParameter("idorder", id),
                new NpgsqlParameter("namekh", NameKach),
                new NpgsqlParameter("pricemake", price)
            };
            return await _context.Database.ExecuteSqlRawAsync(sql.ToString(), parameters.ToArray());
        }

        public async Task<List<RentalSummaryMakeup>> SumGetListMakeup(string type, int year, int? month = null, int? day = null)
        {
            List<NpgsqlParameter> parameters = new List<NpgsqlParameter>();
            StringBuilder sql = new StringBuilder();

            try
            {
                sql.Append(@" SELECT 
                               DATE(b.createdate) AS Date,
                               p.item_key1 as Type,
                               SUM(b.priremake) AS Reverue
                           FROM clothings.revenue b
                           JOIN clothings.paramater p ON b.idorder = p.item_key2
                           WHERE EXTRACT(YEAR FROM b.createdate) = :year ");

                parameters.Add(new NpgsqlParameter("year", year));

                if (day.HasValue)
                {
                    sql.Append(" AND EXTRACT(DAY FROM b.createdate) = :ngay ");
                    parameters.Add(new NpgsqlParameter("ngay", day));
                }

                if (month.HasValue)
                {
                    sql.Append(" AND EXTRACT(MONTH FROM b.createdate) = :month ");
                    parameters.Add(new NpgsqlParameter("month", month));
                }

                sql.Append(" and p.function_name= :type  GROUP BY createdate, item_key1 ORDER BY createdate");
                parameters.Add(new NpgsqlParameter("type", type));

                return await _context.Set<RentalSummaryMakeup>()
                        .FromSqlRaw(sql.ToString(), parameters.ToArray())
                        .ToListAsync();
            }
            catch (Exception ex) { }
            return new List<RentalSummaryMakeup>();
        }

        public async Task<List<InfoMakeUp>> GetListMakeup(string fun, int? year = null, int? month = null, string type = null)
        {
            List<NpgsqlParameter> parameters = new List<NpgsqlParameter>();

            StringBuilder sql = new StringBuilder(@" SELECT b.namekh, 
                                                            p.item_key1 as type,
                                                            b.priremake as price,
                                                            b.createdate
                                                        FROM clothings.revenue b
                                                        JOIN clothings.paramater p ON b.idorder = p.item_key2
                                                        WHERE 1=1 ");
            //EXTRACT(YEAR FROM b.createdate) = :year ");
            //parameters.Add(new NpgsqlParameter("year", year));

            if (month.HasValue)
            {
                sql.Append(" AND EXTRACT(MONTH FROM b.createdate) = :month ");
                parameters.Add(new NpgsqlParameter("month", month));
            }

            if (!string.IsNullOrEmpty(type) && type != "All")
            {
                sql.Append(" and p.item_key1= :type ");
                parameters.Add(new NpgsqlParameter("type", type));
            }

            sql.Append(" and p.function_name=:fun  ORDER BY createdate desc ");
            parameters.Add(new NpgsqlParameter("fun", fun));
            return await _context.Set<InfoMakeUp>()
                    .FromSqlRaw(sql.ToString(), parameters.ToArray())
                    .ToListAsync();
        }

        public async Task<List<TotalDoanhThu>> TotalDoanhThu(int year, int? month = null, int? day = null)
        {
            try
            {
                var parameters = new List<NpgsqlParameter>
                {
                    new NpgsqlParameter("year", year)
                    {
                        DbType = DbType.Int32
                    },
                    new NpgsqlParameter("day", day.HasValue ? day.Value : (object)DBNull.Value)
                    {
                        DbType = DbType.Int32
                    },
                    new NpgsqlParameter("month", month.HasValue ? month.Value : (object)DBNull.Value)
                    {
                        DbType = DbType.Int32
                    }
                };

                var sql = new StringBuilder(@"SELECT 
                                                  merged.date AS Date,
                                                  merged.type AS Type,
                                                  SUM(merged.reverue) AS Reverue
                                              FROM
                                              (
                                                  SELECT 
                                                      DATE(b.createdate) AS date,
                                                      CASE 
                                                           WHEN p.function_name = 'goiMakeup' THEN 'Makeup'
                                                           WHEN p.function_name = 'goiMakeupStu' THEN 'Học Viên Makeup'
                                                      END AS type,
                                                      SUM(b.priremake) AS reverue
                                                  FROM clothings.revenue b
                                                  JOIN clothings.paramater p ON b.idorder = p.item_key2
                                                  WHERE EXTRACT(YEAR FROM b.createdate) = :year
                                                      AND (:day IS NULL OR EXTRACT(DAY FROM b.createdate) = :day)
                                                      AND (:month IS NULL OR EXTRACT(MONTH FROM b.createdate) = :month)
                                                      AND p.function_name IN ('goiMakeup','goiMakeupStu')
                                                  GROUP BY date, type
                                              
                                                  UNION ALL
                                              
                                                  SELECT 
                                                      o.borrowdate::date AS date,
                                                      'Thuê Đồ' AS type,
                                                      SUM(o.totalamount) AS reverue
                                                  FROM clothings.orders o 
                                                  JOIN clothings.products p ON p.productid = o.productid
                                                  WHERE EXTRACT(YEAR FROM o.borrowdate) = :year
                                                      AND (:day IS NULL OR EXTRACT(DAY FROM o.borrowdate) = :day)
                                                      AND (:month IS NULL OR EXTRACT(MONTH FROM o.borrowdate) = :month)
                                                  GROUP BY date
                                              ) AS merged
                                              GROUP BY merged.date, merged.type
                                              ORDER BY merged.date ");

                return await _context.Set<TotalDoanhThu>()
                        .FromSqlRaw(sql.ToString(), parameters.ToArray())
                        .ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return new List<TotalDoanhThu>();
        }
        #endregion

        #region Tam
        public async Task<List<ProductStock>> GetTotalWHAmy(bool all, bool rdNotReturn, string? typeSP = null)
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
                              FROM clothings.productsamy
                              {whereClause}
                              GROUP BY {groupByColumn}
                              ORDER BY {groupByColumn}");
            }
            else if (rdNotReturn)
            {
                sql.Append($@" SELECT {groupByColumn} AS type_production, SUM(stockquantity - saveqty) AS total_quantity
                                 FROM clothings.productsamy
                                 {whereClause}
                                 GROUP BY {groupByColumn}
                                 ORDER BY {groupByColumn}");
            }
            else
            {
                sql.Append($@"SELECT {groupByColumn} AS type_production, SUM(stockquantity) AS total_quantity
                                FROM clothings.productsamy
                                {whereClause}
                                GROUP BY {groupByColumn}
                                ORDER BY {groupByColumn}");
            }

            return await _context.ProductStock
                                       .FromSqlRaw(sql.ToString(), parameters.ToArray())
                                       .ToListAsync();
        }

        public async Task<int> InserProductAmy(long productID, string nameSP, string? DescSP, decimal PriceSP, int QtySP, string sizeSP, string typeSP)
        {
            TimeZoneInfo vnZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            DateTime gioVN = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, vnZone);
            var sql = new StringBuilder(@"INSERT INTO clothings.productsamy (ProductID, ProductName, Description, PricePerDay, StockQuantity, Size, Type_production, saveqty,createdate) 
                         VALUES (@productID, @productName, @description, @priceperday, @stockquantity, @size, @typeproduct, @qtysave,@createdate)");

            var parameters = new List<NpgsqlParameter>
            {
                new NpgsqlParameter("productID", productID),
                new NpgsqlParameter("productName", nameSP),
                new NpgsqlParameter("description", (object?)DescSP ?? ""),
                new NpgsqlParameter("priceperday", PriceSP),
                new NpgsqlParameter("stockquantity", QtySP),
                new NpgsqlParameter("size", sizeSP),
                new NpgsqlParameter("typeproduct", typeSP),
                new NpgsqlParameter("qtysave", QtySP),
                new NpgsqlParameter("createdate", gioVN)
            };
            return await _context.Database.ExecuteSqlRawAsync(sql.ToString(), parameters.ToArray());
        }

        public async Task<List<ProductImageDto>> GetProductIDAmy(string typeProduction)
        {
            var products = await _context.ProductsAmy
                .Where(p => p.type_production == typeProduction)
                .OrderByDescending(p => p.createdate)
                .Select(p => new ProductImageDto
                {
                    ProductID = p.productid,
                    Price = p.priceperday,
                    Size = p.size,
                    Desc = p.description ?? "",
                    StockQTY = p.stockquantity,
                    SaveQTY = p.saveqty,
                    ImageUrl = p.productid + ".jpg",
                    TypeSP = p.type_production,
                    NameSP = p.productname
                })
                .ToListAsync();
            return products;
        }

        public async Task<List<ProductImageDto>> GetProductID1Amy(string typeProduction)
        {
            var products = await _context.ProductsAmy
                .Where(p =>
                    p.saveqty > 0 &&
                    (string.IsNullOrEmpty(typeProduction) || p.type_production == typeProduction)
                )
                .OrderByDescending(p => p.createdate)
                .Select(p => new ProductImageDto
                {
                    ProductID = p.productid,
                    Price = p.priceperday,
                    Size = p.size,
                    Desc = p.description ?? "",
                    StockQTY = p.stockquantity,
                    SaveQTY = p.saveqty,
                    ImageUrl = p.productid + ".jpg",
                    TypeSP = p.type_production,
                    NameSP = p.productname
                })
                .ToListAsync();

            return products;
        }

        public async Task<int> UpdateProductByIdAmy(ProductImageDto updatedProduct)
        {
            string sql = @"update clothings.productsamy set  productId=@productId, productname=@productname, description=@description, priceperday=@priceperday,
                            stockquantity=@stockquantity, size=@size, type_production=@type_production, saveqty=@saveqty
                            WHERE productID=@productId";

            var parameters = new[]
            {
                 new NpgsqlParameter("@productId", updatedProduct.ProductID),
                 new NpgsqlParameter("@productname", updatedProduct.NameSP),
                 new NpgsqlParameter("@description", updatedProduct.Desc),
                 new NpgsqlParameter("@priceperday", updatedProduct.Price),
                 new NpgsqlParameter("@stockquantity",  updatedProduct.StockQTY),
                 new NpgsqlParameter("@size", updatedProduct.Size),
                 new NpgsqlParameter("@type_production", updatedProduct.TypeSP),
                 new NpgsqlParameter("@saveqty",  updatedProduct.StockQTY)
            };
            return await _context.Database.ExecuteSqlRawAsync(sql, parameters);
        }

        public async Task<int> DeleteProductByIdAmy(long productId)
        {
            string sql = @"DELETE from clothings.productsamy WHERE productID=@productId";

            var parameters = new[]
            {
                 new NpgsqlParameter("@productId", productId)
            };

            return await _context.Database.ExecuteSqlRawAsync(sql, parameters);
        }

        public async Task<int> InsertRevenueAmy(string id, string NameKach, decimal price)
        {
            var sql = new StringBuilder(@"INSERT INTO clothings.revenueamy (idorder, namekh, priremake) VALUES (@idorder, @namekh, @pricemake)");

            var parameters = new List<NpgsqlParameter>
            {
                new NpgsqlParameter("idorder", id),
                new NpgsqlParameter("namekh", NameKach),
                new NpgsqlParameter("pricemake", price)
            };
            return await _context.Database.ExecuteSqlRawAsync(sql.ToString(), parameters.ToArray());
        }

        public async Task<List<RentalSummaryMakeup>> SumGetListMakeupAmy(string type, int year, int? month = null, int? day = null)
        {
            List<NpgsqlParameter> parameters = new List<NpgsqlParameter>();
            StringBuilder sql = new StringBuilder();

            try
            {
                sql.Append(@" SELECT 
                               DATE(b.createdate) AS Date,
                               p.item_key1 as Type,
                               SUM(b.priremake) AS Reverue
                           FROM clothings.revenueamy b
                           JOIN clothings.paramater p ON b.idorder = p.item_key2
                           WHERE EXTRACT(YEAR FROM b.createdate) = :year ");

                parameters.Add(new NpgsqlParameter("year", year));

                if (day.HasValue)
                {
                    sql.Append(" AND EXTRACT(DAY FROM b.createdate) = :ngay ");
                    parameters.Add(new NpgsqlParameter("ngay", day));
                }

                if (month.HasValue)
                {
                    sql.Append(" AND EXTRACT(MONTH FROM b.createdate) = :month ");
                    parameters.Add(new NpgsqlParameter("month", month));
                }

                sql.Append(" and p.function_name= :type  GROUP BY createdate, item_key1 ORDER BY createdate");
                parameters.Add(new NpgsqlParameter("type", type));

                return await _context.Set<RentalSummaryMakeup>()
                        .FromSqlRaw(sql.ToString(), parameters.ToArray())
                        .ToListAsync();
            }
            catch (Exception ex) { }
            return new List<RentalSummaryMakeup>();
        }

        public async Task<List<InfoMakeUp>> GetListMakeupAmy(string fun, int? year = null, int? month = null, string type = null)
        {
            List<NpgsqlParameter> parameters = new List<NpgsqlParameter>();

            StringBuilder sql = new StringBuilder(@" SELECT b.namekh, 
                                                            p.item_key1 as type,
                                                            b.priremake as price,
                                                            b.createdate
                                                        FROM clothings.revenueamy b
                                                        JOIN clothings.paramater p ON b.idorder = p.item_key2
                                                        WHERE 1=1 ");
            //EXTRACT(YEAR FROM b.createdate) = :year ");
            //parameters.Add(new NpgsqlParameter("year", year));

            if (month.HasValue)
            {
                sql.Append(" AND EXTRACT(MONTH FROM b.createdate) = :month ");
                parameters.Add(new NpgsqlParameter("month", month));
            }

            if (!string.IsNullOrEmpty(type) && type != "All")
            {
                sql.Append(" and p.item_key1= :type ");
                parameters.Add(new NpgsqlParameter("type", type));
            }

            sql.Append(" and p.function_name=:fun  ORDER BY createdate desc ");
            parameters.Add(new NpgsqlParameter("fun", fun));
            return await _context.Set<InfoMakeUp>()
                    .FromSqlRaw(sql.ToString(), parameters.ToArray())
                    .ToListAsync();
        }

        public async Task<List<TotalDoanhThu>> TotalDoanhThuAmy(int year, int? month = null, int? day = null)
        {
            try
            {
                var parameters = new List<NpgsqlParameter>
                {
                    new NpgsqlParameter("year", year)
                    {
                        DbType = DbType.Int32
                    },
                    new NpgsqlParameter("day", day.HasValue ? day.Value : (object)DBNull.Value)
                    {
                        DbType = DbType.Int32
                    },
                    new NpgsqlParameter("month", month.HasValue ? month.Value : (object)DBNull.Value)
                    {
                        DbType = DbType.Int32
                    }
                };

                var sql = new StringBuilder(@"SELECT 
                                                  merged.date AS Date,
                                                  merged.type AS Type,
                                                  SUM(merged.reverue) AS Reverue
                                              FROM
                                              (
                                                  SELECT 
                                                      DATE(b.createdate) AS date,
                                                      CASE 
                                                           WHEN p.function_name = 'goiMakeupAmy' THEN 'Makeup'
                                                           WHEN p.function_name = 'goiMakeupStuAmy' THEN 'Học Viên Makeup'
                                                      END AS type,
                                                      SUM(b.priremake) AS reverue
                                                  FROM clothings.revenueamy b
                                                  JOIN clothings.paramater p ON b.idorder = p.item_key2
                                                  WHERE EXTRACT(YEAR FROM b.createdate) = :year
                                                      AND (:day IS NULL OR EXTRACT(DAY FROM b.createdate) = :day)
                                                      AND (:month IS NULL OR EXTRACT(MONTH FROM b.createdate) = :month)
                                                      AND p.function_name IN ('goiMakeupAmy','goiMakeupStuAmy')
                                                  GROUP BY date, type
                                              
                                                  UNION ALL
                                              
                                                  SELECT 
                                                      o.borrowdate::date AS date,
                                                      'Thuê Đồ' AS type,
                                                      SUM(o.totalamount) AS reverue
                                                  FROM clothings.ordersamy o 
                                                  JOIN clothings.productsamy p ON p.productid = o.productid
                                                  WHERE EXTRACT(YEAR FROM o.borrowdate) = :year
                                                      AND (:day IS NULL OR EXTRACT(DAY FROM o.borrowdate) = :day)
                                                      AND (:month IS NULL OR EXTRACT(MONTH FROM o.borrowdate) = :month)
                                                  GROUP BY date
                                                    
                                                  UNION ALL

                                                  SELECT 
                                                      DATE(b.datechup) AS date,
                                                      'Chụp ảnh' AS type,
                                                      SUM(b.priremake) AS reverue
                                                  FROM clothings.revenuewedding b
                                                  JOIN clothings.paramater p ON b.idorder = p.item_key2
                                                  WHERE EXTRACT(YEAR FROM b.datechup) = :year
                                                      AND (:day IS NULL OR EXTRACT(DAY FROM b.datechup) = :day)
                                                      AND (:month IS NULL OR EXTRACT(MONTH FROM b.datechup) = :month)
                                                      AND p.function_name IN ('goiChupWedding')
                                                  GROUP BY date, type
                                              ) AS merged
                                              GROUP BY merged.date, merged.type
                                              ORDER BY merged.date ");

                return await _context.Set<TotalDoanhThu>()
                        .FromSqlRaw(sql.ToString(), parameters.ToArray())
                        .ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return new List<TotalDoanhThu>();
        }

        public async Task<List<InfoThueDoDto>> GetListThueDoAmy(string status, int year, int? month = null)
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
                                             FROM clothings.ordersamy b
                                             JOIN clothings.productsamy p ON b.productid = p.productid
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
            sql.Append(" order by borrowdate desc ");
            return await _context.Set<InfoThueDoDto>()
                    .FromSqlRaw(sql.ToString(), parameters.ToArray())
                    .ToListAsync();
        }

        public async Task<int> GetQTYListThueDoAmy(string status, int year, int? month = null)
        {
            List<NpgsqlParameter> parameters = new List<NpgsqlParameter>();

            //StringBuilder sql = new StringBuilder(@" SELECT COUNT(*)
            //                                         FROM (
            //                                             SELECT b.userid, b.sumpay
            //                                             FROM clothings.ordersamy b
            //                                             JOIN clothings.productsamy p ON b.productid = p.productid
            //                                             JOIN clothings.users u ON u.userid = b.userid
            //                                             WHERE EXTRACT(YEAR FROM b.borrowdate) = @year ");

            StringBuilder sql = new StringBuilder(@"  SELECT CAST(COALESCE(SUM(b.qty), 0) AS INT)
                                                         FROM clothings.ordersamy b
                                                         JOIN clothings.productsamy p ON b.productid = p.productid
                                                         JOIN clothings.users u ON u.userid = b.userid
                                                         WHERE EXTRACT(YEAR FROM b.borrowdate) = @year AND b.sumpay != '0' ");

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
            //sql.Append(" AND b.sumpay != '0' GROUP BY b.userid, b.sumpay ) t ");
            using var cmd = _context.Database.GetDbConnection().CreateCommand();
            cmd.CommandText = sql.ToString();

            foreach (var p in parameters)
                cmd.Parameters.Add(p);

            await _context.Database.OpenConnectionAsync();

            var result = await cmd.ExecuteScalarAsync();

            return Convert.ToInt32(result);
        }

        public async Task<int> UpdateReturnOrderAmy(long orderId, decimal? lastmoney, long productid, int QTYThue, string status)
        {
            TimeZoneInfo vnZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            DateTime gioVN = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, vnZone);
            string sql = "";

            if (status == "RETURN")
                sql = @"UPDATE clothings.ordersamy SET status='RETURN', returndate=@timereturn, lastmoney = totalamount 
                         WHERE orderid=@idorder;

                        UPDATE clothings.productsamy SET  saveqty=saveqty + :sl where productid=:bookingid; ";
            else
                sql = @"UPDATE clothings.ordersamy SET status='CANCEL', returndate=@timereturn, lastmoney = totalamount 
                             WHERE orderid=@idorder;

                            UPDATE clothings.productsamy SET  saveqty=saveqty + :sl where productid=:bookingid; ";

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

        public async Task<int> UpdateReturnAllOrderAmy(string sdt, string status)
        {

            var products = await (from o in _context.OrderAmy
                                  join u in _context.Users on o.UserId equals u.UserId
                                  where u.Phone == sdt && o.Status == "BORROW"
                                  select new Data.DTOs.OrderDetailDto
                                  {
                                      idorder = o.OrderId,
                                      QTYThue = o.Qty,
                                      bookingid = o.ProductId,
                                      lastmoney = o.TotalAmount
                                  }).ToListAsync();

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                int totalInserted = 0;

                TimeZoneInfo vnZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
                DateTime gioVN = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, vnZone);

                string sql = "";
                if (status == "RETURN")
                    sql = @"UPDATE clothings.ordersamy SET status='RETURN', returndate=@timereturn, lastmoney = totalamount 
                            WHERE orderid=@idorder;
 
                            UPDATE clothings.productsamy SET  saveqty=saveqty + :sl where productid=:bookingid; ";
                else
                    sql = @"UPDATE clothings.ordersamy SET status='CANCEL', returndate=@timereturn, lastmoney = totalamount 
                            WHERE orderid=@idorder;

                            UPDATE clothings.productsamy SET  saveqty=saveqty + :sl where productid=:bookingid; ";

                foreach (var order in products)
                {
                    var parameters = new[]
                    {
                       new NpgsqlParameter("@idorder", order.idorder),
                       new NpgsqlParameter("@lastmoney", order.lastmoney),
                       new NpgsqlParameter("@timereturn", gioVN),
                       new NpgsqlParameter("@bookingid", order.bookingid),
                       new NpgsqlParameter("@sl", order.QTYThue)
                    };

                    totalInserted += await _context.Database.ExecuteSqlRawAsync(sql, parameters);
                }

                await transaction.CommitAsync();
                return totalInserted;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<int> UpdateReturnAllOrder1Amy()
        {

            var products = await (from o in _context.OrderAmy
                                  where o.Status == "BORROW"
                                  select new Data.DTOs.OrderDetailDto
                                  {
                                      idorder = o.OrderId,
                                      QTYThue = o.Qty,
                                      bookingid = o.ProductId,
                                      lastmoney = o.TotalAmount
                                  }).ToListAsync();

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                int totalInserted = 0;

                TimeZoneInfo vnZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
                DateTime gioVN = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, vnZone);

                string sql = @"UPDATE clothings.ordersamy SET status='RETURN', returndate=@timereturn, lastmoney = totalamount 
                            WHERE orderid=@idorder;
 
                            UPDATE clothings.productsamy SET  saveqty=saveqty + :sl where productid=:bookingid; ";

                foreach (var order in products)
                {
                    var parameters = new[]
                    {
                       new NpgsqlParameter("@idorder", order.idorder),
                       new NpgsqlParameter("@lastmoney", order.lastmoney),
                       new NpgsqlParameter("@timereturn", gioVN),
                       new NpgsqlParameter("@bookingid", order.bookingid),
                       new NpgsqlParameter("@sl", order.QTYThue)
                    };

                    totalInserted += await _context.Database.ExecuteSqlRawAsync(sql, parameters);
                }

                await transaction.CommitAsync();
                return totalInserted;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<int> GetStockQTYAmy(long idproduct)
        {
            int stock = await _context.ProductsAmy
                .Where(p => p.productid == idproduct)
                .Select(p => p.saveqty)
                .FirstOrDefaultAsync();

            return stock;
        }

        public async Task<int> InsertOrderAmy(long UserID, List<Data.DTOs.ProductItem> products, string username, string action)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                int totalInserted = 0;
                string status = "BORROW", sql = "";

                TimeZoneInfo vnZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
                DateTime gioVN = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, vnZone);

                if (action == "MUA")
                {
                    status = "SOLD";
                    sql = @"INSERT INTO clothings.ordersamy
                                   (orderid, userid, totalamount, status, moneycoc, productid, qty, notes, tienphatsinh, borrowdate,sumpay) 
                               VALUES 
                                   (@orderid, @userid, @totalamount, @status, @moneycoc, @productid, @qty, @notes, @tienphatsinh, @borrowdate, @sumpay);

                               UPDATE clothings.productsamy
                               SET stockquantity = stockquantity - @qty 
                               WHERE productid = @productid;";
                }
                else
                {
                    sql = @"INSERT INTO clothings.ordersamy
                                   (orderid, userid, totalamount, status, moneycoc, productid, qty, notes, tienphatsinh, borrowdate,sumpay) 
                               VALUES 
                                   (@orderid, @userid, @totalamount, @status, @moneycoc, @productid, @qty, @notes, @tienphatsinh, @borrowdate, @sumpay);

                               UPDATE clothings.productsamy
                               SET saveqty = saveqty - @qty 
                               WHERE productid = @productid;";
                }

                long PayId = long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss"));
                if (username.ToUpper() != "NHANVIENTHUEDO")
                    PayId = 0;
                foreach (var order in products)
                {
                    long orderId = long.Parse(DateTime.Now.ToString("yyyyMMddHHmmssfff"));
                    var parameters = new[]
                    {
                        new NpgsqlParameter("@orderid", orderId),
                        new NpgsqlParameter("@userid", UserID),
                        new NpgsqlParameter("@totalamount", order.TongTienThueNonCoc),
                        new NpgsqlParameter("@moneycoc", order.TienCoc),
                        new NpgsqlParameter("@productid", order.ProductID),
                        new NpgsqlParameter("@qty", order.QTYThue),
                        new NpgsqlParameter("@notes", NpgsqlTypes.NpgsqlDbType.Text) { Value = (object?)order.Notes ?? "" },
                        new NpgsqlParameter("@tienphatsinh", NpgsqlTypes.NpgsqlDbType.Numeric) { Value = 0m },
                        new NpgsqlParameter("@borrowdate", gioVN),
                        new NpgsqlParameter("@sumpay", PayId),
                        new NpgsqlParameter("@status", status),
                    };

                    totalInserted += await _context.Database.ExecuteSqlRawAsync(sql, parameters);
                }


                await transaction.CommitAsync();
                return totalInserted;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
        #endregion

        #region dung
        public async Task<int> InsertRevenueWedding(string id, string NameKach, decimal price, string photograper, DateTime? datechup,
            DateTime? datetrafile, DateTime? datecuoi, string Notes, long imageID, int qty, string NameThoMake, string NameThoToc)
        {
            var sql = new StringBuilder(@"INSERT INTO clothings.revenuewedding (idorder, namekh, priremake, photograper, datechup, datetrafile, datecuoi, note, imageid, qty, tho_make, tho_toc) 
                                                 VALUES (@idorder, @namekh, @pricemake, @photograper, @datechup, @datetrafile, @datecuoi, @note, @imageid, @qty, @NameThoMake, @NameThoToc)");

            var parameters = new List<NpgsqlParameter>
            {
                new NpgsqlParameter("idorder", id),
                new NpgsqlParameter("namekh", NameKach),
                new NpgsqlParameter("pricemake", price),
                new NpgsqlParameter("photograper", photograper),
                new NpgsqlParameter("datechup", datechup),
                new NpgsqlParameter("datetrafile", datetrafile),
                new("datecuoi", NpgsqlTypes.NpgsqlDbType.Timestamp)
                {
                    Value = (object?)datecuoi ?? DBNull.Value
                },
                new NpgsqlParameter("note", NpgsqlTypes.NpgsqlDbType.Text) { Value = (object?)Notes ?? "" },
                new NpgsqlParameter("imageid", imageID),
                new NpgsqlParameter("qty", qty),
                new NpgsqlParameter("NameThoMake", NameThoMake),
                new NpgsqlParameter("NameThoToc", NameThoToc)
            };
            return await _context.Database.ExecuteSqlRawAsync(sql.ToString(), parameters.ToArray());
        }

        public async Task<List<ListInfoGoiChup>> GetListChupWedding(int? year = null, int? month = null, int? day = null)
        {
            List<NpgsqlParameter> parameters = new List<NpgsqlParameter>();

            //StringBuilder sql = new StringBuilder(@" SELECT b.id, b.idorder, b.namekh, 
            //                                                p.item_key1 as type,
            //                                                b.priremake as price,b.photograper, b.datechup, b.datetrafile, b.datecuoi, b.note,
            //                                                b.createdate, b.imageid, b.qty, b.tho_make as NameThoMake, b.tho_toc as NameThoToc
            //                                            FROM clothings.revenuewedding b
            //                                            JOIN clothings.paramater p ON b.idorder = p.item_key2 
            //                                            WHERE b.status = 'OK'  ");
            //EXTRACT(YEAR FROM b.createdate) = :year ");
            //parameters.Add(new NpgsqlParameter("year", year));

            StringBuilder sql = new StringBuilder(@" SELECT b.id, b.idorder, b.namekh, 
                                                            b.priremake as price, b.datechup, b.datetrafile, b.datecuoi, b.note,
                                                            b.createdate, b.imageid, b.qty, b.tho_make as NameThoMake, b.tho_toc as NameThoToc
                                                        FROM clothings.revenuewedding b
                                                        WHERE b.status = 'OK'  ");

            if (day.HasValue)
            {
                sql.Append(" AND EXTRACT(DAY FROM b.datechup) = :day ");
                parameters.Add(new NpgsqlParameter("day", day));
            }

            if (month.HasValue)
            {
                sql.Append(" AND EXTRACT(MONTH FROM b.datechup) = :month ");
                parameters.Add(new NpgsqlParameter("month", month));
            }

            //if (!string.IsNullOrEmpty(type) && type != "All")
            //{
            //    sql.Append(" and p.item_key1= :type ");
            //    parameters.Add(new NpgsqlParameter("type", type));
            //}

            //if (!string.IsNullOrEmpty(thochup) && thochup != "All")
            //{
            //    sql.Append(" and b.photograper= :thochup ");
            //    parameters.Add(new NpgsqlParameter("thochup", thochup));
            //}

            sql.Append(" ORDER BY createdate desc ");
            return await _context.Set<ListInfoGoiChup>()
                    .FromSqlRaw(sql.ToString(), parameters.ToArray())
                    .ToListAsync();
        }

        public async Task<List<RentalSummaryChup>> SumGetListWedding(string type, int year, int? month = null, int? day = null, string thochup = null)
        {
            List<NpgsqlParameter> parameters = new List<NpgsqlParameter>();
            StringBuilder sql = new StringBuilder();

            try
            {
                sql.Append(@" SELECT 
                               DATE(b.datechup) AS Date,
                               p.item_key1 as Type,
                               SUM(b.priremake) AS Reverue
                           FROM clothings.revenuewedding b
                           JOIN clothings.paramater p ON b.idorder = p.item_key2
                           WHERE b.status = 'OK' AND EXTRACT(YEAR FROM b.createdate) = :year ");

                parameters.Add(new NpgsqlParameter("year", year));

                if (day.HasValue)
                {
                    sql.Append(" AND EXTRACT(DAY FROM b.datechup) = :ngay ");
                    parameters.Add(new NpgsqlParameter("ngay", day));
                }

                if (month.HasValue)
                {
                    sql.Append(" AND EXTRACT(MONTH FROM b.datechup) = :month ");
                    parameters.Add(new NpgsqlParameter("month", month));
                }

                if (!string.IsNullOrEmpty(thochup) && thochup != "All")
                {
                    sql.Append(" and b.photograper= :thochup ");
                    parameters.Add(new NpgsqlParameter("thochup", thochup));
                }

                sql.Append(" and p.function_name= :type  GROUP BY datechup, item_key1 ORDER BY datechup");
                parameters.Add(new NpgsqlParameter("type", type));

                return await _context.Set<RentalSummaryChup>()
                        .FromSqlRaw(sql.ToString(), parameters.ToArray())
                        .ToListAsync();
            }
            catch (Exception ex) { }
            return new List<RentalSummaryChup>();
        }

        public async Task<int> InsertParamaterWeding(string fun, string key1, string? key2, long imageid)
        {
            var sql = new StringBuilder(@"INSERT INTO clothings.paramater (function_name, item_key1, item_key2, imageid)
                                               VALUES (@function_name, @item_key1, @item_key2, @imageid)");

            var parameters = new List<NpgsqlParameter>
            {
                new NpgsqlParameter("function_name", fun),
                new NpgsqlParameter("item_key1", key1),
                new NpgsqlParameter("item_key2", (object?)key2 ?? " "),
                new NpgsqlParameter("imageid", imageid),
            };

            return await _context.Database.ExecuteSqlRawAsync(sql.ToString(), parameters.ToArray());
        }

        public async Task<int> UpdateLichChupWedding(int Id)
        {
            string sql = @"update clothings.revenuewedding set  status='CANCEL'
                            WHERE id=@OrderId";

            var parameters = new[]
            {
                 new NpgsqlParameter("OrderId", Id)
            };
            return await _context.Database.ExecuteSqlRawAsync(sql, parameters);
        }

        public async Task<int> UpdateOrderWeddingById(ListInfoGoiChup updatedProduct)
        {
            TimeZoneInfo vnZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            DateTime gioVN = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, vnZone);
            string sql = @"update clothings.revenuewedding set idorder=@idorder, namekh=@namekh, priremake=@prirechup, photograper=@photograper, qty=@qty,
                            datechup=@datechup, imageid=@imageid, updatetime=@updatetime, tho_make=@tho_make, tho_toc=@tho_toc
                            WHERE id=@productId";

            var parameters = new[]
            {
                new NpgsqlParameter("@productId", updatedProduct.id),
                 new NpgsqlParameter("@idorder", updatedProduct.idorder),
                 new NpgsqlParameter("@namekh", updatedProduct.namekh),
                 new NpgsqlParameter("@prirechup", updatedProduct.price),
                 new NpgsqlParameter("@photograper", "Dũng"),
                 new NpgsqlParameter("@qty",  updatedProduct.qty),
                 new NpgsqlParameter("@datechup", updatedProduct.dateChup),
                 new NpgsqlParameter("@imageid", updatedProduct.imageid),
                 new NpgsqlParameter("@updatetime", gioVN),
                 new NpgsqlParameter("@tho_make", updatedProduct.NameThoMake),
                 new NpgsqlParameter("@tho_toc", updatedProduct.NameThoToc)
            };
            return await _context.Database.ExecuteSqlRawAsync(sql, parameters);
        }
        #endregion
    }
}
