using Microsoft.AspNetCore.Mvc;

namespace RestfulAPI.Controllers
{
    /// <summary>
    /// Some app information 
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class IndexController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            var ret = new
            {
                Version = "1.0",
                Name = "Restful API",
                Swagger = "https://localhost:44317/swagger",
                Description = @" Adicione um Usuário em /register. Acesse com o usuário criado em /login. Adicione o token gerado em 'Authorize' no swagger ('bearer ' + token). /Users: CRUD de usuários. Necessário autenticação. /Default: String. Necessário autenticação."
            };

            return Ok(ret);
        }

    }
}