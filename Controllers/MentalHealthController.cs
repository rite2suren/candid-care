namespace community_resources.Controllers;

using community_resources.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

public class MentalHealthController : Controller
{
    // GET: MentalHealth
    public ActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<ActionResult> Search(string location)
    {
        // API URL with the obtained coordinates
        string apiUrl = $"https://findtreatment.gov/locator/exportsAsJson/v2?limitType=0&limitValue=23";

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
    
}