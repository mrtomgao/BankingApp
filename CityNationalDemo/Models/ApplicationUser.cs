using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace CityNationalDemo.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
		public string CustomerName { get; set; }
		public CustomerType CustomerClass { get; set; } 
    }

	public enum CustomerType {
		savings,
		freechecking,
		premium
	}

}
