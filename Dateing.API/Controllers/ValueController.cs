﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dateing.API.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Dateing.API.Controllers
{

    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class ValueController : ControllerBase
    {
        private readonly DataContext _context;

        public ValueController(DataContext context)
        {
            this._context = context;

        }

        [HttpGet]
        public async Task<IActionResult> GetValue()
        {
            var values = await _context.Values.ToListAsync();
            return Ok(values);
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetValue(int id)
        {
            var header = HttpContext.Request.Headers;
            var value = await _context.Values.FirstOrDefaultAsync(v => v.Id == id);
            return Ok(value);
        }
    }
}
