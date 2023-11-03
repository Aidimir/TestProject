using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace Dal.Models
{
    [Table("Developers")]
    public class Developer
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }
        [MinLength(1)]
        public required string Title { get; set; }

        [JsonIgnore]
        public virtual List<Game> Games { get; set; } = new List<Game>();
    }
}

