using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wman.Repository.Interfaces
{
    public interface IFileRepo
    {
        public Task Create(string path, Stream stream);
    }
}
