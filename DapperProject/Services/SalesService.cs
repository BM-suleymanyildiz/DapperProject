using Dapper;
using DapperProject.Context;
using DapperProject.Dtos;
using Microsoft.Data.SqlClient;

namespace DapperProject.Services
{
    public class SalesService : ISalesService
    {
        private readonly DapperContext _context;

        public SalesService(DapperContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<SalesDto>> GetAllSalesAsync(int page = 1, int pageSize = 100)
        {
            try
            {
                using var connection = _context.CreateConnection();
                var offset = (page - 1) * pageSize;
                
                var query = @"
                    SELECT ID, ORDERID, ORDERDETAILID, DATE_, USERID, USERNAME_, NAMESURNAME, 
                           STATUS_, ITEMID, ITEMCODE, ITEMNAME, AMOUNT, UNITPRICE, PRICE, 
                           TOTALPRICE, CATEGORY1, CATEGORY2, CATEGORY3, CATEGORY4, BRAND, 
                           USERGENDER, USERBIRTHDATE, REGION, CITY, TOWN, DISTRICT, 
                           ADDRESSTEXT, ADDRESSID
                    FROM SALES 
                    ORDER BY ID 
                    OFFSET @Offset ROWS 
                    FETCH NEXT @PageSize ROWS ONLY";

                var result = await connection.QueryAsync<SalesDto>(query, new { Offset = offset, PageSize = pageSize });
                return result ?? Enumerable.Empty<SalesDto>();
            }
            catch (Exception ex)
            {
                return Enumerable.Empty<SalesDto>();
            }
        }

        public async Task<SalesDto?> GetSaleByIdAsync(int id)
        {
            using var connection = _context.CreateConnection();
            var query = "SELECT * FROM SALES WHERE ID = @Id";
            return await connection.QueryFirstOrDefaultAsync<SalesDto>(query, new { Id = id });
        }

        public async Task<int> GetTotalSalesCountAsync()
        {
            try
            {
                using var connection = _context.CreateConnection();
                var query = "SELECT COUNT(*) FROM SALES";
                var result = await connection.ExecuteScalarAsync<int>(query);
                return result;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public async Task<IEnumerable<SalesDto>> GetSalesByDateRangeAsync(DateTime startDate, DateTime endDate, int page = 1, int pageSize = 100)
        {
            using var connection = _context.CreateConnection();
            var offset = (page - 1) * pageSize;
            
            var query = @"
                SELECT ID, ORDERID, ORDERDETAILID, DATE_, USERID, USERNAME_, NAMESURNAME, 
                       STATUS_, ITEMID, ITEMCODE, ITEMNAME, AMOUNT, UNITPRICE, PRICE, 
                       TOTALPRICE, CATEGORY1, CATEGORY2, CATEGORY3, CATEGORY4, BRAND, 
                       USERGENDER, USERBIRTHDATE, REGION, CITY, TOWN, DISTRICT, 
                       ADDRESSTEXT, ADDRESSID
                FROM SALES 
                WHERE DATE_ BETWEEN @StartDate AND @EndDate
                ORDER BY DATE_ DESC
                OFFSET @Offset ROWS 
                FETCH NEXT @PageSize ROWS ONLY";

            return await connection.QueryAsync<SalesDto>(query, new { StartDate = startDate, EndDate = endDate, Offset = offset, PageSize = pageSize });
        }

        public async Task<IEnumerable<SalesDto>> GetSalesByUserAsync(int userId, int page = 1, int pageSize = 100)
        {
            using var connection = _context.CreateConnection();
            var offset = (page - 1) * pageSize;
            
            var query = @"
                SELECT ID, ORDERID, ORDERDETAILID, DATE_, USERID, USERNAME_, NAMESURNAME, 
                       STATUS_, ITEMID, ITEMCODE, ITEMNAME, AMOUNT, UNITPRICE, PRICE, 
                       TOTALPRICE, CATEGORY1, CATEGORY2, CATEGORY3, CATEGORY4, BRAND, 
                       USERGENDER, USERBIRTHDATE, REGION, CITY, TOWN, DISTRICT, 
                       ADDRESSTEXT, ADDRESSID
                FROM SALES 
                WHERE USERID = @UserId
                ORDER BY DATE_ DESC
                OFFSET @Offset ROWS 
                FETCH NEXT @PageSize ROWS ONLY";

            return await connection.QueryAsync<SalesDto>(query, new { UserId = userId, Offset = offset, PageSize = pageSize });
        }

        public async Task<IEnumerable<SalesDto>> GetSalesByCategoryAsync(string category, int page = 1, int pageSize = 100)
        {
            using var connection = _context.CreateConnection();
            var offset = (page - 1) * pageSize;
            
            var query = @"
                SELECT ID, ORDERID, ORDERDETAILID, DATE_, USERID, USERNAME_, NAMESURNAME, 
                       STATUS_, ITEMID, ITEMCODE, ITEMNAME, AMOUNT, UNITPRICE, PRICE, 
                       TOTALPRICE, CATEGORY1, CATEGORY2, CATEGORY3, CATEGORY4, BRAND, 
                       USERGENDER, USERBIRTHDATE, REGION, CITY, TOWN, DISTRICT, 
                       ADDRESSTEXT, ADDRESSID
                FROM SALES 
                WHERE CATEGORY1 = @Category OR CATEGORY2 = @Category OR CATEGORY3 = @Category OR CATEGORY4 = @Category
                ORDER BY DATE_ DESC
                OFFSET @Offset ROWS 
                FETCH NEXT @PageSize ROWS ONLY";

            return await connection.QueryAsync<SalesDto>(query, new { Category = category, Offset = offset, PageSize = pageSize });
        }

        public async Task<IEnumerable<SalesDto>> GetSalesByBrandAsync(string brand, int page = 1, int pageSize = 100)
        {
            using var connection = _context.CreateConnection();
            var offset = (page - 1) * pageSize;
            
            var query = @"
                SELECT ID, ORDERID, ORDERDETAILID, DATE_, USERID, USERNAME_, NAMESURNAME, 
                       STATUS_, ITEMID, ITEMCODE, ITEMNAME, AMOUNT, UNITPRICE, PRICE, 
                       TOTALPRICE, CATEGORY1, CATEGORY2, CATEGORY3, CATEGORY4, BRAND, 
                       USERGENDER, USERBIRTHDATE, REGION, CITY, TOWN, DISTRICT, 
                       ADDRESSTEXT, ADDRESSID
                FROM SALES 
                WHERE BRAND = @Brand
                ORDER BY DATE_ DESC
                OFFSET @Offset ROWS 
                FETCH NEXT @PageSize ROWS ONLY";

            return await connection.QueryAsync<SalesDto>(query, new { Brand = brand, Offset = offset, PageSize = pageSize });
        }

        public async Task<IEnumerable<SalesDto>> GetSalesByRegionAsync(string region, int page = 1, int pageSize = 100)
        {
            using var connection = _context.CreateConnection();
            var offset = (page - 1) * pageSize;
            
            var query = @"
                SELECT ID, ORDERID, ORDERDETAILID, DATE_, USERID, USERNAME_, NAMESURNAME, 
                       STATUS_, ITEMID, ITEMCODE, ITEMNAME, AMOUNT, UNITPRICE, PRICE, 
                       TOTALPRICE, CATEGORY1, CATEGORY2, CATEGORY3, CATEGORY4, BRAND, 
                       USERGENDER, USERBIRTHDATE, REGION, CITY, TOWN, DISTRICT, 
                       ADDRESSTEXT, ADDRESSID
                FROM SALES 
                WHERE REGION = @Region
                ORDER BY DATE_ DESC
                OFFSET @Offset ROWS 
                FETCH NEXT @PageSize ROWS ONLY";

            return await connection.QueryAsync<SalesDto>(query, new { Region = region, Offset = offset, PageSize = pageSize });
        }

        public async Task<decimal> GetTotalRevenueAsync()
        {
            using var connection = _context.CreateConnection();
            var query = "SELECT ISNULL(SUM(TOTALPRICE), 0) FROM SALES";
            return await connection.ExecuteScalarAsync<decimal>(query);
        }

        public async Task<decimal> GetTotalRevenueByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            using var connection = _context.CreateConnection();
            var query = "SELECT ISNULL(SUM(TOTALPRICE), 0) FROM SALES WHERE DATE_ BETWEEN @StartDate AND @EndDate";
            return await connection.ExecuteScalarAsync<decimal>(query, new { StartDate = startDate, EndDate = endDate });
        }

        public async Task<int> GetTotalOrdersAsync()
        {
            using var connection = _context.CreateConnection();
            var query = "SELECT COUNT(DISTINCT ORDERID) FROM SALES";
            return await connection.ExecuteScalarAsync<int>(query);
        }

        public async Task<int> GetTotalCustomersAsync()
        {
            using var connection = _context.CreateConnection();
            var query = "SELECT COUNT(DISTINCT USERID) FROM SALES";
            return await connection.ExecuteScalarAsync<int>(query);
        }

        public async Task<decimal> GetAverageOrderValueAsync()
        {
            using var connection = _context.CreateConnection();
            var query = "SELECT ISNULL(AVG(OrderTotal), 0) FROM (SELECT ORDERID, SUM(TOTALPRICE) as OrderTotal FROM SALES GROUP BY ORDERID) as OrderTotals";
            return await connection.ExecuteScalarAsync<decimal>(query);
        }

        public async Task<IEnumerable<object>> GetTopCategoriesAsync(int top = 10)
        {
            using var connection = _context.CreateConnection();
            var query = @"
                SELECT TOP(@Top) CATEGORY1 as Category, COUNT(*) as SalesCount, SUM(TOTALPRICE) as TotalRevenue
                FROM SALES 
                WHERE CATEGORY1 IS NOT NULL AND CATEGORY1 != ''
                GROUP BY CATEGORY1 
                ORDER BY TotalRevenue DESC";

            return await connection.QueryAsync(query, new { Top = top });
        }

        public async Task<int> GetTotalCategoriesCountAsync()
        {
            using var connection = _context.CreateConnection();
            var query = @"
                SELECT COUNT(DISTINCT CATEGORY1) 
                FROM SALES 
                WHERE CATEGORY1 IS NOT NULL AND CATEGORY1 != ''";
            
            return await connection.ExecuteScalarAsync<int>(query);
        }

        public async Task<IEnumerable<object>> GetTopBrandsAsync(int top = 10)
        {
            using var connection = _context.CreateConnection();
            var query = @"
                SELECT TOP(@Top) BRAND, COUNT(*) as SalesCount, SUM(TOTALPRICE) as TotalRevenue
                FROM SALES 
                WHERE BRAND IS NOT NULL
                GROUP BY BRAND 
                ORDER BY TotalRevenue DESC";

            return await connection.QueryAsync(query, new { Top = top });
        }

        public async Task<IEnumerable<object>> GetTopRegionsAsync(int top = 10)
        {
            using var connection = _context.CreateConnection();
            var query = @"
                SELECT TOP(@Top) REGION, COUNT(*) as SalesCount, SUM(TOTALPRICE) as TotalRevenue
                FROM SALES 
                WHERE REGION IS NOT NULL
                GROUP BY REGION 
                ORDER BY TotalRevenue DESC";

            return await connection.QueryAsync(query, new { Top = top });
        }

        public async Task<IEnumerable<object>> GetSalesByMonthAsync(int year)
        {
            using var connection = _context.CreateConnection();
            var query = @"
                SELECT MONTH(DATE_) as Month, COUNT(*) as SalesCount, SUM(TOTALPRICE) as TotalRevenue
                FROM SALES 
                WHERE YEAR(DATE_) = @Year
                GROUP BY MONTH(DATE_)
                ORDER BY Month";

            return await connection.QueryAsync(query, new { Year = year });
        }

        public async Task<IEnumerable<object>> GetSalesByDayAsync(DateTime date)
        {
            using var connection = _context.CreateConnection();
            var query = @"
                SELECT DAY(DATE_) as Day, COUNT(*) as SalesCount, SUM(TOTALPRICE) as TotalRevenue
                FROM SALES 
                WHERE DATE_ = @Date
                GROUP BY DAY(DATE_)
                ORDER BY Day";

            return await connection.QueryAsync(query, new { Date = date.Date });
        }

        public async Task<IEnumerable<object>> GetTopSellingItemsAsync(int top = 10)
        {
            using var connection = _context.CreateConnection();
            var query = @"
                SELECT TOP(@Top) ITEMNAME, COUNT(*) as SalesCount, SUM(TOTALPRICE) as TotalRevenue
                FROM SALES 
                WHERE ITEMNAME IS NOT NULL
                GROUP BY ITEMNAME 
                ORDER BY TotalRevenue DESC";

            return await connection.QueryAsync(query, new { Top = top });
        }

        public async Task<object> GetDashboardStatsAsync()
        {
            using var connection = _context.CreateConnection();
            var query = @"
                SELECT 
                    (SELECT COUNT(*) FROM SALES) as TotalSales,
                    (SELECT COUNT(DISTINCT USERID) FROM SALES) as TotalCustomers,
                    (SELECT COUNT(DISTINCT USERID) FROM SALES) as UniqueCustomers,
                    (SELECT COUNT(DISTINCT ORDERID) FROM SALES) as TotalOrders,
                    (SELECT ISNULL(AVG(AMOUNT), 0) FROM SALES) as AverageAmount,
                    (SELECT ISNULL(SUM(TOTALPRICE), 0) FROM SALES) as TotalRevenue,
                    (SELECT ISNULL(AVG(OrderTotal), 0) FROM (SELECT ORDERID, SUM(TOTALPRICE) as OrderTotal FROM SALES GROUP BY ORDERID) as OrderTotals) as AverageOrderValue,
                    (SELECT ISNULL(AVG(UNITPRICE), 0) FROM SALES) as AverageProductPrice,
                    (SELECT COUNT(DISTINCT ITEMID) FROM SALES) as TotalProducts,
                    (SELECT COUNT(DISTINCT CATEGORY1) FROM SALES WHERE CATEGORY1 IS NOT NULL) as TotalCategories";

            return await connection.QueryFirstAsync(query);
        }

        public async Task<IEnumerable<object>> GetRecentSalesAsync(int count = 10)
        {
            using var connection = _context.CreateConnection();
            var query = @"
                SELECT TOP(@Count) ID, ORDERID, USERNAME_, NAMESURNAME, ITEMNAME, 
                       AMOUNT, TOTALPRICE, DATE_, BRAND, REGION
                FROM SALES 
                ORDER BY DATE_ DESC";

            return await connection.QueryAsync(query, new { Count = count });
        }

        public async Task<IEnumerable<SalesDto>> GetFilteredSalesAsync(
            string? customer = null, string? category = null, string? brand = null,
            string? region = null, DateTime? filterDate = null,
            decimal? minAmount = null, decimal? maxAmount = null,
            int page = 1, int pageSize = 100)
        {
            try
            {
                using var connection = _context.CreateConnection();
                var offset = (page - 1) * pageSize;
                
                var whereConditions = new List<string>();
                var parameters = new DynamicParameters();
                
                // Filtreleme koşulları
                if (!string.IsNullOrEmpty(customer))
                {
                    whereConditions.Add("NAMESURNAME LIKE @Customer");
                    parameters.Add("@Customer", $"%{customer}%");
                }
                
                if (!string.IsNullOrEmpty(category))
                {
                    whereConditions.Add("CATEGORY1 LIKE @Category");
                    parameters.Add("@Category", $"%{category}%");
                }
                
                if (!string.IsNullOrEmpty(brand))
                {
                    whereConditions.Add("BRAND LIKE @Brand");
                    parameters.Add("@Brand", $"%{brand}%");
                }
                
                if (!string.IsNullOrEmpty(region))
                {
                    whereConditions.Add("(REGION LIKE @Region OR CITY LIKE @Region)");
                    parameters.Add("@Region", $"%{region}%");
                }
                
                if (filterDate.HasValue)
                {
                    whereConditions.Add("CAST(DATE_ AS DATE) = @FilterDate");
                    parameters.Add("@FilterDate", filterDate.Value.Date);
                }
                
                if (minAmount.HasValue)
                {
                    whereConditions.Add("TOTALPRICE >= @MinAmount");
                    parameters.Add("@MinAmount", minAmount.Value);
                }
                
                if (maxAmount.HasValue)
                {
                    whereConditions.Add("TOTALPRICE <= @MaxAmount");
                    parameters.Add("@MaxAmount", maxAmount.Value);
                }
                
                var whereClause = whereConditions.Count > 0 ? "WHERE " + string.Join(" AND ", whereConditions) : "";
                
                var query = $@"
                    SELECT ID, ORDERID, ORDERDETAILID, DATE_, USERID, USERNAME_, NAMESURNAME, 
                           STATUS_, ITEMID, ITEMCODE, ITEMNAME, AMOUNT, UNITPRICE, PRICE, 
                           TOTALPRICE, CATEGORY1, CATEGORY2, CATEGORY3, CATEGORY4, BRAND, 
                           USERGENDER, USERBIRTHDATE, REGION, CITY, TOWN, DISTRICT, 
                           ADDRESSTEXT, ADDRESSID
                    FROM SALES 
                    {whereClause}
                    ORDER BY ID 
                    OFFSET @Offset ROWS 
                    FETCH NEXT @PageSize ROWS ONLY";
                
                parameters.Add("@Offset", offset);
                parameters.Add("@PageSize", pageSize);
                
                var result = await connection.QueryAsync<SalesDto>(query, parameters);
                return result ?? Enumerable.Empty<SalesDto>();
            }
            catch (Exception ex)
            {
                return Enumerable.Empty<SalesDto>();
            }
        }

        public async Task<int> GetFilteredSalesCountAsync(
            string? customer = null, string? category = null, string? brand = null,
            string? region = null, DateTime? filterDate = null,
            decimal? minAmount = null, decimal? maxAmount = null)
        {
            try
            {
                using var connection = _context.CreateConnection();
                
                var whereConditions = new List<string>();
                var parameters = new DynamicParameters();
                
                // Filtreleme koşulları
                if (!string.IsNullOrEmpty(customer))
                {
                    whereConditions.Add("NAMESURNAME LIKE @Customer");
                    parameters.Add("@Customer", $"%{customer}%");
                }
                
                if (!string.IsNullOrEmpty(category))
                {
                    whereConditions.Add("CATEGORY1 LIKE @Category");
                    parameters.Add("@Category", $"%{category}%");
                }
                
                if (!string.IsNullOrEmpty(brand))
                {
                    whereConditions.Add("BRAND LIKE @Brand");
                    parameters.Add("@Brand", $"%{brand}%");
                }
                
                if (!string.IsNullOrEmpty(region))
                {
                    whereConditions.Add("(REGION LIKE @Region OR CITY LIKE @Region)");
                    parameters.Add("@Region", $"%{region}%");
                }
                
                if (filterDate.HasValue)
                {
                    whereConditions.Add("CAST(DATE_ AS DATE) = @FilterDate");
                    parameters.Add("@FilterDate", filterDate.Value.Date);
                }
                
                if (minAmount.HasValue)
                {
                    whereConditions.Add("TOTALPRICE >= @MinAmount");
                    parameters.Add("@MinAmount", minAmount.Value);
                }
                
                if (maxAmount.HasValue)
                {
                    whereConditions.Add("TOTALPRICE <= @MaxAmount");
                    parameters.Add("@MaxAmount", maxAmount.Value);
                }
                
                var whereClause = whereConditions.Count > 0 ? "WHERE " + string.Join(" AND ", whereConditions) : "";
                
                var query = $"SELECT COUNT(*) FROM SALES {whereClause}";
                
                return await connection.ExecuteScalarAsync<int>(query, parameters);
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
    }
}
