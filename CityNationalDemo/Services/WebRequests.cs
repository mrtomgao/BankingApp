using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CityNationalDemo
{
    public static class WebRequests
    {

		public static string TokenGrabber(string User)
		{
			var claimsdata = new[] { new Claim(ClaimTypes.Name, User) };
			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("TomGao1234567890"));
			var signInCred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
			var token = new JwtSecurityToken(
				issuer: "tomssite.com",
				audience: "tomssite.com",
				expires: DateTime.Now.AddSeconds(30),
				claims: claimsdata,
				signingCredentials: signInCred
				);
			var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
			return tokenString;
		}

		public async static Task<string> JsonGet(string url, string securityToken)
		{
			HttpClient client;
			client = new HttpClient();
			client.BaseAddress = new Uri(url);
			client.DefaultRequestHeaders.Accept.Clear();
			client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
			client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", securityToken);
			HttpResponseMessage responseMessage = await client.GetAsync(url);
			if (responseMessage.IsSuccessStatusCode)
			{
				return responseMessage.Content.ReadAsStringAsync().Result;
			}
			else
			{
				return "Error: " + responseMessage.StatusCode;
			}
		}

		public async static Task<string> JsonPost(string url, dynamic obj, string securityToken)
		{
			HttpClient client;
			client = new HttpClient();
			client.BaseAddress = new Uri(url);
			client.DefaultRequestHeaders.Accept.Clear();
			client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
			client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", securityToken);
			var json = JsonConvert.SerializeObject(obj);

			HttpResponseMessage responseMessage = await client.PostAsync(url, new StringContent(json, Encoding.UTF8, "application/json"));
			if (responseMessage.IsSuccessStatusCode)
			{
				return responseMessage.Content.ReadAsStringAsync().Result;
			}
			else
			{
				return "Error: " + responseMessage.StatusCode;
			}
		}
	}
}
