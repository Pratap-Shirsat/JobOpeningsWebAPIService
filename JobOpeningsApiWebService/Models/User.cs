﻿using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace JobOpeningsApiWebService.Models
{
	public class User
	{
		public Guid Id { get; set; }

		[Required, MaxLength(100, ErrorMessage = "Name should be less then 100 characters.")]
		public string Name { get; set; }

		[Required, EmailAddress, MaxLength(100)]
		public string Email { get; set; }

		[Required, Phone]
		public string Phone { get; set; }

		[Required]
		public string Password { get; set; }

		[Required, MaxLength(15, ErrorMessage = "Username should be less then 15 characters.")]
		public string Username { get; set; }
		public userType UserType { get; set; } = userType.User;

		[JsonIgnore]
		public DateTime CreatedOn { get; set; } = DateTime.Now;
		[JsonIgnore]
		public bool IsDeleted { get; set; } = false;
	}

	public enum userType
	{
		User, Admin
	}
}
