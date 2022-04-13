//not used yet, have to figure out identity function
using System;
using System.Collections.Generic;
using CA1.Models;
using System.ComponentModel.DataAnnotations;

namespace CA1.Data
{
	public class UserRole
	{

		public UserRole()
		{
			user = new List<User>();
		}
		[Key]
		public int Id { get; set; }

		public string Userrole { get; set; }

		public virtual ICollection<User> user { get; set; }
	}
}

