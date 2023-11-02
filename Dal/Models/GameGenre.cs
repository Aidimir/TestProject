using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace Dal.Models
{
    public class GameGenre
    {
        [ForeignKey("GameId")]
        [JsonIgnore]
        public int GameId { get; set; }
        [JsonIgnore]
        public virtual required Game Game { get; set; }
        [ForeignKey("GenreId")]
        [JsonIgnore]
        public int GenreId { get; set; }
        public virtual required Genre Genre { get; set; }
    }
}

