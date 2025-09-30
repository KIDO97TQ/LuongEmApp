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
    
}
