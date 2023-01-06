using BusinessLogic.Inputs;
using BusinessLogic.Responses;

namespace BusinessLogic.Managers
{
    public interface IOrderManager
    {
        Task<DataQuery> SearchOrders();
        Task<SingleQuery> SearchById(string id);
        Task<DataQuery> SearchOrderByType(string type);
        Task<ResponseStatus> CreateOrder(OrderInput input);
        Task<ResponseStatus> UpdateOrder(string id, OrderInput input);
        Task<ResponseStatus> DeleteOrder(string id);
    }
}
