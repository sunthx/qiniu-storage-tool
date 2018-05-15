using Windows.ApplicationModel.Resources.Core;

namespace QnStorageClient.Utils
{
    public static class ResourceUtils
    {
        public static string GetText(string key)
        {
            var resourceContext = ResourceContext.GetForViewIndependentUse();
            var resourceMap = ResourceManager.Current.MainResourceMap.GetSubtree("Resources");
            return resourceMap.GetValue(key, resourceContext).ValueAsString;
        }
    }
}
