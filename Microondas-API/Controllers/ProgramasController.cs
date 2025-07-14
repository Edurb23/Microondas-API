using Microondas_API.Models;
using Microondas_API.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static Microondas_API.Models.ProgamaAquecimento;


namespace Microondas_API.Controllers
{
    [ApiController]
    [Route("api")]
    public class ProgramasController : ControllerBase
    {

        private readonly string jwtKey = "k8n@G2$9s!mPqRzT4vXuWcYbLpEsAaDdF";

        [HttpGet("programas/all")]
        public ActionResult<List<ProgramaAquecimento>> GetTodos()
        {
            return Ok(ProgramasService.TodosProgramas());
        }


        [HttpPost("programas/register")]
        [Authorize]
        public ActionResult PostCustomizado([FromBody] ProgramaAquecimento novo)
        {
            var predefinidos = ProgramasService.ObterPreDefinidos();
            var customizados = ProgramasService.CarregarCustomizados();

            if (!ProgramasService.CaractereValido(novo.Caractere, predefinidos, customizados))
                return BadRequest("Caractere já existe ou inválido!");

            ProgramasService.AdicionarCustomizado(novo, predefinidos, customizados);
            return Ok("Programa customizado cadastrado com sucesso!");
        }
       
        [HttpPost("user/register")]
        [Microsoft.AspNetCore.Authorization.AllowAnonymous]
        public IActionResult Register([FromBody] Usuario usuario)
        {
            if (string.IsNullOrEmpty(usuario.Username) || string.IsNullOrEmpty(usuario.Senha))
                return BadRequest("Usuário e senha obrigatórios.");

            if (!UsuarioService.CadastrarUsuario(usuario))
                return BadRequest("Usuário já existe!");

            return Ok("Usuário cadastrado com sucesso.");
        }


        [HttpPost("login")]
        [Microsoft.AspNetCore.Authorization.AllowAnonymous]
        public IActionResult Login([FromBody] Usuario usuario)
        {
            if (!UsuarioService.ValidarLogin(usuario.Username, usuario.Senha))
                return Unauthorized("Usuário ou senha inválidos.");

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(jwtKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, usuario.Username)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return Ok(new
            {
                token = tokenHandler.WriteToken(token)
            });
        }

    }
}

