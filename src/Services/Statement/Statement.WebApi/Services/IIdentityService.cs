namespace Statement.WebApi.Services
{
    using Statement.WebApi.Models;

    public interface IIdentityService
    {
        IdentityModel GetIdentity();
    }
}
