using Microsoft.AspNetCore.Mvc;
using PocGraphQLHotChocolate.Infra.Contracts;
using PocGraphQLHotChocolate.Infra.Model;

namespace PocGraphQLHotChocolate.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorController : ControllerBase
    {

        private IAuthorService _service;
        private string msg;

        public AuthorController(IAuthorService service)
        {
            _service = service;
            msg = "Apenas para testar a recuperação de dados da base";
        }

        /// <summary>
        /// Buscar todos os autores
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Get() => Ok(new
        {
            message = msg,
            data = _service.GetAllAsync()
        });

        /// <summary>
        /// api/Author/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}", Name = "Get")]
        public IActionResult Get(int id) => Ok(new {
            message = msg,
            data = _service.GetByIdAsync(id)
        });

        /// <summary>
        /// api/Author
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Post([FromBody] Author value) => Ok(new
        {
            message = msg,
            data = _service.AddAsync(value)
        });

        /// <summary>
        /// api/ApiWithActions/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public IActionResult Delete(int id) => Ok(
            new {
                message = msg,
                data = _service.RemoveAsync(id)
            }
        );
    }
}
