using HR_ManagementSystem.Models;
using HR_ManagementSystem.Repositories;

namespace HR_ManagementSystem.Implementations
{
    public class CommissionRepo : ICommission
    {
        private  HRDbContext _context;

        public CommissionRepo(HRDbContext context)
        {
            _context = context;
        }

        
        public void Add(CommissionSettings commission)
        {
            _context.CommissionSettings.Add(commission);
        }

        public CommissionSettings? Get()
        {
            return _context.CommissionSettings.FirstOrDefault();
        }

        public void Update(CommissionSettings commission)
        {
            _context?.CommissionSettings.Update(commission);
        }
    }
}
