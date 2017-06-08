#region License

// Distributed under the MIT License
// ============================================================
// Copyright (c) 2016 Hotcakes Commerce, LLC
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software 
// and associated documentation files (the "Software"), to deal in the Software without restriction, 
// including without limitation the rights to use, copy, modify, merge, publish, distribute, 
// sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is 
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or 
// substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR 
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE 
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER 
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, 
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN 
// THE SOFTWARE.

#endregion

using System;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace Hotcakes.AutomatedTesting.TestUtils
{
    public class BasicUnitTestBase
    {
        protected IWebDriver Driver;

        public void JQueryWait()
        {
            Thread.Sleep(7000);
            var js = (IJavaScriptExecutor)Driver;
            // ReSharper disable once RedundantAssignment
            long i = 0;

            do
            {
                i = (long)js.ExecuteScript("return $.active");
                Thread.Sleep(1);
            } while (i != 0);
        }

        public void WaitForReady()
        {
            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(30));
            wait.Until(driver =>
            {
                var isAjaxFinished = (bool)((IJavaScriptExecutor)driver).
                    ExecuteScript("return jQuery.active == 0");
                var isLoaderHidden = (bool)((IJavaScriptExecutor)driver).
                    ExecuteScript("return $('.spinner').is(':visible') == false");
                return isAjaxFinished & isLoaderHidden;
            });
        }

        public void JQueryWait(int? t)
        {
            var js = (IJavaScriptExecutor)Driver;
            long i = 0;

            do
            {
                i = (long)js.ExecuteScript("return $.active");
                Thread.Sleep(100);
            } while (i != 0);
        }

        public IWebElement FindElement(By by)
        {
            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(3));
            return wait.Until(drv => drv.FindElement(by));
        }

        public void CheckCheckBox(string chekboxId)
        {
            var chkBox = Driver.FindElement(By.Id(chekboxId));
            if (!chkBox.Selected)
            {
                chkBox.Click();
            }
        }

        public void UnCheckCheckBox(string chekboxId)
        {
            var chkBox = Driver.FindElement(By.Id(chekboxId));
            if (chkBox.Selected)
            {
                chkBox.Click();
            }
        }

        public bool WhetherTheTextPresent(string textToBeVerified)
        {
            //start
            if (Driver.FindElement(By.XPath("//*[contains(.,'" + textToBeVerified + "')]")) != null)
            {
                return true;
            }
            {
                return false;
            }
        } //end

        public void Refresh()
        {
            try
            {
                using (var timeoutModifier = new TimeoutModifier(Driver, 4))
                {
                    Driver.FindElement(By.LinkText("Refresh")).Click();
                }
            }
            catch (Exception ex)
            {
                ex = ex;
            }
        }
    }
}