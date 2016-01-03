using System;
using System.Text;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using BattleInfoPlugin.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BattleInfoPluginTest
{
    [TestClass]
    public class ConverterTest
    {
        [TestMethod]
        public void CellTypeConverterTest()
        {
            var type = CellType.戦闘 | CellType.夜戦;
            var sigleTypes = type.Split().ToArray();

            Assert.IsTrue(sigleTypes.Length == 2);
            Assert.IsTrue(sigleTypes.Count(t => t == CellType.戦闘) == 1);
            Assert.IsTrue(sigleTypes.Count(t => t == CellType.夜戦) == 1);
        }
    }
}
