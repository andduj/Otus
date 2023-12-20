using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Otus.Teaching.PromoCodeFactory.Core.Abstractions.Repositories;
using Otus.Teaching.PromoCodeFactory.Core.Domain.Administration;
using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;
using Otus.Teaching.PromoCodeFactory.WebHost.Models;

namespace Otus.Teaching.PromoCodeFactory.WebHost.Controllers
{
    /// <summary>
    /// Промокоды
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PromocodesController
        : ControllerBase
    {
        private readonly IRepository<PromoCode> _promocodes;
        private readonly IRepository<Employee> _employees;
        private readonly IRepository<Customer> _customers;

        public PromocodesController(IRepository<PromoCode> promocodes, IRepository<Employee> employees, IRepository<Customer> customers)
        {
            _promocodes = promocodes;
            _employees = employees;
            _customers = customers;
        }

        /// <summary>
        /// Получить все промокоды
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PromoCodeShortResponse>>> GetPromocodesAsync()
        {
            var promocodes = await _promocodes.GetAllAsync();
            var responce = promocodes
                .Select(p => new PromoCodeShortResponse { 
                    Id = p.Id,
                    BeginDate = p.BeginDate.ToString("dd.MM.yyyy"),
                    EndDate = p.EndDate.ToString("dd.MM.yyyy"),
                    Code = p.Code,
                    PartnerName = p.PartnerName,
                    ServiceInfo = p.ServiceInfo });
            return Ok(responce);
        }
        
        /// <summary>
        /// Создать промокод и выдать его клиентам с указанным предпочтением
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> GivePromoCodesToCustomersWithPreferenceAsync(GivePromoCodeRequest request)
        {
            //TODO: Создать промокод и выдать его клиентам с указанным предпочтением
            var customers = await _customers.GetAllAsync();
            var employees = await _employees.GetAllAsync();

            foreach (var customer in customers)
            {
                var customerPreference = customer.Preferences.FirstOrDefault(p => p.Preference.Name == request.Preference);
                if(customerPreference == null)
                {
                    continue;
                }
                var employee = employees.FirstOrDefault(e => e.FullName == request.PartnerName);

                var promocode = new PromoCode
                {
                    Id = Guid.NewGuid(),
                    ServiceInfo = request.ServiceInfo,
                    PartnerName = employee.FullName,
                    Code = request.PromoCode,
                    Preference = customerPreference.Preference,
                    PartnerManager = employee
                };

                await _customers.UpdateAsync(customer);
            }

            return Ok();
        }
    }
}