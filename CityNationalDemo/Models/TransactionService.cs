using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CityNationalDemo
{
    public class TransactionService
    {
		protected Transaction t;
		protected string token;

		public TransactionService(Transaction tran, string securityToken)
		{
			this.t = tran;
			this.token = securityToken;
		}

		public async Task<string> Post()
		{
			return await WebRequests.JsonPost("http://localhost:61506/api/Trans/", t, token);
		}

	}

	public class Transaction
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
