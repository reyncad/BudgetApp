using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BudgetApp.Models;

public class Harcama
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Açıklama alanı zorunludur.")]
    [Display(Name = "Açıklama")]
    public string Açıklama { get; set; } = string.Empty;

    [Required(ErrorMessage = "Miktar alanı zorunludur.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Miktar 0'dan büyük olmalıdır.")]
    [Display(Name = "Miktar (₺)")]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Miktar { get; set; }

    [Required(ErrorMessage = "Tarih alanı zorunludur.")]
    [Display(Name = "Tarih")]
    [DataType(DataType.Date)]
    public DateTime Tarih { get; set; } = DateTime.Today;

    [Required(ErrorMessage = "Kategori seçimi zorunludur.")]
    [Display(Name = "Kategori")]
    public int KategoriId { get; set; }

    public Kategori? Kategori { get; set; }
}
