using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Univali.Api.Entities;
using Univali.Api.Models;

namespace Univali.Api.Controllers;

[ApiController]
[Route("api/customers")]
public class CustomersController : ControllerBase
{
    [HttpGet]
    public ActionResult<IEnumerable<CustomerDto>> GetCustomers()
    {
        var customersToReturn = Data.Instance.Customers.Select(customer => new CustomerDto
        {
            Id = customer.Id,
            Name = customer.Name,
            Cpf = customer.Cpf
        });
        return Ok(customersToReturn);
    }

    [HttpGet("{id}", Name = "GetCustomerById")]
    public ActionResult<CustomerDto> GetCustomerById(int id)
    {
        Console.WriteLine($"id: {id}");
        var customerFromDatabase = Data.Instance.Customers.FirstOrDefault(c => c.Id == id);

        if (customerFromDatabase == null) return NotFound();
        CustomerDto customerToReturn = new CustomerDto
        {
            Id = customerFromDatabase.Id,
            Name = customerFromDatabase.Name,
            Cpf = customerFromDatabase.Cpf
        };
        return Ok(customerToReturn);
        // return customer != null ? Ok(customer) : NotFound();
    }


    [HttpGet("cpf/{cpf}")]
    public ActionResult<CustomerDto> GetCustomerByCpf(string cpf)
    {
        var customerFromDatabase = Data.Instance.Customers.FirstOrDefault(c => c.Cpf == cpf);

        if (customerFromDatabase == null)
        {
            return NotFound();
        }

        CustomerDto customerToReturn = new CustomerDto
        {
            Id = customerFromDatabase.Id,
            Name = customerFromDatabase.Name,
            Cpf = customerFromDatabase.Cpf
        };
        return Ok(customerToReturn);
    }

    [HttpPost]
    public ActionResult<CustomerDto> CreateCustomer(CustomerForCreationDto customerForCreationDto)
    {
        if (!ModelState.IsValid)
        {
            Response.ContentType = "application/problem+json";
            //Cria a f[abrica de um objeto de detalhes de problema de validação
            var problemDetailsFactory = HttpContext.RequestServices.GetRequiredService<ProblemDetailsFactory>();
            //Cria um objeto de detalhes de problema de validação
            var validationProblemDetails = problemDetailsFactory.CreateValidationProblemDetails(HttpContext, ModelState);
            
            //Atribui o status code 422 no corpo do response
            validationProblemDetails.Status = StatusCodes.Status422UnprocessableEntity;
            return UnprocessableEntity(validationProblemDetails);
        }
        var customerEntity = new Customer()
        {
            Id = Data.Instance.Customers.Max(c => c.Id) + 1,
            Name = customerForCreationDto.Name,
            Cpf = customerForCreationDto.Cpf
        };

        Data.Instance.Customers.Add(customerEntity);

        CustomerDto customerToReturn = new CustomerDto()
        {
            Id = customerEntity.Id,
            Name = customerForCreationDto.Name,
            Cpf = customerForCreationDto.Cpf
        };
        return CreatedAtRoute
        (
            "GetCustomerById",
            new { id = customerToReturn.Id },
            customerToReturn
        );
    }

    [HttpPut("{id}")]
    public ActionResult UpdateCustomer(int id, CustomerForUpdateDto customerForUpdateDto)
    {
        if (id != customerForUpdateDto.Id) return BadRequest();

        var customerFromDatabase = Data.Instance.Customers
        .FirstOrDefault(customerForUpdateDto => customerForUpdateDto.Id == id);

        if (customerFromDatabase == null) return NotFound();

        customerFromDatabase.Name = customerForUpdateDto.Name;
        customerFromDatabase.Cpf = customerForUpdateDto.Cpf;

        return NoContent();
    }

    [HttpDelete("{id}")]
    public ActionResult DeleteCustomer(int id)
    {
        var customerFromDatabase = Data.Instance.Customers.FirstOrDefault(customer => customer.Id == id);
        if (customerFromDatabase == null) return NotFound();
        Data.Instance.Customers.Remove(customerFromDatabase);

        return NoContent();
    }

    [HttpPatch("{id}")]
    public ActionResult PartiallyUpdateCustomer(
        [FromBody] JsonPatchDocument<CustomerForPatchDto> patchDocument,
        [FromRoute] int id)
    {
        var customerFromDatabase = Data.Instance.Customers.FirstOrDefault(customer => customer.Id == id);

        if (customerFromDatabase == null) return NotFound();

        var customerToPatch = new CustomerForPatchDto
        {
            Name = customerFromDatabase.Name,
            Cpf = customerFromDatabase.Cpf
        };

        patchDocument.ApplyTo(customerToPatch);

        customerFromDatabase.Name = customerToPatch.Name;
        customerFromDatabase.Cpf = customerToPatch.Cpf;

        return NoContent();
    }


    [HttpGet("with-address")]
    public ActionResult<IEnumerable<CustomerWithAddressesDto>> GetCustomersWithAddresses()
    {
        var customersFromDatabase = Data.Instance.Customers;

        var customersToReturn = customersFromDatabase
        .Select(customer => new CustomerWithAddressesDto
        {
            Id = customer.Id,
            Name = customer.Name,
            Cpf = customer.Cpf,
            Addresses = customer.Addresses.Select(address => new AddressDto
            {
                Id = address.Id,
                City = address.City,
                Street = address.Street
            }).ToList()
        });
        return Ok(customersToReturn);
    }

    
}