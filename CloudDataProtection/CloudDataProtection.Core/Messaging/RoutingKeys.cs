namespace CloudDataProtection.Core.Messaging
{
    public static class RoutingKeys
    {
        public static readonly string BackupConfigurationEntered = "BackupConfigurationEntered";

        public static readonly string UserRegistered = "UserRegistered";
        
        public static readonly string UserDeleted = "UserDeleted";
        
        public static readonly string UserDataDeleted = "UserDataDeleted";

        public static readonly string UserDeletionComplete = "UserDeletionComplete";

        public static readonly string GoogleAccountConnected = "GoogleAccountConnected";

        public static readonly string EmailChangeRequested = "EmailChangeRequested";
    }
}