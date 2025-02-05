using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel;


namespace WeatherApp.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {

    }


    [BindProperty]
    public String City  { get; set; }
    [BindProperty]
    public string Country { get; set; }
    [BindProperty]
    public string Temp{ get; set; }
     [BindProperty]
    public string Winds{ get; set; }
    [BindProperty]
    public string Icon { get; set; }
    [BindProperty]
    public string Description { get; set; }

    public string ErrorMessage { get; set; }

    public async Task<IActionResult>  OnPostAsync()
    {
       if(string.IsNullOrEmpty(City))
       {
        ErrorMessage = "Not Found";
        return Page(); 
       }

       string Apikey = "0bbe5f47e31d124b3ed8575eda341879";
       string  url = $"https://api.openweathermap.org/data/2.5/weather?q={City}&units=metric&appid={Apikey}";

       try{
        using (HttpClient client = new HttpClient())
        {
            var response = await client.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                ErrorMessage = "Error getting data, City not Found";
                return Page();
            }

            var json = await response.Content.ReadAsStringAsync();
            dynamic data = JsonConvert.DeserializeObject(json);

            Country = data.sys.country;
            Temp = data.main.temp.ToString();
            Description = data.weather[0].description;
            Winds = data.wind.speed.ToString();
            

        }

       }
       catch (HttpRequestException )
       {
        ErrorMessage = "network error";
       }
       return Page();
    }
}
