using System.Collections.Generic;
using System.Threading.Tasks;
using CertificateService.Models;

namespace CertificateService.Data
{
    public interface ICertificateRepo
    {
        Task<bool> SaveChangesAsync();

        Task<IEnumerable<Certificate>> GetAllCertificatesAsync();

        Task<Certificate> GetCertificateByIdAsync(int id);

        void CreateCertificate(Certificate certificate);
    }
}
