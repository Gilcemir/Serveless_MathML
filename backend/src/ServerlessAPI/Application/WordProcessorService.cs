using System.Xml.Linq;
using DocumentFormat.OpenXml.Packaging;
using ServerlessAPI.Domain;
using ServerlessAPI.Extensions;
using ServerlessAPI.Infrastructure;

namespace ServerlessAPI.Application;
//Driver adapter
public class WordProcessorService : IWordProcessorService
{
    private readonly IMathMLService _mathMLService;
    private const string formulaXmlWithParaAttrName = "oMathPara";
    private const string formulaXmlNoParaName = "oMath";
    
    public WordProcessorService(IMathMLService mathMlService)
    {
        _mathMLService = mathMlService;
    }
    public async Task<byte[]> ReplaceMathML(IFormFile file)
    {
        using MemoryStream stream = new MemoryStream();
        // Copiar o arquivo original para o MemoryStream
        await file.CopyToAsync(stream);

        using (WordprocessingDocument doc = WordprocessingDocument.Open(stream, true))
        {
            var mainPart = doc.MainDocumentPart;
            var docXml = mainPart.GetStream().ToXDocument();

            var equations = docXml
                .Descendants()
                .Where(e => e.Name.LocalName == formulaXmlWithParaAttrName)
                .ToArray();

            for (int i = 0; i < equations.Length; i++)
            {
                var arg = new MathMLRequestModel
                {
                    equation = equations[i].ToString()
                };
                    
                var mathML = await _mathMLService.ConvertToMathML(arg);
                var run = CreateXElementFromString(mathML.ToString());
                equations[i].ReplaceWith(run);
            }

            equations = docXml
                .Descendants()
                .Where(e => e.Name.LocalName == formulaXmlNoParaName)
                .ToArray();
                
            for (int i = 0; i < equations.Length; i++)
            {
                var arg = new MathMLRequestModel
                {
                    equation = equations[i].ToString()
                };
                    
                var mathML = await _mathMLService.ConvertToMathML(arg);
                var run = CreateXElementFromString(mathML.ToString());
                equations[i].ReplaceWith(run);
            }
            // Salvar as alterações no XML
            mainPart.FeedData(docXml.ToMemoryStream());
        }
        // Retornar os bytes do MemoryStream como um array de bytes
        return stream.ToArray();
    }
    private static XElement CreateXElementFromString(string input)
    {
        XNamespace w = "http://schemas.openxmlformats.org/wordprocessingml/2006/main";
        XElement run = new XElement(
            w + "r",
            new XElement(
                w + "rPr",
                new XElement(
                    w + "rFonts",
                    new XAttribute(w + "ascii", "Times New Roman"),
                    new XAttribute(w + "hAnsi", "Times New Roman"),
                    new XAttribute(w + "cs", "Times New Roman"),
                    new XAttribute(w + "val", "12")
                ),
                new XElement(
                    w + "t",
                    input
                )
            )
        );
        return run;
    }

    public byte[] EndMathML(IFormFile file)
    {
        throw new NotImplementedException();
    }

    public byte[] StartMathML(IFormFile file)
    {
        throw new NotImplementedException();
    }
}