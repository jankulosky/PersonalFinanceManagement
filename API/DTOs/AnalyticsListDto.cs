namespace API.DTOs
{
    public class AnalyticsListDto
    {
        public List<AnalyticsDto> Groups { get; set; }

        public AnalyticsListDto()
        {
            Groups = new();
        }
    }
}
