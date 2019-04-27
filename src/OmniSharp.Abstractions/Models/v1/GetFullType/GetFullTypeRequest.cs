using OmniSharp.Mef;

namespace OmniSharp.Models.GetFullType
{
    [OmniSharpEndpoint(OmniSharpEndpoints.GetFullType, typeof(GetFullTypeRequest), typeof(GetFullTypeResponse))]
    public class GetFullTypeRequest : Request
    {
    }
}
