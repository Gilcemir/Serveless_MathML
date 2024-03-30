using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using ServerlessAPI.Application;

namespace ServerlessAPI.Controllers;

[ApiController]
[Route("[controller]")]
[EnableCors(Consts.CorsPolicyName)]
public class MathMLController : ControllerBase
{
    private readonly IWordProcessorService  _wordProcessorService;
    public MathMLController(IWordProcessorService wordProcessorService)
    {
        _wordProcessorService = wordProcessorService;
    }
    
    [HttpGet]
    public string Get()
    {
        return "Hello World!";
    }
    [HttpPost("replace-mathml")]
    public async Task<IActionResult> ReplaceMathML(IFormFile arquivo)
    {
        var fileName = arquivo.FileName;
        byte[] novoArquivo = await _wordProcessorService.ReplaceMathML(arquivo);
        return File(novoArquivo, "application/vnd.openxmlformats-officedocument.wordprocessingml.document", fileName);
    }
}