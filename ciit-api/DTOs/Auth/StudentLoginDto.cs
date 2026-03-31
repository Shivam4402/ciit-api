namespace ciit_api.DTOs.Auth
{
    public class StudentLoginDto
    {
        public string UserName { get; set; } = null!; // EmailAddress OR MobileNumber
        public string Password { get; set; } = null!;
    }
}


