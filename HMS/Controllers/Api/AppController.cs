using HMS.Dto;
using HMS.Dto.Models;
using HMS.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace HMS.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AppController : ControllerBase
    {
        readonly IAppRepository _appRepository;
        readonly ILogger _logger;
        readonly IConfiguration _configuration;
        private readonly IEmailSender _emailSender;
        private readonly IWebHostEnvironment _env;
        private readonly string _contactEmail;

        public AppController(ILogger<AppController> logger, IConfiguration appConfig, IAppRepository appRepository, IWebHostEnvironment env, IEmailSender emailSender) : base()
        {
            _appRepository = (AppRepository?)appRepository;
            _logger = logger;
            _configuration = appConfig;
            _emailSender = emailSender;
            _env = env;

            // Read the contact email from appsettings.json
            _contactEmail = _configuration["ContactEmailSettings:ContactEmail"] ?? "default@example.com";
        }

        #region Test
        [HttpGet]
        [Route("ping")]
        [AllowAnonymous]
        public async Task<bool>Ping()
        {
            return true;
        }
        #endregion

        #region Hospitals
        [HttpPost]
        [Route("UpsertHospitalDetails")]
        public async Task<ApiResponse<HospitalDto>> UpsertHospitalDetails([FromForm] HospitalDto data)
        {
            return await _appRepository.UpsertHospitalDetails(data);
        }


        [HttpGet]
        [Route("Hospital/{id}")]
        [AllowAnonymous]
        public async Task<HospitalDto> GetHospitalById(int id)
        {
            return await _appRepository.GetHospitalById(id);
        }

       // [Authorize]
        [HttpGet]
        [Route("all-hospitals")]
        [AllowAnonymous]
        public async Task<IEnumerable<HospitalDto>> GetAllHospitals()
        {
            return await _appRepository.GetAllHospitals();
        }

        [HttpPost]
        [Route("DeleteHospital/{id}")]
        public async Task<ApiResponse<HospitalDto>> DeleteHospital(int id)
        {
            return await _appRepository.DeleteHospital(id);
        }
        #endregion

        #region Procedures
        [HttpPost]
        [Route("UpsertProceduresDetails")]
        public async Task<ApiResponse<ProcedureDto>> UpsertProceduresDetails([FromForm] ProcedureDto data)
        {
            return await _appRepository.UpsertProceduresDetails(data);
        }


        [HttpGet]
        [Route("Procedure/{id}")]
        [AllowAnonymous]
        public async Task<ProcedureDto> GetProcedureById(int id)
        {
            return await _appRepository.GetProcedureById(id);
        }

        [HttpGet]
        [Route("all-procedures")]
        [AllowAnonymous]
        public async Task<IEnumerable<ProcedureDto>> GetAllProcedures()
        {
            return await _appRepository.GetAllProcedures();
        }

        [HttpPost]
        [Route("DeleteProcedure/{id}")]
        public async Task<ApiResponse<ProcedureDto>> DeleteProcedure(int id)
        {
            return await _appRepository.DeleteProcedure(id);
        }
        #endregion

        #region Doctors
        [HttpPost]
        [Route("UpsertDoctorsDetails")]
        public async Task<ApiResponse<DoctorsDto>> UpsertDoctorsDetails([FromForm] DoctorsDto doctorsDto)
        {
            return await _appRepository.UpsertDoctorsDetails(doctorsDto);
        }


        [HttpGet]
        [Route("Doctor/{id}")]
        [AllowAnonymous]
        public async Task<DoctorsDto> GetDoctorById(int id)
        {
            return await _appRepository.GetDoctorById(id);
        }

        [HttpGet]
        [Route("all-doctors")]
        [AllowAnonymous]
        public async Task<IEnumerable<DoctorsDto>> GetAllDoctors()
        {
            return await _appRepository.GetAllDoctors();
        }
        [HttpPost]
        [Route("DeleteDoctor/{id}")]
        public async Task<ApiResponse<DoctorsDto>> DeleteDoctor(int id)
        {
            return await _appRepository.DeleteDoctor(id);
        }
        #endregion

        #region Contact Mail

        [HttpPost]
        [Route("contact-doctor")]
        [AllowAnonymous]
        public async Task<IActionResult> ContactDoctor([FromForm] ContactDoctor contactDoctor)
        {
            if (ModelState.IsValid)
            {
                var subject = $"Contact Request for Dr. {contactDoctor.DoctorName}";
                var body = new StringBuilder();

                // HTML-formatted email body
                body.AppendLine("<h3>Doctor Contact Request</h3>");
                body.AppendLine($"<p><strong>Doctor Name:</strong> {contactDoctor.DoctorName}</p>");
                body.AppendLine($"<p><strong>Patient Name:</strong> {contactDoctor.PatientName}</p>");
                body.AppendLine($"<p><strong>Mobile:</strong> {contactDoctor.PatientMobile}</p>");
                if (!string.IsNullOrEmpty(contactDoctor.PatientEmail))
                    body.AppendLine($"<p><strong>Email:</strong> {contactDoctor.PatientEmail}</p>");
                if (!string.IsNullOrEmpty(contactDoctor.Subject))
                    body.AppendLine($"<p><strong>Subject:</strong> {contactDoctor.Subject}</p>");
                body.AppendLine($"<p><strong>Message:</strong><br /> {contactDoctor.Message}</p>");

                try
                {
                    // Sending email
                    await _emailSender.SendEmailAsync(_contactEmail, subject, body.ToString());

                    // Save request to the database
                    var isSaved = await _appRepository.ContactDoctor(contactDoctor);
                    if (!isSaved)
                    {
                        return StatusCode(500, "Error saving the message.");
                    }

                    return Ok(new { Success = true, Message = "Your message has been sent to the doctor." });
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Failed to process contact doctor request: {ex.Message}");
                    return StatusCode(500, "Error sending the message.");
                }
            }

            return BadRequest(ModelState);
        }

        [HttpPost]
        [Route("contact-hospital")]
        [AllowAnonymous]
        public async Task<IActionResult> ContactHospital([FromBody] ContactHospital contactHospital)
        {
            if (ModelState.IsValid)
            {
                var subject = $"Contact Request for {contactHospital.HospitalName}";
                var body = new StringBuilder();

                // HTML-formatted email body
                body.AppendLine("<h3>Hospital Contact Request</h3>");
                body.AppendLine($"<p><strong>Hospital Name:</strong> {contactHospital.HospitalName}</p>");
                body.AppendLine($"<p><strong>Patient Name:</strong> {contactHospital.Name}</p>");
                body.AppendLine($"<p><strong>Mobile:</strong> {contactHospital.Mobile}</p>");
                if (!string.IsNullOrEmpty(contactHospital.Email))
                    body.AppendLine($"<p><strong>Email:</strong> {contactHospital.Email}</p>");
                body.AppendLine($"<p><strong>Description:</strong><br /> {contactHospital.Description}</p>");

                try
                {
                    // Sending email
                    await _emailSender.SendEmailAsync(_contactEmail, subject, body.ToString());

                    // Save request to the database
                    var isSaved = await _appRepository.ContactHospital(contactHospital);
                    if (!isSaved)
                    {
                        return StatusCode(500, "Error saving the message.");
                    }

                    return Ok(new { Success = true, Message = "Your message has been sent to the hospital." });
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Failed to process contact hospital request: {ex.Message}");
                    return StatusCode(500, "Error sending the message.");
                }
            }

            return BadRequest(ModelState);
        }

        #endregion

        #region ProceduresTypes
        [HttpPost]
        [Route("UpsertProceduresTypes")]
        public async Task<ApiResponse<ProcedureTypes>> UpsertProceduresTypes([FromForm] ProcedureTypes data)
        {
            return await _appRepository.UpsertProcedureTypes(data);
        }

        #region ProceduresCatagory
        [HttpPost]
        [Route("UpsertProcedureCatagoryDetails")]
        public async Task<ApiResponse<ProcedureCatagory>> UpsertProceduresCatagoryDetails([FromForm] ProcedureCatagory data)
        {
            return await _appRepository.UpsertProcedureCatagoryDetails(data.ProcedureCatagoryName);
        }


        [HttpGet]
        [Route("ProcedureCatagory/{id}")]
        [AllowAnonymous]
        public async Task<ProcedureCatagory> GetProcedureCatagoryById(int id)
        {
            return await _appRepository.GetProcedureCatagoryById(id);
        }

        [HttpGet]
        [Route("all-proceduresCatagory")]
        [AllowAnonymous]
        public async Task<IEnumerable<ProcedureCatagory>> GetAllProceduresCatagory()
        {
            return await _appRepository.GetAllProceduresCatagory();
        }

        [HttpPost]
        [Route("DeleteProcedureCatagory/{id}")]
        public async Task<ApiResponse<ProcedureCatagory>> DeleteProcedureCatagory(int id)
        {
            return await _appRepository.DeleteProcedureCatagory(id);
        }
        #endregion

        [HttpGet]
        [Route("ProceduresTypes/{id}")]
        [AllowAnonymous]
        public async Task<ProcedureTypes> GetProceduresTypesById(int id)
        {
            return await _appRepository.GetProcedureTypesById(id);
        }

        [HttpGet]
        [Route("all-ProceduresTypess")]
        [AllowAnonymous]
        public async Task<IEnumerable<ProcedureTypes>> GetAllProceduresTypess()
        {
            return await _appRepository.GetAllProcedureTypes();
        }

        [HttpPost]
        [Route("DeleteProcedureTypes/{id}")]
        public async Task<ApiResponse<ProcedureTypes>> DeleteProcedureTypes(int id)
        {
            return await _appRepository.DeleteProcedureTypes(id);
        }
        #endregion

    }
}


