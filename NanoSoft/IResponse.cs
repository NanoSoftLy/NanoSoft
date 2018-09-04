using System.Collections.Generic;

namespace NanoSoft
{
    public interface IResponse
    {
        Dictionary<string, List<string>> Errors { get; }
        bool IsValid { get; }
        string Message { get; }
        ResponseState State { get; }
    }
}