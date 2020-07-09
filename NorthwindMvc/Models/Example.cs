using System.ComponentModel.DataAnnotations;

namespace NorthwindMvc.Models
{
    public class Example
    {
        [Range(1, 10)]
        public int? ID {get; set;}

        [Required]
        public string Color {get; set;}
    }
}