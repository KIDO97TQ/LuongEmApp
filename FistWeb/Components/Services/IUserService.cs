using FistWeb.Components.DTOs;

namespace FistWeb.Components.Services
{
    public interface IUserService
    {
        Task<List<UserOrderDto>> GetUserOrdersAsync();
    }

    public interface IThongKeService
    {
        Task<List<DoanhThuThueDoDto>> GetDoanhThuThueDoUocTinhAsync(string typesp, int year, int? month = null, int? day = null);
    }
}
