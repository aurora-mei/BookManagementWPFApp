namespace BookManagementWPFApp.Dtos
{
    public class CreateOrderRequestDto
    {
        public string intent { get; set; }
        public Purchase_units[] purchase_units { get; set; }

        public class Purchase_units
        {
            public string reference_id { get; set; }
            public string description { get; set; }
            public Amount amount { get; set; }
            public Items[] items { get; set; }
            public Shipping shipping { get; set; }
        }

        public class Amount
        {
            public string currency_code { get; set; }
            public string value { get; set; }
            public Breakdown breakdown { get; set; }
        }

        public class Breakdown
        {
            public Item_total item_total { get; set; }
            public ShippingFee shipping { get; set; }
        }

        public class Item_total
        {
            public string currency_code { get; set; }
            public string value { get; set; }
        }
        public class ShippingFee
        {
            public string currency_code { get; set; }
            public string value { get; set; }
        }

        public class Items
        {
            public string name { get; set; }
            public string description { get; set; }
            public int quantity { get; set; }
            public Unit_amount unit_amount { get; set; }
            public string currency { get; set; }
        }

        public class Unit_amount
        {
            public string currency_code { get; set; }
            public string value { get; set; }
        }

        public class Shipping
        {
            public Name name { get; set; }
            public Address address { get; set; }
        }

        public class Name
        {
            public string full_name { get; set; }
        }

        public class Address
        {
            public string address_line_1 { get; set; }
            public string admin_area_2 { get; set; }
            public string country_code { get; set; }
        }
    }
}