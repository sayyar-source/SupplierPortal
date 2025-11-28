using Microsoft.AspNetCore.Mvc;
using SupplierPortal.Blazor.DTO;
using System.Net.Http.Headers;

namespace SupplierPortal.Blazor.Services;


public class PurchaseService
{
    private HttpClient client;
    private string token=string.Empty;
    public PurchaseService(IHttpClientFactory httpClientFactory)
    {
        client = httpClientFactory.CreateClient("ApiClient");
         
    }
    public async Task<IEnumerable<PurchaseRequestDTO>> GetRequestsBySupplierAsync(int supplierId,string token)
    {
        try
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            string url = $"api/PurchaseRequests/supplier/{supplierId}";

            var response = await client.GetAsync(url);

            // Throw if request failed
            response.EnsureSuccessStatusCode();

            // Deserialize the response body
            var purchases = await response.Content.ReadFromJsonAsync<IEnumerable<PurchaseRequestDTO>>();

            return purchases ?? Enumerable.Empty<PurchaseRequestDTO>();
        }
        catch (Exception)
        {

            throw;
        }


    }
    public async Task<PurchaseRequestDTO> GetPurchaseRequestByIdAsync(int id,string token)
    {
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        string url = $"api/PurchaseRequests/{id}";
        var response = await client.GetAsync(url);

        // Throw if request failed
        response.EnsureSuccessStatusCode();

        // Deserialize the response body
        var purchases = await response.Content.ReadFromJsonAsync<PurchaseRequestDTO>();

        return purchases!;
    }

    public async Task UpdateRequestItemAsync(int purchaseRequestId, UpdatePurchaseRequestItemDTO updateDto,string token)
    {
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        string url = $"api/PurchaseRequests/{purchaseRequestId}/item";
        var response = await client.PutAsync(url, JsonContent.Create(updateDto));
    }

    public async Task CompletePurchaseRequest(int id, string token)
    {
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        string url = $"api/PurchaseRequests/{id}/complete";
        var respone = await client.PostAsync(url, null);
    }
}
