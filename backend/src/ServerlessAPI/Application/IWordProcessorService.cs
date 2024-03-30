namespace ServerlessAPI.Application;
//Driver port
public interface IWordProcessorService
{
    public Task<byte[]> ReplaceMathML(IFormFile file);
    byte[] EndMathML(IFormFile file);
    byte[] StartMathML(IFormFile file);
}