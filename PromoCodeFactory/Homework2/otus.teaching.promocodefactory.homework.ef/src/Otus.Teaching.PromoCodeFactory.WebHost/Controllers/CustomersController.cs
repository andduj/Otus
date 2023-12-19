using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Otus.Teaching.PromoCodeFactory.Core.Abstractions.Repositories;
using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;
using Otus.Teaching.PromoCodeFactory.WebHost.Models;

namespace Otus.Teaching.PromoCodeFactory.WebHost.Controllers
{
    /// <summary>
    /// Клиенты
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CustomersController
        : ControllerBase
    {
        private readonly IRepository<Customer> _customers;
        private readonly IRepository<Preference> _preferences;

        public CustomersController(IRepository<Customer> customers, IRepository<Preference> preferences)
        {
            _customers = customers;
            _preferences = preferences;
        }

        [HttpGet]
        public async Task<ActionResult<CustomerShortResponse>> GetCustomersAsync()
        {
            var customers = await _customers.GetAllAsync();

            var response = customers.Select(customer => new CustomerShortResponse(customer));

            return Ok(response);
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerResponse>> GetCustomerAsync(Guid id)
        {
            var customer = await _customers.GetByIdAsync(id);
            if(customer == default)
            {
                return NotFound();
            }

            return Ok(new CustomerResponse(customer));
        }
        
        [HttpPost]
        public Task<IActionResult> CreateCustomerAsync(CreateOrEditCustomerRequest request)
        {
            //TODO: Добавить создание нового клиента вместе с его предпочтениями
            throw new NotImplementedException();
        }
        
        [HttpPut("{id}")]
        public Task<IActionResult> EditCustomersAsync(Guid id, CreateOrEditCustomerRequest request)
        {
            //TODO: Обновить данные клиента вместе с его предпочтениями
            throw new NotImplementedException();
        }
        
        [HttpDelete]
        public Task<IActionResult> DeleteCustomer(Guid id)
        {
            //TODO: Удаление клиента вместе с выданными ему промокодами
            throw new NotImplementedException();
        }
    }
}