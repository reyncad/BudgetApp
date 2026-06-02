using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BudgetApp.Models;

public class Kategori
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Ad alanı zorunludur.")]
    [Display(Name = "Kategori Adı")]
    public string Ad { get; set; } = string.Empty;

    [Range(0, double.MaxValue, ErrorMessage = "Limit 0 veya daha büyük olmalıdır.")]
    [Display(Name = "Aylık Limit (₺)")]
    [Column(TypeName = "decimal(18,2)")]
    public decimal? AylikLimit { get; set; }

    public ICollection<Harcama> Harcamalar { get; set; } = new List<Harcama>();
}
