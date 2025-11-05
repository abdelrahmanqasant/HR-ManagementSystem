using HR_ManagementSystem.Models;
using HR_ManagementSystem.Repositories;

namespace HR_ManagementSystem.Implementations
{
    public class DeductionRepo : IDeduction
    {
        private HRDbContext _context;
        public DeductionRepo(HRDbContext context)
        {
            _context = context;
        }
        public void Add(DeductionSettings deduction)
        {
           _context.DeductionSettings.Add(deduction);
        }

        public DeductionSettings? Get()
        {
         return _context.DeductionSettings.FirstOrDefault();
        }

        public void Update(DeductionSettings deduction)
        {
            _context.DeductionSettings.Update(deduction);
        }
    }
}
