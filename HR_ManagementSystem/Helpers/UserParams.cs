namespace HR_ManagementSystem.Helpers
{
    public class UserParams
    {
        private const int MaxPageSize = 50;
        public int _PageSize = 8;
        public int PageNumber = 1;
        public int PageSize
        {
            get { return _PageSize; }
            set { _PageSize = value > MaxPageSize ? MaxPageSize : value; }
        }

        public DateOnly StartDate { get; set; } = new DateOnly(2008, 1, 1);
        public DateOnly EndDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);
        public string stringQuery { get; set; } = "";
    }

}
