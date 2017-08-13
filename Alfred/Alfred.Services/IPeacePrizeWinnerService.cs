using Alfred.Services.Models;

namespace Alfred.Services
{
    public interface IPeacePrizeWinnerService
    {
        Prize GetPrizeByYear(long year);
    }
}