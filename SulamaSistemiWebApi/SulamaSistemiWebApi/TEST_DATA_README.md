# Sulama Sistemi - Test Verileri

Bu dokümantasyon, Sulama Sistemi Web API'si için oluşturulan test verilerini açıklamaktadır.

## Otomatik Oluşturulan Test Verileri

Uygulama başlatıldığında otomatik olarak aşağıdaki test verileri oluşturulur:

### Kullanıcılar (Users)
- **admin** (admin@sulama.com) - Şifre: admin123
- **operator1** (operator1@sulama.com) - Şifre: operator123  
- **testuser** (test@sulama.com) - Şifre: test123

### Motorlar (Motors)
- 3 adet motor (2'si kapalı, 1'i açık)
- Farklı kullanıcılar tarafından kontrol edilmiş
- Son durum değişiklikleri farklı zamanlarda

### Sıcaklık ve Nem Verileri (TemperatureHumidity)
- Son 24 saat için her saat başı veri
- Sıcaklık: 15-30°C arası rastgele
- Nem: 30-60% arası rastgele

### Yağmur Sensörü Verileri (RainSensor)
- Son 24 saat için her 2 saatte bir veri
- Yağmur seviyesi: 0-100% arası rastgele

## API Endpoints

### Test Verileri Yönetimi

#### 1. Tek Test Verisi Oluştur
```http
POST /api/testdata/create
```

#### 2. Çoklu Test Verisi Oluştur
```http
POST /api/testdata/create-multiple?count=20
```

#### 3. Test Verilerini Temizle
```http
DELETE /api/testdata/clear
```

## Veritabanı Bağlantısı

Test verileri SQL Server veritabanında saklanır. Veritabanı bağlantı ayarları `appsettings.json` dosyasında bulunur.

## Kullanım Örnekleri

### 1. Uygulamayı Başlat
```bash
dotnet run
```

### 2. Swagger UI'ı Aç
```
http://localhost:5093/swagger
```

### 3. Test Verilerini Kontrol Et
- `/api/users` - Kullanıcıları listele
- `/api/motors` - Motorları listele
- `/api/temperaturehumidity` - Sıcaklık/nem verilerini listele
- `/api/rainsensor` - Yağmur sensörü verilerini listele

### 4. Yeni Test Verileri Oluştur
```bash
curl -X POST "http://localhost:5093/api/testdata/create"
```

## Notlar

- Test verileri sadece veritabanı boşsa oluşturulur
- Mevcut veriler varsa tekrar oluşturulmaz
- Tüm şifreler BCrypt ile hash'lenmiştir
- Tarih/saat bilgileri UTC formatındadır 