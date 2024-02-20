using Refit;

namespace Web.Api.CalenderService
{
    public interface ICalenderService
    {
        [Get("/jalali/{year}/{month}/{day}")]
        Task<CalenderApiResponseModel> GetEventAsync(int year, int month, int day);
    }
}
