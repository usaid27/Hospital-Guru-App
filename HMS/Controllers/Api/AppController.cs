﻿using HMS.Dto;
using HMS.Dto.Models;
//using HMS.EmailService;
using HMS.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Mysqlx.Crud;
using Mysqlx.Expr;
using Mysqlx.Session;
using System;
using static Mysqlx.Error.Types;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Security.AccessControl;

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
        // private readonly IMailService _mailService;
        private readonly IWebHostEnvironment _env;

        //public AppController(ILogger<AppController> logger, IConfiguration appConfig, IAppRepository appRepository, IMailService mailService, IWebHostEnvironment env) : base()
        public AppController(ILogger<AppController> logger, IConfiguration appConfig, IAppRepository appRepository, IWebHostEnvironment env) : base()
        {
            _appRepository = (AppRepository?)appRepository;
            _logger = logger;
            _configuration = appConfig;
            // _mailService = mailService;
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
        public async Task<ApiResponse<DoctorsDto>> UpsertDoctorsDetails(
        [FromForm] string name,
        [FromForm] string specialization,
        [FromForm] IFormFile? image,
        [FromForm] string createdBy,
        [FromForm] string modifiedBy,
        [FromForm] DateTime createdOn,
        [FromForm] DateTime modifiedOn)
        {
            // Create a new DoctorsDto object
            var doctorDto = new DoctorsDto
            {
                Name = name,
                Specialization = specialization,
                CreatedBy = createdBy,
                ModifiedBy = modifiedBy,
                CreatedOn = createdOn,
                ModifiedOn = modifiedOn
            };

            // Convert IFormFile to byte array
            if (image != null)
            {
                using var memoryStream = new MemoryStream();
                await image.CopyToAsync(memoryStream);
                doctorDto.Image = memoryStream.ToArray();
            }

            return await _appRepository.UpsertDoctorsDetails(doctorDto);
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
    }
}


