using Google.Apis.Auth;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SSOConcepto.DataContract;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SSOConcepto.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IIdentityServerInteractionService _interaction;
        private readonly IClientStore _clientStore;
        private readonly IAuthenticationSchemeProvider _schemeProvider;
        private readonly IEventService _events;
        private readonly SignInManager<IdentityUser> _signInManager;
        public AuthenticationController(
            IIdentityServerInteractionService interaction,
            IClientStore clientStore,
            IAuthenticationSchemeProvider schemeProvider,
            IEventService events,
            SignInManager<IdentityUser> signInManager
        )
        {
            _interaction = interaction;
            _clientStore = clientStore;
            _schemeProvider = schemeProvider;
            _events = events;
            _signInManager = signInManager;
        }

        // GET: api/<AuthenticationController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<AuthenticationController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<AuthenticationController>
        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginDC loginDC)
        {
            var user = new IdentityUser();
            user.Email = loginDC.Email;
            user.UserName = loginDC.Username;
            var result = await _signInManager.PasswordSignInAsync(loginDC.Username, loginDC.Password, false, false);

            var context = await _interaction.GetAuthorizationContextAsync("/home");


            /* TODO ESTO GENERA LOS CLAIMS Y CREA EL TOKEN DE ACCESO
             * 
             * var claims = new List<Claim> {
                    new Claim(ClaimTypes.Name, "admin"),
                    new Claim(ClaimTypes.Email, "admin@mywebsite.com"),
                    new Claim("Department", "HR"),
                    new Claim("Admin", "true"),
                    new Claim("Manager", "true"),
                    new Claim("EmploymentDate", "2021-02-01")
                };

            var expiresAt = DateTime.UtcNow.AddMinutes(10);

            return Ok(new
            {
                access_token = CreateToken(claims, expiresAt),
                expires_at = expiresAt
            });*/
            //Cuando usamos identity, hay que de alguna manera habilitar o deshabilitar si se va a usar o no la funcionalidad
            //de la confirmación de email
            if (result.Succeeded)
                return Ok(context);
            else
                return StatusCode(StatusCodes.Status500InternalServerError, result);
            //Habria que controlar todos los errores
        }

        [HttpPost("signin/google")]
        public async Task<ActionResult> SignInGoogle([FromBody] GoogleLoginDC googleLoginDC)
        {
            /*var google_csrf_name = "g_csrf_token";
            var cookie = Request.Cookies[google_csrf_name];
            if (cookie == null)
                return BadRequest("Error bad request");
            var requestBody = Request.Form[google_csrf_name];
            if (requestBody != cookie)
                return BadRequest("Error bad request");
            var idtoken = Request.Form["credential"];*/
            try
            {
                var payload = await GoogleJsonWebSignature.ValidateAsync(googleLoginDC.idToken).ConfigureAwait(false);
                return Ok(payload);
            }
            catch (InvalidJwtException ex)
            {
                return BadRequest(ex.Message);
            }
            //TempDataAttribute["name"] = payload.Name;
            //TempDataAttribute["email"] = payload.Email;
            
        }

        [HttpPost("logout")]
        public async Task<ActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok();
        }

        // PUT api/<AuthenticationController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<AuthenticationController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        /*
         * Genera el token de jwt, hay que ver como lo podemos llegar a integrar con identity o si identity tiene algo 
         * private string CreateToken(IEnumerable<Claim> claims, DateTime expiresAt)
        {
            var secretKey = Encoding.ASCII.GetBytes(configuration.GetValue<string>("SecretKey"));

            var jwt = new JwtSecurityToken(
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: expiresAt,
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(secretKey),
                    SecurityAlgorithms.HmacSha256Signature));

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }*/
    }
}
