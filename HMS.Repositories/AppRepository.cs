using HMS.DataContext;
using HMS.Dto.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using HMS.Dto;
using System.Threading.Channels;

namespace HMS.Repositories
{
    public class AppRepository : BaseRepository, IAppRepository
    {
        public AppDbContext AppDbCxt { get; set; }

        public AppRepository(ILogger<AppRepository> logger, AppDbContext appContext) : base(logger)
        {
            AppDbCxt = appContext;
        }

        #region Details
        public async Task<Details> GetDetailsById(int id)
        {
            Details result = null;

#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            result = AppDbCxt.Details.FirstOrDefault(o => o.Id == id);
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

            return await Task.FromResult(result);
        }

        public async Task<IEnumerable<Details>> GetAllDetails()
        {
            IEnumerable<Details> result = null;

            result = AppDbCxt.Details.ToList();
            return result;
        }
        public async Task<Details> UpsertDetails(Details details)
        {
            Details result = null;

            if (details == null)
                throw new ArgumentNullException("Invalid Details data");

            if (details.Id > 0)
            {


                AppDbCxt.Details.Update(details);

            }
            else
            {
                AppDbCxt.Details.Add(details);
            }

            AppDbCxt.SaveChanges();

            return details;
        }
        #endregion

        #region Hospitals
        public async Task<ApiResponse<HospitalDto>> UpsertHospitalDetails(HospitalDto data)
        {
            var result = new ApiResponse<HospitalDto>();
            try
            {

                if (data == null)
                {
                    result.IsSuccess = true;
                    result.Message = "Invalid Hall data!";
                    return result;
                }

                if (data.Id > 0)
                {
                    AppDbCxt.HospitalDto.Update(data);
                    result.Message = "Data Successfully Updated.";
                }
                else
                {
                    AppDbCxt.HospitalDto.Add(data);
                    result.Message = "Data Successfully Inserted.";
                }

                AppDbCxt.SaveChanges();
                result.IsSuccess = true;
                result.Result = data;
                return result;
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = ex.Message;
                return result;
            }
        }

        public async Task<HospitalDto> GetHospitalById(int id)
        {
            HospitalDto result = null;

#pragma warning disable CS8600
            result = AppDbCxt.HospitalDto.FirstOrDefault(o => o.Id == id);
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

            return await Task.FromResult(result);
        }

        public async Task<IEnumerable<HospitalDto>> GetAllHospitals()
        {
            IEnumerable<HospitalDto> result = null;

            result = AppDbCxt.HospitalDto.ToList();
            return result;
        }

        public async Task<ApiResponse<HospitalDto>> DeleteHospital(int id)
        {
            var result = new ApiResponse<HospitalDto>();
            try
            {
                var existing = AppDbCxt.HospitalDto.First(x => x.Id == id);
                result.Result = existing;
                if (existing == null)
                {
                    result.IsSuccess = true;
                    result.Message = "Hospital not found!";
                    return result;
                }

                AppDbCxt.HospitalDto.Remove(existing);
                await AppDbCxt.SaveChangesAsync();
                result.IsSuccess = true;
                result.Message = "Successfully Deleted!";
                return result;
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = ex.Message;
                return result;
            }
        }

        #endregion


        #region Procedures
        public async Task<ApiResponse<ProcedureDto>> UpsertProceduresDetails(ProcedureDto data)
        {
            var result = new ApiResponse<ProcedureDto>();
            try
            {

                if (data == null)
                {
                    result.IsSuccess = true;
                    result.Message = "Invalid Hall data!";
                    return result;
                }

                if (data.Id > 0)
                {
                    AppDbCxt.ProcedureDto.Update(data);
                    result.Message = "Data Successfully Updated.";
                }
                else
                {
                    AppDbCxt.ProcedureDto.Add(data);
                    result.Message = "Data Successfully Inserted.";
                }

                AppDbCxt.SaveChanges();
                result.IsSuccess = true;
                result.Result = data;
                return result;
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = ex.Message;
                return result;
            }
        }

        public async Task<ProcedureDto> GetProcedureById(int id)
        {
            ProcedureDto result = null;

#pragma warning disable CS8600
            result = AppDbCxt.ProcedureDto.FirstOrDefault(o => o.Id == id);
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

            return await Task.FromResult(result);
        }

        public async Task<IEnumerable<ProcedureDto>> GetAllProcedures()
        {
            IEnumerable<ProcedureDto> result = null;

            result = AppDbCxt.ProcedureDto.ToList();
            return result;
        }


        public async Task<ApiResponse<ProcedureDto>> DeleteProcedure(int id)
        {
            var result = new ApiResponse<ProcedureDto>();
            try
            {
                var existing = AppDbCxt.ProcedureDto.First(x => x.Id == id);
                result.Result = existing;
                if (existing == null)
                {
                    result.IsSuccess = true;
                    result.Message = "Procedure not found!";
                    return result;
                }

                AppDbCxt.ProcedureDto.Remove(existing);
                await AppDbCxt.SaveChangesAsync();
                result.IsSuccess = true;
                result.Message = "Successfully Deleted!";
                return result;
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = ex.Message;
                return result;
            }
        }

        #endregion


        #region Doctors
        public async Task<ApiResponse<DoctorsDto>> UpsertDoctorsDetails(DoctorsDto data)
        {
            var result = new ApiResponse<DoctorsDto>();
            try
            {

                if (data == null)
                {
                    result.IsSuccess = true;
                    result.Message = "Invalid Hall data!";
                    return result;
                }

                if (data.Id > 0)
                {
                    AppDbCxt.DoctorsDto.Update(data);
                    result.Message = "Data Successfully Updated.";
                }
                else
                {
                    AppDbCxt.DoctorsDto.Add(data);
                    result.Message = "Data Successfully Inserted.";
                }

                AppDbCxt.SaveChanges();
                result.IsSuccess = true;
                result.Result = data;
                return result;
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = ex.Message;
                return result;
            }
        }

        public async Task<DoctorsDto> GetDoctorById(int id)
        {
            DoctorsDto result = null;

#pragma warning disable CS8600
            result = AppDbCxt.DoctorsDto.FirstOrDefault(o => o.Id == id);
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

            return await Task.FromResult(result);
        }

        public async Task<IEnumerable<DoctorsDto>> GetAllDoctors()
        {
            IEnumerable<DoctorsDto> result = null;

            result = AppDbCxt.DoctorsDto.ToList();
            return result;
        }

        public async Task<ApiResponse<DoctorsDto>> DeleteDoctor(int id)
        {
            var result = new ApiResponse<DoctorsDto>();
            try
            {
                var existing = AppDbCxt.DoctorsDto.First(x => x.Id == id);
                result.Result = existing;
                if (existing == null)
                {
                    result.IsSuccess = true;
                    result.Message = "Doctor not found!";
                    return result;
                }

                AppDbCxt.DoctorsDto.Remove(existing);
                await AppDbCxt.SaveChangesAsync();
                result.IsSuccess = true;
                result.Message = "Successfully Deleted!";
                return result;
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = ex.Message;
                return result;
            }
        }

        #endregion
    }
}


