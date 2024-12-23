using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SkillService.Models;

namespace SkillService.Data
{
    public class SkillRepo : ISkillRepo
    {
        private readonly AppDbContext _context;

        public SkillRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CertificateExistsAsync(int certificateId)
        {
            return await _context.Certificates.AnyAsync(p => p.Id == certificateId);
        }

        public async Task CreateCertificateAsync(Certificate cer)
        {
            if (cer == null)
            {
                throw new ArgumentNullException(nameof(cer));
            }

            await _context.Certificates.AddAsync(cer);
        }

        public async Task CreateSkillAsync(int certificateId, Skill skill)
        {
            if (skill == null)
            {
                throw new ArgumentNullException(nameof(skill));
            }

            skill.CertificateId = certificateId;
            await _context.Skills.AddAsync(skill);
        }

        public async Task<bool> ExternalCertificateExistsAsync(int externalCertificateId)
        {
            return await _context.Certificates.AnyAsync(p => p.ExternalID == externalCertificateId);
        }

        public async Task<IEnumerable<Certificate>> GetAllCertificatesAsync()
        {
            return await _context.Certificates.ToListAsync();
        }

        public async Task<Skill> GetSkillAsync(int certificateId, int skillId)
        {
            return await _context.Skills
                .Where(s => s.CertificateId == certificateId && s.Id == skillId)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Skill>> GetSkillsForCertificateAsync(int certificateId)
        {
            return await _context.Skills
                .Where(s => s.CertificateId == certificateId)
                .OrderBy(s => s.Name)
                .ToListAsync();
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() >= 0;
        }
    }
}
