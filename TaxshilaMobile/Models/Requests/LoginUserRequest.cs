namespace TaxshilaMobile.Models.Requests
{
    public class LoginUserRequest
    {
        public string UserName { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Token { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string UserId { get; set; }
        public string VerificationCode { get; set; }
        public string DeviceId { get; set; }
        public bool IsForceToUpdate { get; set; }
    }

    public class MobileRegisterRequest
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public string Phonenumber { get; set; }

        public string Name { get; set; }

        public string ConfirmPassword { get; set; }
    }
}