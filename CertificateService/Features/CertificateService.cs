using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using CertificateCommon.Api;
using CertificateService.AsyncDataServices;
using CertificateService.Data;
using CertificateService.Dtos;
using CertificateService.Models;
using CertificateService.SyncDataServices.Http;

namespace CertificateService.Features
{
    public class CertificatesService
    {
        private readonly ICertificateRepo _repository;
        private readonly IMapper _mapper;
        private readonly ISkillDataClient _skillDataClient;
        private readonly IMessageBusClient _messageBusClient;

        public CertificatesService(ICertificateRepo repository, IMapper mapper, ISkillDataClient skillDataClient, IMessageBusClient messageBusClient)
        {
            _repository = repository;
            _mapper = mapper;
            _skillDataClient = skillDataClient;
            _messageBusClient = messageBusClient;
        }

        public async Task<ApiResponse<IEnumerable<CertificateReadDto>>> GetAllCertificatesAsync()
        {
            var certificates = await _repository.GetAllCertificatesAsync();
            var certificateDtos = _mapper.Map<IEnumerable<CertificateReadDto>>(certificates);
            return ApiResponse<IEnumerable<CertificateReadDto>>.SuccessResponse(certificateDtos);
        }

        public async Task<ApiResponse<CertificateReadDto>> GetCertificateByIdAsync(int id)
        {
            var certificate = await _repository.GetCertificateByIdAsync(id);
            if (certificate == null)
            {
                return ApiResponse<CertificateReadDto>.ErrorResponse("Certificate not found", ApiErrorType.NotFound);
            }

            var certificateDto = _mapper.Map<CertificateReadDto>(certificate);
            return ApiResponse<CertificateReadDto>.SuccessResponse(certificateDto);
        }

        public async Task<ApiResponse<CertificateReadDto>> CreateCertificateAsync(CertificateCreateDtos certificateCreate)
        {
            var certificate = _mapper.Map<Certificate>(certificateCreate);
            _repository.CreateCertificate(certificate);
            var success = await _repository.SaveChangesAsync();
            if (!success)
            {
                return ApiResponse<CertificateReadDto>.ErrorResponse("Could not save certificate", ApiErrorType.InternalServerError);
            }

            var certificateReadDto = _mapper.Map<CertificateReadDto>(certificate);

            // Send synchronous message
            try
            {
                await _skillDataClient.SendCertificateToSkill(certificateReadDto);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"==> Could not send synchronously: {ex.Message}");
            }

            // Send asynchronous message
            try
            {
                var certificatePublishedDto = _mapper.Map<CertificatePublishedDto>(certificateReadDto);
                certificatePublishedDto.Event = "Certificate_Published";
                _messageBusClient.PublishNewCertificate(certificatePublishedDto);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"==> Could not send asynchronously: {ex.Message}");
            }

            return ApiResponse<CertificateReadDto>.SuccessResponse(certificateReadDto, "Certificate created successfully");
        }
    }
}
