using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiModels.Errors;
using ApiModels.Identity;
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
        private readonly IOrganizationService _organizationService;

        private readonly IMapper _mapper;

        private readonly ContextPrincipal _principal;

        public OrganizationsController(IOrganizationService organizationService, IMapper mapper) {
            _organizationService = organizationService;
            _mapper = mapper;
            _principal = new ContextPrincipal(HttpContext.User);
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery]int start, [FromQuery]int limit) {

            IEnumerable<Organization> dbObjects = await _organizationService.Find(_principal, org => true, start, limit);
            IEnumerable<OrganizationApiModel> bizModels = _mapper.Map<IEnumerable<Organization>, IEnumerable<OrganizationApiModel>>(dbObjects);

            return Ok(bizModels);
        }

        [HttpGet("{id}", Name = "Get")]
        public async Task<IActionResult> GetById(Guid id) {

            if (id.Equals(Guid.Empty)) {
                return BadRequest(new ErrorResponse().AddError(EErrorCodes.GeneralGetErrorCode, "Invalid Id."));
            }

            Organization dbObject = await _organizationService.Get(_principal, id);

            return Ok(_mapper.Map<OrganizationApiModel>(dbObject));
        }

        // POST: api/Organizations
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] OrganizationApiModel value)
        {
            if (!ModelState.IsValid) {
                return BadRequest(new ErrorResponse()
                    .AddModelStateErrors(ModelState));
            }

            value.Id = await _organizationService.Create(_principal,
                    _mapper.Map<Organization>(value));

            return CreatedAtAction(nameof(GetById), new { id = value.Id }, value);
        }

        // PUT: api/Organizations/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
