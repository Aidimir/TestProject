using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

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
        //[JsonIgnore]
        public virtual Developer Developer { get; set; }

        [JsonIgnore]
        [NotMapped]
        public required string DeveloperTitle { get; set; }

        //[JsonIgnore]
        [NotMapped]
        public List<GameGenre> GameGenres { get; set; } = new List<GameGenre>();
    }
}

