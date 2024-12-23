using System.Threading.Tasks;
using AutoMapper;
using CertificateService.Data;
using Grpc.Core;

namespace CertificateService.SyncDataServices.Grpc
{
    public class GrpcCertificateService : GrpcCertificate.GrpcCertificateBase
    {
        private readonly ICertificateRepo _repository;
        private readonly IMapper _mapper;

        public GrpcCertificateService(ICertificateRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public override async Task<CertificateResponse> GetAllCertificates(GetAllRequest request, ServerCallContext context)
        {
            var response = new CertificateResponse();
            var certificates = await _repository.GetAllCertificatesAsync();

            foreach (var cer in certificates)
            {
                response.Certificate.Add(_mapper.Map<GrpcCertificateModel>(cer));
            }

            return response;
        }
    }
}
