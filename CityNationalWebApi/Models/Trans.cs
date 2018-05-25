using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CityNationalWebApi
{
    public class Trans
    {
		[Key]
		public int TranID { get; set; }
		public string UserName { get; set; }
		public string TranType { get; set; }
		public string AccType { get; set; }
		public decimal Amount { get; set; }
		public DateTime Timestamp { get; set; } = DateTime.Now;

	}
}
