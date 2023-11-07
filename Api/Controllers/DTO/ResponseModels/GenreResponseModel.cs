using Dal.Models;

namespace Api.Controllers.DTO.ResponseModels
{
	public class GenreResponseModel
	{
		public int Id { get; set; }

		public string Name { get; set; }

		public GenreResponseModel(Genre genre)
		{
			Id = genre.Id;
			Name = genre.Name;
		}
	}
}

