namespace CloudDataProtection.Core.Environment
{
    public static class Environment
    {
        public static readonly string Test = "Test";
        public static readonly string Development = "Development";
        
        public static string CurrentEnvironment => EnvironmentVariableHelper.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
    }
}