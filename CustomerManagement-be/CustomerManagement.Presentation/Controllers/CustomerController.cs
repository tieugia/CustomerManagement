using CusstomerManagement.Application.DTOs;
using CustomerManagement.Application.Interfaces.Services;
using CustomerManagement.Common.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CustomerManagement.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class CustomerController : ControllerBase
{
    private readonly ICustomerService _customerService;
    public CustomerController(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var customers = await _customerService.GetAllAsync();
        if (customers == null || !customers.Any())
            return Ok();

        return Ok(customers);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get([NotEmptyGuid] Guid id)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(await _customerService.GetByIdAsync(id));
    }

    [HttpPost]
    public async Task<IActionResult> Post(AddCustomerDto addCustomerDto)
    {
        var createdDto = await _customerService.CreateAsync(addCustomerDto);
        if (createdDto == null)
            return BadRequest("Please provide a valid customer.");

        return CreatedAtAction(nameof(Get), new { id = addCustomerDto.Id }, addCustomerDto);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put([NotEmptyGuid] Guid id, UpdateCustomerDto updateCustomerDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (id != updateCustomerDto.Id)
            return BadRequest("Customer Id mismatch");

        await _customerService.UpdateAsync(updateCustomerDto);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([NotEmptyGuid] Guid id)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        await _customerService.DeleteAsync(id);
        return NoContent();
    }
}