using System;
using System.IO;
using Newtonsoft.Json.Linq;

namespace CloudDataProtection.CI.Secrets
{
    class Program
    {
        /// <summary>
        /// Usage from CI:
        /// Working directory: ./CloudDataProtection
        ///
        /// Arguments should be in this order because of the very simple setup. This might be improved in the feature.
        /// 
        /// Command: dotnet run --project ./CloudDataProtection.CI.Secrets/*.csproj -- --env "Environemt" --secret SuperSecretKey --target "TargetProperty"
        /// Example: dotnet run --project ./CloudDataProtection.CI.Secrets/*.csproj -- --env "Test" --secret SuperSecretKey --target "Jwt.Secret"
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            string environment = args[1];
            string secret = args[3];
            string target = args[5];

            string solutionDirectory = Directory.GetCurrentDirectory();

            Console.WriteLine("\n=== CloudDataProtection Secret Manager===\n");
            Console.Out.WriteLine($"Environment: {environment}");
            Console.Out.WriteLine($"Target: {target}");
            
            DirectoryInfo directoryInfo = new DirectoryInfo(solutionDirectory);
            
            if (!directoryInfo.Exists)
            {
                Console.Error.WriteLine($"Could not find directory {solutionDirectory}!");
                Environment.Exit(1);
            }

            string[] subDirectories = Directory.GetDirectories(solutionDirectory);

            foreach (string directory in subDirectories)
            {
                string fileName = GetFileName(environment);

                string filePath = Path.Combine(directory, fileName);

                if (File.Exists(filePath))
                {
                    string json = File.ReadAllText(filePath);

                    JObject jObject = JObject.Parse(json);

                    JToken token = jObject.SelectToken(target);
                    
                    if (token != null)
                    {
                        token.Replace(secret);
                        
                        File.WriteAllText(filePath, jObject.ToString());

                        Console.Out.WriteLine($"Updated {target} in {filePath}");
                    }
                    else
                    {
                        Console.Out.WriteLine($"Could not find {target} in {filePath}, skipping");
                    }
                }
            }
        }

        static string GetFileName(string environment)
        {
            switch (environment.ToUpper())
            {
                case "PRODUCTION":
                    return "appsettings.json";
                case "CI":
                    return "appsettings.ci.json";
                case "TEST":
                    return "appsettings.Test.json";
                case "DEVELOPMENT":
                    return "appsettings.Development.json";
                default:
                    throw new ArgumentException(nameof(environment));
            }
        }
    }
}