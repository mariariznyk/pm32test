using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers; // Import this for wait helpers
using System;
using System.Collections.Generic;
using System.Linq;

namespace WebTestSelenium.Pages
{
    public class BankManagerPage
    {
        private readonly IWebDriver _driver;
        private WebDriverWait _wait;

        // Define locators
        private readonly By loginButton = By.CssSelector("button.btn-primary[ng-click='manager()']");
        private readonly By customersButton = By.CssSelector("body > div > div > div.ng-scope > div > div.center > button:nth-child(3)");
        private readonly By sortByPostCodeButton = By.XPath("//a[@ng-click=\"sortType = 'postCd'; sortReverse = !sortReverse\"]");
        private readonly By postCodeColumn = By.XPath("//tbody/tr/td[3]"); // Assuming Post Code is in the third column

        public BankManagerPage(IWebDriver driver)
        {
            _driver = driver;
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10)); // 10-second wait
        }

        // Method to click Bank Manager Login with wait
        public void ClickBankManagerLogin()
        {
            _wait.Until(ExpectedConditions.ElementToBeClickable(loginButton)).Click();
        }

        // Method to click Customers button with wait and a refined selector
        public void ClickCustomersButton()
        {
            _wait.Until(ExpectedConditions.ElementToBeClickable(customersButton)).Click();
        }

        // Method to sort by Post Code with wait
        public void SortByPostCode()
        {
            _wait.Until(ExpectedConditions.ElementToBeClickable(sortByPostCodeButton)).Click();
        }

        // Method to fetch Post Codes from the table
        public IList<string> GetPostCodes()
        {
            var postCodesElements = _driver.FindElements(postCodeColumn);
            return postCodesElements.Select(element => element.Text).ToList();
        }
    }
}
