using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using x_nova_template.ViewModel;
using x_nova_template.Extension;

namespace x_nova_template.Models
{
    [Serializable]
    public class Cart
    {
        private bool step1 { get; set; }
        private bool step2 { get; set; }
        private bool step3 { get; set; }
        private bool step4 { get; set; }

        public bool Step1
        {
            get { return step1; }
            set { step1 = value; }
        }
        public bool Step2
        {
            get { return step2; }
            set { step2 = value; }
        }
        public bool Step3
        {
            get { return step3; }
            set { step3 = value; }
        }
        public bool Step4
        {
            get { return step4; }
            set { step4 = value; }
        }

        public Cart()
        {
            Step1 = true;
        }

        private List<CartLine> line = new List<CartLine>();

        public void AddItem(Product product, int quantity = 1)
        {
            CartLine cline = line.Find(x => x.Product.ID == product.ID);
            //.Where(x => x.Product.ID == product.ID)
            //.FirstOrDefault();

            if (cline == null)
            {
                line.Add(new CartLine { Product = product, Quantity = quantity });
            }
            else
            {
                cline.Quantity += quantity;
            }
        }
        public void ChangeQuantity(Product prod, int quantity)
        {
            CartLine cline = line.Find(x => x.Product.ID == prod.ID);
            cline.Quantity = quantity;
        }
        public void RemoveLine(Product product)
        {
            line.RemoveAll(l => l.Product.ID == product.ID);
        }

        public float TotalValue()
        {
            return line.Sum(x => x.Product.Price * x.Quantity);
        }

        public void Clear()
        {
            line.Clear();

        }
        public CartLine GetLine(int id) {
            return line.Find(x => x.Product.ID == id);
        }
        public IEnumerable<CartLine> Lines
        {
            get { return line; }
        }

        public void UpdateClientDetails(CheckoutViewModel vm)
        {
            ClientDetails.Address = vm.Address;
            ClientDetails.FirstName = vm.FirstName;
            ClientDetails.LastName = vm.LastName;
            ClientDetails.Phone = vm.Phone;
            ClientDetails.Email = vm.Email;


        }
        public void UpdateClientDetails(ApplicationUser user) {
            ClientDetails.Address = user.Address;
            ClientDetails.FirstName = user.Firstname;
            ClientDetails.LastName = user.Sirname;
            ClientDetails.Phone = user.Phone;
            ClientDetails.Email = user.Email;
        }
        public void UpdateDelivery(Checkout_Delivery vm)
        {
            ClientDetails.Delivery = vm.Delivery;
        }
        
        public void UpdatePayment(Checkout_Payment vm)
        {
            ClientDetails.Payment = vm.Payment;
        }
        public void UpdateDelivery(ApplicationUser user)
        {
            ClientDetails.Delivery = user.Delivery;
        }

        public void UpdatePayment(ApplicationUser user)
        {
            ClientDetails.Payment = user.Payment;
        }
        private ShippingDetails shipDetails = new ShippingDetails();

        public ShippingDetails ClientDetails { get { return shipDetails; } }
        [Serializable]
        public class CartLine
        {
            public Product Product { get; set; }
            public int Quantity { get; set; }

        }
        [Serializable]
        public class ShippingDetails
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Phone { get; set; }
            public string Delivery { get; set; }
            public string Payment { get; set; }
            public string Address { get; set; }
            public string Email { get; set; }

            public bool HasEmptyProperties()
            {
                var type = GetType();
                var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
                var hasProperty = properties
                    .Where(x=>x.Name!="Delivery"&&x.Name!="Payment")
                    .Select(x => x.GetValue(this, null))
                    .Any(x => x.IsNullOrEmpty());
                return hasProperty;
            }
            
            public void AttachEmptyProperties()
            {
                var type = GetType();
                var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
                var propList = properties.Select(x => x.GetValue(this, null))
                                            .Where(x => x.IsNullOrEmpty());
                
            }
        }
    }
}