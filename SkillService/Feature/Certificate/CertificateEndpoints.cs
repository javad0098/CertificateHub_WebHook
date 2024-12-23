using System.Threading.Tasks;
using CertificateCommon.Api;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SkillService.Dtos;

namespace SkillService.Features
{
    public static class CertificateEndpoints
    {
        public static void MapCertificateEndpoints(this IEndpointRouteBuilder endpoints)
        {
            endpoints.MapGet("/api/s/certificates", async ([FromServices] CertificateService service) =>
            {
                var response = await service.GetCertificatesAsync();
                return ApiResponseHelper.HandleApiResponse(response);
            });

            endpoints.MapPost("/api/s/certificates/test", async ([FromServices] CertificateService service) =>
            {
                var response = await service.TestInboundConnectionAsync();
                return ApiResponseHelper.HandleApiResponse(response);
            });
        }
    }
}
