using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using CertificateCommon.Api;
using SkillService.Data;
using SkillService.Dtos;
using SkillService.Models;

namespace SkillService.Features
{
    public class CertificateService
    {
        private readonly ISkillRepo _repository;
        private readonly IMapper _mapper;

        public CertificateService(ISkillRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ApiResponse<IEnumerable<CertificateReadDto>>> GetCertificatesAsync()
        {
            var certificateItems = await _repository.GetAllCertificatesAsync();
            var certificateDtos = _mapper.Map<IEnumerable<CertificateReadDto>>(certificateItems);
            return ApiResponse<IEnumerable<CertificateReadDto>>.SuccessResponse(certificateDtos);
        }

        public async Task<ApiResponse<string>> TestInboundConnectionAsync()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Inbound test of from Certificate controller");
            Console.ResetColor();
            return ApiResponse<string>.SuccessResponse("Inbound test of from Certificate controller");
        }
    }
}
