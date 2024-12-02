using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using TechTalk.SpecFlow;
using WebTestSelenium.Pages;
using System.Linq;
using System.Collections.Generic;

namespace WebTestSelenium.Steps
{
    [Binding]
    public class SortByPostCodeSteps
    {
        private IWebDriver _driver;
        private BankManagerPage _bankManagerPage;

        [BeforeScenario]
        public void Setup()
        {
            _driver = new ChromeDriver();
            _driver.Navigate().GoToUrl("https://www.globalsqa.com/angularJs-protractor/BankingProject");
            _driver.Manage().Window.Maximize();

            _bankManagerPage = new BankManagerPage(_driver);
        }

        [Given(@"I am logged in as a Bank Manager")]
        public void GivenIAmLoggedInAsABankManager()
        {
            _bankManagerPage.ClickBankManagerLogin();
        }

        [When(@"I navigate to the Customers page")]
        public void WhenINavigateToTheCustomersPage()
        {
            _bankManagerPage.ClickCustomersButton();
        }

        [When(@"I sort customers by Post Code")]
        public void WhenISortCustomersByPostCode()
        {
            _bankManagerPage.SortByPostCode();
        }

        [Then(@"the customer list should be sorted by Post Code")]
        public void ThenTheCustomerListShouldBeSortedByPostCode()
        {
            // Fetch the post codes from the page
            var actualPostCodes = _bankManagerPage.GetPostCodes();

            // Sort the post codes in descending order to get the expected order
            var sortedPostCodes = actualPostCodes.OrderByDescending(code => code).ToList();

            // Verify if the actual post codes are in descending order
            Assert.AreEqual(sortedPostCodes, actualPostCodes, "The customer list is not sorted by Post Code in descending order.");
        }


        [AfterScenario]
        public void TearDown()
        {
           // _driver.Quit();
        }
    }
}
