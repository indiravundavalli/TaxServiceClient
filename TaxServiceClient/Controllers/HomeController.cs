using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TaxServiceClient.Models;
using TaxServiceClient.Services;

namespace TaxServiceClient.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ITaxJarCalculatorService _taxJarCalculatorService;      

        public HomeController(
            ITaxJarCalculatorService taxJarCalculator)
        {
            _taxJarCalculatorService = taxJarCalculator;
            
        }

        public async Task<IActionResult> CalculateSalesTax()
        {
            OrderRequest orderRequest = new OrderRequest();
            orderRequest.from_country = "US";
            orderRequest.from_state = "CA";
            orderRequest.from_zip = "92093";
            orderRequest.to_country = "US";
            orderRequest.to_zip = "90002";
            orderRequest.to_state = "CA";
            orderRequest.amount = 15;
            orderRequest.shipping = 1;
            var salesTax = await CalculateSalesTaxWithRequest(orderRequest);

            return Json(salesTax);
        }

        public async Task<IActionResult> GetRates(string zip)
        {
            try
            {
                var rate = await _taxJarCalculatorService.GetRatesByLocation(zip);

                return Json(rate);
            }
            catch (Exception ex)
            {
                Debug.Write(ex);

                return null;
            }
        }
        public async Task<RateResult> GetRatesWithRequest(string zip)
        {
            try
            {
                var rate = await _taxJarCalculatorService.GetRatesByLocation(zip);

                return rate;
            }
            catch (Exception ex)
            {
                Debug.Write(ex);

                return null;
            }
        }
        public async Task<OrderResponse> CalculateSalesTaxWithRequest(OrderRequest orderRequest)
        {
            try
            {
               var salesTax = await _taxJarCalculatorService.CalculateSalesTax(orderRequest);

                return salesTax;
            }
            catch (Exception ex)
            {
                Debug.Write(ex);

                return null;
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
