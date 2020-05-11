using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using SplitList.Models;

namespace SplitList.Test
{
     [TestFixture]
    public class ModelItemTests
    {
        private Item _uut;
        [SetUp]
        public void BeforeEachTest()
        {
            _uut = new Item("Banana",1,"Fruit");
        }

        [TestCase(1,2)]
        [TestCase(10,11)]
        [TestCase(100, 99)]
        public void Item_IncAmountExecute(int a, int expected)
        {
            for (int i = 0; i < a; i++)
            {
                _uut.IncItemAmountExecute();
            }
            Assert.That(_uut.Amount, Is.EqualTo(expected));
        }

        [TestCase(15, 1)]
        [TestCase(1, 9)]
        [TestCase(5, 5)]
        public void Item_DecAmountExecute(int a, int expected)
        {
            _uut.Amount = 10;
            for (int i = 0; i < a; i++)
            {
                _uut.DecItemAmountCommandExecute();
            }
            Assert.That(_uut.Amount, Is.EqualTo(expected));
        }
    }
}
