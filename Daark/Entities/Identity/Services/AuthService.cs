﻿using Daark.Entities.Identity.Confg;
using Daark.Entities.Identity.Models;
using Daark.Entities.Identity.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Daark.Entities.Identity.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Daark.Entities.Identity.Models;
using Daark.Entities.Identity.Models;
using Daark.Data;
using Microsoft.EntityFrameworkCore;

public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly JWT _jwt;
    private readonly AppDbContext _context;
    public AuthService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager,
        IOptions<JWT> jwt, AppDbContext context)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _jwt = jwt.Value;
        _context = context;
    }

    public async Task<AuthModel> RegisterAsync(RegisterModel model)
    {
        //if (await _userManager.FindByEmailAsync(model.Email) is not null)
        //    return new AuthModel { Message = "Email is already registered!" };

        //if (await _userManager.FindByNameAsync(model.Username) is not null)
        //    return new AuthModel { Message = "Username is already registered!" };

        var user = new ApplicationUser
        {
            UserName = Guid.NewGuid().ToString(),
            Email = $"{Guid.NewGuid()}@mail.com",
            FirstName = model.FirstName,
            LastName = model.LastName,
            PhoneNumber = model.PhoneNumber
        };
        var result = await _userManager.CreateAsync(user, model.Password);
        int teamId = new();
        var team = await _context.Teams.SingleOrDefaultAsync(a => a.Name == model.Team);
       
        if (team != null)
            teamId = team.Id;

        if (team == null)
        {
            await _context.Teams.AddAsync(new Daark.Entities.Team { Name = model.Team });
            await _context.SaveChangesAsync();
            teamId = await _context.Teams.Where(a => a.Name == model.Team).Select(a=>a.Id).SingleOrDefaultAsync();
        }
        await _context.UserTeams.AddAsync(new Daark.Entities.UserTeam { UserId = user.Id,TeamId = teamId });
        await _context.SaveChangesAsync();
       
        if (!result.Succeeded)
        {
            var errors = string.Empty;

            foreach (var error in result.Errors)
                errors += $"{error.Description},";

            return new AuthModel { Message = errors };
        }
        //user.UserId = null;
        // await _userManager.AddToRoleAsync(user, "User");

        var jwtSecurityToken = await CreateJwtToken(user);

        var refreshToken = GenerateRefreshToken();
        user.RefreshTokens?.Add(refreshToken);

        // await _userManager.UpdateAsync(user);
       
        return new AuthModel
        {

            // Email = user.Email,
            // ExpiresOn = jwtSecurityToken.ValidTo,
            IsAuthenticated = true,
            //  Roles = new List<string> { "User" },
            Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
            // Username = user.UserName,
            // RefreshToken = refreshToken.Token,
            //RefreshTokenExpiration = refreshToken.ExpiresOn
            UserId = user.UserId,
            Team = model.Team
        };
    }

    public async Task<AuthModel> GetTokenAsync(TokenRequestModel model)
    {
        var authModel = new AuthModel();
        ApplicationUser user =new();
        if (model.Email is not null)
        {
             user = await _context.Users.Where(a=>a.Email==model.Email).SingleOrDefaultAsync();
        }
        else 
        {
         user = await _context.Users.SingleOrDefaultAsync(a => a.PhoneNumber == model.PhoneNumber);

        }

        if (user is null || !await _userManager.CheckPasswordAsync(user, model.Password))
        {
            authModel.Message = "Email or Password is incorrect!";
            return authModel;
        }
        authModel.UserId = user.UserId;
        var jwtSecurityToken = await CreateJwtToken(user);
      //  var rolesList = await _userManager.GetRolesAsync(user);

        authModel.IsAuthenticated = true;
        authModel.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
        // authModel.Email = user.Email;
        //  authModel.Username = user.UserName;
        // authModel.ExpiresOn = jwtSecurityToken.ValidTo;
        // authModel.Roles = rolesList.ToList();
        authModel.FirstName = user.FirstName;
         authModel.LastName = user.LastName;

        if (user.RefreshTokens.Any(t => t.IsActive))
        {
            var activeRefreshToken = user.RefreshTokens.FirstOrDefault(t => t.IsActive);
            authModel.RefreshToken = activeRefreshToken.Token;
            authModel.RefreshTokenExpiration = activeRefreshToken.ExpiresOn;
        }
        else
        {
            var refreshToken = GenerateRefreshToken();
            authModel.RefreshToken = refreshToken.Token;
            authModel.RefreshTokenExpiration = refreshToken.ExpiresOn;
            user.RefreshTokens.Add(refreshToken);
           // await _userManager.UpdateAsync(user);
        }

        return authModel;
    }

    public async Task<string> AddRoleAsync(AddRoleModel model)
    {
        var user = await _userManager.FindByIdAsync(model.UserId);

        if (user is null || !await _roleManager.RoleExistsAsync(model.Role))
            return "Invalid user ID or Role";

        if (await _userManager.IsInRoleAsync(user, model.Role))
            return "User already assigned to this role";

        var result = await _userManager.AddToRoleAsync(user, model.Role);

        return result.Succeeded ? string.Empty : "Something went wrong";
    }

    private async Task<JwtSecurityToken> CreateJwtToken(ApplicationUser user)
    {
        var userClaims = await _userManager.GetClaimsAsync(user);
        var roles = await _userManager.GetRolesAsync(user);
        var roleClaims = new List<Claim>();

        foreach (var role in roles)
            roleClaims.Add(new Claim("roles", role));

        var claims = new[]
        {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id)
            }
        .Union(userClaims)
        .Union(roleClaims);

        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
        var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

        var jwtSecurityToken = new JwtSecurityToken(
            issuer: _jwt.Issuer,
            audience: _jwt.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwt.DurationInMinutes),
            signingCredentials: signingCredentials);

        return jwtSecurityToken;
    }

    public async Task<AuthModel> RefreshTokenAsync(string token)
    {
        var authModel = new AuthModel();

        var user = await _userManager.Users.SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == token));

        if (user == null)
        {
            authModel.Message = "Invalid token";
            return authModel;
        }

        var refreshToken = user.RefreshTokens.Single(t => t.Token == token);

        if (!refreshToken.IsActive)
        {
            authModel.Message = "Inactive token";
            return authModel;
        }

        refreshToken.RevokedOn = DateTime.UtcNow;

        var newRefreshToken = GenerateRefreshToken();
        user.RefreshTokens.Add(newRefreshToken);
        await _userManager.UpdateAsync(user);

        var jwtToken = await CreateJwtToken(user);
        authModel.IsAuthenticated = true;
        authModel.Token = new JwtSecurityTokenHandler().WriteToken(jwtToken);
        authModel.Email = user.Email;
        authModel.Username = user.UserName;
        var roles = await _userManager.GetRolesAsync(user);
        authModel.Roles = roles.ToList();
        authModel.RefreshToken = newRefreshToken.Token;
        authModel.RefreshTokenExpiration = newRefreshToken.ExpiresOn;

        return authModel;
    }

    public async Task<bool> RevokeTokenAsync(string token)
    {
        var user = await _userManager.Users.SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == token));

        if (user == null)
            return false;

        var refreshToken = user.RefreshTokens.Single(t => t.Token == token);

        if (!refreshToken.IsActive)
            return false;

        refreshToken.RevokedOn = DateTime.UtcNow;

        await _userManager.UpdateAsync(user);

        return true;
    }

    private RefreshToken GenerateRefreshToken()
    {
        var randomNumber = new byte[32];

        using var generator = new RNGCryptoServiceProvider();

        generator.GetBytes(randomNumber);

        return new RefreshToken
        {
            Token = Convert.ToBase64String(randomNumber),
            ExpiresOn = DateTime.UtcNow.AddDays(10),
            CreatedOn = DateTime.UtcNow
        };
    }
}