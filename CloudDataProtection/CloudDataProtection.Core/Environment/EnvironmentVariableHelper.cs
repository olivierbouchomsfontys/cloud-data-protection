using System;
using System.Runtime.InteropServices;

namespace CloudDataProtection.Core.Environment
{
    public static class EnvironmentVariableHelper
    {
        public static string GetEnvironmentVariable(string key)
        {
            string environmentVariable =
                System.Environment.GetEnvironmentVariable(key, EnvironmentVariableTarget.Process) ??
                System.Environment.GetEnvironmentVariable(key);

            if (environmentVariable == null && RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                // On Windows we can fallback to machine and user targets
                environmentVariable = System.Environment.GetEnvironmentVariable(key, EnvironmentVariableTarget.User) ?? 
                         System.Environment.GetEnvironmentVariable(key, EnvironmentVariableTarget.Machine);
            }

            return environmentVariable;
        }
        
        public static string GetHostingEnvironment() => GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
    }
}