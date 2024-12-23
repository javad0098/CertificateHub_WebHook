using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CertificateService.Models;
using Microsoft.EntityFrameworkCore;

namespace CertificateService.Data
{
    public class CertificateRepo : ICertificateRepo
    {
        private readonly AppDBContext _context;

        public CertificateRepo(AppDBContext context)
        {
            _context = context;
        }

        public void CreateCertificate(Certificate certificate)
        {
            if (certificate == null)
            {
                throw new ArgumentNullException(nameof(certificate));
            }

            _context.Certificates.Add(certificate);
        }

        public async Task<IEnumerable<Certificate>> GetAllCertificatesAsync()
        {
            return await _context.Certificates.ToListAsync();
        }

        public async Task<Certificate> GetCertificateByIdAsync(int id)
        {
            return await _context.Certificates.FirstOrDefaultAsync(certificate => certificate.Id == id) ?? new Certificate(); // Use a default or fallback
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
