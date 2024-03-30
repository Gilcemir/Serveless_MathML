using System.Xml.Linq;

namespace ServerlessAPI.Extensions;

public static class Extensions
{
    public static XDocument ToXDocument(this Stream stream)
    {
        using (StreamReader reader = new StreamReader(stream))
        {
            return XDocument.Load(reader);
        }
    }

    public static MemoryStream ToMemoryStream(this XDocument document)
    {
        MemoryStream stream = new MemoryStream();
        document.Save(stream);
        stream.Position = 0;
        return stream;
    }
}