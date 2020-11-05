namespace SpdCommon.Modules
{
    public interface ICoreResultPagination
    {
        int Pages { get; set; }

        long Rows { get; set; }
    }
}