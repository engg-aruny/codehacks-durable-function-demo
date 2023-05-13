using System.Collections.Generic;
using System;

namespace codehacks_durable_function_demo
{
    public class Order
    {
        public string OrderId { get; set; }
        public string CustomerName { get; set; }
        public List<OrderItem> Items { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal TaxRate { get; set; }
        public decimal Weight { get; set; }
        public decimal ShippingRate { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime? DateProcessed { get; set; }

        public Order(string orderId, string customerName, List<OrderItem> items, decimal totalPrice, decimal taxRate, DateTime dateCreated)
        {
            OrderId = orderId;
            CustomerName = customerName;
            Items = items;
            TotalPrice = totalPrice;
            TaxRate = taxRate;
            DateCreated = dateCreated;
        }

        public void SetDateProcessed(DateTime dateProcessed)
        {
            DateProcessed = dateProcessed;
        }

        public bool IsValid()
        {
            if (string.IsNullOrWhiteSpace(OrderId) ||
                string.IsNullOrWhiteSpace(CustomerName) ||
                Items == null ||
                Items.Count == 0 ||
                TotalPrice <= 0 ||
                DateCreated == default)
            {
                return false;
            }
            return true;
        }
    }

    public class OrderItem
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }

        public OrderItem(string name, decimal price, int quantity)
        {
            Name = name;
            Price = price;
            Quantity = quantity;
        }
    }

}