namespace ciit_api.DTOs.Auth
{
    public class StudentChangePasswordDto
    {
        // EmailAddress OR PermanentIdentificationNumber
        public string UserName { get; set; } = null!;
        public string OldPassword { get; set; } = null!;
        public string NewPassword { get; set; } = null!;
        public string ConfirmNewPassword { get; set; } = null!;
    }
}
