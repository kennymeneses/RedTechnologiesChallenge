namespace BusinessLogic
{
    public static class Constants
    {
        public const string StatusOk = "Ok";
        public const string SucesfullResponseDescription = "La solicitud fue ejecutada con éxito.";
        public const string StatusError = "Error";

        public const string OrderQuerySuccessResult = "The search found order data successfully.";
        public const string OrderQueryEmptyResult = "No order found with that type.";
        public const string OrderTypeIsInvalid = "The orde type is invalid.";

        public const string OrderCreated = "Order was created Successfully.";
        public const string OrderErrorCreation = "Order wasnt created due internal server error.";
        public const string OrderErrorType = "Order wasnt created due invalid OrderType";

        public const string OrderUpdated = "Order was updated Successfully.";
        public const string OrderUpdatedError = "Order wasnt updated due internal server error.";
        public const string OrderUpdatedErrorId = "Order doesnt exists.";

        public const string OrderRemoved = "Order was removed Successfully.";
        public const string OrderRemovedError = "Order wasnt removed due internal server error.";

        public const string OrderFounded = "Order was found.";

    }
}
