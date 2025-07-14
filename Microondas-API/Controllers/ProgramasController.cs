using Microondas_API.Models;
using Microondas_API.Service;
using Microsoft.AspNetCore.Mvc;
using static Microondas_API.Models.ProgamaAquecimento;


namespace Microondas_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProgramasController : ControllerBase
    {
        
        [HttpGet]
        public ActionResult<List<ProgramaAquecimento>> GetTodos()
        {
            return Ok(ProgramasService.TodosProgramas());
        }

      
        [HttpPost]
        public ActionResult PostCustomizado([FromBody] ProgramaAquecimento novo)
        {
            var predefinidos = ProgramasService.ObterPreDefinidos();
            var customizados = ProgramasService.CarregarCustomizados();

            if (!ProgramasService.CaractereValido(novo.Caractere, predefinidos, customizados))
                return BadRequest("Caractere já existe ou inválido!");

            ProgramasService.AdicionarCustomizado(novo, predefinidos, customizados);
            return Ok("Programa customizado cadastrado com sucesso!");
        }
    }
}

