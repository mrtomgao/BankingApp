using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CityNationalDemo.Models;
using Microsoft.AspNetCore.Authorization;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace CityNationalDemo.Controllers
{
	public class HomeController : Controller
	{		
		public async Task<IActionResult> Index()
		{
			if (User.Identity.IsAuthenticated)
			{
				var st = WebRequests.TokenGrabber(User.Identity.Name);

				var json = await WebRequests.JsonGet("http://localhost:61506/api/Trans/", st);
				var transList = JsonConvert.DeserializeObject<List<Transaction>>(json);
				BankAccount sav = new SavingsAccount(transList);
				BankAccount chk = new CheckingAccount(transList);
				
				ViewBag.SavingsBalance = sav.GetBalance().ToString("C");
				ViewBag.SavingsInterest = sav.GetInterest().ToString("C");
				ViewBag.CheckingBalance = chk.GetBalance().ToString("C");
				ViewBag.CheckingInterest = chk.GetInterest().ToString("C");
				ViewBag.SavingsLatest = sav.GetLatest();
				ViewBag.CheckingLatest = chk.GetLatest();
				return View();
			}
			return RedirectToAction("Login", "Account");
		}


		[HttpPost]
		public IActionResult Verify(string amount, string tranType, string accType)
		{
			if (string.IsNullOrEmpty(amount))
			{
				return RedirectToAction(nameof(Index));
			}
			var st = WebRequests.TokenGrabber(User.Identity.Name);
			ViewBag.Amount = amount;
			ViewBag.AccType = accType;
			ViewBag.TranType = tranType;
			ViewBag.Token = st;

			return View();
		}

		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}

		[HttpPost]
		public async Task<IActionResult> PostTransaction(string amount, string tranType, string accType, string token)
		{
			if (string.IsNullOrEmpty(amount))
			{
				return RedirectToAction(nameof(Index));
			}
			Transaction t = new Transaction { TranType = tranType, AccType = accType, Amount = Convert.ToDecimal(amount) };
			TransactionService ts = new TransactionService(t, token);
			TempData["Response"] = await ts.Post() + Environment.NewLine;
			return RedirectToAction(nameof(Index));
		}

		
		[HttpPost]
		public async Task<IActionResult> SavingsTransfer(string amount)
		{
			if (string.IsNullOrEmpty(amount))
			{
				return RedirectToAction(nameof(Index));
			}
			var st = WebRequests.TokenGrabber(User.Identity.Name);
			Transaction t1 = new Transaction { TranType = "Debit", AccType = "Savings", Amount = Convert.ToDecimal(amount) };
			Transaction t2 = new Transaction { TranType = "Credit", AccType = "Checking", Amount = Convert.ToDecimal(amount) };
			TransactionService ts = new TransactionService(t1, st);
			TempData["Response"] = await ts.Post() + Environment.NewLine;
			ts = new TransactionService(t2, st);
			TempData["Response"] += await ts.Post() + Environment.NewLine;
			return RedirectToAction(nameof(Index));
		}
	}
}
