﻿using JobOpeningsApiWebService.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace JobOpeningsApiWebService.Dto
{
	public class UserReqDto
	{
		[Required, MaxLength(100, ErrorMessage = "Name should be less then 100 characters.")]
		public string Name { get; set; }

		[Required, EmailAddress, MaxLength(100)]
		public string Email { get; set; }

		[Required, Phone]
		public string Phone { get; set; }

		[Required, MaxLength(13, ErrorMessage = "Maximum character length for password is 13."), MinLength(5, ErrorMessage = "Minimum password length is 5 characters")]
		public string Password { get; set; }

		[Required, MaxLength(15, ErrorMessage = "Username should be less then 15 characters.")]
		public string Username { get; set; }
	}
}
