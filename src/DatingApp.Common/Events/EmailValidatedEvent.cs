namespace DatingApp.Common.Events
{
    public class EmailValidatedEvent
    {
        public string Username { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;
    }
}
