using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Text;

namespace PeerTutor.UITests
{
    
    public class CourseMenuPage
    {
        private readonly IWebDriver _driver;

        public CourseMenuPage(IWebDriver driver)
        {
            _driver = driver;
        }

        public string UserSignedIn => _driver.FindElement(By.Id("manage")).GetAttribute("innerText");
    }
}

