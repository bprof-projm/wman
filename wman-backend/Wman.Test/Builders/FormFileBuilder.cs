using Microsoft.AspNetCore.Http.Internal;
using System.IO;

namespace Wman.Test.Builders
{
    public class FormFileBuilder
    {
        public static FormFile GetFormFile()
        {
            var content = "Hello World from a Fake File";
            var fileName = "test.png";
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(content);
            writer.Flush();
            stream.Position = 0;

            return new FormFile(stream, 0, stream.Length, "id_from_form", fileName);
        }
    }
}
