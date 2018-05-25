using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CityNationalWebApi.Controllers
{
	[Authorize]
	[Produces("application/json")]
	[Route("api/Trans")]
	public class TransController : Controller
	{
		private readonly TransContext _context;

		public TransController(TransContext context)
		{
			_context = context;
		}

		// GET Unspecific
		public async Task<IActionResult> Get()
		{
			var u = User.Identity.Name;
			var trans = await _context.Transactions.Where(t => t.UserName == u).ToListAsync();
			return Ok(trans);
		}

		// GET Specific
		[HttpGet("{id}")]
		public async Task<IActionResult> GetTrans([FromRoute] int id)
		{
			var trans = await _context.Transactions.SingleOrDefaultAsync(m => m.TranID == id);

			if (trans == null)
			{
				return NotFound();
			}

			return Ok(trans);
		}

		// POST Tran
		[HttpPost]
        public async Task<IActionResult> PostTrans([FromBody]Trans trans)
        {
			//can get other claims data if needed
			//var identity = (ClaimsIdentity)User.Identity;
			//IEnumerable<Claim> claims = identity.Claims;

			trans.UserName = User.Identity.Name;
			_context.Transactions.Add(trans);
			await _context.SaveChangesAsync();
			
			return Ok("Secure transaction posted! Hello " + User.Identity.Name + " from the API server :)");
		}


    }
}
