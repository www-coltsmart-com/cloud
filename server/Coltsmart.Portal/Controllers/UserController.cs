using ColtSmart.Entity.Entities;
using ColtSmart.Service;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Web.Http;
using System.Linq;
using System;
using ColtSmart.Service.Service;
using System.Threading.Tasks;

namespace Coltsmart.Portal.Controllers
{
    public class UserController : ApiController
    {
        private IUserService userService = null;

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpGet]
        [Route("api/users")]
        public async Task<PagedResult<TUser>> Get(int page, int size, string username)
        {
            return await userService.GetUsers(page, size, username);
        }
        [HttpGet]
        [Route("api/users/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            if (id <= 0) return StatusCode(System.Net.HttpStatusCode.BadRequest);
            var result = await userService.GetUser(id);
            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                return StatusCode(System.Net.HttpStatusCode.NotFound);
            }
        }

        [HttpDelete]
        [Route("api/users/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0) return StatusCode(System.Net.HttpStatusCode.BadRequest);
            var result = await userService.Delete(id);
            return result ? Ok() : StatusCode(System.Net.HttpStatusCode.InternalServerError);
        }

        [HttpPost]
        [Route("api/users")]
        public async Task<IActionResult> Save([FromBody]TUser user)
        {
            if (user == null || string.IsNullOrEmpty(user.UserName)) return StatusCode(System.Net.HttpStatusCode.BadRequest);
            bool result = false;
            if (user.id <= 0)
            {
                result = await userService.Create(user);
            }
            else
            {
                result = await userService.Update(user);
            }
            return result ? Ok() : StatusCode(System.Net.HttpStatusCode.InternalServerError);
        }

        public string EncryptHelper { get; private set; }
    }
}
