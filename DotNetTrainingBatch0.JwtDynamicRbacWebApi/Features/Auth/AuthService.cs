using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DotNetTrainingBatch0.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace DotNetTrainingBatch0.JwtDynamicRbacWebApi.Features.Auth;

public class AuthService
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _configuration;

    public AuthService(AppDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public async Task<AuthResponse?> LoginAsync(AuthRequest request)
    {
        var user = await _context.TblUsers.FirstOrDefaultAsync(x =>
            x.Username == request.Username &&
            x.Password == request.Password);

        if (user is null) return null;

        var role = await _context.TblRoles.FirstOrDefaultAsync(r => r.Id == user.RoleId);
        var roleName = role?.RoleName ?? "";

        var permissions = await (from rp in _context.TblRolePermissions
                                 join p in _context.TblPermissions on rp.PermissionId equals p.Id
                                 where rp.RoleId == user.RoleId
                                 select p.PermissionName).ToListAsync();

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, roleName),
        };

        foreach (var permission in permissions)
        {
            claims.Add(new Claim("permission", permission));
        }

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!)
        );

        var credentials = new SigningCredentials(
            key,
            SecurityAlgorithms.HmacSha256
        );

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(
                Convert.ToDouble(_configuration["Jwt:ExpireMinutes"])
            ),
            signingCredentials: credentials
        );

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);

        return new AuthResponse
        {
            AccessToken = jwt,
            Role = roleName,
            Permissions = permissions
        };
    }
}
