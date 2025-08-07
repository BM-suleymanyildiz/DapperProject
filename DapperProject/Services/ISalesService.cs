using DapperProject.Dtos;

namespace DapperProject.Services
{
    public interface ISalesService
    {
        // Ana CRUD operasyonları
        Task<IEnumerable<SalesDto>> GetAllSalesAsync(int page = 1, int pageSize = 100);
        Task<SalesDto?> GetSaleByIdAsync(int id);
        Task<int> GetTotalSalesCountAsync();
        
        // Filtreleme ve arama
        Task<IEnumerable<SalesDto>> GetSalesByDateRangeAsync(DateTime startDate, DateTime endDate, int page = 1, int pageSize = 100);
        Task<IEnumerable<SalesDto>> GetSalesByUserAsync(int userId, int page = 1, int pageSize = 100);
        Task<IEnumerable<SalesDto>> GetSalesByCategoryAsync(string category, int page = 1, int pageSize = 100);
        Task<IEnumerable<SalesDto>> GetSalesByBrandAsync(string brand, int page = 1, int pageSize = 100);
        Task<IEnumerable<SalesDto>> GetSalesByRegionAsync(string region, int page = 1, int pageSize = 100);
        
        // Gelişmiş filtreleme
        Task<IEnumerable<SalesDto>> GetFilteredSalesAsync(
            string? customer = null, string? category = null, string? brand = null,
            string? region = null, DateTime? filterDate = null,
            decimal? minAmount = null, decimal? maxAmount = null,
            int page = 1, int pageSize = 100);
        Task<int> GetFilteredSalesCountAsync(
            string? customer = null, string? category = null, string? brand = null,
            string? region = null, DateTime? filterDate = null,
            decimal? minAmount = null, decimal? maxAmount = null);
        
        // İstatistikler
        Task<decimal> GetTotalRevenueAsync();
        Task<decimal> GetTotalRevenueByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<int> GetTotalOrdersAsync();
        Task<int> GetTotalCustomersAsync();
        Task<decimal> GetAverageOrderValueAsync();
        
        // Kategori ve marka istatistikleri
        Task<IEnumerable<object>> GetTopCategoriesAsync(int top = 10);
        Task<int> GetTotalCategoriesCountAsync();
        Task<IEnumerable<object>> GetTopBrandsAsync(int top = 10);
        Task<IEnumerable<object>> GetTopRegionsAsync(int top = 10);
        
        // Zaman bazlı analizler
        Task<IEnumerable<object>> GetSalesByMonthAsync(int year);
        Task<IEnumerable<object>> GetSalesByDayAsync(DateTime date);
        Task<IEnumerable<object>> GetTopSellingItemsAsync(int top = 10);
        
        // Performans metrikleri
        Task<object> GetDashboardStatsAsync();
        Task<IEnumerable<object>> GetRecentSalesAsync(int count = 10);
    }
}
