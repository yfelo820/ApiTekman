namespace Api.Exceptions
{
    public static class BadRequestCode
    {
        public const string UserAlreadyExist = "userAlreadyExist";
        public const string UserNotExist = "userNotExist";
        public const string RecaptchaError = "recaptchaError";
        public const string ResetPasswordError = "resetPasswordError";
    }
}

