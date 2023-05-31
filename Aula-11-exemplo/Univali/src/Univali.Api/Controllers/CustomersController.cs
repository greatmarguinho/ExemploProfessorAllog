
using Microsoft.AspNetCore.Mvc;
using Univali.Api.Entities;
using Univali.Api.Models;

namespace Univali.Api.Controllers;

[ApiController]
[Route("api/customers")]
public class CustomersController : ControllerBase
{
    [HttpGet]
    public ActionResult<IEnumerable<Customer>> GetCustomers()
    {
        var result = Data.Instance.Customers;
        return Ok(result);
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
    public ActionResult<Customer> GetCustomerByCpf(string cpf)
    {
        Console.WriteLine($"cpf: {cpf}");
        var customer = Data.Instance.Customers.FirstOrDefault(c => c.Cpf == cpf);

        if(customer == null)
        {
            return NotFound();
        }

        return Ok(customer);
    }

    [HttpPost]
    public ActionResult<Customer> CreateCustomer( Customer customer)
    {
        var newCustomer = new Customer()
        {
            Id = Data.Instance.Customers.Max(c => c.Id) + 1,
            Name = customer.Name,
            Cpf = customer.Cpf
        };

        Data.Instance.Customers.Add(newCustomer);
        return CreatedAtRoute
        (
            "GetCustomerById",
            new {id = newCustomer.Id },
            newCustomer
        );
    }

}