using Distributed.Caching.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System.Text;
 

namespace Distributed.Caching.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RedisController : ControllerBase
    {
        readonly IDistributedCache _distributedCache;
        public RedisController(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        [HttpGet("SetData")]
        public async Task<IActionResult> SetData(string name, string surname)
        {
            await _distributedCache.SetStringAsync("name", name, options: new()
            {
                AbsoluteExpiration = DateTime.Now.AddSeconds(30),
                SlidingExpiration = TimeSpan.FromSeconds(5)
            });
            await _distributedCache.SetAsync("surname", Encoding.UTF8.GetBytes(surname), options: new()
            {
                AbsoluteExpiration = DateTime.Now.AddSeconds(30),
                SlidingExpiration = TimeSpan.FromSeconds(5)
            });
            return Ok();
        }

        [HttpGet("GetData")]
        public async Task<IActionResult> GetData()
        {
            var name = await _distributedCache.GetStringAsync("name");
            var surnameBinary = await _distributedCache.GetAsync("surname");
            var surname = Encoding.UTF8.GetString(surnameBinary);
            return Ok(new
            {
                name,
                surname
            });
        }

        [HttpPost("SetPerson")]
        public async Task<IActionResult> SetPerson(string personKey,Person person)
        {

            var data= JsonConvert.SerializeObject(person);
            await _distributedCache.SetAsync(personKey, Encoding.UTF8.GetBytes(data), options: new()
            {
                AbsoluteExpiration=DateTime.Now.AddMinutes(10),
                SlidingExpiration=TimeSpan.FromSeconds(30)
            });
            return Ok();
        }

        [HttpGet("GetPerson/{personKey}")]
        public async Task<IActionResult> GetPerson(string personKey)
        {
            var personString = await _distributedCache.GetStringAsync(personKey);
            var person = JsonConvert.DeserializeObject<Person>(personString);
            return Ok(person);
        }

    }
}
