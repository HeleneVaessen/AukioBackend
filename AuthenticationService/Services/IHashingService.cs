namespace AuthenticationService.Services
{
    public interface IHashingService
    {
        byte[] GenerateSalt();
        string HashPassword(string password, byte[] salt);
        string HashInput(string password, string salt);

    }
}
