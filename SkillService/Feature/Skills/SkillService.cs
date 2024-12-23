using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using CertificateCommon.Api;
using SkillService.Data;
using SkillService.Dtos;
using SkillService.Models;

namespace SkillService.Features
{
    public class Skill_Service
    {
        private readonly ISkillRepo _repository;
        private readonly IMapper _mapper;

        public Skill_Service(ISkillRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ApiResponse<IEnumerable<SkillReadDto>>> GetSkillsForCertificateAsync(int certificateId)
        {
            if (!await _repository.CertificateExistsAsync(certificateId))
            {
                return ApiResponse<IEnumerable<SkillReadDto>>.ErrorResponse("Certificate not found", ApiErrorType.NotFound);
            }

            var skills = await _repository.GetSkillsForCertificateAsync(certificateId);
            var skillDtos = _mapper.Map<IEnumerable<SkillReadDto>>(skills);
            return ApiResponse<IEnumerable<SkillReadDto>>.SuccessResponse(skillDtos);
        }

        public async Task<ApiResponse<SkillReadDto>> GetSkillForCertificateAsync(int certificateId, int skillId)
        {
            if (!await _repository.CertificateExistsAsync(certificateId))
            {
                return ApiResponse<SkillReadDto>.ErrorResponse("Certificate not found", ApiErrorType.NotFound);
            }

            var skill = await _repository.GetSkillAsync(certificateId, skillId);
            if (skill == null)
            {
                return ApiResponse<SkillReadDto>.ErrorResponse("Skill not found", ApiErrorType.NotFound);
            }

            var skillDto = _mapper.Map<SkillReadDto>(skill);
            return ApiResponse<SkillReadDto>.SuccessResponse(skillDto);
        }

        public async Task<ApiResponse<SkillReadDto>> CreateSkillForCertificateAsync(int certificateId, SkillCreateDto skillDto)
        {
            if (!await _repository.CertificateExistsAsync(certificateId))
            {
                return ApiResponse<SkillReadDto>.ErrorResponse("Certificate not found", ApiErrorType.NotFound);
            }

            var skill = _mapper.Map<Skill>(skillDto);
            await _repository.CreateSkillAsync(certificateId, skill);
            await _repository.SaveChangesAsync();

            var skillReadDto = _mapper.Map<SkillReadDto>(skill);
            return ApiResponse<SkillReadDto>.SuccessResponse(skillReadDto, "Skill created successfully");
        }
    }
}
