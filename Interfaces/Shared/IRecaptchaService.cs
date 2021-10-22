using System.Threading.Tasks;

namespace Api.Interfaces.Shared
{
	public interface IRecaptchaService
	{
		Task Validate(string token);
	}
}