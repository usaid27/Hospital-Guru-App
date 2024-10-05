using HMS.Dto;
using HMS.Dto.Models;
using HMS.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;

namespace HMS.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AppController : ControllerBase
    {
        readonly AppRepository _appRepository;
        readonly ILogger _logger;
        readonly IConfiguration _configuration;
        private readonly IEmailSender _emailSender;
        private readonly IWebHostEnvironment _env;

        public AppController(ILogger<AppController> logger, IConfiguration appConfig, IAppRepository appRepository, IWebHostEnvironment env, IEmailSender emailSender) : base()
        {
            _appRepository = (AppRepository?)appRepository;
            _logger = logger;
            _configuration = appConfig;
            _emailSender = emailSender;
            _env = env;
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
        public async Task<HospitalDto> GetHospitalById(int id)
        {
            return await _appRepository.GetHospitalById(id);
        }

        [Authorize]
        [HttpGet]
        [Route("all-hospitals")]
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
        public async Task<ProcedureDto> GetProcedureById(int id)
        {
            return await _appRepository.GetProcedureById(id);
        }

        [HttpGet]
        [Route("all-procedures")]
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


        #endregion
    }
}


