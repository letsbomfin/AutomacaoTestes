namespace OpenQA.Selenium.Appium.Android
{
    internal class AndroidDriver
    {
        internal static object setNetworkConnectionSetting;
        private AndroidDriver<AndroidElement> _driver;

        public AndroidDriver(AndroidDriver<AndroidElement> _driver)
        {
            this._driver = _driver;
        }
    }
}