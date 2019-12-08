using System;
using System.ComponentModel.DataAnnotations;

namespace ContactApi.Models
{
    public class Contact : BaseModel
    {
        [Required(ErrorMessage = "First Name is required"), MaxLength(150)]
        public string FirstName { get; set; }
        
        [Required(ErrorMessage = "Last Name is required"), MaxLength(150)]
        public string LastName { get; set; }
        
        [Required(ErrorMessage = "Email address is required"), MaxLength(150), EmailAddress]
        public string Email { get; set; }
        
        [Required(ErrorMessage = "Phone number is required"), MaxLength(10), Phone]
        public string Phone { get; set; }
       
        public bool Status { get; set; }
    }
}
