using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;
using System;

namespace PeerTutor.UITests
{
    public class SignUpPage
    {
        private readonly IWebDriver _driver;
        private const string PageUri = @"https://peertutor.azurewebsites.net/Identity/Account/Register";

        public SignUpPage(IWebDriver driver)
        {
            _driver = driver;
        }

        public static SignUpPage NavigateTo(IWebDriver driver)
        {
            driver.Navigate().GoToUrl(PageUri);

            return new SignUpPage(driver);
        }

        public string FirstName
        {
            set
            {
                _driver.FindElement(By.Name("Input.FirstName")).SendKeys(value);
            }
        }

        public string LastName
        {
            set
            {
                _driver.FindElement(By.Name("Input.LastName")).SendKeys(value);
            }
        }

        //[FindsBy(How = How.Name, Using = "Input.Major")]
        //[FindsBy(How = How.Id, Using = "Input_Major")] 
        public IWebElement Major
        {
            get
            {
                return this._driver.FindElement(By.Name("Input.Major"));
            }
        }

        public void SelectMajor(IWebElement element, string value)
        {
            element.Click();
            var selectedElement = new SelectElement(element);
            selectedElement.SelectByText(value);
        }
        //[FindsBy(How = How.Name, Using = "Input.ClassYear")]
        //public IWebElement ClassYear;
        public IWebElement ClassYear
        {
            get
            {
                return this._driver.FindElement(By.Name("Input.ClassYear"));
            }
        }

        public void SelectYear(IWebElement element, string value)
        {
            element.Click();
            var selectedElement = new SelectElement(element);
            selectedElement.SelectByText(value);
        }

        public string PhoneNumber
        {
            set
            {
                _driver.FindElement(By.Name("Input.PhoneNumber")).SendKeys(value);
            }
        }

        public string Email
        {
            set
            {
                _driver.FindElement(By.Name("Input.Email")).SendKeys(value);
            }
        }

        public string Password
        {
            set
            {
                _driver.FindElement(By.Name("Input.Password")).SendKeys(value);
            }
        }

        public string ConfirmPassword
        {
            set
            {
                _driver.FindElement(By.Name("Input.ConfirmPassword")).SendKeys(value);
            }
        }

        public CourseMenuPage Register()
        {
            _driver.FindElement(By.CssSelector(".btn.btn-primary")).Click();

            return new CourseMenuPage(_driver);
        }
    }

}
