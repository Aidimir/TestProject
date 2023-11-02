using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Controllers.DTO.RequestModels
{
	public class CreateGameRequestModel
	{
		[Required]
		[MinLength(1)]
		public string Title { get; set; }
		[Required]
		[MinLength(1, ErrorMessage = "Game should always has a developer")]
		public string Developer { get; set; }
		[Required]
		[MinLength(1, ErrorMessage = "At least 1 genre should be pointed")]
		public List<string> Genre { get; set; }
	}
}