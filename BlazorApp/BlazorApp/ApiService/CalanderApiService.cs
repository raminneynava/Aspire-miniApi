namespace BlazorApp.ApiService
{
    public class CalanderApiService(HttpClient client)
    {
        public async Task<List<EventApiModel>?> GetEvent(int year,int month,int day)
        {
            return await client.GetFromJsonAsync<List<EventApiModel>>($"/api/events/{year}/{month}/{day}");
        }
    }

    public record EventApiModel(string Description, string AdditionalDescription, bool IsHoliday, bool IsReligious);
}
