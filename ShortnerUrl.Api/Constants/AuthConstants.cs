namespace ShortnerUrl.Api.Constants;

public static class AuthConstants
{
    public static class Roles
    {
        public const string Admin = "1";
        public const string User = "2";
        public const string Developer = "3";
    }
    
    public static class Policies
    {
        public const string RequireAdmin = "RequireAdmin";
        public const string RequireAdminOrDeveloper = "RequireAdminOrDeveloper";
    }
}