﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wman.Repository.Interfaces
{
    public interface IFileRepo
    {
        Task Create(string path, Stream stream);
        Task<DirectoryInfo> GetDetails(string path);
        Task DeleteOldFiles(string path, string extension, DateTime olderThan);
    }
}
