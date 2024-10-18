namespace community_resources.Controllers;

using community_resources.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

public class MentalHealthController : Controller
{

    private readonly HttpClient client = new HttpClient();

    // GET: MentalHealth
    public ActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<ActionResult> Search(string location)
    {
        var geoCode = await GetGeoCode(location);

        // API URL with the obtained coordinates
        string apiUrl = $"https://findtreatment.gov/locator/exportsAsJson/v2?sCodes=OTP,BU,NU,DM&sAddr={geoCode.Latitude},{geoCode.Longitude}&limitType=2&limitValue=80467.2&pageSize=5&page=1&sort=0";        

        using HttpClient client = new HttpClient();

        try
        {
            // Get the JSON response from the API
            var services = await client.GetFromJsonAsync<MentalHealthServicesResponse>(apiUrl);

            if (services == null || services.RecordCount <= 0)
            {
                ViewBag.Message = "No services found for the provided location.";
                return View("Index");
            }
            else
            {
                return View("Results", services.Rows);
            }
        }
        catch (Exception ex)
        {
            ViewBag.Error = $"An error occurred while fetching the data. \n{ex.Message} \n{ex.StackTrace}";
            return View("Index");
        }
    }

    private async Task<GeoCode> GetGeoCode(string location){
        
        using HttpClient client = new HttpClient();
        string apiKey = Environment.GetEnvironmentVariable("google-maps-api-key");
        string address = location;
        string requestUrl = $"https://maps.googleapis.com/maps/api/geocode/json?address={Uri.EscapeDataString(address)}&key={apiKey}";

        GeoCode geoCode = new GeoCode();
        try
        {
            HttpResponseMessage response = await client.GetAsync(requestUrl);
            response.EnsureSuccessStatusCode();

            string jsonResponse = await response.Content.ReadAsStringAsync();
            JObject parsedResponse = JObject.Parse(jsonResponse);

             var results = parsedResponse["results"];
            if (results.HasValues)
            {
                string formattedAddress = (string)results[0]["formatted_address"];
                var result = results[0]["geometry"]["location"];
                double lat = (double)result["lat"];
                double lng = (double)result["lng"];
                geoCode.Latitude = lat.ToString();
                geoCode.Longitude = lng.ToString();
            }
        }
        catch (Exception ex)
        {
            
        }
        return geoCode;
    }
}