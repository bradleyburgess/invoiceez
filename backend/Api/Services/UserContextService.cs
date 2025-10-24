using System.Security.Claims;
using Logic.Database;
using Logic.Models;
using Logic.Utils;

namespace Api.Services;

public class HttpUserContextService(
    IHttpContextAccessor httpContextAccessor,
    AppDbContext dbContext
) : IUserContextService
{
    public Result<Guid> GetUserId()
    {
        var userId = httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
        {
            return Result<Guid>.Failure("Unauthorized");
        }
        return Result<Guid>.Success(Guid.Parse(userId));
    }

    public async Task<Result<User>> GetUserAsync()
    {
        var userIdResult = GetUserId();
        if (!userIdResult.IsSuccess)
        {
            return Result<User>.Failure("Unauthorized");
        }
        var user = await dbContext.Users.FindAsync(userIdResult.Value);
        if (user == null)
        {
            return Result<User>.Failure("User not found");
        }
        return Result<User>.Success(user);
    }
}

public interface IUserContextService
{
    Result<Guid> GetUserId();
    Task<Result<User>> GetUserAsync();
}

