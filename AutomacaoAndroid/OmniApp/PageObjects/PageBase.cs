using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium.MultiTouch;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OmniApp.PageObjects
{
    public class PageBase
    {

        public AndroidDriver<AndroidElement> _driver;
        public static TimeSpan DefaultTimeout = TimeSpan.FromSeconds(60);
        private AppiumDriver<AndroidElement> driver;
        private AppiumdDriver<AndroidElement> driver1;

        public PageBase(AndroidDriver<AndroidElement> driver)
        {
            _driver = driver;
        }

        public PageBase(AppiumDriver<AndroidElement> driver)
        {
            this.driver = driver;
        }

        public PageBase(AppiumdDriver<AndroidElement> driver1)
        {
            this.driver1 = driver1;
        }

        /* Buttons */ 
        public void BtnClick(string id)
        {
            var button = _driver.FindElementById("net.take.omni:id/" + id);
            button.Click();
        }

        public void BtnContentDesc(string contentDesc)
        {
            var btnDesc = _driver.FindElementByXPath("//*[@content-desc='" + contentDesc + "']");
            btnDesc.Click();
        }

       public void BtnAttachment(string attachment)
        {
            var btnAttach = _driver.FindElement(By.XPath("//*[@resource-id='net.take.omni:id/bs_list_title']/..//*[@text='" + attachment + "']"));
            btnAttach.Click();
        }

        public void BtnByText(string text)
        {
            var btnByText = _driver.FindElement(By.XPath("//*[contains(@text,'" + text + "')]"));
            btnByText.Click();
        }

        public void BtnActionBar(string id)
        {
            var buttonActionBar = _driver.FindElementById("net.take.omni:id/" + id);

            buttonActionBar.Click();
            WaitForElement("omni_action_bar_title");
        }

        /* Waits */       
        public void WaitForElement(string id)
        {
           new WebDriverWait(_driver, DefaultTimeout).Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.Id(id)));

        }

        public void WaitForText(string text)
        {
            new WebDriverWait(_driver, DefaultTimeout).Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.XPath("//*[@text='" + text + "']")));
        }

        /* Click texts */
        public void LongClickPartialText(string partial)
        {
            WaitForElement("thread_list_item_details_contact");

            var partialText = _driver.FindElement(By.XPath("//*[contains(@text,'" + partial + "')]"));
            partialText.Tap(1, 2000);
        }

        /* Send message */
        public void SendMessageThreadOpen(string message)
        {
            var editText = _driver.FindElementById("net.take.omni:id/sendMessageEditText");
            editText.SendKeys(message);
            BtnClick("sendMessageButton");
            WaitForElement("sendMessageButtonMic");
        }
        
        /* Open thread */
        public void OpenThread(string threadContact)
        {
            WaitForElement("thread_list_item_details_contact");
            TouchAction openThread = new TouchAction(_driver);
            openThread.Press(_driver.FindElementByXPath("//android.widget.TextView[contains(@text,'" + threadContact + "')]")).Release().Perform();
            WaitForElement("omni_action_bar_title");
        }

        /* Go to Page */
      
        public void GoToMyService()
        {
            var myService = _driver.FindElement(By.XPath("//*[@content-desc='MyServices']"));
            myService.Click();
        }

        public void GoBack()
        {
            Thread.Sleep(800);
            var navigateBack = _driver.FindElementByXPath("//*[@content-desc='Navegar para cima']");
            navigateBack.Click();
            Thread.Sleep(800);
        }

        public void NavigateBack()
        {
            _driver.Navigate().Back();
            Thread.Sleep(1000);
        }

        /* Swipe */
        public void SwipeLeftToRight()
        {
            _driver.Swipe(0, 412, 396, 411, 2000);
        }

        public void SwipeRightToLeft()
        {
            _driver.Swipe(539, 359, 213, 354, 2000);
          
        }

        public void SwipeUpdoDown()
        {
            _driver.Swipe(387, 21, 363, 834, 2000);
        } 

        public void SwipeDownToUp()
        {
            _driver.Swipe(393, 943, 379, 13, 2000);
        }

        /* screenshot */
        /* rooDirectory = local no qual o seu arquivo será salvo  */
        /* fileName = nome e formato no qual a imagem será salva - por padrão o nome do arquivo será a data e hora do sistema */
        public string SaveScreenshot(string fileName, string directory)
        {
            var rootDirectory = @"C:\Users\leticia\Desktop\AuditoriaVivoAlerta";
            fileName = String.Format("{0}\\{1}\\{2}{3}{4}", rootDirectory, directory, fileName, DateTime.Now.ToString("yyyy-MM-ddTHHmmss"), ".png");
            Directory.CreateDirectory(rootDirectory + "\\" + directory);

            var screenShot = ((ITakesScreenshot)_driver).GetScreenshot();
            screenShot.SaveAsFile(fileName, ImageFormat.Png);

            return fileName;

            /* caso queira criar um folder novo para cada screenshot, basta fazer da seguinte forma:

          var directory = "Exemplo" + DateTime.Now.ToString("yyyy-MM-ddTHHmmss");
          SaveScreenshot("Exemplo", directory); 

          Seu folder terá o nome + a data do sistema - Ex: Exemplo2016-09-05T172536
          Ao recuperar a data do sistema, reduz a chance de ter dois files ou folders com mesmo nome
          */

        }


        /* delete thread */
        public void DeleteThread(string thread)
        {
            WaitForElement("thread_list_item_details_contact");
            var longClick = _driver.FindElement(By.XPath("//*[contains(@text,'" + thread + "')]"));
            longClick.Tap(1, 2000);

            WaitForElement("title");
            BtnByText("Apagar conversa");
        }

        /* tap position */
        public void Position(double x, double y)
        {
            TouchAction tapPosition = new TouchAction(_driver);
            tapPosition.Tap(x, y).Release().Perform();
        }
    }
}


