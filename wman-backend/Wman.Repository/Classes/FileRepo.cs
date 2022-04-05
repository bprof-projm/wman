using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wman.Repository.Interfaces;

namespace Wman.Repository.Classes
{
    public class FileRepo : IFileRepo
    {
        public async Task Create(string path, Stream stream)
        {
            using (var fileStream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write))
            {
                stream.Position = 0;
                stream.CopyTo(fileStream);
                fileStream.Close();
            }
        }
        public async Task<DirectoryInfo> GetDetails(string path)
        {
            return new DirectoryInfo(path);
        }
        public async Task DeleteOldFiles(string path, string extension, DateTime olderThan)
        {
            var files = Directory.GetFiles(path).Where(x => x.ToLower().EndsWith(extension.ToLower()));

            foreach (string file in files)
            {
                FileInfo fi = new FileInfo(file);
                if (fi.CreationTime < olderThan)
                    fi.Delete();
            }
        }
    }
}
