namespace MemberTrack.Services.Dtos
{
    public class UserUpdatePasswordDto
    {
        public string OldPassword { get; set; }

        public string NewPassword { get; set; }
    }
}