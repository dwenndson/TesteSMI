using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using AutoMapper;
using IogServices.Enums;
using IogServices.ExceptionHandlers.Exceptions;
using IogServices.Models.DAO;
using IogServices.Models.DTO;
using IogServices.Repositories;
using Microsoft.IdentityModel.Tokens;

namespace IogServices.Services.Impl
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }
        
        public List<UserDto> GetAll()
        {
            List<UserDto> userDtos = _mapper.Map<List<User>, List<UserDto>>(_userRepository.GetAll());
            return userDtos;
        }

        public UserDto GetByEmail(string email)
        {
            return _mapper.Map<User, UserDto>(_userRepository.GetByEmail(email));
        }

        public UserDto Save(UserDto userDto)
        {
            User savedUser = _userRepository.GetByEmail(userDto.Email);
            User user = _mapper.Map<UserDto, User>(userDto);
            if (savedUser == null)
            {
                user.Id = Guid.NewGuid();
                return _mapper.Map<User, UserDto>(_userRepository.Save(user));
            }
            else if (!savedUser.Active)
            {
                savedUser.UpdateFields(user);
                savedUser.Active = true;
                return _mapper.Map<User, UserDto>(_userRepository.Update(savedUser));
            }
            else
            {
                throw new ExistentEntityException("O usuário de e-mail " + savedUser.Email + " já existe");
            }
        }

        public UserDto Update(UserDto userDto)
        {
            User user = _mapper.Map<UserDto, User>(userDto);
            User savedUser = GetExistingUser(userDto.Email);
            savedUser.UpdateFields(user);
            return _mapper.Map<User, UserDto>(_userRepository.Update(savedUser));
        }

        public void Deactivate(string email)
        {
            User savedUser = GetExistingUser(email);
            savedUser.Active = false;
            _userRepository.Update(savedUser);
        }

        public object Login(UserDto userDto, SigningConfigurations signingConfigurations, TokenConfigurations tokenConfigurations)
        {
            UserDto savedUserDto = CheckCredentials(userDto);

            return new
            {
                authenticated = true,
                accessToken = JWTToken(userDto, signingConfigurations, tokenConfigurations),
                message = "OK",
                user = savedUserDto
            };
        }

        public User GetExistingUser(string email)
        {
            User savedUser = _userRepository.GetByEmail(email);
            if (savedUser == null)
            {
                throw new InvalidConstraintException("O usuário informado é inválido");
            }
            return savedUser;
        }
        
        public UserDto CheckCredentials(UserDto userDto)
        {
            User savedUser = _userRepository.GetByEmail(userDto.Email);
            if (savedUser == null) throw new BadCredentialsException();
            
            byte[] salt = Convert.FromBase64String(savedUser.Salt);
            var pass = User.GenerateHash(userDto.Password, salt);
            
            if (savedUser.Password != pass) throw new BadCredentialsException();
            return _mapper.Map<User, UserDto>(savedUser);
        }

        public object JWTToken(UserDto userDto, SigningConfigurations signingConfigurations, TokenConfigurations tokenConfigurations)
        {
            ClaimsIdentity identity = new ClaimsIdentity(
                new GenericIdentity(userDto.Email, "Login"),
                new[] {
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
                    new Claim(JwtRegisteredClaimNames.UniqueName, userDto.Email),
                    new Claim("clientType", userDto.ClientType.ToString()) 
                }
            );

            DateTime createDate = DateTime.Now;
            DateTime endDate = createDate +
                               TimeSpan.FromSeconds(tokenConfigurations.Seconds);

            var handler = new JwtSecurityTokenHandler();
            var securityToken = handler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = tokenConfigurations.Issuer,
                Audience = tokenConfigurations.Audience,
                SigningCredentials = signingConfigurations.SigningCredentials,
                Subject = identity,
                NotBefore = createDate,
                Expires = endDate
            });
            return handler.WriteToken(securityToken);
        }
    }
}