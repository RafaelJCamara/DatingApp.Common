namespace DatingApp.Common.Helpers.User;

public interface IUser
{
    public int? Id { get; }

    public string? Username { get; }

    public bool? IsAuthenticated { get; }
}
