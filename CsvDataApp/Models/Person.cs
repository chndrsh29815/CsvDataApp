using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CsvDataApp.Models
{
    public class Person
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int Identity { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string Sirname { get; set; }

        [Range(0, 120)]
        public int Age { get; set; }

        [Required]
        [StringLength(1)]
        public string Sex { get; set; }

        [Phone]
        public string Mobile { get; set; }

        public bool Active { get; set; }
    }
}
