using System;
using System.Collections.Generic;

namespace API_Estoque.Models;

public partial class Categorium
{
    public int Id { get; set; }

    public string? Nome { get; set; }

    public virtual ICollection<Materiai> Materiais { get; set; } = new List<Materiai>();
}
