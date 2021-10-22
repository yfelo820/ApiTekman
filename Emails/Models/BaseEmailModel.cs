namespace Api.Emails.Models
{
    public abstract class BaseEmailModel
    {
        public string CallbackUrl { get; set; }
        public string ImagesUrl { get; set; }
        public string FontsUrl { get; set; }
    }
}
