#region License

// Distributed under the MIT License
// ============================================================
// Copyright (c) 2017 Hotcakes Commerce, LLC
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
using System.Text;
using System.Threading;
using Hotcakes.AutomatedTesting.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace Hotcakes.AutomatedTesting.TestUtils
{
    public class BasicUnitTests : BasicUnitTestBase
    {
        private string _giftCardNumber01 = "";
        private string _giftCardNumber02 = "";
        protected bool AcceptNextAlert = true;

        protected string Categoryname;
        protected string DeliveryOptionPerItem;
        protected string DeliveryOptionPerOrder;

        protected string Variable1;
        protected string Variable2;
        protected string Variable3;
        protected string Variable4;

        protected string Variablegeneral;
        protected StringBuilder VerificationErrors;

        //            currency	DATE	MLT CHOICE	TEXT
        //product1	    1.21	2013	1	        text1
        //product2	    1.22	2013	2	        text1
        //product3	    1.23	2013	2	        text1
        //product4	    1.24	2014	3	        text1
        //product5	    1.25	2014	3	        text1
        //product6	    1.26	2015	3	        text1

        public BasicUnitTests()
        {
            var now = DateTime.Now;
            var timestapTemplate = "yyyy-MM-dd__HH-mm";

            Variablegeneral = now.ToString(timestapTemplate) + "-11";
            DeliveryOptionPerOrder = Variablegeneral + " Lime Flat Rate Per Order 12,34";
            DeliveryOptionPerItem = Variablegeneral + " Tan Flat Rate Per Item 21,43";
            Categoryname = now.ToString(timestapTemplate);
            Variable1 = now.ToString(timestapTemplate) + "-01";
            Variable2 = now.ToString(timestapTemplate) + "-02";
            Variable3 = now.ToString(timestapTemplate) + "-03";
            Variable4 = now.ToString(timestapTemplate) + "-04";
        }

        public void UserAdminLoginHcc()
        {
            Driver.Navigate().GoToUrl(Globals.BASE_PATH + "/");
            JQueryWait();
            JQueryWait();
            Driver.FindElement(By.Id("hcc_dnnLogin_enhancedLoginLink")).Click();
            JQueryWait(); // for Azure
            Driver.Navigate()
                .GoToUrl(Globals.BASE_PATH + "/Home/tabid/55/ctl/Login/language/en-US/Default.aspx?returnurl=/&popUp=true");
            JQueryWait();
            Driver.FindElement(By.Id("hcc_ctr_Login_Login_DNN_txtUsername")).Clear();
            Driver.FindElement(By.Id("hcc_ctr_Login_Login_DNN_txtUsername")).SendKeys(Globals.ADMIN_USERNAME);
            Driver.FindElement(By.Id("hcc_ctr_Login_Login_DNN_txtPassword")).Clear();
            Driver.FindElement(By.Id("hcc_ctr_Login_Login_DNN_txtPassword")).SendKeys(Globals.ADMIN_PASSWORD);
            Driver.FindElement(By.Id("hcc_ctr_Login_Login_DNN_cmdLogin")).Click();
            JQueryWait();
        }

        public void UserPoloLoginHcc()
        {
            Driver.Navigate().GoToUrl(Globals.BASE_PATH + "/");
            JQueryWait();
            Driver.FindElement(By.Id("hcc_dnnLogin_enhancedLoginLink")).Click();
            Driver.Navigate()
                .GoToUrl(Globals.BASE_PATH + "/Home/tabid/55/ctl/Login/language/en-US/Default.aspx?returnurl=/&popUp=true");
            Driver.FindElement(By.Id("hcc_ctr_Login_Login_DNN_txtUsername")).Clear();
            Driver.FindElement(By.Id("hcc_ctr_Login_Login_DNN_txtUsername")).SendKeys("polo");
            Driver.FindElement(By.Id("hcc_ctr_Login_Login_DNN_txtPassword")).Clear();
            Driver.FindElement(By.Id("hcc_ctr_Login_Login_DNN_txtPassword")).SendKeys("Password1");
            Driver.FindElement(By.Id("hcc_ctr_Login_Login_DNN_cmdLogin")).Click();
            Driver.FindElement(By.LinkText("Home")).Click();
        }

        public void WizardSetup()
        {
            JQueryWait(); // for Azure
            Driver.Navigate()
                .GoToUrl(Globals.BASE_PATH + "/DesktopModules/Hotcakes/Core/Admin/SetupWizard/SetupWizard.aspx?step=1");
            try
            {
                // workaround because the wizard sometimes starts at step 0 in the tests here for some reason
                Driver.FindElement(By.Id("MainContent_Step0Dashboard1_btnStart")).Click();
            }
            catch
            {
                // do nothing
            }
            Driver.FindElement(By.Id("ctl00_MainContent_Step1General1_txtAddressLine1")).SendKeys(Globals.STORE_ADDRESS_STREET1);
            Driver.FindElement(By.Id("ctl00_MainContent_Step1General1_txtCity")).SendKeys(Globals.STORE_ADDRESS_CITY);
            Driver.FindElement(By.Id("ctl00_MainContent_Step1General1_txtZip")).SendKeys(Globals.STORE_ADDRESS_POSTALCODE);
            Driver.FindElement(By.Id("MainContent_Step1General1_btnSave")).Click();
            Driver.FindElement(By.Id("MainContent_Step2Payment1_btnSave")).Click();
            Driver.FindElement(By.Id("MainContent_Step3Shipping1_btnSave")).Click();
            Driver.FindElement(By.Id("MainContent_Step4Taxes1_btnSave")).Click();

            // Store Name & Logo Settings
            JQueryWait(); // for Azure
            Driver.Navigate().GoToUrl(Globals.BASE_PATH + "/DesktopModules/Hotcakes/Core/Admin/configuration/General.aspx");
            var chkBox = Driver.FindElement(By.Id("chkUseLogoImage"));
            if (!chkBox.Selected)
            {
                chkBox.Click();
            }
            Driver.FindElement(By.Id("MainContent_txtSiteName")).Clear();
            Driver.FindElement(By.Id("MainContent_txtSiteName")).SendKeys("QA Automated Testing Store Name");
            Driver.FindElement(By.Id("MainContent_btnSave")).Click();

            // Store's Address
            JQueryWait(); // for Azure
            Driver.Navigate().GoToUrl(Globals.BASE_PATH + "/DesktopModules/Hotcakes/Core/Admin/configuration/StoreInfo.aspx");
            new SelectElement(Driver.FindElement(By.Id("MainContent_ucAddressEditor_lstCountry"))).SelectByText(
                "United States");
            Driver.FindElement(By.Id("MainContent_ucAddressEditor_firstNameField")).Clear();
            Driver.FindElement(By.Id("MainContent_ucAddressEditor_firstNameField")).SendKeys("Ryan");
            Driver.FindElement(By.Id("MainContent_ucAddressEditor_lastNameField")).Clear();
            Driver.FindElement(By.Id("MainContent_ucAddressEditor_lastNameField")).SendKeys("Morgan");
            Driver.FindElement(By.Id("MainContent_ucAddressEditor_CompanyField")).Clear();
            Driver.FindElement(By.Id("MainContent_ucAddressEditor_CompanyField")).SendKeys("Arrow Digital");
            Driver.FindElement(By.Id("MainContent_ucAddressEditor_address1Field")).Clear();
            Driver.FindElement(By.Id("MainContent_ucAddressEditor_address1Field")).SendKeys(Globals.STORE_ADDRESS_STREET1);
            Driver.FindElement(By.Id("MainContent_ucAddressEditor_cityField")).Clear();
            Driver.FindElement(By.Id("MainContent_ucAddressEditor_cityField")).SendKeys(Globals.STORE_ADDRESS_CITY);
            new SelectElement(Driver.FindElement(By.Id("MainContent_ucAddressEditor_lstRegions"))).SelectByText(Globals.SHIPPING_ADDRESS_REGION);
            Driver.FindElement(By.Id("MainContent_ucAddressEditor_postalCodeField")).Clear();
            Driver.FindElement(By.Id("MainContent_ucAddressEditor_postalCodeField")).SendKeys(Globals.STORE_ADDRESS_POSTALCODE);
            Driver.FindElement(By.Id("MainContent_ucAddressEditor_PhoneNumberField")).Clear();
            Driver.FindElement(By.Id("MainContent_ucAddressEditor_PhoneNumberField")).SendKeys(Globals.STORE_ADDRESS_PHONE);
            Driver.FindElement(By.Id("MainContent_btnSave")).Click();
            JQueryWait();

            // Geo Location
            JQueryWait(); // for Azure
            Driver.Navigate().GoToUrl(Globals.BASE_PATH + "/DesktopModules/Hotcakes/Core/Admin/configuration/GeoLocation.aspx");
            new SelectElement(Driver.FindElement(By.Id("MainContent_lstTimeZone"))).SelectByText(
                "(UTC+02:00) Helsinki, Kyiv, Riga, Sofia, Tallinn, Vilnius");
            new SelectElement(Driver.FindElement(By.Id("MainContent_lstCulture"))).SelectByText("Spain 1,23 €");
            Driver.FindElement(By.CssSelector("option[value=\"es-ES\"]")).Click();
            Driver.FindElement(By.Id("MainContent_btnSaveChanges")).Click();

            // Send New Order Emails To
            JQueryWait(); // for Azure
            Driver.Navigate().GoToUrl(Globals.BASE_PATH + "/DesktopModules/Hotcakes/Core/Admin/configuration/Email.aspx");
            Driver.FindElement(By.Id("MainContent_ContactEmailField")).Clear();
            Driver.FindElement(By.Id("MainContent_ContactEmailField")).SendKeys("Send_General_Email_To@change.me");
            Driver.FindElement(By.Id("MainContent_OrderNotificationEmailField")).Clear();
            Driver.FindElement(By.Id("MainContent_OrderNotificationEmailField"))
                .SendKeys("Send_New_Order_Emails_To@change.me");
            Driver.FindElement(By.Id("MainContent_btnSave")).Click();

            // Payment Methods
            SetThePaymentMethodsTestGateway();

            Thread.Sleep(5000);
            JQueryWait();
            CheckCheckBox("MainContent_gvPaymentMethods_chkEnabled_8");
            JQueryWait();
            CheckCheckBox("MainContent_gvPaymentMethods_chkEnabled_7");
            CheckCheckBox("MainContent_gvPaymentMethods_chkEnabled_6");
            CheckCheckBox("MainContent_gvPaymentMethods_chkEnabled_5");
            CheckCheckBox("MainContent_gvPaymentMethods_chkEnabled_4");
            CheckCheckBox("MainContent_gvPaymentMethods_chkEnabled_3");
            CheckCheckBox("MainContent_gvPaymentMethods_chkEnabled_2");
            CheckCheckBox("MainContent_gvPaymentMethods_chkEnabled_1");
            CheckCheckBox("MainContent_gvPaymentMethods_chkEnabled_0");
            JQueryWait();
            Driver.FindElement(By.Id("MainContent_btnSaveChanges")).Click();
            JQueryWait();

            // TAX Schedules
            JQueryWait(); // for Azure
            Driver.Navigate().GoToUrl(Globals.BASE_PATH + "/DesktopModules/Hotcakes/Core/Admin/configuration/TaxClasses.aspx");
            JQueryWait();
            CheckCheckBox("MainContent_chkApplyVATRules");
            Driver.FindElement(By.Id("MainContent_btnSave")).Click();
            JQueryWait();

            // Shipping And Handling settings
            HandlingEnable();

            //Gift Card Settings
            JQueryWait(); // for Azure
            Driver.Navigate()
                .GoToUrl(Globals.BASE_PATH + "/DesktopModules/Hotcakes/Core/Admin/configuration/GiftCardConfiguration.aspx");
            CheckCheckBox("MainContent_cbEnableGiftCards");
            Driver.FindElement(By.Id("MainContent_txtExpiration")).Clear();
            Driver.FindElement(By.Id("MainContent_txtExpiration")).SendKeys("1");
            Driver.FindElement(By.Id("MainContent_txtCardNumberFormat")).Clear();
            Driver.FindElement(By.Id("MainContent_txtCardNumberFormat"))
                .SendKeys("GIFTxxxxxxXxxxxxxxxxXxxxxxxxxxXxxxxxxxxxXxxxxxxxxx");
            CheckCheckBox("MainContent_cbUseSymbols");
            Driver.FindElement(By.Id("MainContent_txtMinAmount")).Clear();
            Driver.FindElement(By.Id("MainContent_txtMinAmount")).SendKeys("1");
            Driver.FindElement(By.Id("MainContent_txtMaxAmount")).Clear();
            Driver.FindElement(By.Id("MainContent_txtMaxAmount")).SendKeys("500");
            Driver.FindElement(By.Id("MainContent_lstCaptureMode_1")).Click();
            Driver.FindElement(By.Id("MainContent_btnOptions")).Click();
            JQueryWait();
            Driver.FindElement(By.Id("MainContent_gatewayEditor_btnSave")).Click();
            JQueryWait();
            Driver.FindElement(By.Id("MainContent_txtAmounts")).Clear();
            Driver.FindElement(By.Id("MainContent_txtAmounts")).SendKeys("25,50,100,250,500");
            Driver.FindElement(By.Id("MainContent_btnSave")).Click();
            JQueryWait();

            //Admin > Orders
            JQueryWait(); // for Azure
            Driver.Navigate().GoToUrl(Globals.BASE_PATH + "/DesktopModules/Hotcakes/Core/Admin/configuration/Orders.aspx");
            CheckCheckBox("MainContent_chkZeroDollarOrders");
            CheckCheckBox("MainContent_chkForceSiteTerms");
            CheckCheckBox("MainContent_chkUseChildChoicesAdjustmentsForBundles");
            Driver.FindElement(By.Id("MainContent_txtOrderLimitQuantity")).Clear();
            Driver.FindElement(By.Id("MainContent_txtOrderLimitQuantity")).SendKeys("999999");
            Driver.FindElement(By.Id("MainContent_btnSave")).Click();
        }

        public void AddProductTypePropertiesTextfield()
        {
            JQueryWait();
            Driver.Navigate()
                .GoToUrl(Globals.BASE_PATH + "/DesktopModules/Hotcakes/Core/Admin/catalog/ProductTypeProperties.aspx");
            Thread.Sleep(7000);
            JQueryWait();
            Driver.FindElement(By.Id("NavContent_btnNew")).Click();
            Driver.FindElement(By.Id("MainContent_txtPropertyName")).Clear();
            Driver.FindElement(By.Id("MainContent_txtPropertyName"))
                .SendKeys(Variablegeneral + " \"TEXT FIELD\" Product Type Property");
            Driver.FindElement(By.Id("MainContent_txtDisplayName")).Clear();
            Driver.FindElement(By.Id("MainContent_txtDisplayName"))
                .SendKeys(Variablegeneral + " \"TEXT FIELD\" Product Type Property");
            Driver.FindElement(By.Id("MainContent_chkDisplayToDropShipper")).Click();
            Driver.FindElement(By.Id("MainContent_chkDisplayOnSearch")).Click();
            Driver.FindElement(By.Id("MainContent_chkIsLocalizable")).Click();
            Driver.FindElement(By.Id("MainContent_txtDefaultTextValue")).Clear();
            Driver.FindElement(By.Id("MainContent_txtDefaultTextValue"))
                .SendKeys(Variablegeneral + " Default Value \"TEXT FIELD\" Product Type Property");
            Driver.FindElement(By.Id("MainContent_btnSave")).Click();
        }

        public void AddProductTypePropertiesMultiplechoice()
        {
            Driver.Navigate()
                .GoToUrl(Globals.BASE_PATH + "/DesktopModules/Hotcakes/Core/Admin/catalog/ProductTypeProperties.aspx");
            new SelectElement(Driver.FindElement(By.Id("NavContent_lstPropertyType"))).SelectByText("Multiple Choice");
            Driver.FindElement(By.Id("NavContent_btnNew")).Click();
            Driver.FindElement(By.Id("MainContent_txtPropertyName")).Clear();
            Driver.FindElement(By.Id("MainContent_txtPropertyName"))
                .SendKeys(Variablegeneral + " \"MLT CHOICE\" Product Type Property");
            Driver.FindElement(By.Id("MainContent_txtDisplayName")).Clear();
            Driver.FindElement(By.Id("MainContent_txtDisplayName"))
                .SendKeys(Variablegeneral + " \"MLT CHOICE\" Product Type Property");
            Driver.FindElement(By.Id("MainContent_chkDisplayToDropShipper")).Click();
            Driver.FindElement(By.Id("MainContent_chkDisplayOnSearch")).Click();
            JQueryWait();
            Thread.Sleep(1000);
            Driver.FindElement(By.Id("MainContent_txtNewChoice")).Clear();
            Driver.FindElement(By.Id("MainContent_txtNewChoice")).SendKeys(Variable1 + " MLT CHOICE No1");
            Driver.FindElement(By.Id("MainContent_btnNewChoice")).Click();
            Assert.AreEqual("Update", Driver.FindElement(By.Id("MainContent_btnUpdateChoice")).Text);
            Driver.FindElement(By.Id("MainContent_btnUpdateChoice")).Click();
            Thread.Sleep(2000);
            JQueryWait();
            Driver.FindElement(By.Id("MainContent_txtNewChoice")).Clear();
            Driver.FindElement(By.Id("MainContent_txtNewChoice")).SendKeys(Variable2 + " MLT CHOICE No2");
            Driver.FindElement(By.Id("MainContent_btnNewChoice")).Click();
            Assert.AreEqual("Update", Driver.FindElement(By.Id("MainContent_btnUpdateChoice")).Text);
            Driver.FindElement(By.Id("MainContent_btnUpdateChoice")).ScrollWindowAndClick(Driver);
            Thread.Sleep(2000);
            JQueryWait();
            Driver.FindElement(By.Id("MainContent_txtNewChoice")).Clear();
            Driver.FindElement(By.Id("MainContent_txtNewChoice")).SendKeys(Variable3 + " MLT CHOICE No3");
            Driver.FindElement(By.Id("MainContent_btnNewChoice")).Click();
            Assert.AreEqual("Update", Driver.FindElement(By.Id("MainContent_btnUpdateChoice")).Text);
            Driver.FindElement(By.Id("MainContent_btnUpdateChoice")).Click();
            Thread.Sleep(3000);
            Driver.FindElement(By.Id("ctl00_MainContent_rgChoices_ctl00_ctl08_chbDefault")).Click();
            Driver.FindElement(By.Id("MainContent_btnSave")).Click();
        }

        public void AddProductTypePropertiesCurrencyfield()
        {
            JQueryWait(); // for Azure
            Driver.Navigate()
                .GoToUrl(Globals.BASE_PATH + "/DesktopModules/Hotcakes/Core/Admin/catalog/ProductTypeProperties.aspx");
            new SelectElement(Driver.FindElement(By.Id("NavContent_lstPropertyType"))).SelectByText("Currency");
            Driver.FindElement(By.Id("NavContent_btnNew")).Click();
            Driver.FindElement(By.Id("MainContent_txtPropertyName")).Clear();
            Driver.FindElement(By.Id("MainContent_txtPropertyName"))
                .SendKeys(Variablegeneral + " \"CURRENCY\" Product Type Property");
            Driver.FindElement(By.Id("MainContent_txtDisplayName")).Clear();
            Driver.FindElement(By.Id("MainContent_txtDisplayName"))
                .SendKeys(Variablegeneral + " \"CURRENCY\" Product Type Property");
            Driver.FindElement(By.Id("MainContent_chkDisplayToDropShipper")).Click();
            Driver.FindElement(By.Id("MainContent_chkDisplayOnSearch")).Click();
            Driver.FindElement(By.Id("MainContent_txtDefaultCurrencyValue")).Clear();
            Driver.FindElement(By.Id("MainContent_txtDefaultCurrencyValue")).SendKeys("1.23");
            Driver.FindElement(By.Id("MainContent_btnSave")).Click();
            new SelectElement(Driver.FindElement(By.Id("NavContent_lstPropertyType"))).SelectByText("Multiple Choice");
        }

        public void AddProductTypePropertiesDatefield()
        {
            JQueryWait(); // for Azure
            Driver.Navigate()
                .GoToUrl(Globals.BASE_PATH + "/DesktopModules/Hotcakes/Core/Admin/catalog/ProductTypeProperties.aspx");
            new SelectElement(Driver.FindElement(By.Id("NavContent_lstPropertyType"))).SelectByText("Date");
            JQueryWait();
            Driver.FindElement(By.Id("NavContent_btnNew")).Click();
            Driver.FindElement(By.Id("MainContent_txtPropertyName")).Clear();
            Driver.FindElement(By.Id("MainContent_txtPropertyName"))
                .SendKeys(Variablegeneral + " \"DATE\" Product Type Property");
            Driver.FindElement(By.Id("MainContent_txtDisplayName")).Clear();
            Driver.FindElement(By.Id("MainContent_txtDisplayName"))
                .SendKeys(Variablegeneral + " \"DATE\" Product Type Property");
            Driver.FindElement(By.Id("MainContent_chkDisplayToDropShipper")).Click();
            Driver.FindElement(By.Id("MainContent_chkDisplayOnSearch")).Click();
            Driver.FindElement(By.Id("ctl00_MainContent_radDefaultDate_popupButton")).Click();
            Driver.FindElement(By.LinkText("1")).Click();
            Thread.Sleep(3000);
            JQueryWait();
            Driver.FindElement(By.Id("MainContent_btnSave")).Click();
            Thread.Sleep(3000);
            JQueryWait();
            Driver.FindElement(By.LinkText("Admin")).Click();
        }

        public void ProductTypesInclude4TypeProperties()
        {
            Thread.Sleep(10000);
            JQueryWait(); // for Azure
            Driver.Navigate().GoToUrl(Globals.BASE_PATH + "/DesktopModules/Hotcakes/Core/Admin/catalog/ProductTypes.aspx");
            JQueryWait(); // for Azure
            Thread.Sleep(10000);
            Driver.FindElement(By.LinkText("Product Types")).Click();
            JQueryWait(); // for Azure
            Driver.FindElement(By.Id("NavContent_txtNewNameField")).Clear();
            JQueryWait(); // for Azure
            Driver.FindElement(By.Id("NavContent_txtNewNameField"))
                .SendKeys(Variablegeneral + " Product Types Include 4 Type Properties");
            Driver.FindElement(By.Id("NavContent_btnNew")).Click();
            Assert.AreEqual("Add", Driver.FindElement(By.Id("MainContent_btnAddProperty")).Text);
            new SelectElement(Driver.FindElement(By.Id("MainContent_lstAvailableProperties"))).SelectByText(
                Variablegeneral + " \"CURRENCY\" Product Type Property");
            Assert.AreEqual("Add", Driver.FindElement(By.Id("MainContent_btnAddProperty")).Text);
            Driver.FindElement(By.Id("MainContent_btnAddProperty")).Click();
            JQueryWait();
            Thread.Sleep(1000);
            Assert.AreEqual("Add", Driver.FindElement(By.Id("MainContent_btnAddProperty")).Text);
            new SelectElement(Driver.FindElement(By.Id("MainContent_lstAvailableProperties"))).SelectByText(
                Variablegeneral + " \"DATE\" Product Type Property");
            Assert.AreEqual("Add", Driver.FindElement(By.Id("MainContent_btnAddProperty")).Text);
            Driver.FindElement(By.Id("MainContent_btnAddProperty")).Click();
            JQueryWait();
            Thread.Sleep(1000);
            Assert.AreEqual("Add", Driver.FindElement(By.Id("MainContent_btnAddProperty")).Text);
            new SelectElement(Driver.FindElement(By.Id("MainContent_lstAvailableProperties"))).SelectByText(
                Variablegeneral + " \"MLT CHOICE\" Product Type Property");
            Assert.AreEqual("Add", Driver.FindElement(By.Id("MainContent_btnAddProperty")).Text);
            Driver.FindElement(By.Id("MainContent_btnAddProperty")).Click();
            JQueryWait();
            Thread.Sleep(1000);
            Assert.AreEqual("Add", Driver.FindElement(By.Id("MainContent_btnAddProperty")).Text);
            new SelectElement(Driver.FindElement(By.Id("MainContent_lstAvailableProperties"))).SelectByText(
                Variablegeneral + " \"TEXT FIELD\" Product Type Property");
            Assert.AreEqual("Add", Driver.FindElement(By.Id("MainContent_btnAddProperty")).Text);
            Driver.FindElement(By.Id("MainContent_btnAddProperty")).Click();
            Thread.Sleep(3000);
            JQueryWait();
            Assert.AreEqual("Add", Driver.FindElement(By.Id("MainContent_btnAddProperty")).Text);
            Driver.FindElement(By.Id("MainContent_btnSave")).Click();
        }

        public void AddTaxSchedules()
        {
            AddTaxSchedule(Variable1 + " Schedule1", 
                "20,12", 
                "15,12", 
                Globals.SHIPPING_ADDRESS_COUNTRY, 
                Globals.SHIPPING_ADDRESS_REGION, 
                Globals.SHIPPING_ADDRESS_POSTALCODE, 
                "10,12",
                "12,12", 
                Globals.BILLING_ADDRESS_COUNTRY, 
                Globals.BILLING_ADDRESS_REGION, 
                Globals.BILLING_ADDRESS_POSTALCODE, 
                "11,02", 
                "13,02");

            AddTaxSchedule(Variable2 + " Schedule2", 
                "25,25", 
                "20,25", 
                Globals.SHIPPING_ADDRESS_COUNTRY, 
                Globals.SHIPPING_ADDRESS_REGION, 
                Globals.SHIPPING_ADDRESS_POSTALCODE, 
                "11,25",
                "10,25", 
                Globals.BILLING_ADDRESS_COUNTRY, 
                Globals.BILLING_ADDRESS_REGION, 
                Globals.BILLING_ADDRESS_POSTALCODE, 
                "12,05", 
                "11,05");
            
            AddTaxSchedule(Variable3 + " Schedule3", 
                "20,33", 
                "18,33", 
                Globals.SHIPPING_ADDRESS_COUNTRY, 
                Globals.SHIPPING_ADDRESS_REGION, 
                Globals.SHIPPING_ADDRESS_POSTALCODE, 
                "12,33",
                "16,33", 
                Globals.BILLING_ADDRESS_COUNTRY, 
                Globals.BILLING_ADDRESS_REGION, 
                Globals.BILLING_ADDRESS_POSTALCODE, 
                "13,03", 
                "17,03");
            
            AddTaxSchedule(Variable4 + " Schedule4", 
                "20,24", 
                "15,12", 
                Globals.SHIPPING_ADDRESS_COUNTRY, 
                Globals.SHIPPING_ADDRESS_REGION, 
                Globals.SHIPPING_ADDRESS_POSTALCODE, 
                "20,24",
                "15,12", 
                Globals.BILLING_ADDRESS_COUNTRY, 
                Globals.BILLING_ADDRESS_REGION, 
                Globals.BILLING_ADDRESS_POSTALCODE, 
                "21,04", 
                "16,02");
        }

        public void AddManufacturers()
        {
            JQueryWait(); // for Azure
            Driver.Navigate().GoToUrl(Globals.BASE_PATH + "/DesktopModules/Hotcakes/Core/Admin/people/manufacturers.aspx");
            Thread.Sleep(5000);
            //JQueryWait();
            Driver.FindElement(By.Id("NavContent_btnNew")).Click();
            Thread.Sleep(3000);
            //JQueryWait();
            Driver.FindElement(By.Id("MainContent_DisplayNameField")).Clear();
            Thread.Sleep(1000);
            //JQueryWait();
            Driver.FindElement(By.Id("MainContent_DisplayNameField")).SendKeys(Variable1 + " manufacturer1");
            Driver.FindElement(By.Id("MainContent_EmailField")).Clear();
            Driver.FindElement(By.Id("MainContent_EmailField")).SendKeys("manufacturer1@manufacturer1.com");
            Driver.FindElement(By.Id("MainContent_btnSaveChanges")).Click();
            Thread.Sleep(1000);
            Driver.FindElement(By.Id("NavContent_btnNew")).Click();
            Driver.FindElement(By.Id("MainContent_DisplayNameField")).Clear();
            Driver.FindElement(By.Id("MainContent_DisplayNameField")).SendKeys(Variable2 + " manufacturer2");
            Driver.FindElement(By.Id("MainContent_EmailField")).Clear();
            Driver.FindElement(By.Id("MainContent_EmailField")).SendKeys("manufacturer2@manufacturer2.com");
            Driver.FindElement(By.Id("MainContent_btnSaveChanges")).Click();
            Thread.Sleep(1000);
            Driver.FindElement(By.Id("NavContent_btnNew")).Click();
            Driver.FindElement(By.Id("MainContent_DisplayNameField")).Clear();
            Driver.FindElement(By.Id("MainContent_DisplayNameField")).SendKeys(Variable3 + " manufacturer3");
            Driver.FindElement(By.Id("MainContent_EmailField")).Clear();
            Driver.FindElement(By.Id("MainContent_EmailField")).SendKeys("manufacturer3@manufacturer3.com");
            Driver.FindElement(By.Id("MainContent_btnSaveChanges")).Click();
        }

        public void AddVendors()
        {
            JQueryWait(); // for Azure
            Driver.Navigate().GoToUrl(Globals.BASE_PATH + "/DesktopModules/Hotcakes/Core/Admin/people/vendors.aspx");
            Thread.Sleep(5000);
            Driver.FindElement(By.Id("NavContent_btnNew")).Click();
            Thread.Sleep(3000);
            Driver.FindElement(By.Id("MainContent_DisplayNameField")).Clear();
            Driver.FindElement(By.Id("MainContent_DisplayNameField")).SendKeys(Variable1 + " vendor1");
            Driver.FindElement(By.Id("MainContent_EmailField")).Clear();
            Driver.FindElement(By.Id("MainContent_EmailField")).SendKeys("vendor1@vendor1com");
            Thread.Sleep(3000);
            Driver.FindElement(By.Id("MainContent_btnSaveChanges")).Click();
            Thread.Sleep(3000);
            Driver.FindElement(By.Id("NavContent_btnNew")).Click();
            Thread.Sleep(3000);
            Driver.FindElement(By.Id("MainContent_DisplayNameField")).Clear();
            Driver.FindElement(By.Id("MainContent_DisplayNameField")).SendKeys(Variable2 + " vendor2");
            Driver.FindElement(By.Id("MainContent_EmailField")).Clear();
            Driver.FindElement(By.Id("MainContent_EmailField")).SendKeys("vendor2@vendor2.com");
            Thread.Sleep(3000);
            Driver.FindElement(By.Id("MainContent_btnSaveChanges")).Click();
            Thread.Sleep(3000);
            Driver.FindElement(By.Id("NavContent_btnNew")).Click();
            Thread.Sleep(3000);
            Driver.FindElement(By.Id("MainContent_DisplayNameField")).Clear();
            Driver.FindElement(By.Id("MainContent_DisplayNameField")).SendKeys(Variable3 + " vendor3");
            Driver.FindElement(By.Id("MainContent_EmailField")).Clear();
            Driver.FindElement(By.Id("MainContent_EmailField")).SendKeys("vendor3@vendor.3com");
            Thread.Sleep(3000);
            Driver.FindElement(By.Id("MainContent_btnSaveChanges")).Click();
        }

        public void AddShippihgMethodFlatRatePerOrder12_34()
        {
            JQueryWait(); // for Azure
            Driver.Navigate()
                .GoToUrl(Globals.BASE_PATH + "/DesktopModules/Hotcakes/Core/Admin/configuration/Shipping_Methods.aspx");
            Thread.Sleep(5000);
            new SelectElement(Driver.FindElement(By.Id("MainContent_lstProviders"))).SelectByText("Flat Rate Per Order");
            Driver.FindElement(By.Id("MainContent_lnkAddNew")).Click();
            Driver.FindElement(By.Id("MainContent_ctl00_NameField")).Clear();
            Driver.FindElement(By.Id("MainContent_ctl00_NameField")).SendKeys(DeliveryOptionPerOrder);
            new SelectElement(Driver.FindElement(By.Id("MainContent_ctl00_lstHighlights"))).SelectByText("Lime");
            Driver.FindElement(By.Id("MainContent_ctl00_AmountField")).Clear();
            Driver.FindElement(By.Id("MainContent_ctl00_AmountField")).SendKeys("12,34");
            JQueryWait();
            Thread.Sleep(1000);
            new SelectElement(Driver.FindElement(By.Id("MainContent_ctl00_lstZones"))).SelectByText(
                "United States - All");
            Driver.FindElement(By.Id("MainContent_ctl00_btnSave")).Click();
        }

        public void AddShippihgMethodFlatRatePerItem21_43()
        {
            Thread.Sleep(5000);
            JQueryWait(); // for Azure
            Driver.Navigate()
                .GoToUrl(Globals.BASE_PATH + "/DesktopModules/Hotcakes/Core/Admin/configuration/Shipping_Methods.aspx");
            new SelectElement(Driver.FindElement(By.Id("MainContent_lstProviders"))).SelectByText("Flat Rate Per Item");
            Driver.FindElement(By.Id("MainContent_lnkAddNew")).Click();
            Driver.FindElement(By.Id("MainContent_ctl00_NameField")).Clear();
            Driver.FindElement(By.Id("MainContent_ctl00_NameField")).SendKeys(DeliveryOptionPerItem);
            new SelectElement(Driver.FindElement(By.Id("MainContent_ctl00_lstHighlights"))).SelectByText("Tan");
            Driver.FindElement(By.Id("MainContent_ctl00_AmountField")).Clear();
            Driver.FindElement(By.Id("MainContent_ctl00_AmountField")).SendKeys("21,43");
            JQueryWait();
            Thread.Sleep(1000);
            new SelectElement(Driver.FindElement(By.Id("MainContent_ctl00_lstZones"))).SelectByText(
                "United States - All");
            Driver.FindElement(By.Id("MainContent_ctl00_btnSave")).Click();
        }

        public void Add3ProductsToCartAndPlaceOrderNo0010()
        {
            JQueryWait(); // for Azure
            Driver.Navigate()
                .GoToUrl(Globals.BASE_PATH + "/HotcakesStore/Product-Viewer/slug/" + Variable1 +
                         "-TAX-01-Name-Shipping-Charges_Charge-Shipping-And-Handling"); // for not localhost.hotcakes
            JQueryWait();
            Thread.Sleep(1000);
            Driver.FindElement(By.Id("addtocartbutton")).Click();
            JQueryWait();

            Driver.Navigate()
                .GoToUrl(Globals.BASE_PATH + "/HotcakesStore/Product-Viewer/slug/" + Variable2 +
                         "-TAX-02-Name-Shipping-Charges_Charge-Shipping-And-Handling"); // for not localhost.hotcakes
            JQueryWait();
            Driver.FindElement(By.Id("addtocartbutton")).Click();
            JQueryWait();
            Driver.Navigate()
                .GoToUrl(Globals.BASE_PATH + "/HotcakesStore/Product-Viewer/slug/" + Variable3 +
                         "-TAX-03-Name-Shipping-Charges_Charge-Shipping-And-Handling"); // for not localhost.hotcakes
            JQueryWait();
            Driver.FindElement(By.Id("addtocartbutton")).Click();
            JQueryWait();
            Driver.FindElement(By.XPath("(//input[@value='Secure Checkout'])[1]")).Click();
            Refresh();
            JQueryWait();
            ShippingAddressCredentialPopulate();
            SelectDeliveryOptionPerOrder();
            //---Delivery Options---
            CreditCardCredentialPopulate(Globals.CREDIT_CARD_DINERS); //Diners Club
            JQueryWait();
            Thread.Sleep(1000);
            Driver.FindElement(By.XPath("//DIV[@class='span6 hc-site-terms']//IMG")).Click();
            JQueryWait();
            Thread.Sleep(1000);
            Driver.FindElement(By.Id("hcTakeOrder")).Click();
            JQueryWait();
        }

        public void OrderCheck3Products0010()
        {
            Refresh();
            JQueryWait();
            WhetherTheTextPresent("6.442,22"); //Subtotal
            WhetherTheTextPresent("104,57"); //Shipping/Handling
            WhetherTheTextPresent("5.773,31"); //Subtotal Before VAT
            WhetherTheTextPresent("92,07"); //Shipping/Handling Before VAT
            WhetherTheTextPresent("5.865,38"); //Total Before VAT
            WhetherTheTextPresent("681,41"); //VAT
            WhetherTheTextPresent("6.546,79"); //Grand Total
        }

        public void RegisterUserPolo()
        {
            UserPoloLoginHcc();

            Driver.Navigate().GoToUrl(Globals.BASE_PATH + "/");
            //JQueryWait();

            Driver.FindElement(By.Id("hcc_dnnUser_enhancedRegisterLink")).Click();
            JQueryWait();
            Driver.Navigate()
                .GoToUrl(Globals.BASE_PATH + "/Home/tabid/55/ctl/register/language/en-US/Default.aspx?returnurl=/&popUp=true");
            Driver.FindElement(By.Id("hcc_ctr_Register_userForm_Username_Username_TextBox")).Clear();
            Driver.FindElement(By.Id("hcc_ctr_Register_userForm_Username_Username_TextBox")).SendKeys("polo");
            Driver.FindElement(By.Id("hcc_ctr_Register_userForm_Password_Password_TextBox")).Clear();
            Driver.FindElement(By.Id("hcc_ctr_Register_userForm_Password_Password_TextBox")).SendKeys("Password1");
            Driver.FindElement(By.Id("hcc_ctr_Register_userForm_PasswordConfirm_PasswordConfirm_TextBox")).Clear();
            Driver.FindElement(By.Id("hcc_ctr_Register_userForm_PasswordConfirm_PasswordConfirm_TextBox"))
                .SendKeys("Password1");
            Driver.FindElement(By.Id("hcc_ctr_Register_userForm_DisplayName_DisplayName_TextBox")).Clear();
            Driver.FindElement(By.Id("hcc_ctr_Register_userForm_DisplayName_DisplayName_TextBox")).SendKeys("polo");
            Driver.FindElement(By.Id("hcc_ctr_Register_userForm_Email_Email_TextBox")).Clear();
            Driver.FindElement(By.Id("hcc_ctr_Register_userForm_Email_Email_TextBox")).SendKeys("polo@polo.com");
            Driver.FindElement(By.Id("hcc_ctr_Register_registerButton")).Click();
            JQueryWait();
            Driver.FindElement(By.Id("hcc_dnnLogin_enhancedLoginLink")).Click();
            JQueryWait();
        }

        public void Buy3GiftCardsNo0020()
        {
            JQueryWait();
            Driver.Navigate()
                .GoToUrl(Globals.BASE_PATH + "/HotcakesStore/Product-Viewer/slug/" + Variablegeneral + "GiftCard-Name");
            JQueryWait();
            Driver.FindElement(By.Id("qty")).Clear();
            Driver.FindElement(By.Id("qty")).SendKeys("3");
            JQueryWait();
            new SelectElement(Driver.FindElement(By.Id("giftcardpredefined"))).SelectByText("500,00 €");
            Driver.FindElement(By.Id("GiftCardRecEmail")).SendKeys("Recipient@email.com");
            Driver.FindElement(By.Id("giftcardrecname")).SendKeys("Recipient Name");
            Driver.FindElement(By.Id("giftcardmessage")).SendKeys("Message for Recipient");
            Driver.FindElement(By.Id("addtocartbutton")).Click(); //Add to Cart
            JQueryWait();
            Driver.FindElement(By.XPath("(//input[@value='Secure Checkout'])[1]")).Click(); //Add to Order
            JQueryWait();
            Refresh();
            BillingAddressCredentialPopulate();
            JQueryWait();
            Refresh();
            Assert.AreEqual("Grand Total:", Driver.FindElement(By.CssSelector("td.totalgrandlabel")).Text);
            CreditCardCredentialPopulate("38520000023237"); //Diners Club
            Driver.FindElement(By.XPath("//DIV[@class='span6 hc-site-terms']//IMG")).Click();
            new SelectElement(Driver.FindElement(By.Id("billingstate"))).SelectByText(Globals.SHIPPING_ADDRESS_REGION);
            JQueryWait();
            Assert.AreEqual("Grand Total:", Driver.FindElement(By.CssSelector("td.totalgrandlabel")).Text);
            JQueryWait();
            new SelectElement(Driver.FindElement(By.Id("billingstate"))).SelectByText(Globals.BILLING_ADDRESS_REGION);
            JQueryWait();
            Assert.AreEqual("Grand Total:", Driver.FindElement(By.CssSelector("td.totalgrandlabel")).Text);
            Driver.FindElement(By.Id("hcTakeOrder")).Click();
            JQueryWait();
            Refresh();
        }

        public void OrderCheck3GiftCards0020()
        {
            Refresh();
            JQueryWait();
            WhetherTheTextPresent("1.386,36"); //Subtotal
            WhetherTheTextPresent("64,79"); //Shipping/Handling
            WhetherTheTextPresent("1.248,75"); //Subtotal Before VAT
            WhetherTheTextPresent("57,33"); //Shipping/Handling Before VAT
            WhetherTheTextPresent("1.306,08"); //Total Before VAT
            WhetherTheTextPresent("145,07"); //VAT
            WhetherTheTextPresent("1.451,15"); //Grand Total
        }

        public void Add1ProductToCartAndPlaceOrderWithPreviousAddressNo0034()
        {
            JQueryWait();
            Thread.Sleep(7000);
            Driver.Navigate()
                .GoToUrl(Globals.BASE_PATH + "/HotcakesStore/Product-Viewer/slug/" + Variablegeneral +
                         "-Sale-Promotion-Product-Name-Shipping-Charges_Charge-Shipping-And-Handling");
                // for not localhost.hotcakes
            Driver.FindElement(By.Id("addtocartbutton")).Click();
            JQueryWait();
            Driver.FindElement(By.XPath("(//input[@value='Secure Checkout'])[1]")).Click();
            JQueryWait();
            Refresh();
            JQueryWait();
            new SelectElement(Driver.FindElement(By.Id("shippingAvailableAddresses"))).SelectByText(Globals.SHIPPING_ADDRESS_STREET1);
            JQueryWait();
            JQueryWait();
            // Uncheck "My billing address is the same as my shipping address"
            Driver.FindElement(By.CssSelector(".hc-billing-section .dnnBoxLabel")).ScrollWindowAndClick(Driver);

            new SelectElement(Driver.FindElement(By.Id("billingAvailableAddresses"))).SelectByText(Globals.BILLING_ADDRESS_STREET1);
            CreditCardCredentialPopulate(Globals.CREDIT_CARD_DISCOVER); //Discover
            Driver.FindElement(By.XPath("//DIV[@class='span6 hc-site-terms']//IMG")).Click();
            SelectDeliveryOptionPerOrder();
            JQueryWait();
            Driver.FindElement(By.Id("hcTakeOrder")).Click();
        }

        public void OrderCheck1ProductAndPlaceOrderWithPreviousAddressNo0034()
        {
            JQueryWait();
            Refresh();
            JQueryWait();
            WhetherTheTextPresent("4.256,84"); //Subtotal
            WhetherTheTextPresent("48,08"); //Shipping/Handling
            WhetherTheTextPresent("3.789,58"); //Subtotal Before VAT
            WhetherTheTextPresent("41,33"); //Shipping/Handling Before VAT
            WhetherTheTextPresent("3.830,91"); //Total Before VAT
            WhetherTheTextPresent("474,01"); //VAT
            WhetherTheTextPresent("4.304,92"); //Grand Total
        }

        public void Catch2GiftCardNumbers()
        {
            Driver.Navigate().GoToUrl(Globals.BASE_PATH + "/DesktopModules/Hotcakes/Core/Admin/catalog/GiftCards.aspx");
            Driver.FindElement(By.LinkText("Gift Cards")).Click();
            JQueryWait();
            new SelectElement(Driver.FindElement(By.Id("MainContent_ucAmountComare_ddlCompareType"))).SelectByText(
                "Less <");
            Driver.FindElement(By.Id("MainContent_ucAmountComare_txtValue")).Clear();
            Driver.FindElement(By.Id("MainContent_ucAmountComare_txtValue")).SendKeys("462,13");
            JQueryWait();
            new SelectElement(Driver.FindElement(By.Id("MainContent_ucBalanceComare_ddlCompareType"))).SelectByText(
                "Greater >");
            JQueryWait();
            Driver.FindElement(By.Id("MainContent_ucBalanceComare_txtValue")).Clear();
            Driver.FindElement(By.Id("MainContent_ucBalanceComare_txtValue")).SendKeys("462,11");
            JQueryWait();
            Driver.FindElement(By.Id("MainContent_btnFind")).Click();
            JQueryWait();
            _giftCardNumber01 =
                Driver.FindElement(By.XPath("//table[@id='MainContent_gvGiftCards']/tbody/tr[2]/td[3]")).Text;
            _giftCardNumber02 =
                Driver.FindElement(By.XPath("//table[@id='MainContent_gvGiftCards']/tbody/tr[3]/td[3]")).Text;
            JQueryWait();
        }

        public void AddToCart3ProductsAndPlaceOrderWith2GiftCardsNo0030()
        {
            JQueryWait();
            Driver.Navigate()
                .GoToUrl(Globals.BASE_PATH + "/HotcakesStore/Product-Viewer/slug/" + Variable1 +
                         "-TAX-01-Name-Shipping-Charges_Charge-Shipping-And-Handling"); // for not localhost.hotcakes
            JQueryWait();
            Driver.FindElement(By.Id("addtocartbutton")).Click();
            JQueryWait();

            Driver.Navigate()
                .GoToUrl(Globals.BASE_PATH + "/HotcakesStore/Product-Viewer/slug/" + Variable2 +
                         "-TAX-02-Name-Shipping-Charges_Charge-Shipping-And-Handling"); // for not localhost.hotcakes
            JQueryWait();
            Driver.FindElement(By.Id("addtocartbutton")).Click();
            JQueryWait();
            Driver.Navigate()
                .GoToUrl(Globals.BASE_PATH + "/HotcakesStore/Product-Viewer/slug/" + Variable3 +
                         "-TAX-03-Name-Shipping-Charges_Charge-Shipping-And-Handling"); // for not localhost.hotcakes
            JQueryWait();
            Driver.FindElement(By.Id("addtocartbutton")).Click();
            JQueryWait();
            Driver.FindElement(By.XPath("(//input[@value='Secure Checkout'])[1]")).Click();
            JQueryWait();
            Refresh();
            JQueryWait();
            ShippingAddressCredentialPopulate();
            JQueryWait();
            Driver.FindElement(By.CssSelector(".hc-billing-section .dnnBoxLabel")).ScrollWindowAndClick(Driver);
            JQueryWait();
            BillingAddressCredentialPopulate();
            // Add Gift Cards to Order. Start
            Driver.FindElement(By.Id("hcGiftCardNumber")).Clear();
            Driver.FindElement(By.Id("hcGiftCardNumber")).SendKeys(_giftCardNumber01);
            Driver.FindElement(By.Id("hcGiftCardButton")).Click();
            JQueryWait();
            Assert.AreEqual("Grand Total:", Driver.FindElement(By.CssSelector("td.totalgrandlabel")).Text);
            Driver.FindElement(By.Id("hcGiftCardNumber")).Clear();
            Driver.FindElement(By.Id("hcGiftCardNumber")).SendKeys(_giftCardNumber02);
            Driver.FindElement(By.Id("hcGiftCardButton")).Click();
            // Add Gift Cards to Order. Finish
            CreditCardCredentialPopulate(Globals.CREDIT_CARD_JCB2); //JCB
            Driver.FindElement(By.XPath("//DIV[@class='span6 hc-site-terms']//IMG")).Click();
            SelectDeliveryOptionPerOrder();
            Driver.FindElement(By.Id("hcTakeOrder")).Click();
            JQueryWait();
        }

        public void OrderCheckWith3ProductsAndUse2GiftCardsForPayment0030()
        {
            Refresh();
            JQueryWait();
            WhetherTheTextPresent("6.442,22"); //Subtotal
            WhetherTheTextPresent("104,57"); //Shipping/Handling
            WhetherTheTextPresent("5.773,31"); //Subtotal Before VAT
            WhetherTheTextPresent("92,07"); //Shipping/Handling Before VAT
            WhetherTheTextPresent("5.865,38"); //Total Before VAT
            WhetherTheTextPresent("681,41"); //VAT
            WhetherTheTextPresent("6.546,79"); //Grand Total
            WhetherTheTextPresent("924,24"); //Gift Cards
            WhetherTheTextPresent("5.622,55"); //Total Due
        }

        public void ShippingAddressCredentialPopulate()
        {
            Refresh();
            JQueryWait();
            Thread.Sleep(1000);
            new SelectElement(Driver.FindElement(By.Id("shippingcountry"))).SelectByText(Globals.SHIPPING_ADDRESS_COUNTRY);
            Driver.FindElement(By.Id("shippingfirstname")).Clear();
            Driver.FindElement(By.Id("shippingfirstname")).SendKeys("Shipping " + Variablegeneral + " First Name");
            Driver.FindElement(By.Id("shippinglastname")).Clear();
            Driver.FindElement(By.Id("shippinglastname")).SendKeys("Shipping " + Variablegeneral + " Last Name");
            Driver.FindElement(By.Id("shippingcompany")).Clear();
            Driver.FindElement(By.Id("shippingcompany")).SendKeys("Shipping " + Variablegeneral + " Company name");
            Driver.FindElement(By.Id("shippingaddress")).Clear();
            Driver.FindElement(By.Id("shippingaddress")).SendKeys(Globals.SHIPPING_ADDRESS_STREET1);
            Driver.FindElement(By.Id("shippingphone")).Clear();
            Driver.FindElement(By.Id("shippingphone")).SendKeys("Shipping " + Variablegeneral + " " + Globals.SHIPPING_ADDRESS_PHONE);
            Driver.FindElement(By.Id("shippingcity")).Clear();
            Driver.FindElement(By.Id("shippingcity")).SendKeys(Globals.SHIPPING_ADDRESS_CITY);
            Driver.FindElement(By.Id("shippingzip")).Clear();
            Driver.FindElement(By.Id("shippingzip")).SendKeys(Globals.SHIPPING_ADDRESS_POSTALCODE);
            new SelectElement(Driver.FindElement(By.Id("shippingstate"))).SelectByText(Globals.SHIPPING_ADDRESS_REGION);
            new SelectElement(Driver.FindElement(By.Id("shippingstate"))).SelectByText(Globals.BILLING_ADDRESS_REGION);
            new SelectElement(Driver.FindElement(By.Id("shippingstate"))).SelectByText(Globals.SHIPPING_ADDRESS_REGION);
        }

        public void BillingAddressCredentialPopulate()
        {
            JQueryWait();
            JQueryWait();
            new SelectElement(Driver.FindElement(By.Id("billingcountry"))).SelectByText(Globals.BILLING_ADDRESS_COUNTRY);
                //must be billing County
            JQueryWait();
            Driver.FindElement(By.Id("billingfirstname")).Clear();
            Driver.FindElement(By.Id("billingfirstname")).SendKeys("Billing " + Variablegeneral + " First Name");
            Driver.FindElement(By.Id("billinglastname")).Clear();
            Driver.FindElement(By.Id("billinglastname")).SendKeys("Billing " + Variablegeneral + " Last Name");
            Driver.FindElement(By.Id("billingcompany")).Clear();
            Driver.FindElement(By.Id("billingcompany")).SendKeys("Billing " + Variablegeneral + " Company name");
            Driver.FindElement(By.Id("billingaddress")).Clear();
            Driver.FindElement(By.Id("billingaddress")).SendKeys(Globals.BILLING_ADDRESS_STREET1);
            Driver.FindElement(By.Id("billingcity")).Clear();
            Driver.FindElement(By.Id("billingcity")).SendKeys(Globals.BILLING_ADDRESS_CITY);
            Driver.FindElement(By.Id("billingzip")).Clear();
            Driver.FindElement(By.Id("billingzip")).SendKeys(Globals.BILLING_ADDRESS_POSTALCODE);
            Driver.FindElement(By.Id("billingphone")).Clear();
            Driver.FindElement(By.Id("billingphone")).SendKeys("Billing " + Variablegeneral + " " + Globals.BILLING_ADDRESS_PHONE);
            new SelectElement(Driver.FindElement(By.Id("billingstate"))).SelectByText(Globals.BILLING_ADDRESS_REGION);
            new SelectElement(Driver.FindElement(By.Id("billingstate"))).SelectByText(Globals.SHIPPING_ADDRESS_REGION);
            new SelectElement(Driver.FindElement(By.Id("billingstate"))).SelectByText(Globals.BILLING_ADDRESS_REGION);
        }

        public void CreditCardCredentialPopulate(string creditCardNumber)
        {
            JQueryWait();
            Driver.FindElement(By.Id("cccardnumber")).Clear();
            Driver.FindElement(By.Id("cccardnumber")).SendKeys(creditCardNumber);
            new SelectElement(Driver.FindElement(By.Name("ccexpmonth"))).SelectByText("12");
            new SelectElement(Driver.FindElement(By.Name("ccexpyear"))).SelectByText("2024");
            Driver.FindElement(By.Name("ccsecuritycode")).Clear();
            Driver.FindElement(By.Name("ccsecuritycode")).SendKeys("123");
            Driver.FindElement(By.Name("cccardholder")).Clear();
            Driver.FindElement(By.Name("cccardholder")).SendKeys("CardName Holder");
        }

        public void SelectDeliveryOptionPerOrder()
        {
            var xpath = string.Format("//LABEL[contains(., '{0}')]", DeliveryOptionPerOrder);
            JQueryWait();
            Driver.FindElement(By.XPath(xpath)).ScrollWindowAndClick(Driver);
            Thread.Sleep(5000);
        }

        public void SelectDeliveryOptionPerItem()
        {
            var xpath = string.Format("//LABEL[contains(., '{0}')]", DeliveryOptionPerItem);
            JQueryWait();
            Driver.FindElement(By.XPath(xpath)).ScrollWindowAndClick(Driver);
            Thread.Sleep(5000);
        }

        public void AddPromotionSale()
        {
            JQueryWait(); // for Azure
            Driver.Navigate().GoToUrl(Globals.BASE_PATH + "/DesktopModules/Hotcakes/Core/Admin/marketing/promotions.aspx");
            Thread.Sleep(10000);
            JQueryWait();
            Driver.FindElement(By.Id("ctl00_NavContent_lstNewType_Arrow")).Click();
            JQueryWait();
            Driver.FindElement(By.XPath("//li[text()='Custom Sale']")).Click();
            JQueryWait();
            Driver.FindElement(By.Id("NavContent_btnNew")).Click();

            Thread.Sleep(5000);
            Driver.FindElement(By.Id("MainContent_chkEnabled")).Click();
            Driver.FindElement(By.Id("MainContent_txtName")).Clear();
            Driver.FindElement(By.Id("MainContent_txtName"))
                .SendKeys("{Promotion Sale." + Variablegeneral + " Promotion Name. Qualifications: Product is:" + "." +
                          Variablegeneral + "-Sale-Promotion-Product-Name-Shipping-Charges_Charge-Shipping-And-Handling" +
                          ". Actions: Decrease Product Price by 7,89}");
            Driver.FindElement(By.Id("MainContent_txtCustomerDescription")).Clear();
            Driver.FindElement(By.Id("MainContent_txtCustomerDescription"))
                .SendKeys("{Promotion Sale." + Variablegeneral + " Promotion Name. Qualifications: Product is:" + "." +
                          Variablegeneral + "-Sale-Promotion-Product-Name-Shipping-Charges_Charge-Shipping-And-Handling" +
                          ". Actions: Decrease Product Price by 7,89}");
            Driver.FindElement(By.Id("ctl00_MainContent_lstNewQualification_Arrow")).Click();
            JQueryWait();
            Driver.FindElement(By.XPath("//li[text()='Product Is...']")).Click();
            JQueryWait();
            Driver.FindElement(By.Id("MainContent_btnNewQualification")).Click();
            JQueryWait();
            Driver.FindElement(
                By.Id("MainContent_Promotions_Edit_Qualification1_ucProductBvinEditor_ucProductPicker_FilterField"))
                .Clear();
            Driver.FindElement(
                By.Id("MainContent_Promotions_Edit_Qualification1_ucProductBvinEditor_ucProductPicker_FilterField"))
                .SendKeys("." + Variablegeneral +
                          "-Sale-Promotion-Product-Name-Shipping-Charges_Charge-Shipping-And-Handling");
            JQueryWait();
            Thread.Sleep(2000);
            Driver.FindElement(
                By.Id("MainContent_Promotions_Edit_Qualification1_ucProductBvinEditor_ucProductPicker_btnGo")).Click();
            JQueryWait();
            Driver.FindElement(
                By.Id(
                    "MainContent_Promotions_Edit_Qualification1_ucProductBvinEditor_ucProductPicker_GridView1_chkSelected_0"))
                .Click();
            JQueryWait();
            Driver.FindElement(By.Id("MainContent_Promotions_Edit_Qualification1_ucProductBvinEditor_btnAddProduct"))
                .Click();
            JQueryWait();
            Driver.FindElement(By.Id("MainContent_btnSaveQualification")).Click();
            JQueryWait();
            Driver.FindElement(By.Id("ctl00_MainContent_lstNewAction_Arrow")).Click();
            JQueryWait();
            Driver.FindElement(By.XPath("//li[text()='Discount Product Price']")).Click();
            JQueryWait();
            Driver.FindElement(By.Id("MainContent_btnNewAction")).Click();
            JQueryWait();
            Driver.FindElement(By.Id("MainContent_Promotions_Edit_Actions1_ucActionAdjustProductPrice_AmountField"))
                .Clear();
            Driver.FindElement(By.Id("MainContent_Promotions_Edit_Actions1_ucActionAdjustProductPrice_AmountField"))
                .SendKeys("-7,89");
            Driver.FindElement(By.Id("MainContent_btnSaveAction")).Click();
            JQueryWait();
            Driver.FindElement(By.Id("MainContent_btnSave")).Click();
        }

        public void AddPromotionOfferForOrderItemsWithCouponEqualVariable1()
        {
            JQueryWait(); // for Azure
            Driver.Navigate().GoToUrl(Globals.BASE_PATH + "/DesktopModules/Hotcakes/Core/Admin/marketing/promotions.aspx");
            JQueryWait();
            Driver.FindElement(By.Id("ctl00_NavContent_lstNewType_Arrow")).Click();
            JQueryWait();
            Driver.FindElement(By.XPath("//li[text()='Custom Offer for Items']")).Click();
            JQueryWait();
            Driver.FindElement(By.Id("NavContent_btnNew")).Click();
            Thread.Sleep(5000);
            Driver.FindElement(By.Id("MainContent_chkEnabled")).Click();
            Driver.FindElement(By.Id("MainContent_txtName")).Clear();
            Driver.FindElement(By.Id("MainContent_txtName"))
                .SendKeys("{Promotion Offer For Order Items. " + Variable1 +
                          " Promotion Name. Qualifications: When Order Has Any of These Coupon Codes: " + Variable1 +
                          " Actions: Decrease Qualifying Item Price by 6,54}");
            Driver.FindElement(By.Id("MainContent_txtCustomerDescription")).Clear();
            Driver.FindElement(By.Id("MainContent_txtCustomerDescription"))
                .SendKeys("{Promotion Offer For Order Items. " + Variable1 +
                          " Customer Description. Qualifications: When Order Has Any of These Coupon Codes: " +
                          Variable1 + " Actions: Decrease Qualifying Item Price by 6,54}");
            Driver.FindElement(By.Id("ctl00_MainContent_lstNewQualification_Arrow")).Click();
            JQueryWait();
            Driver.FindElement(By.XPath("//li[text()='Order Has Coupon Code...']")).Click();
            Driver.FindElement(By.Id("MainContent_btnNewQualification")).Click();
            JQueryWait();
            Driver.FindElement(
                By.Id("MainContent_Promotions_Edit_Qualification1_ucOrderHasCouponEditor_OrderCouponField")).Clear();
            Driver.FindElement(
                By.Id("MainContent_Promotions_Edit_Qualification1_ucOrderHasCouponEditor_OrderCouponField"))
                .SendKeys(Variable1);
            Driver.FindElement(By.LinkText("Add")).Click();
            JQueryWait();
            Driver.FindElement(By.Id("MainContent_btnSaveQualification")).Click();
            JQueryWait();
            Driver.FindElement(By.Id("ctl00_MainContent_lstNewAction_Arrow")).Click();
            JQueryWait();
            Driver.FindElement(By.XPath("//li[text()='Adjust Qualifying Items']")).Click();
            JQueryWait();
            Driver.FindElement(By.Id("MainContent_btnNewAction")).Click();
            JQueryWait();
            Driver.FindElement(
                By.Id("MainContent_Promotions_Edit_Actions1_ucActionAdjustLineItem_LineItemAdjustAmountField")).Clear();
            Driver.FindElement(
                By.Id("MainContent_Promotions_Edit_Actions1_ucActionAdjustLineItem_LineItemAdjustAmountField"))
                .SendKeys("-6,54");
            Driver.FindElement(By.Id("MainContent_btnSaveAction")).Click();
            JQueryWait();
            Driver.FindElement(By.Id("MainContent_btnSave")).Click();
        }

        public void AddPromotionOfferForOrderSubTotalWithCouponEqualVariable2()
        {
            JQueryWait(); // for Azure
            Driver.Navigate().GoToUrl(Globals.BASE_PATH + "/DesktopModules/Hotcakes/Core/Admin/marketing/promotions.aspx");
            JQueryWait();
            Driver.FindElement(By.Id("ctl00_NavContent_lstNewType_Arrow")).Click();
            JQueryWait();
            Driver.FindElement(By.XPath("//li[text()='Custom Offer for Subtotal']")).Click();
            JQueryWait();
            Driver.FindElement(By.Id("NavContent_btnNew")).Click();

            Thread.Sleep(5000);
            Driver.FindElement(By.Id("MainContent_chkEnabled")).Click();
            Driver.FindElement(By.Id("MainContent_txtName")).Clear();
            Driver.FindElement(By.Id("MainContent_txtName"))
                .SendKeys("{Promotion Offer For Order Sub Total. " + Variable2 +
                          " Promotion Name. Qualifications: When Order Has Any of These Coupon Codes: " + Variable2 +
                          " Actions: Decrease Order Total by 5,43}");
            Driver.FindElement(By.Id("MainContent_txtCustomerDescription")).Clear();
            Driver.FindElement(By.Id("MainContent_txtCustomerDescription"))
                .SendKeys("{Promotion Offer For Order Sub Total. " + Variable2 +
                          " Customer Description. Qualifications: When Order Has Any of These Coupon Codes: " +
                          Variable2 + " Actions: Decrease Order Total by 5,43}");
            Driver.FindElement(By.Id("ctl00_MainContent_lstNewQualification_Arrow")).Click();
            JQueryWait();
            Driver.FindElement(By.XPath("//li[text()='Order Has Coupon Code...']")).Click();
            Driver.FindElement(By.Id("MainContent_btnNewQualification")).Click();
            JQueryWait();
            Driver.FindElement(
                By.Id("MainContent_Promotions_Edit_Qualification1_ucOrderHasCouponEditor_OrderCouponField")).Clear();
            Driver.FindElement(
                By.Id("MainContent_Promotions_Edit_Qualification1_ucOrderHasCouponEditor_OrderCouponField"))
                .SendKeys(Variable2);
            Driver.FindElement(By.LinkText("Add")).Click();
            JQueryWait();
            Driver.FindElement(By.Id("MainContent_btnSaveQualification")).Click();
            JQueryWait();
            Driver.FindElement(By.Id("ctl00_MainContent_lstNewAction_Arrow")).Click();
            JQueryWait();
            Driver.FindElement(By.XPath("//li[text()='Discount Order Total']")).Click();
            JQueryWait();
            Driver.FindElement(By.Id("MainContent_btnNewAction")).Click();
            JQueryWait();
            Driver.FindElement(
                By.Id("MainContent_Promotions_Edit_Actions1_ucActionAdjustOrderTotal_OrderTotalAmountField")).Clear();
            Driver.FindElement(
                By.Id("MainContent_Promotions_Edit_Actions1_ucActionAdjustOrderTotal_OrderTotalAmountField"))
                .SendKeys("-5,43");
            Driver.FindElement(By.Id("MainContent_btnSaveAction")).Click();
            JQueryWait();
            Driver.FindElement(By.Id("MainContent_btnSave")).Click();
        }

        public void AddPromotionOfferForShippingSubTotalWithCouponEqualVariable3()
        {
            JQueryWait(); // for Azure
            Driver.Navigate().GoToUrl(Globals.BASE_PATH + "/DesktopModules/Hotcakes/Core/Admin/marketing/promotions.aspx");
            JQueryWait();
            Driver.FindElement(By.Id("ctl00_NavContent_lstNewType_Arrow")).Click();
            JQueryWait();
            Driver.FindElement(By.XPath("//li[text()='Custom Offer for Shipping']")).Click();
            JQueryWait();
            Driver.FindElement(By.Id("NavContent_btnNew")).Click();

            Thread.Sleep(5000);
            Driver.FindElement(By.Id("MainContent_chkEnabled")).Click();
            Driver.FindElement(By.Id("MainContent_txtName")).Clear();
            Thread.Sleep(2000);
            Driver.FindElement(By.Id("MainContent_txtName"))
                .SendKeys("{Promotion Offer for Shipping. " + Variable3 +
                          " Promotion Name. Qualifications: When Order Has Any of These Coupon Codes: " + Variable3 +
                          " Actions: Decrease Order Shipping by 9,87}");
            Thread.Sleep(2000);
            JQueryWait();
            Driver.FindElement(By.Id("MainContent_txtCustomerDescription")).Clear();
            Thread.Sleep(2000);
            Driver.FindElement(By.Id("MainContent_txtCustomerDescription"))
                .SendKeys("{Promotion Offer for Shipping. " + Variable3 +
                          " Customer Description. Qualifications: When Order Has Any of These Coupon Codes: " +
                          Variable3 + " Actions: Decrease Order Shipping by 9,87}");
            Driver.FindElement(By.Id("ctl00_MainContent_lstNewQualification_Arrow")).Click();
            JQueryWait();
            Driver.FindElement(By.XPath("//li[text()='Order Has Coupon Code...']")).Click();
            Driver.FindElement(By.Id("MainContent_btnNewQualification")).Click();
            JQueryWait();
            Driver.FindElement(
                By.Id("MainContent_Promotions_Edit_Qualification1_ucOrderHasCouponEditor_OrderCouponField")).Clear();
            Driver.FindElement(
                By.Id("MainContent_Promotions_Edit_Qualification1_ucOrderHasCouponEditor_OrderCouponField"))
                .SendKeys(Variable3);
            Driver.FindElement(By.LinkText("Add")).Click();
            JQueryWait();
            Driver.FindElement(By.Id("MainContent_btnSaveQualification")).Click();
            JQueryWait();
            Driver.FindElement(By.Id("ctl00_MainContent_lstNewAction_Arrow")).Click();
            JQueryWait();
            Driver.FindElement(By.XPath("//li[text()='Discount Shipping By...']")).Click();
            Thread.Sleep(2000);
            JQueryWait();
            Driver.FindElement(By.Id("MainContent_btnNewAction")).Click();
            JQueryWait();
            Driver.FindElement(
                By.Id(
                    "MainContent_Promotions_Edit_Actions1_ucActionOrderShippingAdjustment_OrderShippingAdjustmentAmount"))
                .Clear();
            Driver.FindElement(
                By.Id(
                    "MainContent_Promotions_Edit_Actions1_ucActionOrderShippingAdjustment_OrderShippingAdjustmentAmount"))
                .SendKeys("-9,87");
            Thread.Sleep(2000);
            Driver.FindElement(By.Id("MainContent_btnSaveAction")).Click();
            JQueryWait();
            Driver.FindElement(By.Id("MainContent_btnSave")).Click();
        }

        public void AddToCartProductForSaleAndPlaceOrder0040()
        {
            JQueryWait();
            Driver.Navigate()
                .GoToUrl(Globals.BASE_PATH + "/HotcakesStore/Product-Viewer/slug/" + Variablegeneral +
                         "-Sale-Promotion-Product-Name-Shipping-Charges_Charge-Shipping-And-Handling");
                // for not localhost.hotcakes
            Driver.FindElement(By.Id("addtocartbutton")).Click();
            JQueryWait();
            Driver.FindElement(By.XPath("(//input[@value='Secure Checkout'])[1]")).Click();
            JQueryWait();
            Refresh();
            JQueryWait();
            ShippingAddressCredentialPopulate();
            JQueryWait();
            CreditCardCredentialPopulate(Globals.CREDIT_CARD_VISA); //Visa
            Driver.FindElement(By.XPath("//DIV[@class='span6 hc-site-terms']//IMG")).Click();
            SelectDeliveryOptionPerOrder();
            Driver.FindElement(By.Id("hcTakeOrder")).Click();
        }

        public void OrderCheckProductForSale0040()
        {
            JQueryWait();
            Refresh();
            JQueryWait();
            WhetherTheTextPresent("4.256,84"); //Subtotal
            WhetherTheTextPresent("48,08"); //Shipping/Handling
            WhetherTheTextPresent("3.789,58"); //Subtotal Before VAT
            WhetherTheTextPresent("41,33"); //Shipping/Handling Before VAT
            WhetherTheTextPresent("3.830,91"); //Total Before VAT
            WhetherTheTextPresent("474,01"); //VAT
            WhetherTheTextPresent("4.304,92"); //Grand Total
        }

        public void AddToCart4ProductsUse3CouponCodesAndPlaceOrderWithVat0050()
        {
            JQueryWait();
            Driver.Navigate()
                .GoToUrl(Globals.BASE_PATH + "/HotcakesStore/Product-Viewer/slug/" + Variable1 +
                         "-TAX-01-Name-Shipping-Charges_Charge-Shipping-And-Handling"); // for not localhost.hotcakes
            JQueryWait();
            Driver.FindElement(By.Id("addtocartbutton")).Click();
            JQueryWait();
            //driver.Navigate().GoToUrl(baseURL + "/store/product1/" + variable2 + "-TAX-02-Name-Shipping-Charges_Charge-Shipping-And-Handling"); // for localhost.hotcakes
            Driver.Navigate()
                .GoToUrl(Globals.BASE_PATH + "/HotcakesStore/Product-Viewer/slug/" + Variable2 +
                         "-TAX-02-Name-Shipping-Charges_Charge-Shipping-And-Handling"); // for not localhost.hotcakes
            JQueryWait();
            Driver.FindElement(By.Id("addtocartbutton")).Click();
            JQueryWait();
            Driver.Navigate()
                .GoToUrl(Globals.BASE_PATH + "/HotcakesStore/Product-Viewer/slug/" + Variable3 +
                         "-TAX-03-Name-Shipping-Charges_Charge-Shipping-And-Handling"); // for not localhost.hotcakes
            JQueryWait();
            Driver.FindElement(By.Id("addtocartbutton")).Click();
            JQueryWait();
            Driver.Navigate()
                .GoToUrl(Globals.BASE_PATH + "/HotcakesStore/Product-Viewer/slug/" + Variablegeneral +
                         "-Sale-Promotion-Product-Name-Shipping-Charges_Charge-Shipping-And-Handling");
                // for not localhost.hotcakes
            Driver.FindElement(By.Id("addtocartbutton")).Click();

            Driver.FindElement(By.Name("couponcode")).Clear();
            Driver.FindElement(By.Name("couponcode")).SendKeys(Variable1);
            JQueryWait();
            Driver.FindElement(By.CssSelector("div.dnnFormItem > input.dnnSecondaryAction")).Click();
            JQueryWait();
            Driver.FindElement(By.Name("couponcode")).Clear();
            Driver.FindElement(By.Name("couponcode")).SendKeys(Variable2);
            Driver.FindElement(By.CssSelector("div.dnnFormItem > input.dnnSecondaryAction")).Click();
            JQueryWait();
            Driver.FindElement(By.Name("couponcode")).Clear();
            Driver.FindElement(By.Name("couponcode")).SendKeys(Variable3);
            Driver.FindElement(By.CssSelector("div.dnnFormItem > input.dnnSecondaryAction")).Click();
            JQueryWait();
            Driver.FindElement(By.XPath("(//input[@value='Secure Checkout'])[1]")).Click();
            JQueryWait();
            Refresh();
            JQueryWait();
            ShippingAddressCredentialPopulate();
            JQueryWait();
            CreditCardCredentialPopulate(Globals.CREDIT_CARD_AMEX); //American Express
            Driver.FindElement(By.XPath("//DIV[@class='span6 hc-site-terms']//IMG")).Click();
            JQueryWait();
            SelectDeliveryOptionPerOrder();
            JQueryWait();
            Driver.FindElement(By.Id("hcTakeOrder")).Click();
            JQueryWait();
        }

        public void OrderCheck4ProductsAnd3CouponCodesUseWithVat0050()
        {
            Refresh();
            JQueryWait();
            WhetherTheTextPresent("10.675,03"); //Before Discounts
            WhetherTheTextPresent("-5,43"); //Promotion Offer For Order Sub Total
            WhetherTheTextPresent("10.669,60"); //Subtotal
            WhetherTheTextPresent("145,56"); //Shipping/Handling Before Discounts
            WhetherTheTextPresent("-9,87"); //Promotion Offer for Shipping
            WhetherTheTextPresent("131,45"); //Shipping/Handling
            WhetherTheTextPresent("9.541,35"); //Subtotal Before VAT
            WhetherTheTextPresent("114,64"); //Shipping/Handling Before VAT
            WhetherTheTextPresent("9.650,56"); //Total Before VAT
            WhetherTheTextPresent("1.150,49"); //VAT
            WhetherTheTextPresent("10.801,05"); //Grand Total
        }

        public void AddToCart4ProductsWith4DifferentShippingChargesUse3CouponCodesAndPlaceOrder0055()
        {
            Thread.Sleep(4000);
            JQueryWait(); // for Azure
            Driver.Navigate()
                .GoToUrl(Globals.BASE_PATH + "/HotcakesStore/Product-Viewer/slug/" + Variable1 +
                         "-TAX-01-Name-Shipping-Charges_Charge-Shipping-And-Handling"); // for not localhost.hotcakes
            JQueryWait();
            Driver.FindElement(By.Id("addtocartbutton")).Click();
            JQueryWait();
            //driver.Navigate().GoToUrl(baseURL + "/store/product1/" + variable2 + "-TAX-02-Name-Shipping-Charges_Charge-Shipping-And-Handling"); // for localhost.hotcakes
            Driver.Navigate()
                .GoToUrl(Globals.BASE_PATH + "/HotcakesStore/Product-Viewer/slug/" + Variable1 +
                         "-TAX-01-Name-Shipping-Charges_No-Charge"); // for not localhost.hotcakes
            JQueryWait();
            Driver.FindElement(By.Id("addtocartbutton")).Click();
            JQueryWait();
            Driver.Navigate()
                .GoToUrl(Globals.BASE_PATH + "/HotcakesStore/Product-Viewer/slug/" + Variable1 +
                         "-TAX-01-Name-Shipping-Charges_Charge-Shipping-Only"); // for not localhost.hotcakes
            JQueryWait();
            Driver.FindElement(By.Id("addtocartbutton")).Click();
            JQueryWait();
            Driver.Navigate()
                .GoToUrl(Globals.BASE_PATH + "/HotcakesStore/Product-Viewer/slug/" + Variable1 +
                         "-TAX-01-Name-Shipping-Charges_Charge-Handling-Only"); // for not localhost.hotcakes
            Driver.FindElement(By.Id("addtocartbutton")).Click();

            Driver.FindElement(By.Name("couponcode")).Clear();
            Driver.FindElement(By.Name("couponcode")).SendKeys(Variable1);
            JQueryWait();
            Driver.FindElement(By.CssSelector("div.dnnFormItem > input.dnnSecondaryAction")).Click();
            JQueryWait();
            Driver.FindElement(By.Name("couponcode")).Clear();
            Driver.FindElement(By.Name("couponcode")).SendKeys(Variable2);
            Driver.FindElement(By.CssSelector("div.dnnFormItem > input.dnnSecondaryAction")).Click();
            JQueryWait();
            Driver.FindElement(By.Name("couponcode")).Clear();
            Driver.FindElement(By.Name("couponcode")).SendKeys(Variable3);
            Driver.FindElement(By.CssSelector("div.dnnFormItem > input.dnnSecondaryAction")).Click();
            JQueryWait();

            Driver.FindElement(By.XPath("(//input[@value='Secure Checkout'])[1]")).Click();
            JQueryWait();
            Refresh();
            JQueryWait();
            ShippingAddressCredentialPopulate();
            JQueryWait();

            CreditCardCredentialPopulate(Globals.CREDIT_CARD_MC2); //MasterCard
            Driver.FindElement(By.XPath("//DIV[@class='span6 hc-site-terms']//IMG")).Click();
            JQueryWait();
            SelectDeliveryOptionPerItem();
            JQueryWait();
            Driver.FindElement(By.Id("hcTakeOrder")).Click();
            JQueryWait();
        }

        public void OrderCheck4ProductsWith4DifferentShippingChargesAnd3CouponCodesUse0055()
        {
            Refresh();
            JQueryWait();
            WhetherTheTextPresent("4.503,16"); //Before Discounts
            WhetherTheTextPresent("-5,43"); //Promotion Offer For Order Sub Total
            WhetherTheTextPresent("4.497,73"); //Subtotal
            WhetherTheTextPresent("97,08"); //Shipping/Handling Before Discounts
            WhetherTheTextPresent("-9,87"); //Promotion Offer for Shipping
            WhetherTheTextPresent("84,97"); //Shipping/Handling
            WhetherTheTextPresent("4.089,32"); //Subtotal Before VAT
            WhetherTheTextPresent("75,77"); //Shipping/Handling Before VAT
            WhetherTheTextPresent("4.159,66"); //Total Before VAT
            WhetherTheTextPresent("423,04"); //VAT
            WhetherTheTextPresent("4.582,70"); //Grand Total
        }

        public void AddToCartProduct4AndPlaceOrder0060()
        {
            JQueryWait();
            Driver.Navigate()
                .GoToUrl(Globals.BASE_PATH + "/HotcakesStore/Product-Viewer/slug/" + Variable4 +
                         "-TAX-04-Name-Shipping-Charges_Charge-Shipping-And-Handling"); // for not localhost.hotcakes
            Driver.FindElement(By.Id("addtocartbutton")).Click();
            JQueryWait();
            Driver.FindElement(By.XPath("(//input[@value='Secure Checkout'])[1]")).Click();
            JQueryWait();
            Refresh();
            JQueryWait();
            ShippingAddressCredentialPopulate();
            JQueryWait();
            CreditCardCredentialPopulate(Globals.CREDIT_CARD_MC); //MasterCard
            Driver.FindElement(By.XPath("//DIV[@class='span6 hc-site-terms']//IMG")).Click();
            SelectDeliveryOptionPerOrder();
            Driver.FindElement(By.Id("hcTakeOrder")).Click();
        }

        public void OrderCheckProduct4TaxScheduler0060()
        {
            Refresh();
            JQueryWait();
            WhetherTheTextPresent("69,00"); //Subtotal
            WhetherTheTextPresent("39,45"); //Shipping/Handling
            WhetherTheTextPresent("57,39"); //Subtotal Before VAT
            WhetherTheTextPresent("34,27"); //Shipping/Handling Before VAT
            WhetherTheTextPresent("91,66"); //Total Before VAT
            WhetherTheTextPresent("16,79"); //VAT
            WhetherTheTextPresent("108,45"); //Grand Total
        }

        public void SwitchOnTax()
        {
            JQueryWait(); // for Azure
            Driver.Navigate().GoToUrl(Globals.BASE_PATH + "/DesktopModules/Hotcakes/Core/Admin/configuration/TaxClasses.aspx");
            JQueryWait();
            UnCheckCheckBox("MainContent_chkApplyVATRules");
            Driver.FindElement(By.Id("MainContent_btnSave")).Click();
        }

        public void AddToCart8ProductsUse3CouponCodesAndPlaceOrderWithTax0070()
        {
            JQueryWait();
            Driver.Navigate()
                .GoToUrl(Globals.BASE_PATH + "/HotcakesStore/Product-Viewer/slug/" + Variable1 +
                         "-TAX-01-Name-Shipping-Charges_Charge-Shipping-And-Handling"); // for not localhost.hotcakes
            JQueryWait();
            Driver.FindElement(By.Id("qty")).Clear();
            Driver.FindElement(By.Id("qty")).SendKeys("1");
            Driver.FindElement(By.Id("addtocartbutton")).Click();
            JQueryWait();

            Driver.Navigate()
                .GoToUrl(Globals.BASE_PATH + "/HotcakesStore/Product-Viewer/slug/" + Variable1 +
                         "-TAX-01-Name-Shipping-Charges_No-Charge");
            JQueryWait();
            Driver.FindElement(By.Id("qty")).Clear();
            Driver.FindElement(By.Id("qty")).SendKeys("2");
            Driver.FindElement(By.Id("addtocartbutton")).Click();
            JQueryWait();

            Driver.Navigate()
                .GoToUrl(Globals.BASE_PATH + "/HotcakesStore/Product-Viewer/slug/" + Variable1 +
                         "-TAX-01-Name-Shipping-Charges_Charge-Shipping-Only");
            JQueryWait();
            Driver.FindElement(By.Id("qty")).Clear();
            Driver.FindElement(By.Id("qty")).SendKeys("3");
            Driver.FindElement(By.Id("addtocartbutton")).Click();
            JQueryWait();

            Driver.Navigate()
                .GoToUrl(Globals.BASE_PATH + "/HotcakesStore/Product-Viewer/slug/" + Variable1 +
                         "-TAX-01-Name-Shipping-Charges_Charge-Handling-Only");
            JQueryWait();
            Driver.FindElement(By.Id("qty")).Clear();
            Driver.FindElement(By.Id("qty")).SendKeys("4");
            Driver.FindElement(By.Id("addtocartbutton")).Click();
            JQueryWait();

            Driver.Navigate()
                .GoToUrl(Globals.BASE_PATH + "/HotcakesStore/Product-Viewer/slug/" + Variable2 +
                         "-TAX-02-Name-Shipping-Charges_Charge-Shipping-And-Handling"); // for not localhost.hotcakes
            JQueryWait();
            Driver.FindElement(By.Id("qty")).Clear();
            Driver.FindElement(By.Id("qty")).SendKeys("5");
            Driver.FindElement(By.Id("addtocartbutton")).Click();
            JQueryWait();

            Driver.Navigate()
                .GoToUrl(Globals.BASE_PATH + "/HotcakesStore/Product-Viewer/slug/" + Variable3 +
                         "-TAX-03-Name-Shipping-Charges_Charge-Shipping-And-Handling"); // for not localhost.hotcakes
            JQueryWait();
            Driver.FindElement(By.Id("qty")).Clear();
            Driver.FindElement(By.Id("qty")).SendKeys("6");
            Driver.FindElement(By.Id("addtocartbutton")).Click();
            JQueryWait();

            Driver.Navigate()
                .GoToUrl(Globals.BASE_PATH + "/HotcakesStore/Product-Viewer/slug/" + Variable4 +
                         "-TAX-04-Name-Shipping-Charges_Charge-Shipping-And-Handling");
            JQueryWait();
            Driver.FindElement(By.Id("qty")).Clear();
            Driver.FindElement(By.Id("qty")).SendKeys("7");
            Driver.FindElement(By.Id("addtocartbutton")).Click();
            JQueryWait();

            Driver.Navigate()
                .GoToUrl(Globals.BASE_PATH + "/HotcakesStore/Product-Viewer/slug/" + Variablegeneral +
                         "-Sale-Promotion-Product-Name-Shipping-Charges_Charge-Shipping-And-Handling");
                // for not localhost.hotcakes
            JQueryWait();
            Driver.FindElement(By.Id("qty")).Clear();
            Driver.FindElement(By.Id("qty")).SendKeys("8");
            Driver.FindElement(By.Id("addtocartbutton")).Click();

            Driver.FindElement(By.Name("couponcode")).Clear();
            Driver.FindElement(By.Name("couponcode")).SendKeys(Variable1);
            JQueryWait();
            Driver.FindElement(By.CssSelector("div.dnnFormItem > input.dnnSecondaryAction")).Click();
            JQueryWait();
            Driver.FindElement(By.Name("couponcode")).Clear();
            Driver.FindElement(By.Name("couponcode")).SendKeys(Variable2);
            Driver.FindElement(By.CssSelector("div.dnnFormItem > input.dnnSecondaryAction")).Click();
            JQueryWait();
            Driver.FindElement(By.Name("couponcode")).Clear();
            Driver.FindElement(By.Name("couponcode")).SendKeys(Variable3);
            Driver.FindElement(By.CssSelector("div.dnnFormItem > input.dnnSecondaryAction")).Click();
            JQueryWait();
            
            Driver.FindElement(By.XPath("(//input[@value='Secure Checkout'])[1]")).Click();
            JQueryWait();
            Refresh();
            JQueryWait();
            ShippingAddressCredentialPopulate();
            JQueryWait();
            
            CreditCardCredentialPopulate(Globals.CREDIT_CARD_VISA2); //Visa
            Driver.FindElement(By.XPath("//DIV[@class='span6 hc-site-terms']//IMG")).Click();
            JQueryWait();
            SelectDeliveryOptionPerOrder();
            JQueryWait();
            
            Driver.FindElement(By.Id("hcTakeOrder")).ScrollWindowAndClick(Driver);
            JQueryWait();
        }

        public void OrderCheck8ProductsAnd3CouponCodesUseWithTax0070()
        {
            Refresh();
            
            WhetherTheTextPresent("81.725,31"); //Before Discounts
            WhetherTheTextPresent("-5,43"); //Promotion Offer For Order Sub Total
            WhetherTheTextPresent("81.719,88"); //Subtotal
            WhetherTheTextPresent("1.010,11"); //Shipping/Handling Before Discounts
            WhetherTheTextPresent("-9,87"); //Promotion Offer for Shipping
            WhetherTheTextPresent("1.000,24"); //Shipping/Handling
            WhetherTheTextPresent("9.863,76"); //TAX
            WhetherTheTextPresent("92.583,88"); //Grand Total
        }

        public void SwitchOnVat()
        {
            JQueryWait(); // for Azure
            Driver.Navigate().GoToUrl(Globals.BASE_PATH + "/DesktopModules/Hotcakes/Core/Admin/configuration/TaxClasses.aspx");
            JQueryWait();
            CheckCheckBox("MainContent_chkApplyVATRules");
            Driver.FindElement(By.Id("MainContent_btnSave")).Click();
        }

        // ---Autorize.net START---
        public void SetThePaymentMethodsAuthorizeNet()
        {
            Thread.Sleep(3000);
            JQueryWait();
            Driver.Navigate().GoToUrl(Globals.BASE_PATH + "/DesktopModules/Hotcakes/Core/Admin/Default.aspx");
            Thread.Sleep(3000);
            JQueryWait();
            Driver.FindElement(By.LinkText("Settings")).Click();
            Thread.Sleep(3000);
            JQueryWait();
            Driver.FindElement(By.LinkText("Payment Methods")).Click();
            JQueryWait();
            Driver.FindElement(By.Id("MainContent_gvPaymentMethods_btnEdit_0")).Click();
            new SelectElement(Driver.FindElement(By.Id("MainContent_methodEditor_MethodEditor_CreditCard_lstGateway")))
                .SelectByText("Authorize.Net");
            Driver.FindElement(By.Id("MainContent_methodEditor_MethodEditor_CreditCard_btnOptions")).Click();
            Driver.FindElement(
                By.Id(
                    "MainContent_methodEditor_MethodEditor_CreditCard_gatewayEditor_GatewayEditor_Authorize.Net_txtUsername"))
                .Clear();
            Driver.FindElement(
                By.Id(
                    "MainContent_methodEditor_MethodEditor_CreditCard_gatewayEditor_GatewayEditor_Authorize.Net_txtUsername"))
                .SendKeys("77pmw32Vh7LS");
            Driver.FindElement(
                By.Id(
                    "MainContent_methodEditor_MethodEditor_CreditCard_gatewayEditor_GatewayEditor_Authorize.Net_txtPassword"))
                .Clear();
            Driver.FindElement(
                By.Id(
                    "MainContent_methodEditor_MethodEditor_CreditCard_gatewayEditor_GatewayEditor_Authorize.Net_txtPassword"))
                .SendKeys("73629XQgg28GW2tp");
            CheckCheckBox(
                "MainContent_methodEditor_MethodEditor_CreditCard_gatewayEditor_GatewayEditor_Authorize.Net_chkEmailCustomer");
            CheckCheckBox(
                "MainContent_methodEditor_MethodEditor_CreditCard_gatewayEditor_GatewayEditor_Authorize.Net_chkDeveloperMode");
            CheckCheckBox(
                "MainContent_methodEditor_MethodEditor_CreditCard_gatewayEditor_GatewayEditor_Authorize.Net_chkTestMode");
            CheckCheckBox(
                "MainContent_methodEditor_MethodEditor_CreditCard_gatewayEditor_GatewayEditor_Authorize.Net_chkDebugMode");
            Driver.FindElement(By.Id("MainContent_methodEditor_MethodEditor_CreditCard_gatewayEditor_btnSave")).Click();
            CheckCheckBox("MainContent_methodEditor_MethodEditor_CreditCard_chkRequireCreditCardSecurityCode");
            Thread.Sleep(1500);
            Driver.FindElement(By.Id("MainContent_methodEditor_MethodEditor_CreditCard_btnSave")).Click();
            Thread.Sleep(1500);
            Driver.FindElement(By.Id("MainContent_btnSaveChanges")).Click();
        }

        public void SetThePaymentMethodsTestGateway()
        {
            JQueryWait(); // for Azure
            Driver.Navigate().GoToUrl(Globals.BASE_PATH + "/DesktopModules/Hotcakes/Core/Admin/configuration/Payment.aspx");
            JQueryWait();
            Driver.FindElement(By.Id("MainContent_gvPaymentMethods_btnEdit_0")).Click();
            JQueryWait();
            Driver.FindElement(By.Id("MainContent_methodEditor_MethodEditor_CreditCard_lstCaptureMode_1")).Click();
            JQueryWait();
            new SelectElement(Driver.FindElement(By.Id("MainContent_methodEditor_MethodEditor_CreditCard_lstGateway")))
                .SelectByText("Test Gateway");
            JQueryWait();
            CheckCheckBox("MainContent_methodEditor_MethodEditor_CreditCard_chkRequireCreditCardSecurityCode");

            CheckCheckBox("MainContent_methodEditor_MethodEditor_CreditCard_chkCardDiners");
            CheckCheckBox("MainContent_methodEditor_MethodEditor_CreditCard_chkCardJCB");
            JQueryWait();
            Driver.FindElement(By.Id("MainContent_methodEditor_MethodEditor_CreditCard_btnSave")).Click();
            Thread.Sleep(1500);
            Driver.FindElement(By.Id("MainContent_btnSaveChanges")).Click();
        }

        // ---Autorize.net FINISH---

        public void The0OrderAllow()
        {
            JQueryWait(); // for Azure
            Driver.Navigate().GoToUrl(Globals.BASE_PATH + "/DesktopModules/Hotcakes/Core/Admin/configuration/Orders.aspx");
            Driver.FindElement(By.LinkText("Admin")).Click();

            CheckCheckBox("MainContent_chkZeroDollarOrders");
            Driver.FindElement(By.Id("MainContent_btnSave")).Click();
        }

        public void HandlingDisable()
        {
            JQueryWait(); // for Azure
            Driver.Navigate()
                .GoToUrl(Globals.BASE_PATH + "/DesktopModules/Hotcakes/Core/Admin/configuration/ShippingHandling.aspx");
            JQueryWait();
            Driver.FindElement(By.Id("MainContent_HandlingFeeAmountTextBox")).Clear();
            Driver.FindElement(By.Id("MainContent_HandlingFeeAmountTextBox")).SendKeys("0");
            Driver.FindElement(By.Id("MainContent_HandlingRadioButtonList_0")).Click();
            UnCheckCheckBox("MainContent_NonShippingCheckBox");
            JQueryWait();
            Driver.FindElement(By.Id("MainContent_btnSave")).Click();
        }

        public void HandlingEnable()
        {
            JQueryWait(); // for Azure
            Driver.Navigate()
                .GoToUrl(Globals.BASE_PATH + "/DesktopModules/Hotcakes/Core/Admin/configuration/ShippingHandling.aspx");
            JQueryWait();
            Driver.FindElement(By.Id("MainContent_HandlingFeeAmountTextBox")).Clear();
            Driver.FindElement(By.Id("MainContent_HandlingFeeAmountTextBox")).SendKeys("22");
            Driver.FindElement(By.Id("MainContent_HandlingRadioButtonList_0")).Click();
            CheckCheckBox("MainContent_NonShippingCheckBox");
            JQueryWait();
            Thread.Sleep(3000);
            Driver.FindElement(By.Id("MainContent_btnSave")).Click();
        }

        public void Add1ProductToCartAndPlaceOrderWithPreviousAddressNo0080()
        {
            JQueryWait();
            Driver.Navigate()
                .GoToUrl(Globals.BASE_PATH + "/HotcakesStore/Product-Viewer/slug/" + Variablegeneral +
                         "-ProductFor0DollarOrderNonShippingProduct");
            Driver.FindElement(By.Id("addtocartbutton")).Click();
            JQueryWait();
            Driver.FindElement(By.XPath("(//input[@value='Secure Checkout'])[1]")).Click();
            JQueryWait();
            Refresh();
            JQueryWait();
            new SelectElement(Driver.FindElement(By.Id("billingAvailableAddresses"))).SelectByText(Globals.SHIPPING_ADDRESS_STREET1);
            Driver.FindElement(By.XPath("//DIV[@class='span6 hc-site-terms']//IMG")).Click();
            JQueryWait();
            Driver.FindElement(By.Id("hcTakeOrder")).Click();
        }

        public void OrderCheck1ProductAndPlaceOrderWithPreviousAddressNo0080()
        {
            Refresh();
            JQueryWait();
            Assert.AreEqual("0,00 €", Driver.FindElement(By.CssSelector("td.totalgrand > strong")).Text);
            WhetherTheTextPresent("0,00");
        }

        public void The0OrderDisable()
        {
            JQueryWait(); // for Azure
            Driver.Navigate().GoToUrl(Globals.BASE_PATH + "/DesktopModules/Hotcakes/Core/Admin/configuration/Orders.aspx");
            Driver.FindElement(By.LinkText("Admin")).Click();

            UnCheckCheckBox("MainContent_chkZeroDollarOrders");
            Thread.Sleep(3000);
            Driver.FindElement(By.Id("MainContent_btnSave")).Click();
        }

        public void CategoryDrillDown()
        {
            JQueryWait(); // for Azure
            Driver.Navigate().GoToUrl(Globals.BASE_PATH + "/DesktopModules/Hotcakes/Core/Admin/catalog/default.aspx");
            Driver.FindElement(By.LinkText("Categories")).Click();
            JQueryWait();
            Driver.FindElement(By.LinkText("Add New Category")).Click();

            Driver.FindElement(By.Id("NameField")).Clear();
            Driver.FindElement(By.Id("NameField")).SendKeys(Categoryname);
            new SelectElement(Driver.FindElement(By.Id("MainContent_TemplateList"))).SelectByText("DrillDown");
            Driver.FindElement(By.CssSelector("option[value=\"DrillDown\"]")).Click();
            Driver.FindElement(By.Id("MainContent_UpdateButton")).Click();
            JQueryWait();
            Driver.FindElement(By.LinkText("Select Products")).Click();
            JQueryWait(); // for Azure
            new SelectElement(Driver.FindElement(By.Id("MainContent_ProductPicker1_DropDownList1"))).SelectByText("50");
            JQueryWait(); // for Azure
            Driver.FindElement(By.CssSelector("option[value=\"50\"]")).Click();
            Driver.FindElement(By.Id("MainContent_ProductPicker1_FilterField")).Clear();
            Driver.FindElement(By.Id("MainContent_ProductPicker1_FilterField")).SendKeys(Categoryname);
            Driver.FindElement(By.Id("MainContent_ProductPicker1_btnGo")).Click();
            JQueryWait(); // for Azure
            Driver.FindElement(By.LinkText("All")).Click();
            Driver.FindElement(By.Id("MainContent_btnAdd")).Click();
            JQueryWait(); // for Azure
            Driver.FindElement(By.Id("MainContent_lnkBack")).Click();
            JQueryWait(); // for Azure
            new SelectElement(Driver.FindElement(By.Id("MainContent_SortOrderDropDownList"))).SelectByText(
                "Product SKU (z-a)");
            Driver.FindElement(By.Id("MainContent_UpdateButton")).Click();
            JQueryWait(); // for Azure
            Driver.FindElement(By.Id("NavContent_lnkViewInStore")).Click();
        }

        // ---Membership Product Type Start---
        public void AddMembershipProductType()
        {
            JQueryWait(); // for Azure
            Driver.Navigate()
                .GoToUrl(Globals.BASE_PATH + "/DesktopModules/Hotcakes/Core/Admin/catalog/MembershipProductTypes.aspx");
            Driver.FindElement(By.Id("NavContent_btnCreate")).Click();
            Driver.FindElement(By.Id("ctl00_MainContent_ucMembershipTypeEdit_txtProductTypeName"))
                .SendKeys(Variablegeneral + " Membership Type Registered Users");
            JQueryWait();
            JQueryWait();
            JQueryWait();
            JQueryWait();
            JQueryWait();
            Driver.FindElement(By.Id("MainContent_ucMembershipTypeEdit_btnAdd")).Click();
        }

        public void AddMembershipProduct()
        {
            AddProductToStore(Variablegeneral, "." + Variablegeneral + "-Membership-SKU", "15,49", "10,55", "14,56",
                Variable1 + " Schedule1", "01", "02", "03", "04", "0,45", "Charge Shipping & Handling",
                Variable1 + " manufacturer1", Variable1 + " vendor1", "." + Variablegeneral + "-Membership-Name",
                Variablegeneral + " Membership Type Registered Users", "1");
        }

        public void AddMembershipProductToCartAndPlaceOrderNo0090()
        {
            JQueryWait(); // for Azure
            Driver.Navigate()
                .GoToUrl(Globals.BASE_PATH + "/HotcakesStore/Product-Viewer/slug/" + Variablegeneral + "-Membership-Name");
            Thread.Sleep(7000);
            Driver.FindElement(By.Id("addtocartbutton")).Click();
            JQueryWait();
            Driver.FindElement(By.XPath("(//input[@value='Secure Checkout'])[1]")).Click();
            JQueryWait();
            Thread.Sleep(1000);
            Refresh();
            JQueryWait();
            BillingAddressCredentialPopulate();
            JQueryWait();
            Assert.AreEqual("Grand Total:", Driver.FindElement(By.CssSelector("td.totalgrandlabel")).Text);
            CreditCardCredentialPopulate(Globals.CREDIT_CARD_JCB); //JCB
            Driver.FindElement(By.XPath("//DIV[@class='span6 hc-site-terms']//IMG")).Click();
            new SelectElement(Driver.FindElement(By.Id("billingstate"))).SelectByText(Globals.SHIPPING_ADDRESS_REGION);
            JQueryWait();
            Assert.AreEqual("Grand Total:", Driver.FindElement(By.CssSelector("td.totalgrandlabel")).Text);
            JQueryWait();
            new SelectElement(Driver.FindElement(By.Id("billingstate"))).SelectByText(Globals.BILLING_ADDRESS_REGION);
            JQueryWait();
            Assert.AreEqual("Grand Total:", Driver.FindElement(By.CssSelector("td.totalgrandlabel")).Text);
            Driver.FindElement(By.Id("hcTakeOrder")).Click();
            Refresh();
        }

        public void OrderCheckMembershipProduct0090()
        {
            Refresh();
            JQueryWait();
            WhetherTheTextPresent("13,46"); //Subtotal
            WhetherTheTextPresent("21,60"); //Shipping/Handling
            WhetherTheTextPresent("12,12"); //Subtotal Before VAT
            WhetherTheTextPresent("19,11"); //Shipping/Handling Before VAT
            WhetherTheTextPresent("31,23"); //Total Before VAT
            WhetherTheTextPresent("3,83"); //VAT
            WhetherTheTextPresent("35,06"); //Grand Total
        }

        // ---Membership Product Type Finish---

        // ---Bundle product Start---
        public void AddBundleProducts()
        {
            JQueryWait();
            Driver.FindElement(By.Id("MainContent_rbBehaviour_1")).Click();
            JQueryWait();
            Driver.FindElement(By.LinkText("Save")).Click();
            JQueryWait();
            Driver.FindElement(By.Id("NavContent_ProductNavigator_hypBundledProducts")).Click();
            Driver.FindElement(By.Id("MainContent_ucProductPicker_FilterField")).Clear();
            Driver.FindElement(By.Id("MainContent_ucProductPicker_FilterField")).SendKeys(Categoryname);
            Driver.FindElement(By.Id("MainContent_ucProductPicker_btnGo")).Click();
            JQueryWait(); // for Azure
            Driver.FindElement(By.LinkText("All")).Click();
            JQueryWait(); // for Azure
            Driver.FindElement(By.Id("MainContent_btnAddProducts")).Click();
            JQueryWait(); // for Azure
            Driver.FindElement(By.Id("MainContent_btnUpdate")).Click();
            JQueryWait(); // for Azure
            Driver.FindElement(By.Id("NavContent_ProductNavigator_hypGeneral")).Click();
            JQueryWait();
            Driver.FindElement(By.LinkText("Save")).Click();
        }

        public void AddBundleProductToCartAndPlaceOrderNo0100()
        {
            JQueryWait();
            Driver.Navigate()
                .GoToUrl(Globals.BASE_PATH + "/HotcakesStore/Product-Viewer/slug/" + Variable3 +
                         "-Bundle-Name-Shipping-Charges_Charge-Shipping-And-Handling");
            JQueryWait();
            Driver.FindElement(By.Id("addtocartbutton")).Click();
            JQueryWait();
            Driver.FindElement(By.XPath("(//input[@value='Secure Checkout'])[1]")).Click();
            Thread.Sleep(15000);
            Refresh();
            JQueryWait();
            ShippingAddressCredentialPopulate();
            JQueryWait();
            Assert.AreEqual("Grand Total:", Driver.FindElement(By.CssSelector("td.totalgrandlabel")).Text);
            CreditCardCredentialPopulate("378734493671000"); //American Express Corporate
            Driver.FindElement(By.XPath("//DIV[@class='span6 hc-site-terms']//IMG")).Click();
            JQueryWait();
            Assert.AreEqual("Grand Total:", Driver.FindElement(By.CssSelector("td.totalgrandlabel")).Text);
            JQueryWait();
            SelectDeliveryOptionPerItem();
            JQueryWait();
            Assert.AreEqual("Grand Total:", Driver.FindElement(By.CssSelector("td.totalgrandlabel")).Text);
            Driver.FindElement(By.Id("hcTakeOrder")).Click();
            Refresh();
        }

        public void OrderCheckBundleProduct0100()
        {
            Refresh();
            JQueryWait();
            WhetherTheTextPresent("5.094,00"); //Subtotal
            WhetherTheTextPresent("77,43"); //Shipping/Handling
            WhetherTheTextPresent("4.534,85"); //Subtotal Before VAT
            WhetherTheTextPresent("66,56"); //Shipping/Handling Before VAT
            WhetherTheTextPresent("4.601,41"); //Total Before VAT
            WhetherTheTextPresent("570,02"); //VAT
            WhetherTheTextPresent("5.171,43"); //Grand Total
        }

        // ---Bundle product Finish---

        public void AddProductToStore(string fieldVariable, string sku, string listPrice, string cost, string sitePrice,
            string tax, string weight, string length, string width, string height, string extrashipfee,
            string shippingcharges, string manufacturer, string vendor, string productname, string producttype,
            string nonshipable)
        {
            JQueryWait(); // for Azure
            Driver.Navigate().GoToUrl(Globals.BASE_PATH + "/DesktopModules/Hotcakes/Core/Admin/Catalog/Products_Edit.aspx");
            Thread.Sleep(7000);
            //---Pricing---
            Driver.FindElement(By.Id("MainContent_ListPriceField")).Clear();
            Driver.FindElement(By.Id("MainContent_ListPriceField")).SendKeys(listPrice);
            Driver.FindElement(By.Id("MainContent_CostField")).Clear();
            Driver.FindElement(By.Id("MainContent_CostField")).SendKeys(cost);
            Driver.FindElement(By.Id("MainContent_SitePriceField")).Clear();
            Driver.FindElement(By.Id("MainContent_SitePriceField")).SendKeys(sitePrice);
            //---Pricing---
            //---Tax---
            new SelectElement(Driver.FindElement(By.Id("MainContent_lstTaxClasses"))).SelectByText(tax);
            //---Tax---
            //---Shipping---
            Driver.FindElement(By.Id("MainContent_txtWeight")).Clear();
            Driver.FindElement(By.Id("MainContent_txtWeight")).SendKeys(weight);
            Driver.FindElement(By.Id("MainContent_txtLength")).Clear();
            Driver.FindElement(By.Id("MainContent_txtLength")).SendKeys(length);
            Driver.FindElement(By.Id("MainContent_txtWidth")).Clear();
            Driver.FindElement(By.Id("MainContent_txtWidth")).SendKeys(width);
            Driver.FindElement(By.Id("MainContent_txtHeight")).Clear();
            Driver.FindElement(By.Id("MainContent_txtHeight")).SendKeys(height);
            Driver.FindElement(By.Id("MainContent_ExtraShipFeeField")).Clear();
            Driver.FindElement(By.Id("MainContent_ExtraShipFeeField")).SendKeys(extrashipfee);
            new SelectElement(Driver.FindElement(By.Id("MainContent_lstShippingCharge"))).SelectByText(shippingcharges);
            //---Shipping---
            //---Properties---
            Driver.FindElement(By.Id("ctl00_MainContent_lstManufacturers_Arrow")).ScrollWindowAndClick(Driver);
            JQueryWait();
            Thread.Sleep(1000);
            Driver.FindElement(By.XPath("//li[text()='" + manufacturer + "']")).Click();
            JQueryWait();
            Thread.Sleep(1000);
            Driver.FindElement(By.Id("ctl00_MainContent_lstVendors_Arrow")).ScrollWindowAndClick(Driver);
            JQueryWait();
            Thread.Sleep(1000);
            Driver.FindElement(By.XPath("//li[text()='" + vendor + "']")).Click();
            //---Properties---
            //---Main---
            Driver.FindElement(By.Id("MainContent_SkuField")).Clear();
            Driver.FindElement(By.Id("MainContent_SkuField")).SendKeys(sku);
            Driver.FindElement(By.Id("txtProductName")).Clear();
            Driver.FindElement(By.Id("txtProductName")).SendKeys(productname);
            JQueryWait();
            Thread.Sleep(10000);
            Driver.FindElement(By.Id("ctl00_MainContent_lstProductType_Arrow")).ScrollWindowAndClick(Driver);
            Thread.Sleep(20000);
            JQueryWait();
            Driver.FindElement(By.XPath("//li[text()='" + producttype + "']")).Click();
            //---Main---
            if (nonshipable.Equals("1"))
            {
                Driver.FindElement(By.Id("MainContent_chkNonShipping")).ScrollWindowAndClick(Driver);
            }
            //Driver.FindElement(By.LinkText("Save and Close")).Click();
            Driver.FindElement(By.LinkText("Save")).Click();
        }

        public void AddGiftCardToStore()
        {
            AddGiftCardToStore(Variablegeneral, "." + Variablegeneral + "GiftCard-SKU", Variable1 + " Schedule1", "01",
                "02", "03", "04", "23,45", "Charge Shipping & Handling", Variable1 + " manufacturer1",
                Variable1 + " vendor1", "." + Variablegeneral + "GiftCard-Name",
                Variablegeneral + " Product Types Include 4 Type Properties", "1");
        }

        public void AddGiftCardToStore(string fieldVariable, string sku, string tax, string weight, string length,
            string width, string height, string extrashipfee, string shippingcharges, string manufacturer, string vendor,
            string productname, string producttype, string nonshipable)
        {
            JQueryWait(); // for Azure
            Driver.Navigate().GoToUrl(Globals.BASE_PATH + "/DesktopModules/Hotcakes/Core/Admin/Catalog/Products_Edit.aspx");

            Thread.Sleep(7000);
            JQueryWait(); // for Azure
            //---Tax---
            new SelectElement(Driver.FindElement(By.Id("MainContent_lstTaxClasses"))).SelectByText(tax);
            //---Tax---
            //---Shipping---
            Driver.FindElement(By.Id("MainContent_txtWeight")).Clear();
            Driver.FindElement(By.Id("MainContent_txtWeight")).SendKeys(weight);
            Driver.FindElement(By.Id("MainContent_txtLength")).Clear();
            Driver.FindElement(By.Id("MainContent_txtLength")).SendKeys(length);
            Driver.FindElement(By.Id("MainContent_txtWidth")).Clear();
            Driver.FindElement(By.Id("MainContent_txtWidth")).SendKeys(width);
            Driver.FindElement(By.Id("MainContent_txtHeight")).Clear();
            Driver.FindElement(By.Id("MainContent_txtHeight")).SendKeys(height);
            Driver.FindElement(By.Id("MainContent_ExtraShipFeeField")).Clear();
            Driver.FindElement(By.Id("MainContent_ExtraShipFeeField")).SendKeys(extrashipfee);
            new SelectElement(Driver.FindElement(By.Id("MainContent_lstShippingCharge"))).SelectByText(shippingcharges);
            //---Shipping---
            //---Properties---
            Driver.FindElement(By.Id("ctl00_MainContent_lstManufacturers_Arrow")).ScrollWindowAndClick(Driver);
            JQueryWait();
            Thread.Sleep(1000);
            Driver.FindElement(By.XPath("//li[text()='" + manufacturer + "']")).Click();
            JQueryWait();
            Thread.Sleep(1000);
            Driver.FindElement(By.Id("ctl00_MainContent_lstVendors_Arrow")).ScrollWindowAndClick(Driver);
            JQueryWait();
            Thread.Sleep(1000);
            Driver.FindElement(By.XPath("//li[text()='" + vendor + "']")).Click();
            //---Properties---
            //---Main---
            Driver.FindElement(By.Id("MainContent_SkuField")).Clear();
            Driver.FindElement(By.Id("MainContent_SkuField")).SendKeys(sku);
            Driver.FindElement(By.Id("txtProductName")).Clear();
            Driver.FindElement(By.Id("txtProductName")).SendKeys(productname);
            Driver.FindElement(By.Id("MainContent_rbBehaviour_2")).ScrollWindowAndClick(Driver);
            Thread.Sleep(1000);
            JQueryWait();
            Driver.FindElement(By.Id("ctl00_MainContent_lstProductType_Arrow")).ScrollWindowAndClick(Driver);
            Thread.Sleep(1000);
            JQueryWait();
            Driver.FindElement(By.XPath("//li[text()='" + producttype + "']")).Click();
            //---Main---
            if (nonshipable.Equals("1"))
            {
                Driver.FindElement(By.Id("MainContent_chkNonShipping")).ScrollWindowAndClick(Driver);
            }
            Driver.FindElement(By.LinkText("Save and Close")).Click();
        }

        public void AddTaxSchedule(string txtScheduleName, string txtDefaultRate, string txtDefaultShippingRate,
            string Country1, string Region1,
            string PostalCode1, string Rate1, string ShippingRate1, string Country2, string Region2, string PostalCode2,
            string Rate2, string ShippingRate2)
        {
            JQueryWait(); // for Azure
            Driver.FindElement(By.Id("MainContent_btnAddNewWithPoup")).Click();
            Thread.Sleep(3000);
            JQueryWait(); // for Azure

            // enter the default tax rate information
            Driver.FindElement(By.Id("MainContent_ucTaxScheduleEditor_txtScheduleName")).Clear();
            Driver.FindElement(By.Id("MainContent_ucTaxScheduleEditor_txtScheduleName")).SendKeys(txtScheduleName);
            FindElement(By.Id("MainContent_ucTaxScheduleEditor_txtDefaultRate")).Clear();
            FindElement(By.Id("MainContent_ucTaxScheduleEditor_txtDefaultRate")).SendKeys(txtDefaultRate);
            Driver.FindElement(By.Id("MainContent_ucTaxScheduleEditor_txtDefaultShippingRate")).Clear();
            Driver.FindElement(By.Id("MainContent_ucTaxScheduleEditor_txtDefaultShippingRate"))
                .SendKeys(txtDefaultShippingRate);
            
            //
            // Enter and save first tax area
            //
            
            // select the country from the dropdownlist
            new SelectElement(Driver.FindElement(By.Id("MainContent_ucTaxScheduleEditor_ddlCountries"))).SelectByText(Country1);
            Driver.FindElement(By.CssSelector("option[value=\"USA\"]")).Click();
            
            // select the region from the dropdownlist
            new SelectElement(Driver.FindElement(By.Id("MainContent_ucTaxScheduleEditor_ddlRegions"))).SelectByText(Region1);
            
            // enter the tax area information
            Driver.FindElement(By.Id("MainContent_ucTaxScheduleEditor_txtPostalCode")).Clear();
            Driver.FindElement(By.Id("MainContent_ucTaxScheduleEditor_txtPostalCode")).SendKeys(PostalCode1);
            Driver.FindElement(By.Id("MainContent_ucTaxScheduleEditor_txtRate")).Clear();
            Driver.FindElement(By.Id("MainContent_ucTaxScheduleEditor_txtRate")).SendKeys(Rate1);
            Driver.FindElement(By.Id("MainContent_ucTaxScheduleEditor_txtShippingRate")).Clear();
            Driver.FindElement(By.Id("MainContent_ucTaxScheduleEditor_txtShippingRate")).SendKeys(ShippingRate1);
            JQueryWait();
            CheckCheckBox("MainContent_ucTaxScheduleEditor_chkApplyToShipping");

            // save the tax area
            Driver.FindElement(By.Id("MainContent_ucTaxScheduleEditor_btnNew")).Click();
            Thread.Sleep(3000);

            //
            // Enter and save second tax area
            //

            // select the country from the dropdownlist
            new SelectElement(Driver.FindElement(By.Id("MainContent_ucTaxScheduleEditor_ddlCountries"))).SelectByText(Country2);
            Driver.FindElement(By.CssSelector("option[value=\"USA\"]")).Click();
            
            // select the region from the dropdownlist
            new SelectElement(Driver.FindElement(By.Id("MainContent_ucTaxScheduleEditor_ddlRegions"))).SelectByText(Region2);

            // enter the tax area information
            Driver.FindElement(By.Id("MainContent_ucTaxScheduleEditor_txtPostalCode")).Clear();
            Driver.FindElement(By.Id("MainContent_ucTaxScheduleEditor_txtPostalCode")).SendKeys(PostalCode2);
            Driver.FindElement(By.Id("MainContent_ucTaxScheduleEditor_txtRate")).Clear();
            Driver.FindElement(By.Id("MainContent_ucTaxScheduleEditor_txtRate")).SendKeys(Rate2);
            Driver.FindElement(By.Id("MainContent_ucTaxScheduleEditor_txtShippingRate")).Clear();
            Driver.FindElement(By.Id("MainContent_ucTaxScheduleEditor_txtShippingRate")).SendKeys(ShippingRate2);
            JQueryWait();
            Thread.Sleep(1000);
            CheckCheckBox("MainContent_ucTaxScheduleEditor_chkApplyToShipping");

            // save the tax area
            Driver.FindElement(By.Id("MainContent_ucTaxScheduleEditor_btnNew")).ScrollWindowAndClick(Driver);

            //
            // save all changes
            //
            Driver.FindElement(By.Id("MainContent_btnSaveChanges")).ScrollWindowAndClick(Driver);
        }

        public void SetVatEnable()
        {
            JQueryWait(); // for Azure
            Driver.Navigate().GoToUrl(Globals.BASE_PATH + "/DesktopModules/Hotcakes/Core/Admin/configuration/TaxClasses.aspx");
            Thread.Sleep(5000);
            CheckCheckBox("MainContent_chkApplyVATRules");
            Thread.Sleep(3000);
            Driver.FindElement(By.Id("MainContent_btnSave")).Click();
        }
    }
}