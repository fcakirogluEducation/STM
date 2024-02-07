namespace Micro1.Services
{
    public class Micro2Services(HttpClient httpClient)
    {
        public async Task<string> GetMicro2()
        {
            //http://localhost:5065/api/Values
            var response = await httpClient.GetAsync("/api/Values");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                return "Error";
            }
        }
    }
}