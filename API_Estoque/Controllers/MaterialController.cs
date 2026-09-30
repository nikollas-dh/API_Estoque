using API_Estoque.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API_Estoque.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaterialController : ControllerBase
    {
        private readonly DbEstoqueContext ct;

        public MaterialController(DbEstoqueContext ct)
        {
            this.ct = ct;
        }

        [HttpGet]
        public IActionResult ListarProdutos()
        {
            try
            {
                var produtos = ct.Materiais.ToList();
                return Ok(produtos);
            }
            catch (Exception)
            {

                return StatusCode(500, "Erro de conexão com o servidor");
            }
        }

        [HttpPost("Cadastrar")]
        public IActionResult CadastrarProduto(Materiai material)
        {
            try
            {
                if (material.Nome == "" || material.Tipo == "" || material.UnidadeMedida == "" || material.IdCategoria == null)
                {
                    return BadRequest("Erro ao cadastrar produto. Por favor, preencha todos os campos!");
                }
                if (material.Quantidade < 0 || material.ValorUnitario < 0)
                {
                    return BadRequest("Quantidade e valor unitário não podem ser menor do que 0!");
                }
                ct.Materiais.Add(material);

                ct.SaveChanges();
                return StatusCode(201, "Produto Cadastrado com sucesso");
            }
            catch (Exception)
            {

                return StatusCode(500, "Erro de conexão com o servidor");
            }
        }
    }
}
