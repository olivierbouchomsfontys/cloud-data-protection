namespace CloudDataProtection.Core.Messaging
{
    public static class RoutingKeys
    {
        public static readonly string BackupConfigurationEntered = "BackupConfigurationEntered";

        public static readonly string UserRegistered = "UserRegistered";
        
        public static readonly string UserDeleted = "UserDeleted";
        
        public static string UserDataDeleted = "UserDataDeleted";

        public static readonly string GoogleAccountConnected = "GoogleAccountConnected";
    }
}