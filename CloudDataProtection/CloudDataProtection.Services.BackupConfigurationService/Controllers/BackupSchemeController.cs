using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using CloudDataProtection.Core.Controllers;
using CloudDataProtection.Core.Jwt;
using CloudDataProtection.Core.Result;
using CloudDataProtection.Services.Subscription.Business;
using CloudDataProtection.Services.Subscription.Dto;
using CloudDataProtection.Services.Subscription.Entities;
using Microsoft.AspNetCore.Mvc;

namespace CloudDataProtection.Services.Subscription.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BackupSchemeController : ServiceController
    {
        private readonly Lazy<BackupSchemeBusinessLogic> _logic;
        private readonly IMapper _mapper;

        public BackupSchemeController(IJwtDecoder jwtDecoder, Lazy<BackupSchemeBusinessLogic> logic, IMapper mapper) : base(jwtDecoder)
        {
            _logic = logic;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("")]
        public async Task<ActionResult> GetAll()
        {
            BusinessResult<ICollection<BackupScheme>> businessResult = await _logic.Value.GetAll();

            if (!businessResult.Success)
            {
                return Problem("An error occured while attempting to retrieve the backup schemes");
            }

            return Ok(_mapper.Map<ICollection<BackupSchemeResult>>(businessResult.Data));
        }
    }
}