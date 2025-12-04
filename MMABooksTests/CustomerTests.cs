using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.TestPlatform.AdapterUtilities;
using MMABooksEFClasses.Models;
using NUnit.Framework;

namespace MMABooksTests
{
    [TestFixture]
    public class CustomerTests
    {
        
        MMABooksContext dbContext;
        Customer? c;
        List<Customer>? customers;
        int testCustomerId;
        

        [SetUp]
        public void Setup()
        {
            dbContext = new MMABooksContext();
            dbContext.Database.ExecuteSqlRaw("call usp_testingResetData()");
        }

        // Test customer that is used for being updated and deleted
        [SetUp]
        public void TestCustomer()
        {
            c = new Customer();
            c.Name = "Donald Duck";
            c.Address = "101 Main st";
            c.City = "Orlando";
            c.State = "FL";
            c.ZipCode = "10001";
            dbContext.Customers.Add(c);
            dbContext.SaveChanges();

            testCustomerId = c.CustomerId;
        }

        // Verify that retrieving all customers returns the correct total count,
        // and that the first customer matches the expected record.
        [Test]
        public void GetAllTest()
        {
            customers = dbContext.Customers.OrderBy(c => c.Name).ToList();
            Assert.AreEqual(696, customers.Count);
            Assert.AreEqual("Abeyatunge, Derek", customers[0].Name);
            PrintAll(customers);
        }

        // Test that verify's a customer can be retrieved by its primary key CustomerID("102")
        [Test]
        public void GetByPrimaryKeyTest()
        {
            c = dbContext.Customers.Find(102);
            Assert.IsNotNull(c);
            Assert.AreEqual("Hernandez, Esta", c.Name);
            Console.WriteLine(c);
        }

        // Test to verify that we can retrieve multiple customers or a single specific customer.
        [Test]
        public void GetUsingWhere()
        {
            // get a list of all of the customers who live in OR
            customers = dbContext.Customers.Where(c => c.State.StartsWith("OR")).OrderBy(c => c.Name).ToList();
            Assert.AreEqual(5, customers.Count);
            Assert.AreEqual("Erpenbach, Lee", customers[0].Name);
            PrintAll(customers);
        }

        // Test to verify that we are getting the customer with ID 20's invoices
        [Test]
        public void GetWithInvoicesTest()
        {
            // get the customer whose id is 20 and all of the invoices for that customer

            c = dbContext.Customers.Include(c => c.Invoices).SingleOrDefault(c => c.CustomerId == 20);
            Assert.IsNotNull(c);
            Assert.AreEqual("1942 S. Gaydon Avenue", c.Address);
            Assert.AreEqual(20, c.CustomerId);
            Assert.AreEqual(3, c.Invoices.Count);
            Console.WriteLine(c);
        }

        // Test retrieving customers with their state name by joining Customers.State to States.StateCode
        [Test]
        public void GetWithJoinTest()
        {
            // get a list of objects that include the customer id, name, statecode and statename
            var customers = dbContext.Customers.Join(
               dbContext.States,
               c => c.State,
               s => s.StateCode,
               (c, s) => new { c.CustomerId, c.Name, c.State, s.StateName }).OrderBy(r => r.StateName).ToList();
            Assert.AreEqual(696, customers.Count);
            // I wouldn't normally print here but this lets you see what each object looks like
            foreach (var c in customers)
            {
                Console.WriteLine(c);
            }
        }

        // Test that Verifies that a Customer has been deleted
        [Test]
        public void DeleteTest()
        {
            c = dbContext.Customers.Find(testCustomerId);
            dbContext.Customers.Remove(c);
            dbContext.SaveChanges();
            Assert.IsNull(dbContext.Customers.Find(testCustomerId));
        }

        // Test that verifies the customer Minnie mouse has been created. 
        [Test]
        public void CreateTest()
        {
            c = new Customer();
            c.Name = "Minnie Mouse";
            c.Address = "101 Main st";
            c.City = "Orlando";
            c.State = "FL";
            c.ZipCode = "10001";
            dbContext.Customers.Add(c);
            dbContext.SaveChanges();


            Assert.IsNotNull(dbContext.Customers.Find(c.CustomerId));
            Console.WriteLine(c);
        }

        // Test that verifies that a Customer has been updated
        [Test]
        public void UpdateTest()
        {
            c = dbContext.Customers.Find(testCustomerId);
            c.Address = "101 Main Street";
            c.ZipCode = "10005";
            dbContext.Customers.Update(c);
            dbContext.SaveChanges();

            c = dbContext.Customers.Find(testCustomerId);
            Assert.AreEqual("101 Main Street", c.Address);
            Assert.AreEqual("10005", c.ZipCode);
            Console.WriteLine(c);
        }

        public void PrintAll(List<Customer> customers)
        {
            foreach (Customer c in customers)
            {
                Console.WriteLine(c);
            }
        }
        
    }
}