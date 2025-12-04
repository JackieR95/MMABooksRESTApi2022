using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using MMABooksEFClasses.Models;
using NUnit.Framework;

namespace MMABooksTests
{
    [TestFixture]
    public class ProductTests
    {
        
        MMABooksContext dbContext;
        Product? p;
        List<Product>? products;
        string testProduct;

        [SetUp]
        public void Setup()
        {
            dbContext = new MMABooksContext();
            dbContext.Database.ExecuteSqlRaw("call usp_testingResetData()");
        }

        // Test Product that is used for being updated and deleted
        [SetUp]
        public void TestProduct()
        {
            p = new Product();
            p.ProductCode = "LD5T";
            p.Description = "Murach's C# 2015";
            p.UnitPrice = 24.50000m;
            p.OnHandQuantity = 233;
            dbContext.Products.Add(p);
            dbContext.SaveChanges();

            testProduct = p.ProductCode;
        }

        // Verify that retrieving all Product returns the correct total count,
        // and that the first Product matches the expected record.
        [Test]
        public void GetAllTest()
        {
            products = dbContext.Products.OrderBy(p => p.ProductCode).ToList();
            Assert.AreEqual(16, products.Count);
            Assert.AreEqual("A4CS", products[0].ProductCode);
            PrintAll(products);
        }

        // Test that verifys a product can be retrieved by its primary key ProductCode("JAVP")
        [Test]
        public void GetByPrimaryKeyTest()
        {
            p = dbContext.Products.Find("JAVP");
            Assert.IsNotNull(p);
            Assert.AreEqual("Murach's Java Programming", p.Description);
            Console.WriteLine(p);
        }

        // Test to verify that we can retrieve multiple customers or a single specific product.
        [Test]
        public void GetUsingWhere()
        {
            // get a list of all of the products that have a unit price of 56.50

            products = dbContext.Products.Where(p => p.UnitPrice == 56.5000m).OrderBy(p => p.ProductCode).ToList();
            Assert.AreEqual(7, products.Count);
            Assert.AreEqual("Murach's Java Programming", products[5].Description);
            PrintAll(products);
        }

        [Test]
        public void GetWithCalculatedFieldTest()
        {
            // get a list of objects that include the productcode, unitprice, quantity and inventoryvalue
            var products = dbContext.Products.Select(
            p => new { p.ProductCode, p.UnitPrice, p.OnHandQuantity, Value = p.UnitPrice * p.OnHandQuantity }).
            OrderBy(p => p.ProductCode).ToList();
            Assert.AreEqual(16, products.Count);
            foreach (var p in products)
            {
                Console.WriteLine(p);
            }
        }

        // Test that Verifies that a Product has been deleted
        [Test]
        public void DeleteTest()
        {
            p = dbContext.Products.Find(testProduct);
            dbContext.Products.Remove(p);
            dbContext.SaveChanges();
            Assert.IsNull(dbContext.Products.Find(testProduct));
        }

        // Test that verifies that Product with code DF6E has been created. 
        [Test]
        public void CreateTest()
        {
            p = new Product();
            p.ProductCode = "DF6E";
            p.Description = "Murach's C# 2022";
            p.UnitPrice = 34.50000m;
            p.OnHandQuantity = 500;
            dbContext.Products.Add(p);
            dbContext.SaveChanges();


            Assert.IsNotNull(dbContext.Products.Find("DF6E"));
            Console.WriteLine(p);
        }

        // Test that verifies that testProduct with code LD5T has been updated
        [Test]
        public void UpdateTest()
        {
            p = dbContext.Products.Find(testProduct);
            p.Description = "Murach's C# 2016";
            p.OnHandQuantity = 178;
            dbContext.Products.Update(p);
            dbContext.SaveChanges();

            p = dbContext.Products.Find(testProduct);
            Assert.AreEqual("Murach's C# 2016", p.Description);
            Assert.AreEqual(178, p.OnHandQuantity);
            Console.WriteLine(p);
        }

        public void PrintAll(List<Product> products)
        {
            foreach (Product p in products)
            {
                Console.WriteLine(p);
            }
        }

    }
}