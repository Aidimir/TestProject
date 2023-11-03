using System;
using System.ComponentModel.DataAnnotations;
using Dal.Interfaces;

namespace Api.Controllers.DTO.RequestModels
{
	public class UpdatedGameDto : IPublicGame
	{
        [Required]
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Developer { get; set; }

        [Required]
        public List<string> Genres { get; set; }
    }
}

