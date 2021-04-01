using Microsoft.AspNetCore.Mvc;
using MongoDB101.API.Model;
using MongoDB101.API.Service;
using MongoDB101.API.Settings;
using System.Collections.Generic;

namespace MongoDB101.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        UserService _userService;

        public UserController(IDbSettings settings)
        {
            _userService = new UserService(settings);
        }


        [HttpGet]
        public ActionResult<List<User>> GetAll() => _userService.GetAll();

        [HttpGet("{id:length(24)}")]
        public ActionResult<User> Get(string id) => _userService.GetSingle(id);

        [HttpPost]
        public ActionResult<User> Add(User user) => _userService.Add(user);

        [HttpPut]
        public ActionResult Update(User currentUser)
        {
            var user = _userService.GetSingle(currentUser.Id);
            if (user == null)
                return NotFound();

            _userService.Update(currentUser);
            return Ok();
        }

        [HttpDelete("{id:length(24)}")]
        public ActionResult Delete(string id)
        {
            var user = _userService.GetSingle(id);

            if (user == null)
                return NotFound();

            _userService.Delete(id);
            return Ok();
        }
    }
}
