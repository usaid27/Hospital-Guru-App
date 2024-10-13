namespace HMS.Dto
{
    public class ContactDoctor : BaseEntity
    {
        public int DoctorId { get; set; }  // Identifier for the doctor
        public string DoctorName { get; set; }  // Doctor's name
        public string PatientName { get; set; }  // Patient's name
        public string PatientMobile { get; set; }  // Patient's contact number
        public string? PatientEmail { get; set; }  // Patient's email (optional)
        public string? Subject { get; set; }  // Reason for contacting (e.g., appointment, inquiry)
        public string? Message { get; set; }  // Detailed message from the patient
        public bool Consent { get; set; }  // Checkbox for patient consent to share their data
    }
}
