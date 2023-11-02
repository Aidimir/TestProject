using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace Dal.Models
{
    [Table("Genres")]
    public class Genre
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }
        [MinLength(1)]
        public required string Name { get; set; }
        [NotMapped]
        [JsonIgnore]
        public List<GameGenre> GameGenres { get; set; }
    }
}

