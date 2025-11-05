using HR_ManagementSystem.Models;

namespace HR_ManagementSystem.Repositories
{
    public interface IDeduction
    {
        public void Add(DeductionSettings deduction);

        public void Update(DeductionSettings deduction);

        public DeductionSettings? Get();
    }
}
