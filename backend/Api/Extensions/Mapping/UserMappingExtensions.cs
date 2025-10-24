using Api.DTOs.Auth;
using Logic.Models;

namespace Api.Extensions.Mapping;

public static class UserMappingExtensions
{
    public static UserDto MapToDto(this Logic.Models.User user) => new UserDto
    {
        Id = user.Id,
        FirstName = user.FirstName,
        LastName = user.LastName,
        Email = user.Email!
    };

    public static User MapToEntity(this AuthRegisterRequestDto dto) => new User
    {
        FirstName = dto.FirstName,
        LastName = dto.LastName,
        Email = dto.Email,
        UserName = dto.Email,
    };
}
