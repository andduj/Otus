using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Otus.Teaching.PromoCodeFactory.Core.Abstractions.Repositories;
using Otus.Teaching.PromoCodeFactory.Core.Domain.Administration;
using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;
using Otus.Teaching.PromoCodeFactory.WebHost.Models;
using SQLitePCL;

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

        /// <summary>
        /// Получение списка клиентов
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<CustomerShortResponse>> GetCustomersAsync()
        {
            var customers = await _customers.GetAllAsync();

            var response = customers.Select(customer => new CustomerShortResponse(customer));

            return Ok(response);
        }

        /// <summary>
        /// Получение клиента по id
        /// </summary>
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

        /// <summary>
        /// Создание клиента
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<CustomerResponse>> CreateCustomerAsync(CreateOrEditCustomerRequest request)
        {
            var customer = new Customer 
            { 
                Id = Guid.NewGuid(),
                FirstName = request.FirstName, 
                LastName = request.LastName, 
                Email = request.Email
            };

            var preferences = await _preferences.GetAllAsync();

            customer.Preferences = preferences
                .Where(p => request.PreferenceIds.Contains(p.Id))
                .Select(p => new CustomerPreference { CustomerId = customer.Id, PreferenceId = p.Id, Customer = customer, Preference = p })
                .ToList();

            await _customers.AddAsync(customer);

            return Ok(new CustomerResponse(customer));
        }
        
        [HttpPut("{id}")]
        public Task<IActionResult> EditCustomersAsync(Guid id, CreateOrEditCustomerRequest request)
        {
            //TODO: Обновить данные клиента вместе с его предпочтениями
            throw new NotImplementedException();
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(Guid id)
        {
            //TODO: Удаление клиента вместе с выданными ему промокодами
            var customer = await _customers.GetByIdAsync(id);
            if (customer == default)
            {
                return NotFound();
            }
            await _customers.DeleteAsync(customer);
            return Ok();
        }
    }
}