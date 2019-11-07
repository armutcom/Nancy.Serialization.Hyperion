using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Nancy.Configuration;
using Nancy.Responses.Negotiation;

namespace Nancy.Serialization.Hyperion
{
    public class HyperionProcessor : IResponseProcessor
    {
        private static readonly IEnumerable<Tuple<string, MediaRange>> ExtensionMappingsStatic = new Tuple<string, MediaRange>[1]
        {
            new Tuple<string, MediaRange>("hyperion", HyperionHelper.GetHyperionMediaRange())
        };

        private readonly INancyEnvironment _environment;

        private readonly ISerializer _serializer;

        public HyperionProcessor(IEnumerable<ISerializer> serializers, INancyEnvironment environment)
        {
            _serializer = serializers.FirstOrDefault(x => x.CanSerialize(HyperionHelper.HyperionContentType));
            _environment = environment;
        }

        public ProcessorMatch CanProcess(MediaRange requestedMediaRange, dynamic model, NancyContext context)
        {
            return requestedMediaRange.Matches(HyperionHelper.HyperionContentType)
                       ? new ProcessorMatch {ModelResult = MatchResult.ExactMatch, RequestedContentTypeResult = MatchResult.DontCare}
                       : new ProcessorMatch {RequestedContentTypeResult = MatchResult.DontCare, ModelResult = MatchResult.NoMatch};
        }

        public Response Process(MediaRange requestedMediaRange, dynamic model, NancyContext context)
        {
            return new HyperionResponse(model, _serializer);
        }

        public IEnumerable<Tuple<string, MediaRange>> ExtensionMappings => ExtensionMappingsStatic;
    }
}