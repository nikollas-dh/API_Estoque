using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace API_Estoque.Models;

public partial class Materiai
{
    public int Id { get; set; }

    public string? Nome { get; set; }

    public decimal? Quantidade { get; set; }

    public decimal? ValorUnitario { get; set; }

    public int? IdCategoria { get; set; }

    public string? UnidadeMedida { get; set; }
    [JsonIgnore]
    public string? Tipo { get; set; }

    [JsonIgnore]
    public virtual Categorium? IdCategoriaNavigation { get; set; }
    [JsonIgnore]

    public virtual ICollection<Movimentacao> Movimentacaos { get; set; } = new List<Movimentacao>();
}
