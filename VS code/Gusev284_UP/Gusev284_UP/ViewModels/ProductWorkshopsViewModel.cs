namespace Gusev284_UP.ViewModels
{
    public class ProductWorkshopsViewModel
    {
        public string ProductName { get; set; }
        public List<WorkshopDetailViewModel> Workshops { get; set; }
    }

    public class WorkshopDetailViewModel
    {
        public string WorkshopName { get; set; }
        public int EmployeeCount { get; set; }
        public double TimeHours { get; set; }
    }
}