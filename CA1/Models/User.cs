using System;
using System.ComponentModel.DataAnnotations;

namespace CA1.Models
{
	public class User
	{
		public User()
		{
			Id = new Guid();
		}
		public Guid Id { get; set; }

		[Required]
		[MaxLength(30,ErrorMessage = "The User Name Field must not be more than 30 characters long")]
		[MinLength(6, ErrorMessage = "The User Name Field must be at least 6 characters long")]
		public string UserName { get; set; }

		[Required]
		[MaxLength(50)]
		public string PassHash { get; set; }

		public Guid? sessionId { get; set; }

		/* navigational property: 1-to-1 relationship to shopcart */
		public virtual ShopCart shopcart { get; set; }
	}
}

