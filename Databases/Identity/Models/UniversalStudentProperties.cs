namespace Api.Identity.Models
{
    public class UniversalUserProperties
    {
        public string UserId { get; set; }

        public string Name { get; set; }

        public bool AcceptNewsletters { get; set; }

        public string SchoolName { get; set; }

        public string SchoolCity { get; set; }

        public string ProfileType { get; set; }

        public ApplicationUser User { get; set; }

}
}