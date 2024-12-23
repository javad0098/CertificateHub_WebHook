using System.Collections.Generic;
using System.Threading.Tasks;
using SkillService.Models;

namespace SkillService.Data
{
    public interface ISkillRepo
    {
        Task<bool> SaveChangesAsync();

        // Certificates
        Task<IEnumerable<Certificate>> GetAllCertificatesAsync();

        Task CreateCertificateAsync(Certificate cer);

        Task<bool> CertificateExistsAsync(int certificateId);

        Task<bool> ExternalCertificateExistsAsync(int externalCertificateId);

        // Skills
        Task<IEnumerable<Skill>> GetSkillsForCertificateAsync(int certificateId);

        Task<Skill> GetSkillAsync(int certificateId, int skillId);

        Task CreateSkillAsync(int certificateId, Skill skill);
    }
}
