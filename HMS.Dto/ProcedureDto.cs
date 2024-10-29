using HMS.Dto;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class ProcedureDto : Auditable
{
    [Required]
    public string ProcedureName { get; set; } = string.Empty;

    [NotMapped]
    public IFormFile? ImageFormFile { get; set; }
    public byte[]? IntroductionMedia { get; set; }

    [Required]
    public string ProcedureOverview { get; set; } = string.Empty;

    public string? TypicalDuration { get; set; }

    public string? RecoveryTime { get; set; }

    public double? SuccessRate { get; set; }

    public string? Causes { get; set; }
    public string? Symptoms { get; set; }
    public string? Diagnosis { get; set; }  
    public string? TreatmentDetails { get; set; }

    // Initialize lists to avoid null reference exceptions
    public List<ReferenceLinks> ReferenceLinks { get; set; } = new List<ReferenceLinks>();
    public List<ProcedureDoctorMapping> ProcedureDoctorMapping { get; set; } = new List<ProcedureDoctorMapping>();
    public List<ProcedureHospitalMapping> ProcedureHospitalMapping { get; set; } = new List<ProcedureHospitalMapping>();
}

public class ReferenceLinks : BaseEntity
{
    public int ProcedureId {  get; set; }    

    [Url] 
    public string? Link { get; set; }
}

public class ProcedureDoctorMapping : BaseEntity
{

    public int DoctorId { get; set; }
    public int ProcedureId { get; set; }
    public virtual DoctorsDto ? Doctor { get; set; }
}

public class ProcedureHospitalMapping : BaseEntity  
{
    public int HospitalId { get; set; }
    public int ProcedureId { get; set; }
    public virtual HospitalDto ? Hospital { get; set; }
}
