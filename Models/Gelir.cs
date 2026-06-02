using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BudgetApp.Models;

public enum GelirTipi
{
    [Display(Name = "Maaş")] Maas,
    [Display(Name = "Freelance")] Freelance,
    [Display(Name = "Kira Geliri")] KiraGeliri,
    [Display(Name = "Yatırım")] Yatirim,
    [Display(Name = "Diğer")] Diger
}

public class Gelir
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Açıklama zorunludur.")]
    [Display(Name = "Açıklama")]
    public string Açıklama { get; set; } = string.Empty;

    [Required(ErrorMessage = "Miktar zorunludur.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Miktar 0'dan büyük olmalıdır.")]
    [Display(Name = "Miktar (₺)")]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Miktar { get; set; }

    [Required(ErrorMessage = "Tarih zorunludur.")]
    [Display(Name = "Tarih")]
    [DataType(DataType.Date)]
    public DateTime Tarih { get; set; } = DateTime.Today;

    [Display(Name = "Gelir Tipi")]
    public GelirTipi Tip { get; set; } = GelirTipi.Maas;
}
