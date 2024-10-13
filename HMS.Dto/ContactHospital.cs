namespace HMS.Dto
{
    public class ContactHospital : BaseEntity
    {
        public int HospitalId { get; set; }
        public string HospitalName { get; set; }  
        public string Name { get; set; }  // Patient's name
        public string Mobile { get; set; }  // Patient's contact number
        public string? Email { get; set; }  // Patient's email (optional)
        public string? Subject { get; set; }  // Subject or reason for contacting
        public string? Message { get; set; }  // Message from the patient
        public string? Description { get; set; }  // Additional description if needed
        public bool Consent { get; set; }  // Checkbox for patient consent to share their data
    }
}
