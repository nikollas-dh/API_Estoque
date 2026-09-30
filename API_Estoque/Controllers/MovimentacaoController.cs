using API_Estoque.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_Estoque.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovimentacaoController : ControllerBase
    {
        private readonly DbEstoqueContext _ct;

        public MovimentacaoController(DbEstoqueContext ct)
        {
            _ct = ct;
        }

        [HttpPost("Entrada")]
        public IActionResult RegistrarEntrada([FromBody] Movimentacao movimentacao)
        {
            try
            {
                if (movimentacao == null || movimentacao.Quantidade <= 0)
                    return BadRequest("A quantidade deve ser maior que zero.");

                var material = _ct.Materiais.Find(movimentacao.IdMaterial);
                if (material == null)
                    return NotFound("Produto/Material não encontrado.");

                
                material.Quantidade += movimentacao.Quantidade;

                movimentacao.Tipo = "ENTRADA";
                movimentacao.ValorUnitario = (decimal)material.ValorUnitario;
                movimentacao.DataLancamento = DateTime.Now;

                _ct.Movimentacaos.Add(movimentacao);
                _ct.SaveChanges();

                return Ok(new
                {
                    mensagem = "Entrada registrada com sucesso!",
                    novoSaldo = material.Quantidade
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao processar entrada: {ex.Message}");
            }
        }

        [HttpPost("Saida")]
        public IActionResult RegistrarSaida([FromBody] Movimentacao movimentacao)
        {
            try
            {
                if (movimentacao == null || movimentacao.Quantidade <= 0)
                    return BadRequest("A quantidade deve ser maior que zero.");

                var material = _ct.Materiais.Find(movimentacao.IdMaterial);
                if (material == null)
                    return NotFound("Produto/Material não encontrado.");

                if (material.Quantidade < movimentacao.Quantidade)
                    return BadRequest($"Estoque insuficiente. Saldo atual: {material.Quantidade}");

                material.Quantidade -= movimentacao.Quantidade;

                movimentacao.Tipo = "SAIDA";
                movimentacao.ValorUnitario = (decimal)material.ValorUnitario;
                movimentacao.DataLancamento = DateTime.Now;

                _ct.Movimentacaos.Add(movimentacao);
                _ct.SaveChanges();

                return Ok(new
                {
                    mensagem = "Saída registrada com sucesso!",
                    novoSaldo = material.Quantidade
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao processar saída: {ex.Message}");
            }
        }

        [HttpGet("Saidas")]
        public IActionResult ListarSaidas()
        {
            try
            {
                var saidas = _ct.Movimentacaos
                    .Include(m => m.IdMaterialNavigation) 
                    .Where(m => m.Tipo == "SAIDA")
                    .OrderByDescending(m => m.DataLancamento) 
                    .Select(m => new
                    {
                        m.Id,
                        NomeProduto = m.IdMaterialNavigation != null ? m.IdMaterialNavigation.Nome : "",
                        m.Quantidade,
                        m.ValorUnitario,
                        m.DataLancamento
                    })
                    .ToList();

                return Ok(saidas);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao listar saídas: {ex.Message}");
            }
        }

        [HttpGet("Entradas")]
        public IActionResult ListarEntradas()
        {
            try
            {
                var entradas = _ct.Movimentacaos
                    .Include(m => m.IdMaterialNavigation)
                    .Where(m => m.Tipo == "ENTRADA")
                    .OrderByDescending(m => m.DataLancamento)
                    .Select(m => new
                    {
                        m.Id,
                        NomeProduto = m.IdMaterialNavigation != null ? m.IdMaterialNavigation.Nome : "",
                        m.Quantidade,
                        m.ValorUnitario,
                        m.DataLancamento
                    })
                    .ToList();

                return Ok(entradas);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao listar entradas: {ex.Message}");
            }
        }

        [HttpGet]
        public IActionResult ListarTodas()
        {
            try
            {
                var movimentacoes = _ct.Movimentacaos
                    .Include(m => m.IdMaterialNavigation)
                    .OrderByDescending(m => m.DataLancamento)
                    .Select(m => new
                    {
                        m.Id,
                        TipoMovimentacao = m.Tipo,
                        NomeProduto = m.IdMaterialNavigation != null ? m.IdMaterialNavigation.Nome : "",
                        m.Quantidade,
                        m.ValorUnitario,
                        m.DataLancamento
                    })
                    .ToList();

                return Ok(movimentacoes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao listar movimentações: {ex.Message}");
            }
        }
    }
}
