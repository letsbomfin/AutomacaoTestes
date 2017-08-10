using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomacaoWeb.PageObjects
{
    class TestePage : PageBase
    {
        public TestePage(IWebDriver driver)
            : base(driver)
        {
        }

        public void TesteParametro(string parametro)
        {
            //TODO
        }

        public void TesteSemParametro()
        {
            //TODO
        }

        public override void GoTo()
        {
            GoToPath("ALGUM LUGAR");
        }
    }
}
