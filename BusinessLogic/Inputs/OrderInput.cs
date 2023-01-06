namespace BusinessLogic.Inputs
{
    public class OrderInput
    {
        public string Type { get; set; }
        public string CustomerName { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedByUsername { get; set; }
    }
}
