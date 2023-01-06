using DataAccess.Entities;

namespace BusinessLogic.Responses
{
    public class DataQuery
    {
        public List<Order>? data { get; set; }
        public string ResponseStatus { get; set; }
        public string ResponseDescription { get; set; }
        public int total { get; set; }

        public DataQuery()
        {
            data = new List<Order>();
            ResponseDescription = Constants.StatusOk;
        }
    }
}
