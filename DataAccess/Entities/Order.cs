namespace DataAccess.Entities
{
    public class Order
    {
        public string Id { get; set; }
        public string Type { get; set; }
        //public OrderType Type { get; set; }
        public string CustomerName { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedByUsername { get; set; }
    }

    public enum OrderType
    {
        Standart,
        SaleOrder,
        PurchaseOrder,
        TransferOrder,
        ReturnOrder
    }
}
