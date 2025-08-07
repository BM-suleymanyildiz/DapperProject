using Microsoft.AspNetCore.Mvc;
using DapperProject.Services;
using DapperProject.Dtos;
using OfficeOpenXml;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Text;

namespace DapperProject.Controllers
{
    public class SalesController : Controller
    {
        private readonly ISalesService _salesService;
        private readonly ILogger<SalesController> _logger;

        public SalesController(ISalesService salesService, ILogger<SalesController> logger)
        {
            _salesService = salesService;
            _logger = logger;
        }

     
        public async Task<IActionResult> Index()
        {
            try
            {
                var dashboardStats = await _salesService.GetDashboardStatsAsync();
                var topCategories = await _salesService.GetTopCategoriesAsync(7); 
                var topBrands = await _salesService.GetTopBrandsAsync(5);
                var topRegions = await _salesService.GetTopRegionsAsync(5);
                var recentSales = await _salesService.GetRecentSalesAsync(10);
                
                // Grafik verileri için
                var currentYear = DateTime.Now.Year;
                var salesByMonth = await _salesService.GetSalesByMonthAsync(currentYear);
                var topSellingItems = await _salesService.GetTopSellingItemsAsync(5);
                var topRegionsForChart = await _salesService.GetTopRegionsAsync(8); 

                ViewBag.DashboardStats = dashboardStats;
                ViewBag.TopCategories = topCategories;
                ViewBag.TopBrands = topBrands;
                ViewBag.TopRegions = topRegions;
                ViewBag.RecentSales = recentSales;
                ViewBag.SalesByMonth = salesByMonth;
                ViewBag.TopSellingItems = topSellingItems;
                ViewBag.TopRegionsForChart = topRegionsForChart;

                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading dashboard data");
                return View();
            }
        }

       
        public IActionResult Details(int id)
        {
            return View();
        }

      
        public IActionResult Create()
        {
            return View();
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(SalesDto salesDto)
        {
            if (ModelState.IsValid)
            {
             
                return RedirectToAction(nameof(Index));
            }
            return View(salesDto);
        }

     
        public IActionResult Edit(int id)
        {
            return View();
        }

      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, SalesDto salesDto)
        {
            if (ModelState.IsValid)
            {
           
                return RedirectToAction(nameof(Index));
            }
            return View(salesDto);
        }

        public IActionResult Delete(int id)
        {
            return View();
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
          
            return RedirectToAction(nameof(Index));
        }

      
        public async Task<IActionResult> Table(int page = 1, int pageSize = 50, 
            string? customer = null, string? category = null, string? brand = null, 
            string? region = null, string? filterDate = null, 
            decimal? minAmount = null, decimal? maxAmount = null)
        {
            try
            {
                _logger.LogInformation("Table action called with page={Page}, pageSize={PageSize}", page, pageSize);
                
                // Filtreleme parametrelerini ViewBag'e ekle
                ViewBag.CustomerFilter = customer;
                ViewBag.CategoryFilter = category;
                ViewBag.BrandFilter = brand;
                ViewBag.RegionFilter = region;
                ViewBag.FilterDate = filterDate;
                ViewBag.MinAmount = minAmount;
                ViewBag.MaxAmount = maxAmount;

                // DateTime? tipine çevir
                DateTime? parsedFilterDate = null;
                if (!string.IsNullOrEmpty(filterDate) && DateTime.TryParse(filterDate, out var tableDate))
                {
                    parsedFilterDate = tableDate;
                }

                // Filtreleme varsa filtrelenmiş verileri al
                IEnumerable<SalesDto> sales;
                int totalCount;
                
                if (!string.IsNullOrEmpty(customer) || !string.IsNullOrEmpty(category) || 
                    !string.IsNullOrEmpty(brand) || !string.IsNullOrEmpty(region) || 
                    parsedFilterDate.HasValue || minAmount.HasValue || maxAmount.HasValue)
                {
                    _logger.LogInformation("Using filtered data");
                    // Filtrelenmiş veriler
                    sales = await _salesService.GetFilteredSalesAsync(
                        customer, category, brand, region, parsedFilterDate, 
                        minAmount, maxAmount, page, pageSize);
                    
                    totalCount = await _salesService.GetFilteredSalesCountAsync(
                        customer, category, brand, region, parsedFilterDate, 
                        minAmount, maxAmount);
                }
                else
                {
                    _logger.LogInformation("Using all data");
                    // Tüm veriler
                    sales = await _salesService.GetAllSalesAsync(page, pageSize);
                    totalCount = await _salesService.GetTotalSalesCountAsync();
                }
                
                _logger.LogInformation("Retrieved {Count} sales records, total count: {TotalCount}", 
                    sales?.Count() ?? 0, totalCount);
                
                var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);
                
                ViewBag.CurrentPage = page;
                ViewBag.TotalPages = totalPages;
                ViewBag.PageSize = pageSize;
                ViewBag.TotalCount = totalCount;
                ViewBag.Sales = sales;
                
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading table data: {Message}", ex.Message);
                
                // Hata durumunda boş verilerle devam et
                ViewBag.CurrentPage = page;
                ViewBag.TotalPages = 1;
                ViewBag.PageSize = pageSize;
                ViewBag.TotalCount = 0;
                ViewBag.Sales = new List<SalesDto>();
                
                return View();
            }
        }

        // Excel Export
        public async Task<IActionResult> ExportToExcel(string? customer = null, string? category = null, string? brand = null,
            string? region = null, string? filterDate = null, decimal? minAmount = null, decimal? maxAmount = null)
        {
            try
            {
                // DateTime? tipine çevir
                DateTime? parsedFilterDate = null;
                if (!string.IsNullOrEmpty(filterDate) && DateTime.TryParse(filterDate, out var excelDate))
                {
                    parsedFilterDate = excelDate;
                }

                // Filtrelenmiş verileri al
                var sales = await _salesService.GetFilteredSalesAsync(
                    customer, category, brand, region, parsedFilterDate, 
                    minAmount, maxAmount, 1, int.MaxValue); // Tüm verileri al

                // Excel oluştur
                using var package = new ExcelPackage();
                var worksheet = package.Workbook.Worksheets.Add("Satış Verileri");

                // Başlıkları ekle
                worksheet.Cells[1, 1].Value = "Müşteri Adı";
                worksheet.Cells[1, 2].Value = "Email";
                worksheet.Cells[1, 3].Value = "Ürün";
                worksheet.Cells[1, 4].Value = "Kategori";
                worksheet.Cells[1, 5].Value = "Marka";
                worksheet.Cells[1, 6].Value = "Miktar";
                worksheet.Cells[1, 7].Value = "Toplam Fiyat (TL)";
                worksheet.Cells[1, 8].Value = "Bölge";
                worksheet.Cells[1, 9].Value = "Şehir";
                worksheet.Cells[1, 10].Value = "Tarih";

                // Başlık stilini ayarla
                using (var range = worksheet.Cells[1, 1, 1, 10])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                }

                // Verileri ekle
                int row = 2;
                foreach (var sale in sales)
                {
                    worksheet.Cells[row, 1].Value = sale.NAMESURNAME;
                    worksheet.Cells[row, 2].Value = sale.USERNAME_;
                    worksheet.Cells[row, 3].Value = sale.ITEMNAME;
                    worksheet.Cells[row, 4].Value = sale.CATEGORY1;
                    worksheet.Cells[row, 5].Value = sale.BRAND;
                    worksheet.Cells[row, 6].Value = sale.AMOUNT?.ToString("F0") ?? "0";
                    worksheet.Cells[row, 7].Value = sale.TOTALPRICE?.ToString("F2") ?? "0.00";
                    worksheet.Cells[row, 8].Value = sale.REGION;
                    worksheet.Cells[row, 9].Value = sale.CITY;
                    worksheet.Cells[row, 10].Value = sale.DATE_?.ToString("dd.MM.yyyy") ?? "-";
                    row++;
                }

                // Sütun genişliklerini otomatik ayarla
                worksheet.Cells.AutoFitColumns();

                // Excel dosyasını döndür
                var content = package.GetAsByteArray();
                var fileName = $"SatisVerileri_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
                
                return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error exporting to Excel");
                return RedirectToAction("Table");
            }
        }

        // PDF Export
        public async Task<IActionResult> ExportToPdf(string? customer = null, string? category = null, string? brand = null,
            string? region = null, string? filterDate = null, decimal? minAmount = null, decimal? maxAmount = null)
        {
            try
            {
                // DateTime? tipine çevir
                DateTime? parsedFilterDate = null;
                if (!string.IsNullOrEmpty(filterDate) && DateTime.TryParse(filterDate, out var pdfDate))
                {
                    parsedFilterDate = pdfDate;
                }

                // Filtrelenmiş verileri al
                var sales = await _salesService.GetFilteredSalesAsync(
                    customer, category, brand, region, parsedFilterDate, 
                    minAmount, maxAmount, 1, int.MaxValue); // Tüm verileri al

                using (MemoryStream ms = new MemoryStream())
                {
                    Document document = new Document(PageSize.A4, 10f, 10f, 10f, 10f);
                    PdfWriter writer = PdfWriter.GetInstance(document, ms);

                    document.Open();

                    // Başlık
                    Font titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16);
                    titleFont.SetStyle(Font.UNDEFINED);
                    Paragraph title = new Paragraph("Satis Verileri Raporu", titleFont);
                    title.Alignment = Element.ALIGN_CENTER;
                    document.Add(title);
                    document.Add(new Paragraph(" ")); // Boşluk

                    // Tarih
                    Font dateFont = FontFactory.GetFont(FontFactory.HELVETICA, 10);
                    dateFont.SetStyle(Font.UNDEFINED);
                    Paragraph date = new Paragraph($"Olusturulma Tarihi: {DateTime.Now:dd.MM.yyyy HH:mm}", dateFont);
                    document.Add(date);
                    document.Add(new Paragraph(" ")); // Boşluk

                    // Tablo oluştur
                    PdfPTable table = new PdfPTable(8);
                    table.WidthPercentage = 100;

                    // Başlık hücreleri
                    Font headerFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 9);
                    headerFont.SetStyle(Font.UNDEFINED);
                    Font cellFont = FontFactory.GetFont(FontFactory.HELVETICA, 8);
                    cellFont.SetStyle(Font.UNDEFINED);

                    string[] headers = { "Musteri", "Email", "Urun", "Kategori", "Marka", "Miktar", "Tutar (TL)", "Tarih" };
                                         foreach (string header in headers)
                     {
                         PdfPCell cell = new PdfPCell(new Phrase(header, headerFont));
                         cell.BackgroundColor = new BaseColor(240, 240, 240); // Açık gri
                         cell.HorizontalAlignment = Element.ALIGN_CENTER;
                         cell.Padding = 5;
                         table.AddCell(cell);
                     }

                    // Veri hücreleri
                    foreach (var sale in sales)
                    {
                        table.AddCell(new PdfPCell(new Phrase(sale.NAMESURNAME ?? "", cellFont)));
                        table.AddCell(new PdfPCell(new Phrase(sale.USERNAME_ ?? "", cellFont)));
                        table.AddCell(new PdfPCell(new Phrase(sale.ITEMNAME ?? "", cellFont)));
                        table.AddCell(new PdfPCell(new Phrase(sale.CATEGORY1 ?? "", cellFont)));
                        table.AddCell(new PdfPCell(new Phrase(sale.BRAND ?? "", cellFont)));
                        table.AddCell(new PdfPCell(new Phrase(sale.AMOUNT?.ToString("F0") ?? "0", cellFont)));
                        table.AddCell(new PdfPCell(new Phrase(sale.TOTALPRICE?.ToString("F2") ?? "0.00", cellFont)));
                        table.AddCell(new PdfPCell(new Phrase(sale.DATE_?.ToString("dd.MM.yyyy") ?? "-", cellFont)));
                    }

                    document.Add(table);
                    document.Close();

                    var content = ms.ToArray();
                    var fileName = $"SatisVerileri_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";
                    
                    return File(content, "application/pdf", fileName);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error exporting to PDF");
                return RedirectToAction("Table");
            }
        }
    }
}
