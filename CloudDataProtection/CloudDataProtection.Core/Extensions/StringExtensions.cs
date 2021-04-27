namespace CloudDataProtection.Core.Extensions
{
    public static class StringExtensions
    {
        public static string Remove(this string str, string c)
        {
            return str.Replace(c, "");
        }  
    }
}