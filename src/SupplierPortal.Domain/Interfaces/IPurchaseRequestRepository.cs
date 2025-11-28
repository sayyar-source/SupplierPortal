using SupplierPortal.Domain.Entities;
using SupplierPortal.Domain.Enums;

namespace SupplierPortal.Domain.Interfaces;

public interface IPurchaseRequestRepository : IRepository<PurchaseRequest>
{
    Task<IEnumerable<PurchaseRequest>> GetBySupplierIdAsync(int supplierId);
    Task<IEnumerable<PurchaseRequest>> GetCompletedRequestsAsync();
    Task<IEnumerable<PurchaseRequest>> GetByStatusAsync(PurchaseRequestStatus status);
    Task<PurchaseRequest?> GetByRequestNumberAsync(string requestNumber);
    Task<string> GenerateRequestNumberAsync();
}