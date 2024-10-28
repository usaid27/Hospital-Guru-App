using HMS.Dto;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class ProcedureCatagory : BaseEntity
{
    [Required]
    public string ProcedureCatagoryName { get; set; } = string.Empty;

}