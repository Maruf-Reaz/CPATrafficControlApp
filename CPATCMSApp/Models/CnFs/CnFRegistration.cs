using System.ComponentModel.DataAnnotations;

namespace CPATCMSApp.Models.CnFs
{
    public class CnFRegistration
    {
        [Key]
        public string Id { get; set; }

        [Required(ErrorMessage = "The Name field is required.")]
        [Display(Name = "Name:")]
        public string CnFName { get; set; }

        [Required(ErrorMessage = "The C&F Code field is required.")]
        [Display(Name = "C&F Code:")]
        public string CnFCode { get; set; }

        [Required(ErrorMessage = "The Phone Number field is required.")]
        [RegularExpression(@"(^(\01)?(01){1}[23456789]{1}(\d){8})$", ErrorMessage = "Please provide a valid mobile number")]
        [Display(Name = "Phone Number:")]
        public string CnFPhoneNumber { get; set; }

        [Display(Name = "Address:")]
        public string CnFAddress { get; set; }
    }
}
