using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OmniApp.PageObjects;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;

namespace OmniApp
{
    [TestClass]
    public class Authentication : TestBase
    {
        private PageBase pageBase;
      
        [TestInitialize]
        public void Init()
        {
           pageBase = new PageBase(driver);
           
        }

        [TestMethod]
        public void MetodoTeste()
        {
            pageBase.BtnClick(""); 
        }

    }
}