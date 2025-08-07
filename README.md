# 📊 DapperProject

## 📌 Genel Bakış

**DapperProject**, Kaggle’dan indirilen **10 milyon satırlık satış verisi** üzerinde gelişmiş analizler yapılmasını sağlayan, **ASP.NET Core 9.0 MVC** mimarisi ve **Dapper** kullanılarak geliştirilmiş bir web tabanlı satış analiz ve raporlama uygulamasıdır. Bu proje; güçlü filtreleme seçenekleri, interaktif grafiklerle zenginleştirilmiş dashboard ekranı ve yüksek performanslı veri işleme yetenekleri sayesinde, büyük veri kümeleri üzerinde hızlı, esnek ve kullanıcı dostu bir analiz ortamı sunar. Gelişmiş arayüz tasarımıyla kullanıcılar için etkili bir deneyim hedeflenmiştir.



## ✨ Özellikler

- 📊 **Dashboard:** Gerçek zamanlı satış istatistikleri
- 📋 **Veri Tablosu:** Sayfalama ve filtreleme özellikli satış verileri
- 🔍 **Gelişmiş Filtreleme:** Müşteri, kategori, marka, bölge ve tarih bazlı
- 📤 **Raporlama:** Excel ve PDF formatlarında veri dışa aktarımı
- 📱 **Responsive Tasarım:** Mobil uyumlu modern arayüz



## 🧪 Teknolojiler

| Katman     | Teknoloji                            |
|------------|---------------------------------------|
| Backend    | ASP.NET Core 9.0, Dapper, SQL Server |
| Frontend   | Bootstrap 5, jQuery, Chart.js        |
| Raporlama  | EPPlus (Excel), iTextSharp (PDF)     |

## 📥 Veri Seti
Kaynak: https://www.kaggle.com/datasets/omercolakoglu/10million-rows-turkish-market-sales-dataset
Kapsam: Ürün, müşteri, sipariş tarihi, bölge, şehir, kategori, fiyat vb.
Not: Veri ön işleme yapılmış ve normalize edilmiş biçimde SQL Server’a yüklenmiştir.

## 💻 Kullanım

### 📊 Dashboard
Ana sayfada satış istatistiklerini görselleştirilmiş biçimde takip edebilirsiniz:

- ✅ Toplam satış sayısı ve toplam gelir
- ✅ Müşteri ve sipariş istatistikleri
- ✅ Kategori ve marka bazlı satış analizleri

### 📋 Veri Tablosu

`/Sales/Table` sayfası üzerinden:

- 🔍 Tüm satış verilerini görüntüleyin
- 📑 Sayfalama ile büyük veri setlerini yönetin
- 🎯 Gelişmiş filtreleme seçeneklerini kullanın
- 📤 Excel veya PDF formatında dışa aktarım yapın



## 🎯 Özellik Detayları

### 📈 Dashboard

- Gerçek zamanlı güncellenen istatistikler
- Chart.js ile görsel grafikler (Bar, Pie, Line)
- Satış performans metrikleri
- Son işlemler tablosu

### 📤 Raporlama

- **Excel Export:** EPPlus kütüphanesi ile filtrelenmiş verileri dışa aktarın
- **PDF Export:** iTextSharp ile biçimlendirilmiş PDF çıktılar oluşturun
- Otomatik tablo formatlama desteği
- Tüm raporlar filtreye duyarlı şekilde çalışır



## 📷 Ekran Görüntüleri

<img width="1920" height="1622" alt="screencapture-localhost-7091-Sales-Index-2025-08-08-00_07_16" src="https://github.com/user-attachments/assets/463f99df-404e-4abe-a18e-eec85b14d483" />

<img width="1920" height="3954" alt="screencapture-localhost-7091-Sales-Table-2025-08-08-00_09_25" src="https://github.com/user-attachments/assets/66f6d429-f52d-41d9-b547-0d749fdf396a" />

<img width="1898" height="883" alt="Ekran görüntüsü 2025-08-08 002512" src="https://github.com/user-attachments/assets/30287fbf-e806-4ac3-ab3b-4c34793a1d26" />

<img width="733" height="581" alt="Ekran görüntüsü 2025-08-08 002156" src="https://github.com/user-attachments/assets/ef9ed28a-905f-4625-9102-1027a123f82c" />

<img width="1341" height="217" alt="Ekran görüntüsü 2025-08-08 002852" src="https://github.com/user-attachments/assets/d1e46756-43ec-4232-ba7b-354bfba528e6" />
























