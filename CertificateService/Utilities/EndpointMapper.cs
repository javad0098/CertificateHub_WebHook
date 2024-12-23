// Utilities/EndpointMapper.cs
using System.IO;
using System.Threading.Tasks;
using CertificateService.Data;
using CertificateService.Features;
using CertificateService.Models;
using CertificateService.SyncDataServices.Grpc;
using Microsoft.AspNetCore.Builder;

namespace CertificateService.Utilities
{
    public static class EndpointMapper
    {
        public static void MapAllEndpoints(WebApplication app)
        {
            app.MapGrpcService<GrpcCertificateService>();
            app.MapCertificateEndpoints();
        }
    }
}
