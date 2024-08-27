using HMS.Dto;
using HMS.Dto.Models;
using HMS.EmailService;
using HMS.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

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
        private readonly IMailService _mailService;
        private readonly IWebHostEnvironment _env;

        public AppController(ILogger<AppController> logger, IConfiguration appConfig, IAppRepository appRepository, IMailService mailService, IWebHostEnvironment env) : base()
       // public AppController(ILogger<AppController> logger, IConfiguration appConfig, IAppRepository appRepository, IWebHostEnvironment env) : base()
        {
            _appRepository = (AppRepository?)appRepository;
            _logger = logger;
            _configuration = appConfig;
            _mailService = mailService;
            _env = env;
        }

        #region Details
        [HttpGet]
        [Route("details/{id}")]
        public async Task<Details> GetDetailsById(int id)
        {
            return await _appRepository.GetDetailsById(id);
        }

        [HttpGet]
        [Route("all-details")]
        [AllowAnonymous]
        public async Task<IEnumerable<Details>> GetAllDetails()
        {
            return await _appRepository.GetAllDetails();
        }

        [HttpPost]
        [Route("UpsertDetails")]
        public async Task<Details> UpsertDetails(Details details)
        {

            return await _appRepository.UpsertDetails(details);
        }
        #endregion

        #region Hospitals
        [HttpPost]
        [Route("UpsertHospitalDetails")]
        public async Task<ApiResponse<HospitalDto>> UpsertHospitalDetails(HospitalDto data)
        {
            return await _appRepository.UpsertHospitalDetails(data);
        }


        [HttpGet]
        [Route("Hospital/{id}")]
        public async Task<HospitalDto> GetHospitalById(int id)
        {
            return await _appRepository.GetHospitalById(id);
        }

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
        public async Task<ApiResponse<ProcedureDto>> UpsertProceduresDetails(ProcedureDto data)
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
        public async Task<ApiResponse<DoctorsDto>> UpsertDoctorsDetails(DoctorsDto data)
        {
            return await _appRepository.UpsertDoctorsDetails(data);
        }

        [HttpGet]
        [Route("Doctor/{id}")]
        public async Task<DoctorsDto> GetDoctorById(int id)
        {
            return await _appRepository.GetDoctorById(id);
        }

        [HttpGet]
        [Route("all-doctors")]
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
    }
}
