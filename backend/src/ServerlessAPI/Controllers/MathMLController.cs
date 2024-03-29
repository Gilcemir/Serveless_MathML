using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace ServerlessAPI.Controllers;

[ApiController]
[Route("[controller]")]

//[EnableCors(Consts.CorsPolicyName)]
public class MathMLController : ControllerBase
{
    public MathMLController()
    {
        
    }
    
    [HttpGet]
    public string Get()
    {
        return "Hello World!";
    }
}