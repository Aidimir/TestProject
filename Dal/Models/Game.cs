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

		[MinLength(1)]
        public int DeveloperId { get; set; }

        [ForeignKey("DeveloperId")]
        [JsonIgnore]
        public virtual Developer Developer { get; set; }

        public required string DeveloperTitle { get; set; }

        [MinLength(1)]
        public required List<string> Genre { get; set; }
	}
}

