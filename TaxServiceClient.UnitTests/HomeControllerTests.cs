using System;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using Xunit;
using TaxServiceClient.Controllers;
using TaxServiceClient.Services;
using TaxServiceClient.Models;
using System.Threading.Tasks;
using Moq;

namespace TaxServiceClient.UnitTests
{
    public class HomeControllerTests
    {
        [Fact]
        public async Task GetRates_Zip_ReturnRate()
        {
            //Setup
            RateResult expectedRateResult = new RateResult();
            expectedRateResult.rate = new Rate();
            expectedRateResult.rate.state_rate = "0.0625";
            expectedRateResult.rate.zip = "90002";
            string zip = "78665";

            //Arrange
            var mockTaxJarService = new Mock<ITaxJarCalculatorService>();
            mockTaxJarService.Setup(t => t.GetRatesByLocation(It.IsAny<string>())).ReturnsAsync(expectedRateResult);
            HomeController controller = new HomeController(mockTaxJarService.Object);
            //Act
            var rateResponse = await controller.GetRatesWithRequest(zip);

            //Assert
            Assert.Equal("0.0625", rateResponse.rate.state_rate);
            Assert.Equal("90002", rateResponse.rate.zip);
        }
        [Fact]
        public async Task CalculateSalesTax_OrderRequest_ReturnOrderResponse()
        {
            //Setup
            OrderRequest postRequest = new OrderRequest();
            postRequest.from_country = "US";
            postRequest.from_state = "CA";
            postRequest.from_zip = "92093";
            postRequest.from_city = "La Jolla";
            postRequest.from_street = "9500 Gilman Drive";
            postRequest.to_country = "US";
            postRequest.to_zip = "90002";
            postRequest.to_state = "CA";
            postRequest.amount = 15;
            postRequest.shipping = 1;

            OrderResponse expectedResponse = new OrderResponse();
            expectedResponse.tax = new Tax();
            expectedResponse.tax.amount_to_collect = (float)1.54;
            expectedResponse.tax.freight_taxable = false;
            expectedResponse.tax.has_nexus = true;
            expectedResponse.tax.jurisdictions = new Jurisdictions();
            expectedResponse.tax.jurisdictions.city = "LYNWOOD";
            expectedResponse.tax.jurisdictions.state = "CA";
            expectedResponse.tax.jurisdictions.country = "US";
            expectedResponse.tax.jurisdictions.county = "LOS ANGELES COUNTY";
            expectedResponse.tax.order_total_amount = (float)16.0;
            expectedResponse.tax.rate = (float)0.1025;
            expectedResponse.tax.shipping = (float)1.0;
            expectedResponse.tax.tax_source = "destination";
            expectedResponse.tax.taxable_amount = (float)15.0;


            //Arrange
            var mockTaxJarService = new Mock<ITaxJarCalculatorService>();
            mockTaxJarService.Setup(t => t.CalculateSalesTax(It.IsAny<OrderRequest>())).ReturnsAsync(expectedResponse);
            HomeController controller = new HomeController(mockTaxJarService.Object);
            //Act
            var rateResponse = await controller.CalculateSalesTaxWithRequest(postRequest);

            //Assert
            Assert.Equal(expectedResponse,rateResponse);
        }
    }
}




