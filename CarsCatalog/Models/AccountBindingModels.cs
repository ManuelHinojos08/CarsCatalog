using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace CarsCatalog.Models
{
    // Modelos usados como parámetros para las acciones de AccountController.



    public class AddExternalLoginBindingModel
    {
        [Required]
        [Display(Name = "Token de acceso externo")]
        public string ExternalAccessToken { get; set; }
    }

    public class ChangePasswordBindingModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña actual")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "El número de caracteres de {0} debe ser al menos {2}.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Nueva contraseña")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmar la nueva contraseña")]
        [Compare("NewPassword", ErrorMessage = "La nueva contraseña y la contraseña de confirmación no coinciden.")]
        public string ConfirmPassword { get; set; }
    }

    public class RegisterExternalBindingModel
    {
        [Required]
        [Display(Name = "Correo electrónico")]
        public string Email { get; set; }
    }

    public class RemoveLoginBindingModel
    {
        [Required]
        [Display(Name = "Proveedor de inicio de sesión")]
        public string LoginProvider { get; set; }

        [Required]
        [Display(Name = "Clave de proveedor")]
        public string ProviderKey { get; set; }
    }

    public class SetPasswordBindingModel
    {
        [Required]
        [StringLength(100, ErrorMessage = "El número de caracteres de {0} debe ser al menos {2}.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Nueva contraseña")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmar la nueva contraseña")]
        [Compare("NewPassword", ErrorMessage = "La nueva contraseña y la contraseña de confirmación no coinciden.")]
        public string ConfirmPassword { get; set; }
    }


    #region OwnClases
    public abstract class RegisterModel
    {        

        [Required]
        [JsonProperty("FirstName")]
        [Display(Name = "Nombres")]
        public string FirstName { get; set; }

        [Required]
        [JsonProperty("LastName")]
        [Display(Name = "Apellidos")]
        public string LastName { get; set; }

        [Required]
        [JsonProperty("UserName")]
        [Display(Name = "UserName")]
        public string UserName { get; set; }

        [Required]
        [JsonProperty("Email")]
        [Display(Name = "Correo electrónico")]
        public string Email { get; set; }

        [Required]
        [JsonProperty("Password")]
        [StringLength(100, ErrorMessage = "El número de caracteres de {0} debe ser al menos {2}.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [JsonProperty("ConfirmPassword")]
        [Display(Name = "Confirmar contraseña")]
        [Compare("Password", ErrorMessage = "La contraseña y la contraseña de confirmación no coinciden.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [JsonProperty("Address")]
        [Display(Name = "Dirección")]
        public string Address { get; set; }

        [Required]
        [JsonProperty("Phone")]
        [Display(Name = "Número de contacto")]
        public string Phone { get; set; }

        [Required]
        [JsonProperty("AccountTypeId")]
        [Display(Name = "AccountTypeId")]
        public int AccountTypeId { get; set; }

        [JsonProperty("[FacebookToken]")]
        public string FacebookToken { get; set; }
    }
                

    public class LoginBindingModel
    {
        [Required]
        [JsonProperty("UserName")]
        [Display(Name = "UserName")]
        public string UserName { get; set; }

        [Required]
        [JsonProperty("Password")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
    }

    public class RegisterBindingModel : RegisterModel
    {
        
    }

    public class UpdateAccountBindingModel : RegisterModel
    {
        [Required]
        [JsonProperty("UpdateId")]
        [Display(Name = "UpdateId")]
        public int UpdateId { get; set; }

        [Required]
        [JsonProperty("EditingUserId")]
        [Display(Name = "EditingUserId")]
        public int EditingUserId { get; set; }
    }

    public class DeleteAccountBindingModel
    {
        [Required]
        [JsonProperty("DeleteId")]
        [Display(Name = "DeleteId")]
        public int DeleteId { get; set; }

        [Required]
        [JsonProperty("EditingUserId")]
        [Display(Name = "EditingUserId")]
        public int EditingUserId { get; set; }
    }
    #endregion
}
