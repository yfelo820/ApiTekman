using System.Threading.Tasks;

namespace Api.Interfaces.Shared
{
	public interface IEmailService
	{
		Task SendConfirmationEmailAsync(string email, string name, string callbackUrl, string clientUrl);
		Task SendRemindPasswordEmailAsync(string email, string callbackUrl, string clientUrl);
		Task SendWelcomeEmailAsync(string email, string clientUrl);
	}
}