using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AutomacaoWeb.PageObjects
{
    public abstract class PageBase
    {
        public const string Host = "COLE SUA URL AQUI";
        private static TimeSpan DefaultTimeout = TimeSpan.FromSeconds(60);

        protected IWebDriver _driver;

        protected PageBase(IWebDriver driver)
        {
            _driver = driver;
        }


        public abstract void GoTo();

        protected virtual IWebElement SetInputById(string id, string value)
        {
            var element = _driver.FindElement(By.Id(id));
            element.SendKeys(value);
            return element;
        }

        protected void GoToPath(string path)
        {
            _driver.Navigate().GoToUrl($"{Host}{path}");
        }

        protected void GoToUrl(string url)
        {
            _driver.Navigate().GoToUrl(url);
        }

        public void WaitForElementByXPath(string xpath)
        {
            new WebDriverWait(_driver, DefaultTimeout).Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.XPath(xpath)));
        }

        public void WaitForElementByText(string text)
        {
            new WebDriverWait(_driver, DefaultTimeout).Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.XPath(".//span[contains(., '" + text + "')]")));
        }

        public void WaitForElementById(string id)
        {
            new WebDriverWait(_driver, DefaultTimeout).Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.Id(id)));
        }
    }
}
