namespace Univali.Api.Models;

public class CustomerForUpdateDto
{
    public int Id {get; set;}
    public string Name{get; set;} = string.Empty;
    public string Cpf{get; set;} = string.Empty;   
}