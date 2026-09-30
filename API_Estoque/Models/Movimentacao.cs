using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace API_Estoque.Models;

public partial class Movimentacao
{
    public int Id { get; set; }

    public int IdMaterial { get; set; }

    public DateTime DataLancamento { get; set; }

    public decimal ValorUnitario { get; set; }

    public int Quantidade { get; set; }
    [JsonIgnore]
    public string ?Tipo { get; set; } = null!;
    [JsonIgnore]
    public virtual Materiai IdMaterialNavigation { get; set; } = null!;
}
