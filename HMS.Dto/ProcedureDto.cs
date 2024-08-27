using HMS.Dto;
using System.ComponentModel.DataAnnotations;

public class ProcedureDto :Auditable
{
    [Required]
    public string ProcedureName { get; set; } 

    public byte[]? IntroductionMedia { get; set; } 

    [Required]
    public string ProcedureOverview { get; set; } 
    public string TypicalDuration { get; set; } 

    public string RecoveryTime { get; set; } 

    public double? SuccessRate { get; set; } 
}
