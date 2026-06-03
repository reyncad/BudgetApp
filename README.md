# BudgetApp

Kişisel bütçe takip uygulaması. Gelir ve harcamalarınızı kategorilere göre yönetin, bütçe limitleri belirleyin, tasarruf hedeflerinizi takip edin.

## Özellikler

- **Harcama Yönetimi** — Ekle, düzenle, sil. Ay/yıl/kategori bazlı filtreleme. Limit aşımında uyarı.
- **Gelir Yönetimi** — Maaş, freelance, kira geliri, yatırım gibi türlere göre sınıflandırma.
- **Kategoriler** — Özel kategoriler oluştur, aylık harcama limiti belirle.
- **Bütçe Limitleri** — Tüm kategoriler tek ekranda, inline limit güncelleme.
- **Tasarruf Hedefleri** — Hedef tutarı ve tarihi belirle, ilerleme barıyla takip et, tamamlanınca konfeti efekti.
- **Dashboard** — Aylık özet kartları, 6 aylık trend grafiği, kategori bazlı pasta grafik, limit ilerleme barları.

## Teknoloji

- .NET 8 / ASP.NET Core MVC
- Entity Framework Core 8 + SQLite
- Bootstrap 5 + Bootstrap Icons
- Chart.js 4

## Kurulum

```bash
git clone https://github.com/reyncad/BudgetApp.git
cd BudgetApp
dotnet restore
dotnet ef database update
dotnet run --urls "http://localhost:5050"
```

Tarayıcıda `http://localhost:5050` adresini aç.

## Gereksinimler

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
