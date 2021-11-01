﻿namespace Ticketeer.Domain.Enums
{
    public enum OrderStatus
    {
        // When the order has been created, but the
        // ticket it is trying to order has not been reserved
        Created,

        // The ticket the order is trying to reserve has already
        // been reserved, or when the user has cancelled the order.
        // The order expires before payment
        Cancelled,

        // The order has successfully reserved the ticket
        AwaitingPayment,

        // The order has reserved the ticket and the user has
        // provided payment successfully
        Complete
    }
}
