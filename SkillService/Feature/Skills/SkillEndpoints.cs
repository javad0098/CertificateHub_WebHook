using System.Threading.Tasks;
using CertificateCommon.Api;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SkillService.Dtos;

namespace SkillService.Features
{
    public static class SkillEndpoints
    {
        public static void MapSkillEndpoints(this IEndpointRouteBuilder endpoints)
        {
            endpoints.MapGet("/api/s/certificates/{certificateId}/skills", async ([FromServices] Skill_Service service, int certificateId) =>
            {
                var response = await service.GetSkillsForCertificateAsync(certificateId);
                return ApiResponseHelper.HandleApiResponse(response);
            });

            endpoints.MapGet("/api/s/certificates/{certificateId}/skills/{skillId}", async ([FromServices] Skill_Service service, int certificateId, int skillId) =>
            {
                var response = await service.GetSkillForCertificateAsync(certificateId, skillId);
                return ApiResponseHelper.HandleApiResponse(response);
            }).WithName("GetSkillForCertificate");

            endpoints.MapPost("/api/s/certificates/{certificateId}/skills", async ([FromServices] Skill_Service service, int certificateId, [FromBody] SkillCreateDto skillDto) =>
            {
                var response = await service.CreateSkillForCertificateAsync(certificateId, skillDto);
                if (response.Success && response.Data != null)
                {
                    return Results.CreatedAtRoute("GetSkillForCertificate", new { certificateId, skillId = response.Data.Id }, response);
                }

                return ApiResponseHelper.HandleApiResponse(response);
            });
        }
    }
}
