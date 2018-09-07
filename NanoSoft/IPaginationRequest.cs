namespace NanoSoft
{
    public interface IPaginationRequest
    {
        int CurrentPage { get; }
        int PageSize { get; }
    }
}
