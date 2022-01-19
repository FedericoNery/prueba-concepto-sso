using System.ComponentModel.DataAnnotations;

namespace SSOConcepto.DataContract
{
    public class CreateUserDC
    {
        [Required]
        public string Email { get; set; }
        
        [DataType(dataType: DataType.Password)]
        public string Password { get; set; }
        
        [Required]
        public string Username { get; set; }
    }
}
