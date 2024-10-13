namespace community_resources.Models;
public class MentalHealthService
{
    public int _irow { get; set; }
    public string Name1 { get; set; }
    public string Name2 { get; set; }
    public string Street1 { get; set; }
    public string City { get; set; }
    public string Street2 { get; set; }
    public string State { get; set; }
    public string Zip { get; set; }
    public string Phone { get; set; }
    public string Intake1 { get; set; }
    public string Hotline1 { get; set; }
    public string Website { get; set; }
    public string Latitude { get; set; }
    public string Longitude { get; set; }
    public double Miles { get; set; }
    public List<ServiceDetail> Services { get; set; }
    public string TypeFacility { get; set; }
}