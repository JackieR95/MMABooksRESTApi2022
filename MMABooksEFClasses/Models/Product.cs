using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

namespace MMABooksEFClasses.Models
{
    public partial class Product
    {
        public Product()
        {
            Invoicelineitems = new HashSet<Invoicelineitem>();
        }

        public string ProductCode { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal UnitPrice { get; set; }
        public int OnHandQuantity { get; set; }

        public virtual ICollection<Invoicelineitem> Invoicelineitems { get; set; }

        public override string ToString()
        {
            return $"{ProductCode},  {Description}, {UnitPrice}, {OnHandQuantity}";
        }
    }
}
