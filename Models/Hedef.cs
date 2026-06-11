using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BudgetApp.Models;

public class Hedef
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Hedef adı zorunludur.")]
    [Display(Name = "Hedef Adı")]
    public string Ad { get; set; } = string.Empty;

    [Display(Name = "Açıklama")]
    public string? Aciklama { get; set; }

    [Required(ErrorMessage = "Hedef miktar zorunludur.")]
    [Range(1, double.MaxValue, ErrorMessage = "Miktar 0'dan büyük olmalıdır.")]
    [Display(Name = "Hedef Miktar (₺)")]
    [Column(TypeName = "decimal(18,2)")]
    public decimal HedefMiktar { get; set; }

    [Range(0, double.MaxValue)]
    [Display(Name = "Biriktirilen (₺)")]
    [Column(TypeName = "decimal(18,2)")]
    public decimal BirikmisMiktar { get; set; } = 0;

    [Display(Name = "Hedef Tarihi")]
    [DataType(DataType.Date)]
    public DateTime? HedefTarihi { get; set; }

    public DateTime OlusturmaTarihi { get; set; } = DateTime.Today;

    [NotMapped]
    public int YuzdeOran => HedefMiktar > 0
        ? (int)Math.Min(100, BirikmisMiktar / HedefMiktar * 100)
        : 0;

    [NotMapped]
    public decimal KalanMiktar => Math.Max(0, HedefMiktar - BirikmisMiktar);

    [NotMapped]
    public bool Tamamlandi => BirikmisMiktar >= HedefMiktar;
}
