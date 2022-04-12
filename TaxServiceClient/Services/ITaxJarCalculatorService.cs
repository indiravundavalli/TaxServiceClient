using TaxServiceClient.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TaxServiceClient.Services
{
    public interface ITaxJarCalculatorService
    {
        Task<RateResult> GetRatesByLocation(string zip);
        Task<OrderResponse> CalculateSalesTax(OrderRequest request);

    }
}
