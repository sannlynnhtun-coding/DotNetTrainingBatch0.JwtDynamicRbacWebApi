using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;
using DotNetTrainingBatch0.Database;
using Microsoft.EntityFrameworkCore;

namespace DotNetTrainingBatch0.JwtDynamicRbacWebApi.Filters;

public class PermissionAttribute : TypeFilterAttribute
{
    public PermissionAttribute(string permission, bool checkFromDb = false) : base(typeof(PermissionFilter))
    {
        Arguments = new object[] { permission, checkFromDb };
    }
}

public class PermissionFilter : IAsyncAuthorizationFilter
{
    private readonly string _permission;
    private readonly bool _checkFromDb;
    private readonly AppDbContext _context;

    public PermissionFilter(string permission, bool checkFromDb, AppDbContext context)
    {
        _permission = permission;
        _checkFromDb = checkFromDb;
        _context = context;
    }

    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        bool hasPermission = false;


        if (_checkFromDb)
        {
            var userIdString = context.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (int.TryParse(userIdString, out int userId))
            {
                var user = await _context.TblUsers.FirstOrDefaultAsync(u => u.Id == userId);
                if (user != null)
                {
                    hasPermission = await (from rp in _context.TblRolePermissions
                                           join p in _context.TblPermissions on rp.PermissionId equals p.Id
                                           where rp.RoleId == user.RoleId && p.PermissionName == _permission
                                           select p.Id).AnyAsync();
                }
            }
        }
        else
        {
            hasPermission = context.HttpContext.User.HasClaim(c => 
                c.Type == "permission" && c.Value == _permission);
        }

        if (!hasPermission)
        {
            context.Result = new ForbidResult();
        }
    }
}
