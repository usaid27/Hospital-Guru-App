﻿using HMS.Dto;
using HMS.Dto.Models;

namespace HMS.Repositories
{
    public interface IAppRepository
    {
        Task<Details> GetDetailsById(int id);
        Task<IEnumerable<Details>> GetAllDetails();


        Task<ApiResponse<HospitalDto>> UpsertHospitalDetails(HospitalDto data);
        Task<HospitalDto> GetHospitalById(int id);
        Task<IEnumerable<HospitalDto>> GetAllHospitals();
        Task<ApiResponse<HospitalDto>> DeleteHospital(int id);


        Task<ApiResponse<ProcedureDto>> UpsertProceduresDetails(ProcedureDto data);
        Task<ProcedureDto> GetProcedureById(int id);
        Task<IEnumerable<ProcedureDto>> GetAllProcedures();
        Task<ApiResponse<ProcedureDto>> DeleteProcedure(int id);

        Task<ApiResponse<ProcedureCatagory>> UpsertProcedureCatagoryDetails(string procedureCatagoryName);
        Task<ProcedureCatagory> GetProcedureCatagoryById(int id);
        Task<IEnumerable<ProcedureCatagory>> GetAllProceduresCatagory();
        Task<ApiResponse<ProcedureCatagory>> DeleteProcedureCatagory(int id);


        Task<ApiResponse<DoctorsDto>> UpsertDoctorsDetails(DoctorsDto data);
        Task<DoctorsDto> GetDoctorById(int id);
        Task<IEnumerable<DoctorsDto>> GetAllDoctors();
        Task<ApiResponse<DoctorsDto>> DeleteDoctor(int id);


        Task<bool> ContactDoctor(ContactDoctor contactDoctor);
        Task<bool> ContactHospital(ContactHospital contactHospital);

        Task<ProcedureTypes> GetProcedureTypesById(int id);

        Task<IEnumerable<ProcedureTypes>> GetAllProcedureTypes();
        Task<ApiResponse<ProcedureTypes>> UpsertProcedureTypes(ProcedureTypes data);
        Task<ApiResponse<ProcedureTypes>> DeleteProcedureTypes(int id);
    }
}
