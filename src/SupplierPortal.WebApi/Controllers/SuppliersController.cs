using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using SupplierPortal.Application.DTOs;
using SupplierPortal.Application.Services;

namespace SupplierPortal.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]

public class SuppliersController : ControllerBase
{
    private readonly ISupplierService _supplierService;

    public SuppliersController(ISupplierService supplierService)
    {
        _supplierService = supplierService;
    }

    [HttpPost]
    public async Task<ActionResult<SupplierResultDTO>> CreateSupplier([FromBody] CreateSupplierDTO createSupplierDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var supplier = await _supplierService.CreateSupplierAsync(createSupplierDto);
            Log.Information("Supplier created: {@SupplierCode}", supplier.SupplierProfileDTO!.Code);
            return CreatedAtAction(nameof(GetSupplierById), new { id = supplier.Id }, supplier);
        }
        catch (InvalidOperationException ex)
        {
            Log.Warning(ex, "Invalid operation while creating supplier");
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error creating supplier");
            return StatusCode(500, new { message = "An error occurred while creating the supplier" });
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<SupplierResultDTO>> GetSupplierById(int id)
    {
        var supplier = await _supplierService.GetSupplierByIdAsync(id);
        if (supplier == null)
            return NotFound(new { message = "Supplier not found" });

        return Ok(supplier);
    }

    [HttpGet("username/{username}")]
    public async Task<ActionResult<SupplierResultDTO>> GetSupplierByUsername(string username)
    {
        var supplier = await _supplierService.GetSupplierByUsernameAsync(username);
        if (supplier == null)
            return NotFound(new { message = "Supplier not found" });

        return Ok(supplier);
    }

    [HttpGet("code/{code}")]
    public async Task<ActionResult<SupplierResultDTO>> GetByCodeAsync(string code)
    {
        var supplier = await _supplierService.GetByCodeAsync(code);
        if (supplier == null)
            return NotFound(new { message = "Supplier not found" });

        return Ok(supplier);
    }

    [HttpGet("title/{title}")]
    public async Task<ActionResult<SupplierResultDTO>> GetByTitleAsync(string title)
    {
        var supplier = await _supplierService.GetByTitleAsync(title);
        if (supplier == null)
            return NotFound(new { message = "Supplier not found" });

        return Ok(supplier);
    }

    [HttpGet("email/{email}")]
    public async Task<ActionResult<SupplierResultDTO>> GetByEmailAsync(string email)
    {
        var supplier = await _supplierService.GetByEmailAsync(email);
        if (supplier == null)
            return NotFound(new { message = "Supplier not found" });

        return Ok(supplier);
    }

    [HttpGet("active-suppliers")]
    public async Task<ActionResult<IEnumerable<SupplierResultDTO>>> GetActiveSuppliersAsync()
    {
        var supplier = await _supplierService.GetAllSuppliersAsync();
        if (supplier == null)
            return NotFound(new { message = "Supplier not found" });

        return Ok(supplier);
    }

    [HttpGet]
    //[Authorize(Roles = "Admin,User")]
    public async Task<ActionResult<IEnumerable<SupplierResultDTO>>> GetAllSuppliers()
    {
        var suppliers = await _supplierService.GetAllSuppliersAsync();
        return Ok(suppliers);
    }

    [HttpPost("verify")]
    public async Task<ActionResult> VerifyCredentials([FromBody] VerifyCredentialsDTO credentials)
    {
        var isValid = await _supplierService.VerifySupplierPasswordAsync(credentials.Username, credentials.Password);
        if (!isValid)
            return Unauthorized(new { message = "Invalid credentials" });

        return Ok(new { message = "Credentials verified" });
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateSupplier(int id, [FromBody] SupplierDTO supplierDto)
    {
        try
        {
            await _supplierService.UpdateSupplierAsync(id, supplierDto);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteSupplier(int id)
    {
        try
        {
            await _supplierService.DeleteSupplierAsync(id);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
}
public class VerifyCredentialsDTO
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}