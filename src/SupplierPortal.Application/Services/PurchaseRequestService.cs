using AutoMapper;
using Microsoft.Extensions.Logging;
using SupplierPortal.Application.DTOs;
using SupplierPortal.Domain.Entities;
using SupplierPortal.Domain.Enums;
using SupplierPortal.Domain.Interfaces;

namespace SupplierPortal.Application.Services;

public class PurchaseRequestService : IPurchaseRequestService
{
    private readonly IPurchaseRequestRepository _purchaseRequestRepository;
    private readonly ISupplierProfileRepository _supplierRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<PurchaseRequestService> _logger;

    public PurchaseRequestService(
        IPurchaseRequestRepository purchaseRequestRepository,
        ISupplierProfileRepository supplierRepository,
        IMapper mapper,
        ILogger<PurchaseRequestService> logger)
    {
        _purchaseRequestRepository = purchaseRequestRepository;
        _supplierRepository = supplierRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<PurchaseRequestDTO> CreatePurchaseRequestAsync(CreatePurchaseRequestDTO createDto)
    {
        try
        {
            // Verify supplier exists
            var supplier = await _supplierRepository.GetByIdAsync(createDto.SupplierId);
            if (supplier == null)
                throw new InvalidOperationException($"Supplier with ID {createDto.SupplierId} not found");

            string requestNumber = await _purchaseRequestRepository.GenerateRequestNumberAsync();
            // Create purchase request entity
            var purchaseRequest = new PurchaseRequest
            {
                RequestNumber = requestNumber,
                SupplierId = createDto.SupplierId,
                Status = PurchaseRequestStatus.Pending,
                Notes = createDto.Notes
            };

            // Add items
            foreach (var itemDto in createDto.Items)
            {
                purchaseRequest.Items!.Add(new PurchaseRequestItem
                {
                    ProductName = itemDto.ProductName,
                    Quantity = itemDto.Quantity,
                    Unit = itemDto.Unit,
                    IsPriced = false
                });
            }

            await _purchaseRequestRepository.AddAsync(purchaseRequest);
            _logger.LogInformation($"Purchase request created: {purchaseRequest.RequestNumber} for supplier {createDto.SupplierId}");

            return _mapper.Map<PurchaseRequestDTO>(purchaseRequest);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating purchase request");
            throw;
        }
    }

    public async Task<PurchaseRequestDTO?> GetPurchaseRequestByIdAsync(int id)
    {
        var purchaseRequest = await _purchaseRequestRepository.GetByIdAsync(id);
        return purchaseRequest == null ? null : _mapper.Map<PurchaseRequestDTO>(purchaseRequest);
    }

    public async Task<IEnumerable<PurchaseRequestDTO>> GetPurchaseRequestsBySupplierAsync(int supplierId)
    {
        var purchaseRequests = await _purchaseRequestRepository.GetBySupplierIdAsync(supplierId);
        return _mapper.Map<IEnumerable<PurchaseRequestDTO>>(purchaseRequests);
    }

    public async Task<IEnumerable<CompletedPurchaseRequestDTO>> GetCompletedPurchaseRequestsAsync()
    {
        var completedRequests = await _purchaseRequestRepository.GetCompletedRequestsAsync();
        return _mapper.Map<IEnumerable<CompletedPurchaseRequestDTO>>(completedRequests);
    }

    public async Task UpdatePurchaseRequestItemAsync(int purchaseRequestId, UpdatePurchaseRequestItemDTO updateDto)
    {
        try
        {
            var purchaseRequest = await _purchaseRequestRepository.GetByIdAsync(purchaseRequestId);
            if (purchaseRequest == null)
                throw new InvalidOperationException($"Purchase request with ID {purchaseRequestId} not found");

            // Domain method handles validation
            purchaseRequest.UpdateItemPrice(updateDto.ItemId, updateDto.Price, updateDto.DeliveryDate);

            // Mark request as in progress if just starting
            if (purchaseRequest.Status == PurchaseRequestStatus.Pending)
            {
                purchaseRequest.StartProgress();
            }

            await _purchaseRequestRepository.UpdateAsync(purchaseRequest);
            _logger.LogInformation($"Item {updateDto.ItemId} updated in request {purchaseRequestId}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating purchase request item");
            throw;
        }
    }

    public async Task CompletePurchaseRequestAsync(int purchaseRequestId)
    {
        try
        {
            var purchaseRequest = await _purchaseRequestRepository.GetByIdAsync(purchaseRequestId);
            if (purchaseRequest == null)
                throw new InvalidOperationException($"Purchase request with ID {purchaseRequestId} not found");

            // Domain method handles validation
            purchaseRequest.MarkAsCompleted();

            await _purchaseRequestRepository.UpdateAsync(purchaseRequest);
            _logger.LogInformation($"Purchase request completed: {purchaseRequest.RequestNumber}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error completing purchase request");
            throw;
        }
    }

    public async Task<IEnumerable<PurchaseRequestDTO>> GetPendingPurchaseRequestsAsync()
    {
        var pendingRequests = await _purchaseRequestRepository.GetByStatusAsync(PurchaseRequestStatus.Pending);
        return _mapper.Map<IEnumerable<PurchaseRequestDTO>>(pendingRequests);
    }
}