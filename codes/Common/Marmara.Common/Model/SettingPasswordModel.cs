using System;
using System.ComponentModel.DataAnnotations;

namespace Marmara.Common.Model
{
    public class SettingPasswordModel
    {
        public string AppUserGuid { get; set; }

        [Required(ErrorMessage = "Please enter your password.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Please enter your new password.")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Please re-enter the new password for confirmation.")]
        [Compare("NewPassword", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; }

    }
}
