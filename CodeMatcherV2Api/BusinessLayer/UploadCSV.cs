using AutoMapper;
using CodeMatcherV2Api.BusinessLayer.Interfaces;
using CodeMatcherV2Api.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace CodeMatcherV2Api.BusinessLayer
{
    public class UploadCSV : IUploadCSV
    {
        private readonly IMapper _mapper;
        public UploadCSV(IMapper mapper)
        {
            _mapper = mapper;
        }
        public async Task<string> GetUploadCSVAsync(UploadModel uploadModel)
        {
            return "CSV File Uploaded";
        }

        public async Task<string> WriteFile(IFormFile file)
        {
            string fileName;
            try
            {
                var extension = "." + file.FileName.Split('.')[file.FileName.Split('.').Length - 1];
                fileName = DateTime.Now.Ticks + extension;
                var pathBuilt = Path.Combine(Directory.GetCurrentDirectory(), "Resources\\Files");
                if (!Directory.Exists(pathBuilt))
                {
                    Directory.CreateDirectory(pathBuilt);
                }

                var path = Path.Combine(Directory.GetCurrentDirectory(), "Resources\\Files", fileName);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                return path;
            }
            catch (Exception ex)
            {
            }
            return "";
        }
    }
}
