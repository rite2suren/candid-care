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
        // API URL with the obtained coordinates
        string apiUrl = $"https://findtreatment.gov/locator/exportsAsJson/v2?sCodes=OTP,BU,NU,DM&sAddr=33.876118,-117.921410&limitType=2&limitValue=80467.2&pageSize=5&page=1&sort=0";

        //https://findtreatment.gov/locator/exportsAsJson/v2?sCodes=OTP,BU,NU,DM&sAddr=33.876118,-117.921410&limitType=2&limitValue=80467.2&pageSize=100&page=1&sort=0
        //https://findtreatment.gov/locator/exportsAsJson/v2?limitType=0&limitValue=23

        //https://findtreatment.gov/locator/exportsAsJson/v2?sAddr=%2239.141375,-77.203552%22&limitType=0&limitValue=23

        // apiUrl = $"https://findtreatment.gov/locator/exportsAsJson/v2?sAddr={System.Net.WebUtility.UrlEncode(zipCode)}&limitType=0&limitValue=23";

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
                foreach (var service in services.Rows)
                {
                    if (service.Latitude != null && service.Longitude != null)
                    {
                        //await SendToGoogleApi(service.Latitude.ToString(), service.Longitude.ToString());
                        var locations = services.Rows
                .Where(service => service.Latitude != null && service.Longitude != null)
                .Select(service => new { Latitude = service.Latitude, Longitude = service.Longitude })
                .ToList();

            // Pass the locations to the view via ViewBag
            ViewBag.Locations = locations;
            



                    }
                }
                return View("Results", services.Rows);
            }
        }
        catch (Exception ex)
        {
            ViewBag.Error = $"An error occurred while fetching the data. \n{ex.Message} \n{ex.StackTrace}";
            return View("Index");
        }
    }
    
    public async Task SendToGoogleApi(string latitude, string longitude)
    {
        string googleApiUrl = $"https://maps.googleapis.com/maps/api/geocode/json?latlng={latitude},{longitude}&key=AIzaSyB9h8rQ2nQoTUTQw_02fnfo0WB_PtwD2B8";

        try
        {
            var response = await client.GetStringAsync(googleApiUrl);
            var jsonResponse = JObject.Parse(response);
            // Process the response as needed
            // Example: log or store results
        }
        catch (Exception ex)
        {
            // Log or handle the error as necessary
        }
    }


}