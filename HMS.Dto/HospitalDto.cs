using System.ComponentModel.DataAnnotations;
using HMS.Dto;

public class HospitalDto : Auditable
{
    public byte[]? ImageFile { get; set; }

    //[Required]
    public string Name { get; set; } 

    public string? About { get; set; }
    public string? Infrastructure { get; set; }
    public string? TeamAndSpecialities { get; set; }

    [Required]
    public string Location { get; set; } = string.Empty;

    public string? AccreditationAndAwards { get; set; }

    //[Phone]
    public string? Phone { get; set; }

    //[EmailAddress]
    public string? Email { get; set; }

    public string? ContactInfo { get; set; }

    public string? MapLocationLatLong { get; set; }

    // Navigation properties
    public virtual ICollection<MedicalcoreAndSpecialities> MedicalCoreAndSpecialities { get; set; } = new List<MedicalcoreAndSpecialities>();
    public virtual ICollection<OtherSpecialities> OtherSpecialities { get; set; } = new List<OtherSpecialities>(); // Changed to virtual for lazy loading
}


public class MedicalcoreAndSpecialities : BaseEntity
{
    public int HospitalDtoId { get; set; } // Foreign key to HospitalDto
    public string CoreName { get; set; } = string.Empty;

    // Navigation property to access the related HospitalDto
    //public virtual HospitalDto Hospital { get; set; }
}


public class OtherSpecialities : BaseEntity
{
    public int HospitalDtoId { get; set; } // Foreign key to HospitalDto
    public string Speciality { get; set; } = string.Empty;

    // Navigation property
    //public virtual HospitalDto Hospital { get; set; }

    public virtual List<SpecialitiesName> SpecialitiesNames { get; set; } = new List<SpecialitiesName>();
}


public class SpecialitiesName : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    public int OtherSpecialityId { get; set; } // Foreign key to OtherSpecialities
    //public virtual OtherSpecialities OtherSpeciality { get; set; } 
}

