using HMS.Dto;
using System.ComponentModel.DataAnnotations;

public class DoctorsDto : Auditable
{
    [Required]
    public string Name { get; set; } = string.Empty;

    public string? ProfileContents { get; set; }
    public string? About { get; set; }
    public string? EducationAndTraining { get; set; }
    public string? Experience { get; set; }
    public string? Membership { get; set; }
    public string? AccomplishmentOrAward { get; set; }

    public byte[]? Image { get; set; }

    [Required]
    public string Specialization { get; set; } = string.Empty;

    public string? InstagramLink { get; set; } 
    public string? FbLink { get; set; } //Facebook link
    public string? LinkedInLink { get; set; }  
    public string? XLink { get; set; }          // Assuming 'Xlink' is for Twitter
    public string? ThreadLink { get; set; }     // Assuming 'ThreadLink' is for Threads app
}
