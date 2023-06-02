using Microsoft.AspNetCore.Mvc;
using Univali.Api.Models;

namespace Univali.Api.Controllers;

[ApiController]
[Route("api/customers/{customerId}/addresses")]
public class AddressController : ControllerBase
{
    [HttpGet]
    public ActionResult <IEnumerable<AddressDto>> GetAddresses(int customerId)
    {
        var customerFromDatabase = Data.Instance.Customers.FirstOrDefault(customer => customer.Id == customerId);

        if(customerFromDatabase == null) return NotFound();
        var addressesToReturn = new List<AddressDto>();

        foreach(var address in customerFromDatabase.Addresses){
            addressesToReturn.Add(new AddressDto{
                Id = address.Id,
                Street = address.Street,
                City = address.City
            });
        }
        return Ok(addressesToReturn);
    }

    // Nesse falta implementar Dto
    [HttpGet("{addressId}")]
    public ActionResult<AddressDto> GetAddress(int customerId, int addressId)
    {
        var addressToReturn = Data.Instance.Customers.FirstOrDefault(customer => customer.Id == customerId)?.Addresses.FirstOrDefault(address => address.Id == addressId);

        return addressToReturn != null ? Ok(addressToReturn) : NotFound();
    }
}