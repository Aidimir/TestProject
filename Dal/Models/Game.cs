using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using Dal.Interfaces;

namespace Dal.Models
{
    [Table("Games")]
    public class Game
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        [MinLength(1)]
        public required string Title { get; set; }

        [JsonIgnore]
        [ForeignKey("DeveloperId")]
        public int DeveloperId { get; set; }
        public virtual Developer Developer { get; set; }

        [JsonIgnore]
        [NotMapped]
        public required string DeveloperTitle { get; set; }

        public virtual List<Genre> Genres { get; set; } = new List<Genre>();
    }
}