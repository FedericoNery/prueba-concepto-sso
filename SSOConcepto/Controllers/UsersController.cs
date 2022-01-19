using IdentityServer4.Services;
using IdentityServer4.Stores;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SSOConcepto.DataContract;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SSOConcepto.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IIdentityServerInteractionService _interaction;
        private readonly IClientStore _clientStore;
        private readonly IAuthenticationSchemeProvider _schemeProvider;
        private readonly IEventService _events;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        public UsersController(
            IIdentityServerInteractionService interaction,
            IClientStore clientStore,
            IAuthenticationSchemeProvider schemeProvider,
            IEventService events,
            SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager
        )
        {
            _interaction = interaction;
            _clientStore = clientStore;
            _schemeProvider = schemeProvider;
            _events = events;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        // GET: api/<UsersController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<UsersController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<UsersController>
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] CreateUserDC createUserDC)
        {
            var _hasher = new PasswordHasher<IdentityUser>();
            var user = new IdentityUser
            {
                Email = createUserDC.Email,
                UserName = createUserDC.Username,
            };
            var passwordHashed = _hasher.HashPassword(user, createUserDC.Password);
            user.PasswordHash = passwordHashed;

            var result = await _userManager.CreateAsync(user);

            if(result.Succeeded)
                return Ok("Usuario creado con exito");
            else
                return StatusCode(StatusCodes.Status500InternalServerError, result.Errors);
        }

        // PUT api/<UsersController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<UsersController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
