using System.ComponentModel.DataAnnotations;

namespace BudgetApp.Models;

public class Kategori
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Ad alanı zorunludur.")]
    [Display(Name = "Kategori Adı")]
    public string Ad { get; set; } = string.Empty;

    public ICollection<Harcama> Harcamalar { get; set; } = new List<Harcama>();
}
