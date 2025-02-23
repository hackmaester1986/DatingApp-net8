using System;
using System.Diagnostics.Eventing.Reader;
using API.DTOs;
using API.Entities;

namespace API.Interfaces;

public interface IUserRepository
{
    void Update(AppUser user);

    Task<bool> SavelAllAsync();

    Task<IEnumerable<AppUser>> GetUsersAsync();
    Task<AppUser?> GetUserByIdAsync(int id);
    Task<AppUser?> GetUserByUserNameAsync(string username);
    Task<IEnumerable<MemberDto>> GetMembersAsync();

    Task<MemberDto?> GetMemberAsync(string username);
}
