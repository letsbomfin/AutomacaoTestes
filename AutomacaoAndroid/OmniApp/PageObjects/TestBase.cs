using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Remote;
using System.Diagnostics;

namespace OmniApp.PageObjects
{

    //Pegue as informacoes necessarias com o desenvolvedor responsavel pelo app
    [TestClass]
    public class TestBase
    {
        private static string AppPackage = "";
        private static string MainActivityPackage = "";
        /* Create instance for appium driver */
        public static AndroidDriver<AndroidElement> driver;

        public static TimeSpan DefaultTimeout = TimeSpan.FromSeconds(120);

        [AssemblyInitialize]
        public static void Startup(TestContext tContext)
        {
            Process ExternalProcess = new Process();

            ExternalProcess.StartInfo.FileName = @"""C:\Program Files (x86)\Appium\node.exe""";
            ExternalProcess.StartInfo.Arguments = @"""C:\Program Files (x86)\Appium\node_modules\appium\lib\server\main.js"" --address 127.0.0.1 --port 4723 --platform-name Android --platform-version 23 --automation-name Appium --log-no-color";
            ExternalProcess.Start();
            ExternalProcess.WaitForExit(9000);

            DesiredCapabilities cap = new DesiredCapabilities();
            cap.SetCapability("deviceName", "Moto");
            //cap.SetCapability("app", "E:/Omni_appium/apk/app-qa-release.apk"); //se o app estiver instalado no celular, comente essa linha
            cap.SetCapability("androidPackage", AppPackage);
            cap.SetCapability("appActivity", MainActivityPackage);
            cap.SetCapability("plataformName", "Android");

            driver = new AndroidDriver<AndroidElement>(new Uri("http://127.0.0.1:4723/wd/hub"), cap, TimeSpan.FromSeconds(180));
        }

        [TestInitialize]
        public void BeforeEachTest()
        {
            driver.StartActivity(AppPackage, MainActivityPackage);
        }

        [AssemblyCleanup]
        public static void TearDown()
        {
             driver.Quit();     
        }

    }
}