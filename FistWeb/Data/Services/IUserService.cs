using FistWeb.Data.DTOs;
using System.Data;

namespace FistWeb.Data.Services
{
    public interface IGetUserInfoService
    {
        Task<List<UserOrderDto>> GetUserInfo();
    }
    public interface IThongKeService
    {
        Task<List<DoanhThuThueDoDto>> GetDoanhThuThueDoUocTinhAsync(string typesp, int year, int? month = null, int? day = null);
    }

    public interface SumGetListThueDo
    {
        Task<List<RentalSummary>> SumGetListThueDo(string status, int year, int? month = null);
    }

    public interface GetListThueDo
    {
        Task<List<InfoThueDoDto>> GetListThueDo(string status, int year, int? month = null);
    }

    public interface IGetParamaterService
    {
        Task<List<ListParamater>> GetParamater();
    }
    public interface IGetParaUserService
    {
        Task<List<ListParaUser>> GetLoginUser(string fun, string user, string? pass);
    }

    public interface IAddParaService
    {
        Task<int> InsertParamaterRawAsync(string fun, string user, string? pass);
    }

    public interface IDeleteParaService
    {
        Task<int> DeleteParamaterRawAsync(string fun, string user, string? pass);
    }

    public interface IInsertSPService
    {
        Task<int> InserProduct(long productID, string nameSP, string? DescSP, decimal PriceSP, int QtySP, string sizeSP, string typeSP);
    }

    public interface IGetSumWHService
    {
        Task<List<ProductStock>> GetTotalWH(bool all, bool rdNotReturn, string? typeSP = null);
    }

    public interface IGetProductIDService
    {
        Task<List<ProductImageDto>> GetProductID(string typeSP);
    }

    public interface IStockQTYService
    {
        Task<int> GetStockQTY(long productID);
    }

    public interface IInserUserService
    {
        Task<int> InsertUser(long id, string NameKach, string SdtKhach);
    }

    public interface IInsertOrdersService
    {
        Task<int> InsertOrder(long userID, List<Data.DTOs.ProductItem> ProductList);
    }
    public interface IGetUserIDService
    {
        Task<long> GetUserID(string ContactKH);
    }

    public interface IUpdateReturnOderService
    {
        Task<int> UpdateReturnOrder(long orderId, decimal? lastmoney, long productid, int QTYThue, string status);
    }

    public interface IUpdatePWService
    {
        Task<bool> UpdatePasswordAsync(string pw);
    }

    public interface IDeleteProductService
    {
        Task<int> DeleteProductById(long productID);
    }

    public interface IUpdateProductByIdService
    {
        Task<int> UpdateProductById(ProductImageDto updatedProduct);
    }

    public interface IUpdateReturnAllOrderService
    {
        Task<int> UpdateReturnAllOrder(string SDTuser, string status);
    }

    public interface IGetUserInfo1Service
    {
        Task<List<UserOrderDto>> GetUserInfo1(string SDTuser);
    }
    public interface IUpdateUserService
    {
        Task<int> UpdateUser(List<Data.DTOs.UserOrderDto> userInfo, string NewNameKH);
    }

    public interface UpdateReturnAllOrder1
    {
        Task<int> UpdateReturnAllOrder1();
    }
    public interface IGetParamaterMakeupService
    {
        Task<List<ListParamaterMakeup>> GetParamaterMakeUp();
    }

    public interface IInsertRevenueService
    {
        Task<int> InsertRevenue(string id, string NameKach, decimal price);
    }

    public interface IGetSumRevenueService
    {
        Task<List<RentalSummaryMakeup>> SumGetListMakeup(string type, int year, int? month = null, int? day = null);
    }

    public interface IGetListMakeupService
    {
        Task<List<InfoMakeUp>> GetListMakeup(string fun, int year, int? month = null, string? type = null);
    }
}
