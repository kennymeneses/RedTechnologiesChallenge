using DataAccess.Entities;

namespace BusinessLogic.Responses
{
    public class SingleQuery
    {
        public string ResponseStatus { get; set; }
        public string DescriptionResponse { get; set; }
        public Order? Order { get; set; }
    }
}
