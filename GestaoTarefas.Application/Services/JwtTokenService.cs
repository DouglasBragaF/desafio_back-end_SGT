using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace GestaoTarefas.Application.Services
{
  public class JwtTokenService
  {
    private readonly string _issuer;
    private readonly string _audience;
    private readonly SymmetricSecurityKey _key;

    public JwtTokenService(string secretKey, string issuer, string audience)
    {
      if (secretKey.Length < 32)
      {
        throw new ArgumentException("A chave secreta deve ter pelo menos 32 caracteres.", nameof(secretKey));
      }

      _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
      _issuer = issuer;
      _audience = audience;
    }

    public string GenerateToken(string user)
    {
      var tokenHandler = new JwtSecurityTokenHandler();
      var tokenDescriptor = new SecurityTokenDescriptor
      {
        Subject = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, user) }),
        Expires = DateTime.UtcNow.AddHours(1),
        Issuer = _issuer,
        Audience = _audience,
        SigningCredentials = new SigningCredentials(_key, SecurityAlgorithms.HmacSha256Signature)
      };

      var token = tokenHandler.CreateToken(tokenDescriptor);
      return tokenHandler.WriteToken(token);
    }

    public TokenValidationParameters GetTokenValidationParameters()
    {
      return new TokenValidationParameters
      {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = _issuer,
        ValidAudience = _audience,
        IssuerSigningKey = _key
      };
    }
  }
}
