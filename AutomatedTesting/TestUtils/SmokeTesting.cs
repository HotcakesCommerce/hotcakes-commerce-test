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
using System.IO;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;

namespace Hotcakes.AutomatedTesting.TestUtils
{
    [TestClass]
    public class SmokeTesting : BasicUnitTests
    {
        [TestInitialize]
        public void SetupTest()
        {
            var pathForAutomatedTesting = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory).FullName;

            var profile = new FirefoxProfile(@"C:\Work\Hotcakes\QA\AutomatedTesting 3.XX.XX\Selenium\Firefoxprofile");

            var chromeOptions = new ChromeOptions();
            chromeOptions.AddArgument("no-sandbox");

            Driver = new ChromeDriver(chromeOptions);
            VerificationErrors = new StringBuilder();

            var ts = TimeSpan.FromSeconds(120);
            Driver.Manage().Timeouts().ImplicitlyWait(ts);
            Driver.Manage().Timeouts().SetPageLoadTimeout(ts);
            Driver.Manage().Timeouts().SetScriptTimeout(ts);
        }

        [TestMethod]
        public void SmokeTest()
        {
            UserAdminLoginHcc();
            WizardSetup();

            //---AddTaxSchedules---Start
            SetVatEnable();
            AddTaxSchedules();
            //---AddTaxSchedules---End

            AddManufacturers();
            AddVendors();

            AddMembershipProductType();
            //---Add Product Membership
            AddMembershipProduct();

            AddProductTypePropertiesTextfield();
            AddProductTypePropertiesMultiplechoice();
            AddProductTypePropertiesCurrencyfield();
            AddProductTypePropertiesDatefield();
            ProductTypesInclude4TypeProperties();

            //---Add Gift Card
            AddGiftCardToStore();

            //---Add Product -TAX-01-Name-Shipping-Charges_Charge-Shipping-And-Handling
            AddProductToStore(Variable1, "." + Variable1 + "-TAX-01-SKU-Chg-Ship-And-Hand", "1330,49", "1145,55",
                "1234,56", Variable1 + " Schedule1", "01", "02", "03", "04", "05,11", "Charge Shipping & Handling",
                Variable1 + " manufacturer1", Variable1 + " vendor1",
                "." + Variable1 + "-TAX-01-Name-Shipping-Charges_Charge-Shipping-And-Handling",
                Variablegeneral + " Product Types Include 4 Type Properties", "");
            //---Add Product -TAX-02-Name-Shipping-Charges_Charge-Shipping-And-Handling
            AddProductToStore(Variable2, "." + Variable2 + "-TAX-02-SKU-Chg-Ship-And-Hand", "2527,93", "2176,55",
                "2345,67", Variable2 + " Schedule2", "06", "07", "08", "09", "10,22", "Charge Shipping & Handling",
                Variable2 + " manufacturer2", Variable2 + " vendor2",
                "." + Variable2 + "-TAX-02-Name-Shipping-Charges_Charge-Shipping-And-Handling",
                Variablegeneral + " Product Types Include 4 Type Properties", "");
            //---Add Product -TAX-03-Name-Shipping-Charges_Charge-Shipping-And-Handling
            AddProductToStore(Variable3, "." + Variable3 + "-TAX-03-SKU-Chg-Ship-And-Hand", "3725,37", "3207,55",
                "3456,78", Variable3 + " Schedule3", "11", "12", "13", "14", "15,33", "Charge Shipping & Handling",
                Variable3 + " manufacturer3", Variable3 + " vendor3",
                "." + Variable3 + "-TAX-03-Name-Shipping-Charges_Charge-Shipping-And-Handling",
                Variablegeneral + " Product Types Include 4 Type Properties", "");

            AddShippihgMethodFlatRatePerOrder12_34();
            AddShippihgMethodFlatRatePerItem21_43();

            Add3ProductsToCartAndPlaceOrderNo0010();
            OrderCheck3Products0010();

            Buy3GiftCardsNo0020();
            OrderCheck3GiftCards0020();
            Catch2GiftCardNumbers();

            AddToCart3ProductsAndPlaceOrderWith2GiftCardsNo0030();
            OrderCheckWith3ProductsAndUse2GiftCardsForPayment0030();

            //---AddProduct05ForSalePromotionShippingCharges_ChargeShippingAndHandling
            AddProductToStore(Variablegeneral, "." + Variablegeneral + "-Sale-Promotion-Product-SKU", "4567,89",
                "4567,89", "4567,89", Variable3 + " Schedule3", "1", "2", "3", "4", "14,56",
                "Charge Shipping & Handling", Variable3 + " manufacturer3", Variable3 + " vendor3",
                "." + Variablegeneral + "-Sale-Promotion-Product-Name-Shipping-Charges_Charge-Shipping-And-Handling",
                Variablegeneral + " Product Types Include 4 Type Properties", "");

            AddPromotionSale(); //---Promotion
            AddPromotionOfferForOrderItemsWithCouponEqualVariable1(); //---Promotion
            AddPromotionOfferForOrderSubTotalWithCouponEqualVariable2(); //---Promotion
            AddPromotionOfferForShippingSubTotalWithCouponEqualVariable3(); //---Promotion

            Add1ProductToCartAndPlaceOrderWithPreviousAddressNo0034();
            OrderCheck1ProductAndPlaceOrderWithPreviousAddressNo0034();

            AddToCartProductForSaleAndPlaceOrder0040();
            OrderCheckProductForSale0040();

            AddToCart4ProductsUse3CouponCodesAndPlaceOrderWithVat0050();
            OrderCheck4ProductsAnd3CouponCodesUseWithVat0050();

            //---AddProduct01TAX01ShippingCharges_NoCharge
            AddProductToStore(Variable1, "." + Variable1 + "-TAX-01-SKU-No-Charge", "1330,49", "1145,55", "1234,56",
                Variable1 + " Schedule1", "01", "02", "03", "04", "5,11", "No Charge", Variable1 + " manufacturer1",
                Variable1 + " vendor1", "." + Variable1 + "-TAX-01-Name-Shipping-Charges_No-Charge",
                Variablegeneral + " Product Types Include 4 Type Properties", "");
            //---AddProduct01TAX01ShippingCharges_ChargeHandlingOnly
            AddProductToStore(Variable1, "." + Variable1 + "-TAX-01-SKU-Chg-Handling-Only", "1330,49", "1145,55",
                "1234,56", Variable1 + " Schedule1", "01", "02", "03", "04", "5,11", "Charge Handling Only",
                Variable1 + " manufacturer1", Variable1 + " vendor1",
                "." + Variable1 + "-TAX-01-Name-Shipping-Charges_Charge-Handling-Only",
                Variablegeneral + " Product Types Include 4 Type Properties", "");
            //---AddProduct01TAX01ShippingCharges_ChargeShippingOnly
            AddProductToStore(Variable1, "." + Variable1 + "-TAX-01-SKU-Chg-Ship-Only", "1330,49", "1145,55", "1234,56",
                Variable1 + " Schedule1", "01", "02", "03", "04", "5,11", "Charge Shipping Only",
                Variable1 + " manufacturer1", Variable1 + " vendor1",
                "." + Variable1 + "-TAX-01-Name-Shipping-Charges_Charge-Shipping-Only",
                Variablegeneral + " Product Types Include 4 Type Properties", "");
            //---AddProduct04TAX04ShippingCharges_ChargeShippingAndHandling
            AddProductToStore(Variable4, "." + Variable4 + "-TAX-04-SKU-Chg-Ship-And-Hand", "74,36", "64,03", "69",
                Variable4 + " Schedule4", "01", "02", "03", "04", "5,11", "Charge Shipping & Handling",
                Variable1 + " manufacturer1", Variable1 + " vendor1",
                "." + Variable4 + "-TAX-04-Name-Shipping-Charges_Charge-Shipping-And-Handling",
                Variablegeneral + " Product Types Include 4 Type Properties", "");
            //---AddProductFor0DollarOrderNonShippingProduct
            AddProductToStore(Variablegeneral, "." + Variablegeneral + "-For0USDOrderNonShipping-SKU", "0", "0", "0",
                Variable3 + " Schedule3", "1", "2", "3", "4", "0", "Charge Shipping & Handling",
                Variable3 + " manufacturer3", Variable3 + " vendor3",
                "." + Variablegeneral + "-ProductFor0DollarOrderNonShippingProduct",
                Variablegeneral + " Product Types Include 4 Type Properties", "1");
            // ---Add Product 08 Bundle--- Start
            AddProductToStore(Variablegeneral, "." + Variable3 + "-Bundle-SKU-Chg-Ship-And-Hand", "5725,37", "5207,55",
                "5456,78", Variable3 + " Schedule3", "31", "32", "33", "34", "35,33", "Charge Shipping & Handling",
                Variable3 + " manufacturer3", Variable3 + " vendor3",
                "." + Variable3 + "-Bundle-Name-Shipping-Charges_Charge-Shipping-And-Handling",
                Variablegeneral + " Product Types Include 4 Type Properties", "0");
            AddBundleProducts();
            // ---Add Product 08 Bundle--- Finish

            AddToCart4ProductsWith4DifferentShippingChargesUse3CouponCodesAndPlaceOrder0055();
            OrderCheck4ProductsWith4DifferentShippingChargesAnd3CouponCodesUse0055();

            AddToCartProduct4AndPlaceOrder0060();
            // Bug 14764:Bug: Incorrect Order calculation if Default Shipping Rate=Shipping Rate and (or) Default Rate=Rate
            OrderCheckProduct4TaxScheduler0060();

            SwitchOnTax(); //---Order Placing with TAX Rule---
            AddToCart8ProductsUse3CouponCodesAndPlaceOrderWithTax0070();
            OrderCheck8ProductsAnd3CouponCodesUseWithTax0070();
            SwitchOnVat();

            SetThePaymentMethodsAuthorizeNet();
            AddToCart4ProductsWith4DifferentShippingChargesUse3CouponCodesAndPlaceOrder0055();
            OrderCheck4ProductsWith4DifferentShippingChargesAnd3CouponCodesUse0055();
            SetThePaymentMethodsTestGateway();

            The0OrderAllow();
            HandlingDisable();
            Add1ProductToCartAndPlaceOrderWithPreviousAddressNo0080();
            OrderCheck1ProductAndPlaceOrderWithPreviousAddressNo0080();
            The0OrderDisable();
            HandlingEnable();

            // ---Membership Product Type Start
            AddMembershipProductToCartAndPlaceOrderNo0090();
            OrderCheckMembershipProduct0090();
            // ---Membership Product Type Finish

            // ---Bundle Product Start
            AddBundleProductToCartAndPlaceOrderNo0100();
            OrderCheckBundleProduct0100();
            // ---Bundle Product Finish

            CategoryDrillDown();
        }


        [TestMethod]
        public void DebugingSection()
        {
            UserAdminLoginHcc();
            WizardSetup();

            //---AddTaxSchedules---Start
            SetVatEnable();
            AddTaxSchedule(Variable1 + " Schedule1", "20,12", "15,12", "United States", "Florida", "33401", "10,12",
                "12,12", "United States", "District Of Columbia", "20301", "11,02", "13,02");
            AddTaxSchedule(Variable2 + " Schedule2", "25,25", "20,25", "United States", "Florida", "33401", "11,25",
                "10,25", "United States", "District Of Columbia", "20301", "12,05", "11,05");
            AddTaxSchedule(Variable3 + " Schedule3", "20,33", "18,33", "United States", "Florida", "33401", "12,33",
                "16,33", "United States", "District Of Columbia", "20301", "13,03", "17,03");
            AddTaxSchedule(Variable4 + " Schedule4", "20,24", "15,12", "United States", "Florida", "33401", "20,24",
                "15,12", "United States", "District Of Columbia", "20301", "21,04", "16,02");
            //---AddTaxSchedules---End

            AddManufacturers();
            AddVendors();

            AddMembershipProductType();
            //---Add Product Membership
            AddProductToStore(Variablegeneral, "." + Variablegeneral + "-Membership-SKU", "15,49", "10,55", "14,56",
                Variable1 + " Schedule1", "01", "02", "03", "04", "0,45", "Charge Shipping & Handling",
                Variable1 + " manufacturer1", Variable1 + " vendor1", "." + Variablegeneral + "-Membership-Name",
                Variablegeneral + " Membership Type Registered Users", "1");

            AddProductTypePropertiesTextfield();
            AddProductTypePropertiesMultiplechoice();
            AddProductTypePropertiesCurrencyfield();
            AddProductTypePropertiesDatefield();
            ProductTypesInclude4TypeProperties();

            //---Add Gift Card
            AddGiftCardToStore(Variablegeneral, "." + Variablegeneral + "GiftCard-SKU", Variable1 + " Schedule1", "01",
                "02", "03", "04", "23,45", "Charge Shipping & Handling", Variable1 + " manufacturer1",
                Variable1 + " vendor1", "." + Variablegeneral + "GiftCard-Name",
                Variablegeneral + " Product Types Include 4 Type Properties", "1");

            //---Add Product -TAX-01-Name-Shipping-Charges_Charge-Shipping-And-Handling
            AddProductToStore(Variable1, "." + Variable1 + "-TAX-01-SKU-Chg-Ship-And-Hand", "1330,49", "1145,55",
                "1234,56", Variable1 + " Schedule1", "01", "02", "03", "04", "05,11", "Charge Shipping & Handling",
                Variable1 + " manufacturer1", Variable1 + " vendor1",
                "." + Variable1 + "-TAX-01-Name-Shipping-Charges_Charge-Shipping-And-Handling",
                Variablegeneral + " Product Types Include 4 Type Properties", "");
            //---Add Product -TAX-02-Name-Shipping-Charges_Charge-Shipping-And-Handling
            AddProductToStore(Variable2, "." + Variable2 + "-TAX-02-SKU-Chg-Ship-And-Hand", "2527,93", "2176,55",
                "2345,67", Variable2 + " Schedule2", "06", "07", "08", "09", "10,22", "Charge Shipping & Handling",
                Variable2 + " manufacturer2", Variable2 + " vendor2",
                "." + Variable2 + "-TAX-02-Name-Shipping-Charges_Charge-Shipping-And-Handling",
                Variablegeneral + " Product Types Include 4 Type Properties", "");
            //---Add Product -TAX-03-Name-Shipping-Charges_Charge-Shipping-And-Handling
            AddProductToStore(Variable3, "." + Variable3 + "-TAX-03-SKU-Chg-Ship-And-Hand", "3725,37", "3207,55",
                "3456,78", Variable3 + " Schedule3", "11", "12", "13", "14", "15,33", "Charge Shipping & Handling",
                Variable3 + " manufacturer3", Variable3 + " vendor3",
                "." + Variable3 + "-TAX-03-Name-Shipping-Charges_Charge-Shipping-And-Handling",
                Variablegeneral + " Product Types Include 4 Type Properties", "");

            AddShippihgMethodFlatRatePerOrder12_34();
            AddShippihgMethodFlatRatePerItem21_43();

            Add3ProductsToCartAndPlaceOrderNo0010();
            OrderCheck3Products0010();

            Buy3GiftCardsNo0020();
            OrderCheck3GiftCards0020();
            Catch2GiftCardNumbers();

            AddToCart3ProductsAndPlaceOrderWith2GiftCardsNo0030();
            OrderCheckWith3ProductsAndUse2GiftCardsForPayment0030();

            //---AddProduct05ForSalePromotionShippingCharges_ChargeShippingAndHandling
            AddProductToStore(Variablegeneral, "." + Variablegeneral + "-Sale-Promotion-Product-SKU", "4567,89",
                "4567,89", "4567,89", Variable3 + " Schedule3", "1", "2", "3", "4", "14,56",
                "Charge Shipping & Handling", Variable3 + " manufacturer3", Variable3 + " vendor3",
                "." + Variablegeneral + "-Sale-Promotion-Product-Name-Shipping-Charges_Charge-Shipping-And-Handling",
                Variablegeneral + " Product Types Include 4 Type Properties", "");

            AddPromotionSale(); //---Promotion
            AddPromotionOfferForOrderItemsWithCouponEqualVariable1(); //---Promotion
            AddPromotionOfferForOrderSubTotalWithCouponEqualVariable2(); //---Promotion
            AddPromotionOfferForShippingSubTotalWithCouponEqualVariable3(); //---Promotion

            Add1ProductToCartAndPlaceOrderWithPreviousAddressNo0034();
            OrderCheck1ProductAndPlaceOrderWithPreviousAddressNo0034();

            AddToCartProductForSaleAndPlaceOrder0040();
            OrderCheckProductForSale0040();

            AddToCart4ProductsUse3CouponCodesAndPlaceOrderWithVat0050();
            OrderCheck4ProductsAnd3CouponCodesUseWithVat0050();

            //---AddProduct01TAX01ShippingCharges_NoCharge
            AddProductToStore(Variable1, "." + Variable1 + "-TAX-01-SKU-No-Charge", "1330,49", "1145,55", "1234,56",
                Variable1 + " Schedule1", "01", "02", "03", "04", "5,11", "No Charge", Variable1 + " manufacturer1",
                Variable1 + " vendor1", "." + Variable1 + "-TAX-01-Name-Shipping-Charges_No-Charge",
                Variablegeneral + " Product Types Include 4 Type Properties", "");
            //---AddProduct01TAX01ShippingCharges_ChargeHandlingOnly
            AddProductToStore(Variable1, "." + Variable1 + "-TAX-01-SKU-Chg-Handling-Only", "1330,49", "1145,55",
                "1234,56", Variable1 + " Schedule1", "01", "02", "03", "04", "5,11", "Charge Handling Only",
                Variable1 + " manufacturer1", Variable1 + " vendor1",
                "." + Variable1 + "-TAX-01-Name-Shipping-Charges_Charge-Handling-Only",
                Variablegeneral + " Product Types Include 4 Type Properties", "");
            //---AddProduct01TAX01ShippingCharges_ChargeShippingOnly
            AddProductToStore(Variable1, "." + Variable1 + "-TAX-01-SKU-Chg-Ship-Only", "1330,49", "1145,55", "1234,56",
                Variable1 + " Schedule1", "01", "02", "03", "04", "5,11", "Charge Shipping Only",
                Variable1 + " manufacturer1", Variable1 + " vendor1",
                "." + Variable1 + "-TAX-01-Name-Shipping-Charges_Charge-Shipping-Only",
                Variablegeneral + " Product Types Include 4 Type Properties", "");
            //---AddProduct04TAX04ShippingCharges_ChargeShippingAndHandling
            AddProductToStore(Variable4, "." + Variable4 + "-TAX-04-SKU-Chg-Ship-And-Hand", "74,36", "64,03", "69",
                Variable4 + " Schedule4", "01", "02", "03", "04", "5,11", "Charge Shipping & Handling",
                Variable1 + " manufacturer1", Variable1 + " vendor1",
                "." + Variable4 + "-TAX-04-Name-Shipping-Charges_Charge-Shipping-And-Handling",
                Variablegeneral + " Product Types Include 4 Type Properties", "");
            //---AddProductFor0DollarOrderNonShippingProduct
            AddProductToStore(Variablegeneral, "." + Variablegeneral + "-For0USDOrderNonShipping-SKU", "0", "0", "0",
                Variable3 + " Schedule3", "1", "2", "3", "4", "0", "Charge Shipping & Handling",
                Variable3 + " manufacturer3", Variable3 + " vendor3",
                "." + Variablegeneral + "-ProductFor0DollarOrderNonShippingProduct",
                Variablegeneral + " Product Types Include 4 Type Properties", "1");
            // ---Add Product 08 Bundle--- Start
            AddProductToStore(Variablegeneral, "." + Variable3 + "-Bundle-SKU-Chg-Ship-And-Hand", "5725,37", "5207,55",
                "5456,78", Variable3 + " Schedule3", "31", "32", "33", "34", "35,33", "Charge Shipping & Handling",
                Variable3 + " manufacturer3", Variable3 + " vendor3",
                "." + Variable3 + "-Bundle-Name-Shipping-Charges_Charge-Shipping-And-Handling",
                Variablegeneral + " Product Types Include 4 Type Properties", "0");
            AddBundleProducts();
            // ---Add Product 08 Bundle--- Finish

            AddToCart4ProductsWith4DifferentShippingChargesUse3CouponCodesAndPlaceOrder0055();
            OrderCheck4ProductsWith4DifferentShippingChargesAnd3CouponCodesUse0055();

            AddToCartProduct4AndPlaceOrder0060();
            // Bug 14764:Bug: Incorrect Order calculation if Default Shipping Rate=Shipping Rate and (or) Default Rate=Rate
            OrderCheckProduct4TaxScheduler0060();

            SwitchOnTax(); //---Order Placing with TAX Rule---
            AddToCart8ProductsUse3CouponCodesAndPlaceOrderWithTax0070();
            OrderCheck8ProductsAnd3CouponCodesUseWithTax0070();
            SwitchOnVat();

            SetThePaymentMethodsAuthorizeNet();
            AddToCart4ProductsWith4DifferentShippingChargesUse3CouponCodesAndPlaceOrder0055();
            OrderCheck4ProductsWith4DifferentShippingChargesAnd3CouponCodesUse0055();
            SetThePaymentMethodsTestGateway();

            The0OrderAllow();
            HandlingDisable();
            Add1ProductToCartAndPlaceOrderWithPreviousAddressNo0080();
            OrderCheck1ProductAndPlaceOrderWithPreviousAddressNo0080();
            The0OrderDisable();
            HandlingEnable();

            // ---Membership Product Type Start
            AddMembershipProductToCartAndPlaceOrderNo0090();
            OrderCheckMembershipProduct0090();
            // ---Membership Product Type Finish

            // ---Bundle Product Start
            AddBundleProductToCartAndPlaceOrderNo0100();
            OrderCheckBundleProduct0100();
            // ---Bundle Product Finish

            CategoryDrillDown();
        }
    }
}