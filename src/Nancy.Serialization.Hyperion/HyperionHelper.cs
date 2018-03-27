using System;

namespace Nancy.Serialization.Hyperion
{
    internal static class HyperionHelper
    {
        public static bool IsHyperion(string contentType)
        {
            if (string.IsNullOrEmpty(contentType))
            {
                return false;
            }

            return contentType.Equals("application/x-hyperion", StringComparison.OrdinalIgnoreCase);
        }
    }
}