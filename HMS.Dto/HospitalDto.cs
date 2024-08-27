using System.ComponentModel.DataAnnotations;
using HMS.Dto;
public class HospitalDto : Auditable
{
    public byte[]? ImageFile { get; set; }
    [Required]
    public string Name { get; set; }

    [Required]
    public string Location { get; set; }

    public string Accreditation { get; set; }

    [Phone]
    public string Phone { get; set; }

    [EmailAddress]
    public string Email { get; set; }

    public string MapLocationUrl { get; set; }
}
