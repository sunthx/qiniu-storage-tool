using Microsoft.Toolkit.Uwp.UI.Controls;

namespace QnStorageClient.Services
{
    public static class NotificationService
    {
        private static InAppNotification _inAppNotification;

        public static void Initialize(InAppNotification inAppNotification)
        {
            _inAppNotification = inAppNotification;
            _inAppNotification.ShowDismissButton = false;
        }

        public static void ShowMessage(string message,int duration = 0)
        {
            _inAppNotification.Show(message, duration);
        }

        public static void Dismiss()
        {
            _inAppNotification.Dismiss();
        }
    }
}
