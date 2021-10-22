using System;
using System.Linq;

namespace Api.Helpers
{
	public static class RandomString
	{
		const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
		
		public static string Generate(int length)
		{
			Random random = new Random();
			return new string(
				Enumerable.Repeat(chars, length)
				.Select(s => s[random.Next(s.Length)])
				.ToArray()
			);
		}
	}
}