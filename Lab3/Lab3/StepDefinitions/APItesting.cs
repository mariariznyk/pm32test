using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using NUnit.Framework.Internal;
using RestSharp;
using System;
using System.Net;
using TechTalk.SpecFlow;

namespace SpecFlowRestApi.Steps
{
    [Binding]
    public class APItesting
    {
        private RestClient client;
        private RestRequest request;
        private RestResponse response;

        [Given(@"connect to (.*)")]
        public void GivenConnectTo(string url)
        {
            client = new RestClient(url);

        }

        [Given(@"create (.*) request to (.*)")]
        public void GivenCreateRequest(string method, string url)
        {
            Method restMethod;
            switch (method.ToUpper())
            {
                case "GET":
                    restMethod = Method.Get;
                    break;
                case "POST":
                    restMethod = Method.Post;
                    break;
                case "DELETE":
                    restMethod = Method.Delete;
                    break;
                case "PUT":
                    restMethod = Method.Put;
                    break;
                default:
                    throw new ArgumentException($"Unsupported HTTP method: {method}");
            }

            request = new RestRequest(url, restMethod);
        }

        [Given(@"add authorization token")]
        public void GivenAddAuthToken()
        {
            RestRequest authRequest = new RestRequest("auth", Method.Post);
            authRequest.AddJsonBody(new
            {
                username = "admin",
                password = "password123"
            });

            RestResponse authResponse = client.Execute(authRequest);
            dynamic authResponseObject = JObject.Parse(authResponse.Content);
            string token = authResponseObject.token;

            Uri uri = client.BuildUri(request);
            string path = uri.AbsolutePath;
            string domain = uri.Host;
            request.AddCookie("token", token, path, domain);
        }

        [Given(@"add header (.*) with value (.*)")]
        public void GivenAddHeader(string header, string value)
        {
            request.AddHeader(header, value);
        }
        [Given(@"add parameter (.*) with value (.*)")]
        public void GivenAddParameter(string parameter, string value)
        {
            request.AddParameter(parameter, value);
        }

        [When(@"send request")]
        public void WhenSendRequest()
        {
            response = client.Execute(request);
        }
        public int id;
        [Given(@"create booking")]
        public void GivenCreateBooking()
        {
            APItesting t = new APItesting();
            t.GivenConnectTo("https://restful-booker.herokuapp.com");
            t.GivenCreateRequest("POST", "booking");
            t.GivenAddHeader("Accept", "application/json");
            t.GivenAddJSONWithBooking();
            t.WhenSendRequest();
            string jsonResponse = t.response.Content;
            BookingAndID bookingResponse = JsonConvert.DeserializeObject<BookingAndID>(jsonResponse);
            id = bookingResponse.bookingid;
        }
        [Given(@"set parameter id")]
        public void GivenSetParameterID()
        {
            request.AddParameter("id", id, ParameterType.UrlSegment);
        }

        [Then(@"response is (.*)")]
        public void ThenResponseIs(string expectedResponseType)
        {
            HttpStatusCode expectedStatusCode;
            switch (expectedResponseType.ToUpper())
            {
                case "OK":
                    expectedStatusCode = HttpStatusCode.OK;
                    break;
                case "CREATED":
                    expectedStatusCode = HttpStatusCode.Created;
                    break;
                default:
                    throw new ArgumentException($"Unsupported response type: {expectedResponseType}");
            }

            Assert.That(response.StatusCode, Is.EqualTo(expectedStatusCode));
        }
        [Given(@"add json with booking")]
        public void GivenAddJSONWithBooking()
        {
            var booking = new Booking
            {
                firstname = "Jim",
                lastname = "Brown",
                totalprice = 111,
                depositpaid = true,
                bookingdates = new BookingDates
                {
                    checkin = "2018-01-01",
                    checkout = "2019-01-01"
                },
                additionalneeds = "Breakfast"
            };
            request.AddJsonBody(booking);
        }

        [Then(@"response contains json with booking and booking ID")]
        public void ThenResponseContainsJSONWithBookingAndBokingID()
        {
            string jsonResponse = response.Content;
            BookingAndID bookingResponse = JsonConvert.DeserializeObject<BookingAndID>(jsonResponse);

            Assert.GreaterOrEqual(bookingResponse.bookingid, 0);
            Assert.AreEqual("Jim", bookingResponse.booking.firstname);
            Assert.AreEqual("Brown", bookingResponse.booking.lastname);
            Assert.AreEqual(111, bookingResponse.booking.totalprice);
            Assert.IsTrue(bookingResponse.booking.depositpaid);
            Assert.AreEqual("2018-01-01", bookingResponse.booking.bookingdates.checkin);
            Assert.AreEqual("2019-01-01", bookingResponse.booking.bookingdates.checkout);
            Assert.AreEqual("Breakfast", bookingResponse.booking.additionalneeds);
        }
        [Then(@"response contains json with booking")]
        public void ThenResponseContainsJSONWithBooking()
        {
            string jsonResponse = response.Content;
            Booking bookingResponse = JsonConvert.DeserializeObject<Booking>(jsonResponse);

            Assert.AreEqual("Jim", bookingResponse.firstname);
            Assert.AreEqual("Brown", bookingResponse.lastname);
            Assert.AreEqual(111, bookingResponse.totalprice);
            Assert.IsTrue(bookingResponse.depositpaid);
            Assert.AreEqual("2018-01-01", bookingResponse.bookingdates.checkin);
            Assert.AreEqual("2019-01-01", bookingResponse.bookingdates.checkout);
            Assert.AreEqual("Breakfast", bookingResponse.additionalneeds);
        }
        [Then(@"response contains booking IDs")]
        public void ThenResponseContainsBookingIDs()
        {
            List<BookingIdResponse> bookingIdResponses = JsonConvert.DeserializeObject<List<BookingIdResponse>>(response.Content);

            foreach (var bookingIdResponse in bookingIdResponses)
            {
                Assert.IsTrue(bookingIdResponse.bookingid >= 0);
            }
        }

        [Then(@"response contains json with value of new currency")]
        public void ThenResponseContainsJSONWithValueOfNewCurrency()
        {
            string jsonResponse = response.Content;
            CurrencyConversionResponse currencyResponse = JsonConvert.DeserializeObject<CurrencyConversionResponse>(jsonResponse);
            Assert.IsTrue(currencyResponse.Valid, "The 'valid' field is not true");
            Assert.AreEqual("USD", currencyResponse.FromType, "Invalid 'from-type' value");
            Assert.AreEqual(100, currencyResponse.FromValue, "Invalid 'from-value' value");
            Assert.IsTrue(currencyResponse.Result > 0, "Invalid 'result' value");
            Assert.IsTrue(currencyResponse.ResultFloat > 0, "Invalid 'result-float' value");
            Assert.AreEqual("EUR", currencyResponse.ToType, "Invalid 'to-type' value");
        }
    }
}
