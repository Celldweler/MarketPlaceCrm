using System;

namespace MarketPlaceCrm.Data.Entities
{
    public class Shipper
    {
        public int ID { get; set; }
        public string CompanyName { get; set; }
        public string Phone { get; set; }
    }

    public class Shipment : BaseEntity<int>
    {
        /// <summary>
        /// Gets or sets the order identifier
        /// </summary>
        public int OrderId { get; set; }
        public Order Order { get; set; }
        
        /// <summary>
        /// Gets or sets the tracking number of this shipment
        /// </summary>
        public string TrackingNumber { get; set; }

        /// <summary>
        /// Gets or sets the total weight of this shipment
        /// It's nullable for compatibility with the previous version of nopCommerce where was no such property
        /// </summary>
        public decimal? TotalWeight { get; set; }

        /// <summary>
        /// Gets or sets the shipped date and time
        /// </summary>
        public DateTime? ShippedDateUtc { get; set; }

        /// <summary>
        /// Gets or sets the delivery date and time
        /// </summary>
        public DateTime? DeliveryDateUtc { get; set; }

        /// <summary>
        /// Gets or sets the ready for pickup date and time
        /// </summary>
        public DateTime? ReadyForPickupDateUtc { get; set; }

        /// <summary>
        /// Gets or sets the admin comment
        /// </summary>
        public string AdminComment { get; set; }
    }

    public class ShippingMethod
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
    }
}