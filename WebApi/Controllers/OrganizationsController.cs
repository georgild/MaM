﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BizModels.Errors;
using BizModels.Identity;
using AutoMapper;
using BusinessServicesContracts.Identity;
using CustomPolicyAuth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.Identity;

namespace WebApi.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class OrganizationsController : ControllerBase
    {
        private readonly IUserService _userService;

        private readonly IOrganizationService _organizationService;

        private readonly IMapper _mapper;

        private readonly ContextPrincipal _principal;

        public OrganizationsController(
            IOrganizationService organizationService, 
            IUserService userService, 
            IMapper mapper
        ) {
            _organizationService = organizationService;
            _userService = userService;
            _mapper = mapper;
            _principal = new ContextPrincipal(HttpContext.User);
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery]int start, [FromQuery]int limit) {

            IEnumerable<Organization> dbObjects = await _organizationService.Find(_principal, org => true, start, limit);
            IEnumerable<OrganizationBizModel> bizModels = _mapper.Map<IEnumerable<Organization>, IEnumerable<OrganizationBizModel>>(dbObjects);

            return Ok(bizModels);
        }

        [HttpGet("{id}", Name = "GetOrganizationById")]
        public async Task<IActionResult> GetOrganizationById(Guid id) {

            if (id.Equals(Guid.Empty)) {
                return BadRequest(new ErrorResponse().AddError(EErrorCodes.GeneralGetErrorCode, "Invalid Id."));
            }

            Organization dbObject = await _organizationService.Get(_principal, id);

            return Ok(_mapper.Map<OrganizationBizModel>(dbObject));
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] OrganizationBizModel value)
        {
            if (!ModelState.IsValid) {
                return BadRequest(new ErrorResponse()
                    .AddModelStateErrors(ModelState));
            }

            value.Id = await _organizationService.Create(_principal,
                    _mapper.Map<Organization>(value));

            return CreatedAtAction(nameof(GetOrganizationById), new { id = value.Id }, value);
        }

        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        [HttpGet("{id}/users")]
        public async Task<IActionResult> GetUsers(
            Guid id,
            [FromQuery]int start,
            [FromQuery]int limit
        ) {

            IEnumerable<Guid> childrenGuids = await _organizationService.GetDirectChildrenAsGuids(_principal, id, start, limit);

            IEnumerable<UserBizModel> bizModels = await _userService.FindAsBiz(_principal, u => childrenGuids.Contains(u.Id), start, limit);

            return Ok(bizModels);
        }

        [HttpPost("{id}/users")]
        public async Task<IActionResult> Post(Guid id, [FromBody] UserBizModel value) {
            if (!ModelState.IsValid) {
                return BadRequest(new ErrorResponse()
                    .AddModelStateErrors(ModelState));
            }

            value.Id = await _userService.Create(_principal,
                    _mapper.Map<User>(value), id);

            return CreatedAtAction(nameof(GetOrganizationById), new { id = value.Id }, value);
        }
    }
}
