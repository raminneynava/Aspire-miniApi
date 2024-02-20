using System.Text.Json.Serialization;

namespace Web.Api.CalenderService
{
    public record CalenderApiResponseModel(
        [property: JsonPropertyName("is_holiday")] bool IsHoliday,
        [property: JsonPropertyName("events")] IReadOnlyList<Event> Events
        );

    public record Event(
     [property: JsonPropertyName("description")] string Description,
     [property: JsonPropertyName("additional_description")] string AdditionalDescription,
      [property: JsonPropertyName("is_holiday")] bool IsHoliday,
      [property: JsonPropertyName("is_religious")] bool IsReligious
     );
}
