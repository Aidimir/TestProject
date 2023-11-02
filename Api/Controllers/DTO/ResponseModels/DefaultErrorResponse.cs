using System;
using System.ComponentModel.DataAnnotations;

namespace Api.Controllers.DTO.ResponseModels
{
	public class DefaultErrorResponseModel
	{
		[Display(Name = "detail")]
		public string Detail { get; set; }
	}
}

