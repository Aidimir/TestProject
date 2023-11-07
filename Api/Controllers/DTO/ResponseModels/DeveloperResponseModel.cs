using System;
using System.ComponentModel.DataAnnotations;
using Dal.Models;

namespace Api.Controllers.DTO.ResponseModels
{
	public class DeveloperResponseModel
	{
        public int Id { get; set; }
        
        public string Title { get; set; }

        public DeveloperResponseModel(Developer developer)
		{
			Id = developer.Id;
			Title = developer.Title;
		}
	}
}

