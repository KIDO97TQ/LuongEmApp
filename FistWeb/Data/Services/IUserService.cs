using FistWeb.Data.DTOs;
using System.Data;

namespace FistWeb.Data.Services
{
    public interface IGetUserInfoService
    {
        Task<List<UserOrderDto>> GetUserInfo();
    }

    #region Luong
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

    public interface IGetProductID1Service
    {
        Task<List<ProductImageDto>> GetProductID1(string typeSP);
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
        Task<int> InsertOrder(long userID, List<Data.DTOs.ProductItem> ProductList, string action);
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
        Task<List<InfoMakeUp>> GetListMakeup(string fun, int? year = null, int? month = null, string? type = null);
    }

    public interface IGetTotalDoanhThuService
    {
        Task<List<TotalDoanhThu>> TotalDoanhThu(int year, int? month = null, int? day = null);
    }
    #endregion

    #region Tam
    public interface IGetSumWHAmyService
    {
        Task<List<ProductStock>> GetTotalWHAmy(bool all, bool rdNotReturn, string? typeSP = null);
    }

    public interface IInsertSPAmyService
    {
        Task<int> InserProductAmy(long productID, string nameSP, string? DescSP, decimal PriceSP, int QtySP, string sizeSP, string typeSP);
    }

    public interface IGetProductIDAmyService
    {
        Task<List<ProductImageDto>> GetProductIDAmy(string typeSP);
    }

    public interface IUpdateProductByIdAmyService
    {
        Task<int> UpdateProductByIdAmy(ProductImageDto updatedProduct);
    }

    public interface IDeleteProductAmyService
    {
        Task<int> DeleteProductByIdAmy(long productID);
    }

    public interface IInsertRevenueAmyService
    {
        Task<int> InsertRevenueAmy(string id, string NameKach, decimal price);
    }

    public interface IGetSumRevenueAmyService
    {
        Task<List<RentalSummaryMakeup>> SumGetListMakeupAmy(string type, int year, int? month = null, int? day = null);
    }

    public interface IGetListMakeupAmyService
    {
        Task<List<InfoMakeUp>> GetListMakeupAmy(string fun, int? year = null, int? month = null, string? type = null);
    }

    public interface IGetTotalDoanhThuAmyService
    {
        Task<List<TotalDoanhThu>> TotalDoanhThuAmy(int year, int? month = null, int? day = null);
    }

    public interface GetListThueDoAmy
    {
        Task<List<InfoThueDoDto>> GetListThueDoAmy(string status, int year, int? month = null);
    }

    public interface GetQTYListThueDoAmy
    {
        Task<int> GetQTYListThueDoAmy(string status, int year, int? month = null);
    }

    public interface IUpdateReturnOderAmyService
    {
        Task<int> UpdateReturnOrderAmy(long orderId, decimal? lastmoney, long productid, int QTYThue, string status);
    }

    public interface IUpdateReturnAllOrderAmyService
    {
        Task<int> UpdateReturnAllOrderAmy(string SDTuser, string status);
    }

    public interface UpdateReturnAllOrder1Amy
    {
        Task<int> UpdateReturnAllOrder1Amy();
    }

    public interface IStockQTYAmyService
    {
        Task<int> GetStockQTYAmy(long productID);
    }

    public interface IInsertOrdersAmyService
    {
        Task<int> InsertOrderAmy(long userID, List<Data.DTOs.ProductItem> ProductList, string username, string action);
    }
    public interface IGetProductID1AmyService
    {
        Task<List<ProductImageDto>> GetProductID1Amy(string typeSP);
    }
    #endregion

    #region dung
    public interface IInsertRevenueWeddingService
    {
        Task<int> InsertRevenueWedding(string id, string NameKach, decimal price, string photograper, DateTime? datechup, DateTime? datetrafile,
            DateTime? datecuoi, string Notes, long imageID, int qty, string NameThoMake, string NameThoToc, string NameNVnhanJob, string imageURL);
    }

    public interface IGetListWeddingService
    {
        Task<WeddingDataResponse> GetListChupWedding(string NameNV, string NameJob, int? year = null, int? month = null, int? day = null);
    }

    public interface IGetSumRevenueWeddingService
    {
        Task<List<RentalSummaryChup>> SumGetListWedding(string type, int year, int? month = null, int? day = null, string thochup = null);
    }

    public interface IAddParaWeddingService
    {
        Task<int> InsertParamaterWeding(string fun, string user, string? pass, long imageid);
    }

    public interface IUpdateLichChupWedding
    {
        Task<int> UpdateLichChupWedding(int id);
    }

    public interface IUpdateOrderWeddingByIdService
    {
        Task<int> UpdateOrderWeddingById(ListInfoGoiChup updatedProduct);
    }
    #endregion

    #region Chi tieu Kido
    public interface IGetCategories
    {
        Task<List<CategoriesInfo>> GetCategories();
    }
    public interface IGetUserAcount
    {
        Task<List<UserAccount>> GetUserAcount();
    }

    public interface InsertParamaterChiTieu
    {
        Task<int> InsertParamaterChiTieu(DateTime date, decimal amount, string? note, Guid CategoryID, string description, string User, Guid accountId);
    }
    public interface InsertCategory
    {
        Task<int> InsertCategory(string name, string icon, string Description);
    }
    public interface UpdateCategory
    {
        Task<int> UpdateCategory(Guid id, string name, string icon, Boolean status, string type);
    }
    public interface IGetChiTieu
    {
        Task<List<ExpenseInfo>> GetChiTieu(DateTime? FromDate, DateTime? ToDate, int? month);
    }
    public interface IUpdateExpese
    {
        Task<int> UpdateExpese(Guid id, string userName, DateTime expenseDate, decimal amount, Guid categoryId, string description, string? note);
    }

    public interface IDelChiTieu
    {
        Task<int> DelChiTieu(Guid id);
    }
    #endregion

    #region Chi tieu Amy
    public interface IGetCategoriesAmy
    {
        Task<List<CategoriesInfo>> GetCategoriesAmy();
    }
    public interface InsertParamaterChiTieuAmy
    {
        Task<int> InsertParamaterChiTieuAmy(DateTime date, decimal amount, string? note, Guid CategoryID, string description, string User);
    }
    public interface InsertCategoryAmy
    {
        Task<int> InsertCategoryAmy(string name, string icon, string Description);
    }
    public interface UpdateCategoryAmy
    {
        Task<int> UpdateCategoryAmy(Guid id, string name, string icon, Boolean status, string type);
    }
    public interface IGetChiTieuAmy
    {
        Task<List<ExpenseInfo>> GetChiTieuAmy(DateTime? FromDate, DateTime? ToDate, int? month);
    }
    public interface IUpdateExpeseAmy
    {
        Task<int> UpdateExpeseAmy(Guid id, string userName, DateTime expenseDate, decimal amount, Guid categoryId, string description, string? note);
    }

    public interface IDelChiTieuAmy
    {
        Task<int> DelChiTieuAmy(Guid id);
    }
    #endregion

    #region Tich Luy
    public interface IInsertIncome
    { 
        Task<int> InsertIncome(DateTime incomeDate, decimal amount, string incomeType, string userName, string? note, Guid accountID);
    }

    public interface IInsertGold
    {                      
        Task<int> InsertGold(string goldType, decimal weight, decimal purchasePrice, DateTime purchaseDate, string userName, string? note, Guid accountId, bool NoMoney);
    }

    public interface IGetIncome
    {
        Task<List<Income>> GetIncome();
    }

    public interface IGetGoldAssets
    {
        Task<List<GoldAsset>> GetGoldAssets();
    }

    public interface IGetGoldInfo
    {
        Task<List<GoldInfo>> GetGoldInfo();
    }

    public interface IUpdateAccount
    {
        Task<int> UpdateAccount(Guid id, string name, string accountType, decimal balance, string userName, string? description, bool isActive);
    }

    public interface IUpdateGoldPrice
    {
        Task<int> UpdateGoldPrice(Guid id, string goldType, string? description, decimal buyPrice, decimal nowPrice);
    }

    #endregion
}
