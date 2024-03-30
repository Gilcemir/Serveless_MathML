using ServerlessAPI.Domain;

namespace ServerlessAPI.Infrastructure;
//Driven Port
public interface IMathMLService
{
    public Task<string> ConvertToMathML(MathMLRequestModel obj);
}