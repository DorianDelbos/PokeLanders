﻿using LandAPI.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace LandAPI.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class TypeController : ControllerBase
    {
        private readonly TypeService _typeService;

        public TypeController(TypeService typeService)
        {
            _typeService = typeService;
        }

        [HttpGet]
        public ActionResult<List<Models.Type>> GetTypes()
        {
            var types = _typeService.GetAllTypes();
            return Ok(types);
        }

        [HttpGet("{id}")]
        public ActionResult<Models.Type> GetTypeById(int id)
        {
            var Stat = _typeService.GetTypeById(id);
            if (Stat == null) return NotFound();
            return Ok(Stat);
        }

        [HttpGet("name/{name}")]
        public ActionResult<Models.Type> GetStatByType(string name)
        {
            return Ok(_typeService.GetTypeByName(name));
        }
    }
}
