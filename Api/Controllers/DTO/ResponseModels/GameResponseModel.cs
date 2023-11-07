using System.ComponentModel.DataAnnotations;
using Dal.Models;
namespace Api.Controllers.DTO.ResponseModels
{
	public class GameResponseModel
	{
        public int Id { get; set; }

        public string Title { get; set; }

		public DeveloperResponseModel Developer { get; set; }

		public List<GenreResponseModel> Genres { get; set; }
		
        public GameResponseModel(Game game)
		{
			Id = game.Id;
			Title = game.Title;
			Developer = new DeveloperResponseModel(game.Developer);
			Genres = game.Genres.Select(g => new GenreResponseModel(g)).ToList();
		}
	}
}

