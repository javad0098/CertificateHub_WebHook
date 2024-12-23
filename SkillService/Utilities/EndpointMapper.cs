// Utilities/EndpointMapper.cs

using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using SkillService.Features;

namespace SkillService.Utilities
{
    public static class EndpointMapper
    {
        public static void MapAllEndpoints(WebApplication app)
        {
            CertificateEndpoints.MapCertificateEndpoints(app);
            SkillEndpoints.MapSkillEndpoints(app);
        }
    }
}
