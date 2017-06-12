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

namespace Hotcakes.AutomatedTesting
{
    /// <summary>
    ///     These are the global values used with many of the automated tests.
    /// </summary>
    /// <remarks>
    ///     The test process can take up to an hour, depending on the computer running the test.
    /// </remarks>
    public class Globals
    {
        /// <summary>
        ///     At the minimum, this value should most likely be changed prior to beginning the test process.
        /// </summary>
        /// <remarks>
        ///     Should contain a fully qualified domain with no trailing slash, such as: 
        /// 
        ///     http://mytesturl.localhost
        /// 
        ///     NOT:
        /// 
        ///     http://mytesturl.localhost/
        /// </remarks>
        public const string BASE_PATH = "http://mytesturl.localhost";

        /// <summary>
        ///     The superuser username you used when installing the site.
        /// </summary>
        public const string ADMIN_USERNAME = "host";

        /// <summary>
        ///     The superuser password you used when installing the site.
        /// </summary>
        public const string ADMIN_PASSWORD = "password";

        // -------------------------------------------------------------------- //
        //
        //  The values below generally remain unchanged
        //
        // -------------------------------------------------------------------- //

        public static TimeSpan TIMEOUT
        {
            get { return TimeSpan.FromSeconds(120); }
        }

        public const string CREDIT_CARD_VISA = "4012888888881881";
        public const string CREDIT_CARD_VISA2 = "4111111111111111";
        public const string CREDIT_CARD_MC = "5555555555554444";
        public const string CREDIT_CARD_MC2 = "5105105105105100";
        public const string CREDIT_CARD_DISCOVER = "6011000990139424";
        public const string CREDIT_CARD_AMEX = "378282246310005";
        public const string CREDIT_CARD_JCB = "3530111333300000";
        public const string CREDIT_CARD_JCB2 = "3566002020360505";
        public const string CREDIT_CARD_DINERS = "30569309025904";

        public const string STORE_ADDRESS_COUNTRY = "United States";
        public const string STORE_ADDRESS_REGION = "Florida";
        public const string STORE_ADDRESS_STREET1 = "319 N CLEMATIS ST";
        public const string STORE_ADDRESS_CITY = "WEST PALM BCH";
        public const string STORE_ADDRESS_POSTALCODE = "33401";
        public const string STORE_ADDRESS_PHONE = "561.714.7926";

        public const string BILLING_ADDRESS_COUNTRY = "United States";
        public const string BILLING_ADDRESS_REGION = "Florida";
        public const string BILLING_ADDRESS_STREET1 = "319 N CLEMATIS ST";
        public const string BILLING_ADDRESS_CITY = "WEST PALM BCH";
        public const string BILLING_ADDRESS_POSTALCODE = "33401";
        public const string BILLING_ADDRESS_PHONE = "5617147926";

        public const string SHIPPING_ADDRESS_COUNTRY = "United States";
        public const string SHIPPING_ADDRESS_REGION = "California";
        public const string SHIPPING_ADDRESS_STREET1 = "890 LAUREL ST";
        public const string SHIPPING_ADDRESS_CITY = "SAN CARLOS";
        public const string SHIPPING_ADDRESS_POSTALCODE = "94070";
        public const string SHIPPING_ADDRESS_PHONE = "1234567890";
    }
}