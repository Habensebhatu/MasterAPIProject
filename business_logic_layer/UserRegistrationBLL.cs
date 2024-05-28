using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using business_logic_layer.ViewModel;
using Data_layer;
using Data_layer.Context.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;



namespace business_logic_layer
{
    public class UserRegistrationBLL
    {
        private readonly IConfiguration _configuration;
        private readonly IDbContextFactory _dbContextFactory;

        public UserRegistrationBLL(IDbContextFactory dbContextFactory, IConfiguration configuration)
        {
            _dbContextFactory = dbContextFactory;
            _configuration = configuration;
        }


        public async Task<string> RegisterUser(UserRegistrationModel userViewModel, string connectionString)
        {
            var UserRegistrationDAL = new UserRegistrationDAL(_dbContextFactory, connectionString);
            string password = userViewModel.Password;

            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

            var userEntity = new UserRegistrationEntityModel
            {
                UserId = Guid.NewGuid(),
                BedrijfsNaam = userViewModel.BedrijfsNaam,
                KvkNummer = userViewModel.KvkNummer,
                BTW = userViewModel.BTW,
                IsApproved = false,
                FirstName = userViewModel.FirstName,
                LastName = userViewModel.LastName,
                Password = hashedPassword,
                Email = userViewModel.Email,



                Address = new Address
                {
                    Street = userViewModel.Address.Street,
                    PhoneNumber = userViewModel.Address.PhoneNumber,
                    Residence = userViewModel.Address.Residence,
                    ZipCode = userViewModel.Address.ZipCode
                }
            };

            await UserRegistrationDAL.AddUser(userEntity);
            string token = CreateToken(userEntity);

            return token;



        }

        private string CreateToken(UserRegistrationEntityModel userEntity)
        {

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenSetting = _configuration?.GetSection("AppSettings:Token");
            var key = Encoding.UTF8.GetBytes(tokenSetting.Value);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                new Claim(ClaimTypes.NameIdentifier, userEntity.UserId.ToString()),
                new Claim(ClaimTypes.Email, userEntity.Email),
                new Claim("firstName", userEntity.FirstName),
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return tokenString;
        }

        public async Task<string> LoginUser(Login loginModel, string connectionString)
        {
            var UserRegistrationDAL = new UserRegistrationDAL(_dbContextFactory, connectionString);
            var userEntity = await UserRegistrationDAL.GetUserByEmail(loginModel.Username);

            if (userEntity == null)
            {
                return null;
            }

            bool isValidPassword = BCrypt.Net.BCrypt.Verify(loginModel.Password, userEntity.Password);

            if (!isValidPassword)
            {
                return null;
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenSetting = _configuration?.GetSection("AppSettings:Token");
            var key = Encoding.UTF8.GetBytes(tokenSetting.Value);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                new Claim(ClaimTypes.NameIdentifier, userEntity.UserId.ToString()),
                new Claim(ClaimTypes.Email, userEntity.Email),
                new Claim("firstName", userEntity.FirstName),
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return tokenString;
        }

        public async Task<List<UserRegistrationModel>> GetAllUsers(string connectionString)
        {
            var UserRegistrationDAL = new UserRegistrationDAL(_dbContextFactory, connectionString);
            var users = await UserRegistrationDAL.GetAllUsers();
            return users.Select(u => new UserRegistrationModel
            {
                BedrijfsNaam = u.BedrijfsNaam,
                KvkNummer = u.KvkNummer,
                BTW = u.BTW,
                IsApproved = u.IsApproved,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Email = u.Email,
                UserId = u.UserId,

                Address = new Addres
                {
                    Street = u.Address.Street,
                    PhoneNumber = u.Address.PhoneNumber,
                    Residence = u.Address.Residence,
                    ZipCode = u.Address.ZipCode
                }
            }).ToList();
        }

        public async Task<UserRegistrationModel> GetUserById(Guid userId, string connectionString)
        {
            var UserRegistrationDAL = new UserRegistrationDAL(_dbContextFactory, connectionString);
            var userEntity = await UserRegistrationDAL.GetUserById(userId);
            if (userEntity == null)
            {
                return null;
            }
            return new UserRegistrationModel
            {
                // Map properties from userEntity to UserRegistrationModel

                BedrijfsNaam = userEntity.BedrijfsNaam,
                KvkNummer = userEntity.KvkNummer,
                BTW = userEntity.BTW,
                Email = userEntity.Email,
                FirstName = userEntity.FirstName,
                IsApproved = userEntity.IsApproved,
                LastName = userEntity.LastName,
                UserId = userEntity.UserId,


                // ...other properties
                Address = new Addres
                {
                    Street = userEntity.Address.Street,
                    PhoneNumber = userEntity.Address.PhoneNumber,
                    Residence = userEntity.Address.Residence,
                    ZipCode = userEntity.Address.ZipCode
                }
            };
        }

        public async Task<UserRegistrationModel> ApproveUser(Guid userId, string connectionString)
        {
            var UserRegistrationDAL = new UserRegistrationDAL(_dbContextFactory, connectionString);
            var userEntity = await UserRegistrationDAL.Update(userId);
            if (userEntity == null)
            {
                return null;
            }
            return new UserRegistrationModel
            {
                BedrijfsNaam = userEntity.BedrijfsNaam,
                KvkNummer = userEntity.KvkNummer,
                BTW = userEntity.BTW,
                Email = userEntity.Email,
                FirstName = userEntity.FirstName,
                IsApproved = userEntity.IsApproved,
                LastName = userEntity.LastName,
                UserId = userEntity.UserId,
                Address = new Addres
                {
                    Street = userEntity.Address.Street,
                    PhoneNumber = userEntity.Address.PhoneNumber,
                    Residence = userEntity.Address.Residence,
                    ZipCode = userEntity.Address.ZipCode
                }
            };

        }

        public async Task<bool> RejectUser(Guid userId, string connectionString)
        {
            var UserRegistrationDAL = new UserRegistrationDAL(_dbContextFactory, connectionString);

            if (userId != null)
            {
                UserRegistrationDAL.Removeuser(userId);
                return true;
            }

            return false;
        }



    }
}

