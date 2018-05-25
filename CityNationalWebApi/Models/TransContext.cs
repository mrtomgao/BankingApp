using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityNationalWebApi
{
    public class TransContext : DbContext
	{
		public TransContext(DbContextOptions<TransContext> options) : base(options)
		{

		}
		public DbSet<Trans> Transactions { get; set; }
	}
}
