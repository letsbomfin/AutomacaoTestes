using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using System.Globalization;
using OpenQA.Selenium.PhantomJS;
using System.Drawing;
using OpenQA.Selenium.Chrome;
using System.Threading;
using System.IO;
using OpenQA.Selenium.Support.UI;

namespace AutomacaoWeb
{
    [TestClass]
    public abstract class TestBase
    {
        public static TimeSpan DefaultTimeout = TimeSpan.FromSeconds(15);

        protected static IWebDriver driver;
        protected static CultureInfo culturoInfo;

        private static readonly bool defaultUseHeadlessBrowser;
        private static readonly string defaltUseLanguage;
        private static string language;
        private static bool shouldUseHeadlessBrowser;

        public TestContext TestContext { get; set; }
        protected string RootPath = "/home/console";

        static TestBase()
        {
#if DEBUG
            defaultUseHeadlessBrowser = false;
#else
            defaultUseHeadlessBrowser = false;
#endif
            //defaltUseLanguage = "en-US";
            defaltUseLanguage = "pt-BR";
            //defaltUseLanguage = "es";
        }

        [AssemblyInitialize]
        public static void AssemblyInitilize(TestContext context)
        {

            language = GetLanguage(context);
            culturoInfo = CultureInfo.CreateSpecificCulture(language);
            shouldUseHeadlessBrowser = ShouldUseHeadlessBrowser(context);

            LogLine($">> Execute using language {language}", context);
            LogLine($">> Execute using headless browser: {shouldUseHeadlessBrowser}", context);

            if (shouldUseHeadlessBrowser)
            {
                var driverService = PhantomJSDriverService.CreateDefaultService();
                driverService.HideCommandPromptWindow = true;
                var options = new PhantomJSOptions();
                options.AddAdditionalCapability("phantomjs.page.customHeaders.Accept-Language", language);
                driver = new PhantomJSDriver(driverService, options);
                driver.Manage().Window.Size = new Size(1280, 760);
            }
            else
            {

                //driver = new ChromeDriver(new ChromeOptions { Proxy = null });

                var options = new ChromeOptions();
                options.AddArguments("--disable-infobars");
                options.AddUserProfilePreference("credentials_enable_service", false);
                options.AddUserProfilePreference("profile.password_manager_enabled", false);
                driver = new ChromeDriver(options);
                driver.Manage().Window.Maximize();
            }

            driver.Manage().Timeouts().ImplicitlyWait(DefaultTimeout);

            //var decoratedDrive = new DriverDecorator(drive);
            //decoratedDrive.Navigated += (sender, e) => { DisableCssAnimations(); };
            //drive = decoratedDrive;
            Thread.CurrentThread.CurrentUICulture = culturoInfo;
        }
        [Ignore]
        [AssemblyCleanup]
        public static void AssemblyCleanup()
        {
            driver.Quit();
        }

        [TestInitialize]
        public virtual void TestInitialize()
        {
            if (!shouldUseHeadlessBrowser)
            {
                // Since Chrome Driver does not have an initialization settings for the 
                // prefered language, let´s set it before every request
                driver.Navigate().GoToUrl("COLE SUA URL AQUI");
            }
        }

        [TestCleanup]
        public virtual void TestCleanup()
        {
            try
            {

                driver.Manage().Cookies.DeleteAllCookies();

                if (TestContext.CurrentTestOutcome == UnitTestOutcome.Error ||
                    TestContext.CurrentTestOutcome == UnitTestOutcome.Failed)
                {
                    var ss = ((ITakesScreenshot)driver).GetScreenshot();
                    var file = new FileInfo($"{TestContext.TestName}.jpg");
                    ss.SaveAsFile(file.FullName, System.Drawing.Imaging.ImageFormat.Jpeg);
                    LogLine($"Screenshot saved to {file.FullName}");
                }
            }
            catch (Exception e)
            {
                LogLine($"{nameof(TestCleanup)} failed: {e.Message}");
            }
        }

        public DefaultWait<IWebDriver> ExplicityWait()
        {
            return new WebDriverWait(driver, DefaultTimeout);
        }

        public void WaitForErrorMessageFor(string id)
        {
            new WebDriverWait(driver, DefaultTimeout).Until(_ => !string.IsNullOrWhiteSpace(FindErrorMessage(id).Text));
        }

        public void WaitForElement(By by)
        {
            new WebDriverWait(driver, DefaultTimeout).Until(d => (d.FindElement(@by).Displayed == true));
        }

        protected string GetCurrentPath()
        {
            return new Uri(driver.Url).AbsolutePath;
        }

        protected IWebElement FindErrorMessage(string id)
        {
            return driver.FindElement(By.Id(id)).FindElement(By.XPath("following-sibling::error-messages"));
        }

        protected string GetDateUpdate()
        {
            var date = DateTime.Now.ToString("dd/MM/yyyy");
            return date;
        }

        protected void LogLine(string message)
        {
            LogLine(message, TestContext);
        }

        private static bool ShouldUseHeadlessBrowser(TestContext context)
        {
            var configurationUseHeadlessBrowser = context.Properties["headlessBrowser"]?.ToString();
            return string.IsNullOrWhiteSpace(configurationUseHeadlessBrowser) ? defaultUseHeadlessBrowser : bool.Parse(configurationUseHeadlessBrowser);
        }

        private static string GetLanguage(TestContext context)
        {
            var configurationLanguage = context.Properties["language"]?.ToString();
            return string.IsNullOrWhiteSpace(configurationLanguage) ? defaltUseLanguage : configurationLanguage;
        }

        private static void LogLine(string message, TestContext context)
        {
            context.WriteLine(message);
        }

        private static void DisableCssAnimations()
        {
            var javaScriptExecutor = (IJavaScriptExecutor)driver;
            javaScriptExecutor.ExecuteScript(@"
function addStyleString(str) {
    var node = document.createElement('style');
    node.innerHTML = str;
    document.body.appendChild(node);
}

addStyleString('
* {
 transition-property: none !important;
 -o-transition-property: none !important;
 -moz-transition-property: none !important;
 -ms-transition-property: none !important;
 -webkit-transition-property: none !important;

 transform: none !important;
 -o-transform: none !important;
 -moz-transform: none !important;
 -ms-transform: none !important;
 -webkit-transform: none !important;

 animation: none !important;
 -o-animation: none !important;
 -moz-animation: none !important;
 -ms-animation: none !important;
 -webkit-animation: none !important;
}
');
".Replace("\r", "").Replace("\n", ""));
        }
    }
}

