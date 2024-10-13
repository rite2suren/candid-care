namespace community_resources.Models;
public class MentalHealthServicesResponse
{
    public int Page { get; set; }
    public int TotalPages { get; set; }
    public int RecordCount { get; set; }
    public List<MentalHealthService> Rows { get; set; }
}