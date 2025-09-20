using FistWeb.Data.DTOs;
using System.Data;

namespace FistWeb.Data.Services
{
    public interface IUserService
    {
        Task<List<UserOrderDto>> GetUserOrdersAsync();
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
}
