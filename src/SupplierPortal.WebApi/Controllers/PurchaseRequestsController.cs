using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using SupplierPortal.Application.DTOs;
using SupplierPortal.Application.Services;

namespace SupplierPortal.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PurchaseRequestsController : ControllerBase
{
    private readonly IPurchaseRequestService _purchaseRequestService;

    public PurchaseRequestsController(IPurchaseRequestService purchaseRequestService)
    {
        _purchaseRequestService = purchaseRequestService;
    }

    [HttpPost]
    public async Task<ActionResult<PurchaseRequestDTO>> CreatePurchaseRequest([FromBody] CreatePurchaseRequestDTO createDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var purchaseRequest = await _purchaseRequestService.CreatePurchaseRequestAsync(createDto);
            Log.Information("Purchase request created: {@RequestNumber}", purchaseRequest.RequestNumber);
            return CreatedAtAction(nameof(GetPurchaseRequestById), new { id = purchaseRequest.Id }, purchaseRequest);
        }
        catch (InvalidOperationException ex)
        {
            Log.Warning(ex, "Invalid operation while creating purchase request");
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error creating purchase request");
            return StatusCode(500, new { message = "An error occurred while creating the purchase request" });
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PurchaseRequestDTO>> GetPurchaseRequestById(int id)
    {
        var purchaseRequest = await _purchaseRequestService.GetPurchaseRequestByIdAsync(id);
        if (purchaseRequest == null)
            return NotFound(new { message = "Purchase request not found" });

        return Ok(purchaseRequest);
    }

    [HttpGet("supplier/{supplierId}")]
    public async Task<ActionResult<IEnumerable<PurchaseRequestDTO>>> GetRequestsBySupplier(int supplierId)
    {
        var requests = await _purchaseRequestService.GetPurchaseRequestsBySupplierAsync(supplierId);
        return Ok(requests);
    }

    [HttpGet("status/pending")]
    public async Task<ActionResult<IEnumerable<PurchaseRequestDTO>>> GetPendingRequests()
    {
        var requests = await _purchaseRequestService.GetPendingPurchaseRequestsAsync();
        return Ok(requests);
    }

    [HttpPut("{purchaseRequestId}/item")]
    public async Task<ActionResult> UpdateRequestItem(int purchaseRequestId, [FromBody] UpdatePurchaseRequestItemDTO updateDto)
    {
        try
        {
            await _purchaseRequestService.UpdatePurchaseRequestItemAsync(purchaseRequestId, updateDto);
            Log.Information("Purchase request item updated: RequestId={RequestId}, ItemId={ItemId}", purchaseRequestId, updateDto.ItemId);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            Log.Warning(ex, "Invalid operation while updating purchase request item");
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error updating purchase request item");
            return StatusCode(500, new { message = "An error occurred while updating the item" });
        }
    }

    [HttpPost("{id}/complete")]
    public async Task<ActionResult> CompletePurchaseRequest(int id)
    {
        try
        {
            await _purchaseRequestService.CompletePurchaseRequestAsync(id);
            Log.Information("Purchase request completed: Id={RequestId}", id);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            Log.Warning(ex, "Invalid operation while completing purchase request");
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error completing purchase request");
            return StatusCode(500, new { message = "An error occurred while completing the request" });
        }
    }

    [HttpGet("completed")]
    public async Task<ActionResult<IEnumerable<CompletedPurchaseRequestDTO>>> GetCompletedRequests()
    {
        try
        {
            var completedRequests = await _purchaseRequestService.GetCompletedPurchaseRequestsAsync();
            Log.Information("Retrieved {Count} completed purchase requests", completedRequests.Count());
            return Ok(completedRequests);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error retrieving completed purchase requests");
            return StatusCode(500, new { message = "An error occurred while retrieving completed requests" });
        }
    }
}