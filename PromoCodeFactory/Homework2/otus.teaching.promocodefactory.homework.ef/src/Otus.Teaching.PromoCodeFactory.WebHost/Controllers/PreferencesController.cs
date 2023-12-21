using Microsoft.AspNetCore.Mvc;
using Otus.Teaching.PromoCodeFactory.Core.Abstractions.Repositories;
using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;
using Otus.Teaching.PromoCodeFactory.WebHost.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Otus.Teaching.PromoCodeFactory.WebHost.Controllers
{
    /// <summary>
    /// Предпочтения
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PreferencesController 
        : ControllerBase
    {
        private readonly IRepository<Preference> _preferences;
        public PreferencesController(IRepository<Preference> preferences)
        {
            _preferences = preferences;
        }

        /// <summary>
        /// Получение списка предпочтений
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PreferenceResponse>>> GetResponsesAsync()
        {
            var preferences = await _preferences.GetAllAsync();
            var response = preferences
                .Select(p => new PreferenceResponse { Id = p.Id, Name = p.Name });
            return Ok(response);
        }
    }
}
