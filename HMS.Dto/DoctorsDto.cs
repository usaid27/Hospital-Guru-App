using HMS.Dto;
using System.ComponentModel.DataAnnotations;

public class DoctorsDto : Auditable
{
    [Required]
    public string Name { get; set; }

    public byte[]? Image { get; set; }

    [Required]
    public string Specialization { get; set; }

    //[Url]
    // public string ProfileLink { get; set; } 

    // public List<PatientReviewDto> PatientReviews { get; set; } = new List<PatientReviewDto>();

    //public class PatientReviewDto
    //{
    //    [Required]
    //    public string ReviewerName { get; set; }

    //    [Range(1, 5)]
    //    public int Rating { get; set; }

    //    [Required]
    //    public string ReviewText { get; set; }

    //    public DateTime ReviewDate { get; set; } = DateTime.UtcNow;
    //}
}
