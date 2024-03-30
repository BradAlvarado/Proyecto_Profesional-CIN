using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sistema_CIN.Models
{
    [NotMapped]
    public class PmeViewModel
    {
  
        public Encargados Encargado { get; set; }
        public Pme Pme { get; set; }
    }
}
