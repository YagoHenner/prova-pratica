using System.ComponentModel.DataAnnotations;

namespace Domain.Enums;

public enum StatusProdutoEnum
{
    [Display(Name = "Inativo")]
    Inativo,

    [Display(Name = "Ativo")]
    Ativo
}