using AutoMapper;
using Microsoft.EntityFrameworkCore;
using HotDeskBooking.Models.Dto;
using HotDeskBooking.Models.Entity;
using HotDeskBooking.Models.Responses;
using HotDeskBooking.Commands.Users.Create;
using HotDeskBooking.Commands.Users.Delete;
using HotDeskBooking.Queries.Users.GetUserById;
using HotDeskBooking.Commands.Users.Login;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using HotDeskBooking.Models.Helpers;
using HotDeskBooking.Helpers.SignatureGenerator;
using HotDeskBooking.Helpers;

namespace HotDeskBooking.Services;

public class UserService : IUserService
{
    private readonly HotDeskBookingContext _context;
    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserService(
        HotDeskBookingContext context,
        IMapper mapper,
        IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<IReadOnlyList<UserDto>> GetAllAsync(CancellationToken ct)
    {
        var users = await _context.Users
            .Include(x => x.Role)
            .ToListAsync(cancellationToken: ct);

        return _mapper.Map<List<UserDto>>(users);
    }

    public async Task<IReadOnlyList<UserRoleDto>> GetUserRolesAsync(CancellationToken ct)
    {
        var users = await _context.UserRoles
            .ToListAsync(cancellationToken: ct);

        return _mapper.Map<List<UserRoleDto>>(users);
    }


    public async Task<UserDto> GetByIdAsync(GetUserByIdQuery query, CancellationToken ct)
    {
        var user = await GetUserAsync(query.Id, ct);

        return _mapper.Map<UserDto>(user);
    }

    public async Task<CreateUserResponse> CreateAsync(CreateUserCommand command, CancellationToken ct)
    {
        try
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(x => x.Email == command.CreateRequest.Email, cancellationToken: ct);

            if(user is not null) 
                throw new Exception("An account with the given address already exists. Try logging in.");

            var entity = new User
            {
                Email = command.CreateRequest.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(command.CreateRequest.Password),
                RoleId = command.CreateRequest.RoleId,
            };
            await _context.Users.AddAsync(entity, ct);
            await _context.SaveChangesAsync(ct);
            return new CreateUserResponse
            {
                Success = true,
                User = _mapper.Map<UserDto>(entity)
            };
        }
        catch (Exception exception)
        {
            return new CreateUserResponse
            {
                Success = false,
                Error = exception.Message
            };
        }

    }

    public async Task<StandardResponse> DeleteAsync(DeleteUserCommand command, CancellationToken ct)
    {
        try
        {
            var entity = await GetUserAsync(command.Id, ct);
            _context.Users.Remove(entity);
            await _context.SaveChangesAsync(ct);
            return new StandardResponse
            {
                Success = true
            };
        }
        catch (Exception exception)
        {
            return new StandardResponse
            {
                Success = false,
                Error = exception.Message
            };
        }
    }

    public async Task<AuthenticationResponse> AuthenticateAsync(LoginCommand model, CancellationToken ct)
    {
        var user = await _context.Users
            .Include(u => u.Role)
            .SingleOrDefaultAsync(x => x.Email.ToLower() == model.LoginRequest.Email.ToLower(), cancellationToken: ct)
            ?? throw new Exception("Incorrect password or email address.");

        var verifiedPass = BCrypt.Net.BCrypt.Verify(model.LoginRequest.Password, user.PasswordHash);

        if (!verifiedPass)
            throw new Exception("Incorrect password or email address");

        // authentication successful
        var accessTokenDto = GenerateToken(user);
        var refreshTokenDto = GenerateRefreshToken();

        await CreateOrUpdateUserRefreshTokenAsync(user.Id, refreshTokenDto);

        return new AuthenticationResponse
        {
            Success = true,
            Token = accessTokenDto.Token,
            TokenExpires = accessTokenDto.Expires,
            RefreshToken = refreshTokenDto.Token,
            RefreshTokenExpires = refreshTokenDto.Expires,
            UserRole = new UserRoleDto(user.Role.Type, user.RoleId),
            UserId = 1,
        };
    }

    public int? GetUserIdFromHttpContextIfExist()
    {
        var context = _httpContextAccessor.HttpContext;
        if (context == null)
        {
            throw new KeyNotFoundException("HttpContext is null");
        }
        var userId = context.Items[UserHttpContextConstants.UserIdFromToken];

        var claimsUserId = context.User.Claims.FirstOrDefault(c => c.Type.Contains("nameidentifier"));

        var uId = context.User.Claims.FirstOrDefault(c => c.Type == "id");

        if (claimsUserId is not null)
        {
            return int.Parse(claimsUserId.Value);
        }
        if (uId is not null)
        {
            return int.Parse(uId.Value);
        }
        if (userId is null)
        {
            return null;
        }

        return (int)userId;
    }

    #region Helper methods

    private async Task<int> CreateOrUpdateUserRefreshTokenAsync(int userId, TokenDto refreshTokenDto)
    {
        var refreshToken = await _context.UserRefreshTokens
            .FindAsync(userId);

        var tokenHash = BCrypt.Net.BCrypt.HashString(refreshTokenDto.Token);
        if (refreshToken is null)
        {
            _context.Add(new UserRefreshToken()
            {
                UserId = userId,
                RefreshTokenHash = tokenHash
            });
        }
        else
        {
            refreshToken.RefreshTokenHash = tokenHash;
            refreshToken.RefreshTokenExpires = refreshTokenDto.Expires;
        }

        return await _context.SaveChangesAsync();
    }

    private async Task<User> GetUserAsync(int id, CancellationToken ct)
    {
        var user = await _context.Users
            .Include(x => x.Role)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken: ct)
            ?? throw new KeyNotFoundException($"User with Id {id} not found.");

        return user;
    }
    private TokenDto GenerateRefreshToken()
    {
        return new(SignatureGenerator.Generate(), GetRefreshTokenExpirationDate());
    }

    private TokenDto GenerateToken(User user)
    {
        // generate token that is valid for 1 days
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AppSettings.Secret));
        var claims = new List<Claim>
        {
            new Claim("id", user.Id.ToString()),
            new Claim(ClaimTypes.Role, user.RoleId.ToString())
        };

        var expires = GetAccessTokenExpirationDate();
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
        var token = new JwtSecurityToken(claims: claims, expires: expires, signingCredentials: creds);

        return new(tokenHandler.WriteToken(token), expires);
    }

    private DateTime GetAccessTokenExpirationDate()
    {
        var expireDate = DateTime.UtcNow;

        int.TryParse(AppSettings.JwtExpireMinutes, out int expireMinutes);
        if (expireMinutes == 0)
            expireDate.AddMinutes(1440);

        expireDate = expireDate.AddMinutes(expireMinutes);

        return expireDate.ToUniversalTime();
    }

    private DateTime GetRefreshTokenExpirationDate()
    {
        var expireDate = DateTime.UtcNow;

        if (int.TryParse(AppSettings.RefreshExpireDays, out int expireDays))
        {
            expireDate = expireDate.AddDays(expireDays);
        }
        else
        {
            expireDate = expireDate.AddDays(30);
        }

        return expireDate.ToUniversalTime();
    }
    #endregion
}