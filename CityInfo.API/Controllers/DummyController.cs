using CityInfo.API.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.API.Controllers
{
    public class DummyController: Controller
    {
        CityInfoContext _ctx;

        public DummyController(CityInfoContext ctx)
        {
            _ctx = ctx;
        }

        [Route("api/test")]
        [HttpGet]
        public IActionResult Test()
        {
            return Ok();
        }
    }
}
