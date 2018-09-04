using NanoSoft.Attributes;
using NanoSoft.Resources;

namespace NanoSoft
{
    public enum ResponseState
    {
        [RequestMessage(nameof(SharedMessages.ResponseState_Valid))]
        Valid,

        [RequestMessage(nameof(SharedMessages.ResponseState_BadRequest))]
        BadRequest,

        [RequestMessage(nameof(SharedMessages.ResponseState_NotFound))]
        NotFound,

        [RequestMessage(nameof(SharedMessages.ResponseState_Forbidden))]
        Forbidden,

        [RequestMessage(nameof(SharedMessages.ResponseState_Unauthorized))]
        Unauthorized,

        [RequestMessage(nameof(SharedMessages.ResponseState_Unavailable))]
        Unavailable,

        [RequestMessage(nameof(SharedMessages.ResponseState_Unacceptable))]
        Unacceptable
    }
}
