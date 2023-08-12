using AutoMapper;
using CodeMappingEfCore.DatabaseModels;
using CodeMatcherV2Api.BusinessLayer.Interfaces;
using CodeMatcherV2Api.EntityFrameworkCore;
using CodeMatcherV2Api.Middlewares;
using CodeMatcherV2Api.Middlewares.HttpHelper;
using CodeMatcherV2Api.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeMatcherV2Api.BusinessLayer
{
    public class CodeGenerationOverwrite : ICodeGenerationOverwrite
    {
        private readonly IMapper _mapper;
        private CodeMatcherDbContext _context;
        public CodeGenerationOverwrite(IMapper mapper, CodeMatcherDbContext context)
        {
             _mapper= mapper;
            _context= context;
        }

        public async Task<List<CodeGenerationOverwriteModel>> GetAllCodeGenerationOverwriteAsync()
        {
            List<CodeGenerationOverwriteDto> data = new List<CodeGenerationOverwriteDto>();
            data = await _context.CodeGenerationOverwrites.Include(i => i.HistoryData).ToListAsync();
            return _mapper.Map<List<CodeGenerationOverwriteModel>>(data);
        }


        public async Task<CodeGenerationOverwriteModel> GetCodeGenerationOverwriteByIdAsync(int id)
        {
            CodeGenerationOverwriteDto data = new CodeGenerationOverwriteDto();
            data = await _context.CodeGenerationOverwrites.Include(i => i.HistoryData).FirstOrDefaultAsync(f=>f.Id==id);
            return _mapper.Map<CodeGenerationOverwriteModel>(data);
        }


        public async Task<bool> UpdateCodeGenerationOverwriteAsync(CodeGenerationOverwriteModel codeGenerationOverwrite)
        {
            if (codeGenerationOverwrite != null)
            {
                var existModel =await _context.CodeGenerationOverwrites.FirstOrDefaultAsync(f => f.Id == codeGenerationOverwrite.Id);
                if (existModel != null)
                {
                    existModel.SerialNumber = codeGenerationOverwrite.SerialNumber;
                    existModel.Link = codeGenerationOverwrite.Link;
                    existModel.Frm = codeGenerationOverwrite.Frm;
                    existModel.ResponseReference = codeGenerationOverwrite.ResponseReference;
                    existModel.Description = codeGenerationOverwrite.Description;
                    existModel.Too = codeGenerationOverwrite.Too;
                }
                _context.Add(new CodeGenerationOverwriteHistoryDto { From = existModel.Frm, To = existModel.Too });
                _context.Update(existModel);
                _context.SaveChanges();
                return true;
            }
            return false;
        }
    }
}
