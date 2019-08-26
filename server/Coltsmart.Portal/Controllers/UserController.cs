﻿using ColtSmart.Entity.Entities;
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

        [HttpDelete]
        [Route("api/users/{id}")]
        public async Task<bool> Delete(int id)
        {
            return await userService.Delete(id);
        }

        [HttpPost]
        [Route("api/users")]
        public async Task<bool> Create([FromBody]TUser user)
        {
            return await userService.Create(user);
        }

        public string EncryptHelper { get; private set; }
    }
}
