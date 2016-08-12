using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Resources;
using CourseProject.Environment;

namespace CourseProject.Models
{

    public class ManageModel
    {
        [Display(Name = "Изображение")]
        public string Picture { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "PasswordCurrent", ResourceType = typeof(Resource))]
        public string OldPassword { get; set; }

        [StringLength(100, ErrorMessageResourceName = "LengthError", ErrorMessageResourceType = typeof(Resource), MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "PasswordNew", ResourceType = typeof(Resource))]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "PasswordConfirmNew", ResourceType = typeof(Resource))]
        [Compare("NewPassword", ErrorMessageResourceName = "PasswordCompareError", ErrorMessageResourceType = typeof(Resource))]
        public string ConfirmPassword { get; set; }
    }

    public class IndexViewModel
    {
        public bool HasPassword { get; set; }
        public IList<UserLoginInfo> Logins { get; set; }
        public string PhoneNumber { get; set; }
        public bool TwoFactor { get; set; }
        public bool BrowserRemembered { get; set; }
    }

    public class ManageLoginsViewModel
    {
        public IList<UserLoginInfo> CurrentLogins { get; set; }
        public IList<AuthenticationDescription> OtherLogins { get; set; }
    }

    public class FactorViewModel
    {
        public string Purpose { get; set; }
    }

    public class SetPasswordViewModel
    {
        [Required]
        [StringLength(100, ErrorMessageResourceName = "LengthError", ErrorMessageResourceType = typeof(Resource), MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "PasswordNew", ResourceType = typeof(Resource))]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "PasswordConfirmNew", ResourceType = typeof(Resource))]
        [Compare("NewPassword", ErrorMessageResourceName = "PasswordCompareError", ErrorMessageResourceType = typeof(Resource))]
        public string ConfirmPassword { get; set; }
    }

    public class ChangePasswordViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "PasswordCurrent", ResourceType = typeof(Resource))]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessageResourceName = "LengthError", ErrorMessageResourceType = typeof(Resource), MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "PasswordNew", ResourceType = typeof(Resource))]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "PasswordConfirmNew", ResourceType = typeof(Resource))]
        [Compare("NewPassword", ErrorMessageResourceName = "PasswordCompareError", ErrorMessageResourceType = typeof(Resource))]
        public string ConfirmPassword { get; set; }
    }

    public class AddPhoneNumberViewModel
    {
        [Required]
        [Phone]
        [Display(Name = "Номер телефона")]
        public string Number { get; set; }
    }

    public class VerifyPhoneNumberViewModel
    {
        [Required]
        [Display(Name = "Код")]
        public string Code { get; set; }

        [Required]
        [Phone]
        [Display(Name = "Номер телефона")]
        public string PhoneNumber { get; set; }
    }

    public class ConfigureTwoFactorViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
    }
}