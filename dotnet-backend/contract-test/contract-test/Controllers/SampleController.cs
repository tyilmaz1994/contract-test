using contract_test.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace contract_test.Controllers
{
    [ApiController]
    [Route("api/v1.0")]
    public class SampleController : ControllerBase
    {
        [HttpGet("orders")]
        public IActionResult GetOrders()
        {
            var sampleModels = new List<SampleModel>
            {
                new SampleModel
                {
                    Code = "123",
                    Number = 123
                },
                new SampleModel
                {
                    Code = "124",
                    Number = 124
                },
            };

            return Ok(value: sampleModels);
        }
    }
}