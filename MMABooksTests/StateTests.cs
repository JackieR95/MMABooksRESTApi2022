using System.Collections.Generic;
using System.Linq;
using System;

using NUnit.Framework;
using MMABooksEFClasses.Models;
using Microsoft.EntityFrameworkCore;

namespace MMABooksTests
{
    [TestFixture]
    public class StateTests
    {
        // ignore this warning about making dbContext nullable.
        // if you add the ?, you'll get a warning wherever you use dbContext
        MMABooksContext dbContext;
        State? s;
        List<State>? states;

        [SetUp]
        public void Setup()
        {
            dbContext = new MMABooksContext();
            dbContext.Database.ExecuteSqlRaw("call usp_testingResetData()");
        }

        // Verify that retrieving all states returns the correct total count,
        // and that the first state matches the expected record.
        [Test]
        public void GetAllTest()
        {
            states = dbContext.States.OrderBy(s => s.StateName).ToList();
            Assert.AreEqual(52, states.Count);
            Assert.AreEqual("Alabama", states[0].StateName);
            PrintAll(states);
        }

        // Test that verify's a State can be retrieved by its primary key StateName("OR")
        [Test]
        public void GetByPrimaryKeyTest()
        {
            s = dbContext.States.Find("OR");
            Assert.IsNotNull(s);
            Assert.AreEqual("Ore", s.StateName);
            Console.WriteLine(s);
        }

        // Test to verify that we can retrieve multiple customers or a single specific state
        [Test]
        public void GetUsingWhere()
        {
            states = dbContext.States.Where(s => s.StateName.StartsWith("A")).OrderBy(s => s.StateName).ToList();
            Assert.AreEqual(4, states.Count);
            Assert.AreEqual("Alabama", states[0].StateName);
            PrintAll(states);
        }

        // Test that get's the default or a single state
        [Test]
        public void GetWithCustomersTest()
        {
            s = dbContext.States.Include("Customers").Where(s => s.StateCode == "OR").SingleOrDefault();
            Assert.IsNotNull(s);
            Assert.AreEqual("Ore", s.StateName);
            Assert.AreEqual(5, s.Customers.Count);
            Console.WriteLine(s);
        }

        // Test that Verifies that a State has been deleted
        [Test]
        public void DeleteTest()
        {
            s = dbContext.States.Find("HI");
            dbContext.States.Remove(s);
            dbContext.SaveChanges();
            Assert.IsNull(dbContext.States.Find("HI"));
        }

        // Test that verifies that Hawaii with stateCode (HI) has been created. 
        [Test]
        public void CreateTest()
        {
            s = new State();
            s.StateCode = "HI";
            s.StateName = "Hawaii";
            dbContext.States.Add(s);
            dbContext.SaveChanges();


            Assert.IsNotNull(dbContext.States.Find("HI"));
            Console.WriteLine(s);
        }
           
        // Test that verifies that a state has been updated
        [Test]
        public void UpdateTest()
        {
            s = dbContext.States.Find("OR");
            s.StateName = "Ore";
            dbContext.States.Update(s);
            dbContext.SaveChanges();

            s = dbContext.States.Find("OR");
            Assert.AreEqual("Ore", s.StateName);
            Console.WriteLine(s);
        }

        public void PrintAll(List<State> states)
        {
            foreach (State s in states)
            {
                Console.WriteLine(s);
            }
        }
    }
}