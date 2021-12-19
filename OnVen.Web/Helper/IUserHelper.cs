using Microsoft.AspNetCore.Identity;
using OnVen.Common.Enum;
using OnVen.Web.Data.Entities;
using OnVen.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnVen.Web.Helper
{
    public interface IUserHelper
    {
        Task<User> GetUserAsync(string email);

        Task<IdentityResult> AddUserAsync(User user, string password);

        Task CheckRoleAsync(string roleName);

        Task AddUserToRoleAsync(User user, string roleName);

        Task<bool> IsUserInRoleAsync(User user, string roleName);

        Task<SignInResult> LoginAsync(LoginViewModel model);

        Task LogoutAsync();


        //Se pasa el Usuario y el Pass, para ver si es Valido el Usuario, nos da un Result
        Task<SignInResult> ValidatePasswordAsync(User user, string password);

        //Para Autoregistro de Usuarios
        Task<User> AddUserAsync(AddUserViewModel model, string imageId, UserType userType);


        //Para Administrar El Cambio de Clave de los usuarios
        Task<IdentityResult> ChangePasswordAsync(User user, string oldPassword, string newPassword);

        Task<IdentityResult> UpdateUserAsync(User user);

        Task<User> GetUserAsync(Guid userId);


        //Metodos para Validar el Correo del Usuario para activar la cuenta

        Task<string> GenerateEmailConfirmationTokenAsync(User user);

        Task<IdentityResult> ConfirmEmailAsync(User user, string token);


        //Recuperacion de la Clave de Usuario
        Task<string> GeneratePasswordResetTokenAsync(User user);

        Task<IdentityResult> ResetPasswordAsync(User user, string token, string password);
    }
}
