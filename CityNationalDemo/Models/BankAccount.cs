using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityNationalDemo.Models
{
    public abstract class BankAccount
    {
		protected IEnumerable<Transaction> t;

		public BankAccount(IEnumerable<Transaction> trans)
		{
			this.t = trans;
		}

		public decimal GetBalance()
		{
			decimal total = 0.0M;
			foreach (var itm in t)
			{
				if (itm.TranType == "Credit")
				{
					total += itm.Amount;
				}
				else
				{
					total -= itm.Amount;
				}				
			}
			return total;
		}

		public abstract decimal GetInterest();

		public IEnumerable<Transaction> GetLatest()
		{
			return t.OrderByDescending(n => n.Timestamp).Take(5);
		}

	}

	public class SavingsAccount : BankAccount
	{
		public SavingsAccount(IEnumerable<Transaction> t) : base(t.Where(x => x.AccType == "Savings"))
		{
		}

		public override decimal GetInterest()
		{
			return GetBalance() * .047M;
		}

	}
	public class CheckingAccount : BankAccount
	{
		public CheckingAccount(IEnumerable<Transaction> t) : base(t.Where(x => x.AccType == "Checking"))
		{
		}

		public override decimal GetInterest()
		{
			return GetBalance() * .007M;
		}
	}
}
