using HR_ManagementSystem.Models;

namespace HR_ManagementSystem.Repositories
{
    public interface ICommission
    {
        public void Add(CommissionSettings commission);

        public void Update(CommissionSettings commission);

        public CommissionSettings? Get();
    }
}
