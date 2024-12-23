using System.Threading.Tasks;
using CertificateCommon.Api;
using CertificateService.Dtos;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CertificateService.Features
{
    public static class CertificateEndpoints
    {
        public static void MapCertificateEndpoints(this IEndpointRouteBuilder endpoints)
        {
            endpoints.MapGet("/certificates", async ([FromServices] CertificatesService service) =>
            {
                var response = await service.GetAllCertificatesAsync();
                return ApiResponseHelper.HandleApiResponse(response);
            });

            endpoints.MapGet("/certificates/{id}", async ([FromServices] CertificatesService service, int id) =>
            {
                var response = await service.GetCertificateByIdAsync(id);
                return ApiResponseHelper.HandleApiResponse(response);
            });

            endpoints.MapPost("/certificates", async ([FromServices] CertificatesService service, [FromBody] CertificateCreateDtos certificateCreate) =>
            {
                var response = await service.CreateCertificateAsync(certificateCreate);
                return ApiResponseHelper.HandleApiResponse(response);
            });
        }
    }
}
