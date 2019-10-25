using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using TechTalk.SpecFlow;
using Xunit;

namespace PeerTutor.UITests
{
    [Binding]
    public class SignUpSteps
    {
        private IWebDriver _driver;
        private SignUpPage _signUpPage;
        private CourseMenuPage _courseMenuPage;


        [Given(@"I am on the register page")]
        public void GivenIAmOnTheRegisterPage()
        {
            _driver = new ChromeDriver((Environment.CurrentDirectory));

            _signUpPage = SignUpPage.NavigateTo(_driver);
        }
        
        [Given(@"I enter a first name of (.*)")]
        public void GivenIEnterAFirstNameOf(string firstName)
        {
            _signUpPage.FirstName = firstName;
        }
        
        [Given(@"I enter a last name of (.*)")]
        public void GivenIEnterALastNameOf(string lastName)
        {
            _signUpPage.LastName = lastName;
        }
        
        [Given(@"I select the major (.*)")]
        public void GivenISelectTheMajor(string major)
        {
            _signUpPage.SelectMajor(_signUpPage.Major, major);
        }
        
        [Given(@"I select the year (.*)")]
        public void GivenISelectTheYear(int year)
        {
            _signUpPage.SelectYear(_signUpPage.ClassYear, year.ToString());
        }
        
        [Given(@"I enter a phone (.*)")]
        public void GivenIEnterAPhone(string phoneNum)
        {
            //IWebElement phoneNumber = _driver.FindElement(By.Name("Input.PhoneNumber"));
            //phoneNumber.SendKeys(phoneNum);
            _signUpPage.PhoneNumber = phoneNum;
        }
        
        [Given(@"I enter an email (.*)")]
        public void GivenIEnterAnEmail(string emailAddress)
        {
            _signUpPage.Email = emailAddress;
        }
        
        [Given(@"I enter a password (.*)")]
        public void GivenIEnterAPassword(string enteredPassword)
        {
            _signUpPage.Password = enteredPassword;
        }
        
        [Given(@"I confirm the password entered above (.*)")]
        public void GivenIConfirmThePasswordEnteredAbove(string confirmedPassword)
        {
            _signUpPage.ConfirmPassword = confirmedPassword;
        }
        
        [When(@"I press register")]
        public void WhenIPressRegister()
        {
            _courseMenuPage = _signUpPage.Register();
        }
        
        [Then(@"I should see the CourseMenu page")]
        public void ThenIShouldSeeTheCourseMenuPage()
        {
            Assert.Equal("Hello ssanders@mnsu.edu!", _courseMenuPage.UserSignedIn);
        }

        [AfterScenario]
        public void DisposeWebDriver()
        {
            _driver.Dispose();
        }
    }
}
