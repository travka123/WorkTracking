﻿using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace WebAPI.Utils;

public class AuthHelper
{
    public const string ISSUER = "BBServer";
    public const string AUDIENCE = "BBClient";

    const string KEY = "key123456789qwerty";

    public static SymmetricSecurityKey GetSymmetricSecurityKey()
    {

        return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(KEY));
    }

    public static TokenValidationParameters TokenValidationParameters { get; } = new()
    {
        ValidateIssuer = true,
        ValidIssuer = ISSUER,
        ValidateAudience = true,
        ValidAudience = AUDIENCE,
        ValidateLifetime = false,
        IssuerSigningKey = GetSymmetricSecurityKey(),
        ValidateIssuerSigningKey = true,
    };

    public static string CreateToken(int id, string role)
    {

        var jwt = new JwtSecurityToken(
            issuer: ISSUER,
            audience: AUDIENCE,
            claims: new List<Claim> { new Claim(ClaimTypes.NameIdentifier, id.ToString()), new Claim(ClaimTypes.Role, role) },
            signingCredentials: new SigningCredentials(GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

        return new JwtSecurityTokenHandler().WriteToken(jwt);
    }

    public static int GetUserId(ClaimsPrincipal claimsPrincipal)
    {
        var nameIdentifier = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier);
        if (nameIdentifier is null)
        {
            throw new Exception("ClaimTypes.NameIdentifier is null");
        }
        return int.Parse(nameIdentifier.Value);
    }
}
