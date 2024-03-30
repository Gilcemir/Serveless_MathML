using System.Text;
using System.Text.RegularExpressions;
using ServerlessAPI.Domain;
using Newtonsoft.Json;

namespace ServerlessAPI.Infrastructure;
//Driven Adapter
public class MathMLService : IMathMLService
{
    private readonly IHttpClientFactory _clientFactory;
    public MathMLService(IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory;
    }
    public async Task<string> ConvertToMathML(MathMLRequestModel obj)
    {
        var httpClient = _clientFactory.CreateClient();
        
        var url = "http://localhost:3000/convert";
        
        var body = new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json");
        
        var response = await httpClient.PostAsync(url, body);

        if (response.IsSuccessStatusCode)
        {
            var responseContent = await response.Content.ReadAsStringAsync();
            
            dynamic jsonResponse = JsonConvert.DeserializeObject(responseContent) ?? string.Empty;

            // Obter o valor da propriedade "html"
            string html = jsonResponse.html;

            // Manipular o conteúdo HTML
            string manipulatedHtml = ManipulateHtml(html);

            return manipulatedHtml;
        }
        return $"Error: {response.StatusCode}";
    }
    
    private string ManipulateHtml(string html)
    {
        // Substituir todas as ocorrências de <tagName> por <mml:tagName>
        html = Regex.Replace(html, @"<(\w+)", "<mml:$1");

        // Substituir todas as ocorrências de </tagName> por </mml:tagName>
        html = Regex.Replace(html, @"<\/(\w+)>", "</mml:$1>");
        html = Regex.Replace(html, @"\s\s+", "");
        return html;
    }
}