using System;

using Nancy.Responses.Negotiation;

namespace Nancy.Serialization.Hyperion
{
    internal static class HyperionHelper
    {
        public const string HyperionContentType = "application/x-hyperion";

        public static bool IsHyperion(string contentType)
        {
            return !string.IsNullOrEmpty(contentType) 
                   && contentType.Equals(HyperionContentType, StringComparison.OrdinalIgnoreCase);
        }

        public static MediaRange GetHyperionMediaRange()
        {
            return new MediaRange(HyperionContentType);
        }
    }
}