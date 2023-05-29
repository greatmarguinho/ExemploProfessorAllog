using Microsoft.AspNetCore.Mvc;
using Univali.Api.Entities;

namespace Univali.Api.Controllers;

[ApiController]
[Route("api/Customers")]
public class CustomersController : ControllerBase
{
    [HttpGet]
    public ActionResult<IEnumerable<Customer>> GetCustomers()
    {
        var result = Data.Instance.Customers;
            
        return Ok(result);
    }

    [HttpGet("{id}", Name = "GetCustomerById")]
    public ActionResult<Customer> GetCustomerById(int id)
    {
        Console.WriteLine($"id: {id}");
        var customer = Data.Instance.Customers.FirstOrDefault(c => c.Id == id);
        if(customer == null){
            return NotFound();
        }
            return Ok(customer);
        // return customer != null ? Ok(customer) : NotFound();
    }

    
    [HttpGet("cpf/{cpf}")]
    public ActionResult<Customer> GetCustomerByCpf (string cpf)
    {
        Console.WriteLine($"cpf: {cpf}");
        var customer = Data.Instance.Customers.FirstOrDefault(c => c.Cpf == cpf);
        if(customer == null){
            return NotFound();
        }
            return Ok(customer);
        // return customer != null ? Ok(customer) : NotFound();
    }

    [HttpPost]
    public ActionResult<Customer> CreateCustomer(Customer customer)
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
            new {id = newCustomer.Id},
            newCustomer
        );
    }

    [HttpDelete("{id}")]
    public ActionResult<Customer> DeleteCustomer(int id)
    {
        var customer = Data.Instance.Customers.FirstOrDefault(c => c.Id == id);   
        if(customer == null)
        {
            return NotFound();
        }
            Data.Instance.Customers.Remove(customer);
            return NoContent();
    }

    [HttpPut("{id}")]
    public ActionResult<Customer> UpdateCustomer(int id, Customer customer){

        var updateCustomer = Data.Instance.Customers.FirstOrDefault(c => c.Id == id);
        if(updateCustomer == null)
        {
            return NotFound();
        }
            updateCustomer.Name = customer.Name;
            updateCustomer.Cpf = customer.Cpf;
            return NoContent();
    }
}
