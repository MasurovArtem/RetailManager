using System;
using System.Net.Http;
using System.Threading.Tasks;
using RMDesktopUI.Library.Models;

namespace RMDesktopUI.Library.Api
{
    public class SaleEndPoint : ISaleEndPoint
    {
        private readonly IApiHelper _apiHelper;

        public SaleEndPoint(IApiHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }

        public async Task PostSale(SaleModel sale)
        {
            using (HttpResponseMessage response = await _apiHelper.ApiClient.PostAsJsonAsync("/api/Sale", sale))
            {
                if (response.IsSuccessStatusCode)
                {
                    //Log successful call??
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

    }
}