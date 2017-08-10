using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AutomacaoWeb.PageObjects;

namespace AutomacaoWeb
{
    [TestClass]
    public class Teste : TestBase
    {
        private TestePage testePage;

        [TestInitialize]
        public override void TestInitialize()
        {
            base.TestInitialize();
            testePage = new TestePage(driver);
        }

        [TestMethod]
        public void TestMethod()
        {
            testePage.TesteParametro("Meu parametro");
            testePage.TesteSemParametro();
        }
    }
}
