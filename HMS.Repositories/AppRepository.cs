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
                    result.IsSuccess = false;
                    result.Message = "Invalid hospital data!";
                    return result;
                }

                if (data.ImageFormFile != null && data.ImageFormFile.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await data.ImageFormFile.CopyToAsync(memoryStream);
                        data.ImageFile = memoryStream.ToArray(); // Convert to byte[]
                    }
                }

                if (data.Id > 0)
                {
                    // Get the existing hospital from the database
                    var existingHospital = await AppDbCxt.HospitalDto
                        .Include(h => h.MedicalCoreAndSpecialities)
                        .Include(h => h.OtherSpecialities).ThenInclude(os => os.SpecialitiesNames)
                        .FirstOrDefaultAsync(h => h.Id == data.Id);

                    if (existingHospital == null)
                    {
                        result.IsSuccess = false;
                        result.Message = "Hospital not found!";
                        return result;
                    }

                    // Update Hospital fields
                    AppDbCxt.Entry(existingHospital).CurrentValues.SetValues(data);

                    // Handle MedicalCoreAndSpecialities
                    foreach (var medicalCore in data.MedicalCoreAndSpecialities)
                    {
                        var existingMedicalCore = existingHospital.MedicalCoreAndSpecialities
                            .FirstOrDefault(m => m.Id == medicalCore.Id);

                        if (existingMedicalCore != null)
                        {
                            // Update existing
                            AppDbCxt.Entry(existingMedicalCore).CurrentValues.SetValues(medicalCore);
                        }
                        else
                        {
                            // Add new
                            existingHospital.MedicalCoreAndSpecialities.Add(medicalCore);
                        }
                    }
                    // Remove cores that are not present in the incoming data
                    foreach (var medicalCore in existingHospital.MedicalCoreAndSpecialities.ToList())
                    {
                        if (!data.MedicalCoreAndSpecialities.Any(m => m.Id == medicalCore.Id))
                        {
                            AppDbCxt.MedicalCoreAndSpecialities.Remove(medicalCore);
                        }
                    }

                    // Handle OtherSpecialities
                    foreach (var otherSpeciality in data.OtherSpecialities)
                    {
                        var existingOtherSpeciality = existingHospital.OtherSpecialities
                            .FirstOrDefault(os => os.Id == otherSpeciality.Id);

                        if (existingOtherSpeciality != null)
                        {
                            // Update existing
                            AppDbCxt.Entry(existingOtherSpeciality).CurrentValues.SetValues(otherSpeciality);

                            // Handle SpecialitiesNames within OtherSpecialities
                            foreach (var specialityName in otherSpeciality.SpecialitiesNames)
                            {
                                var existingSpecialityName = existingOtherSpeciality.SpecialitiesNames
                                    .FirstOrDefault(sn => sn.Id == specialityName.Id);

                                if (existingSpecialityName != null)
                                {
                                    AppDbCxt.Entry(existingSpecialityName).CurrentValues.SetValues(specialityName);
                                }
                                else
                                {
                                    existingOtherSpeciality.SpecialitiesNames.Add(specialityName);
                                }
                            }
                            // Remove speciality names that are not present in the incoming data
                            foreach (var specialityName in existingOtherSpeciality.SpecialitiesNames.ToList())
                            {
                                if (!otherSpeciality.SpecialitiesNames.Any(sn => sn.Id == specialityName.Id))
                                {
                                    AppDbCxt.SpecialitiesNames.Remove(specialityName);
                                }
                            }
                        }
                        else
                        {
                            // Add new OtherSpecialities
                            existingHospital.OtherSpecialities.Add(otherSpeciality);
                        }
                    }
                    // Remove OtherSpecialities that are not present in the incoming data
                    foreach (var otherSpeciality in existingHospital.OtherSpecialities.ToList())
                    {
                        if (!data.OtherSpecialities.Any(os => os.Id == otherSpeciality.Id))
                        {
                            AppDbCxt.OtherSpecialities.Remove(otherSpeciality);
                        }
                    }

                    result.Message = "Data successfully updated.";
                }
                else
                {
                    // Add new hospital
                    AppDbCxt.HospitalDto.Add(data);
                    result.Message = "Data successfully inserted.";
                }

                await AppDbCxt.SaveChangesAsync();
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
            // Use Include to load related entities
            var result = await AppDbCxt.HospitalDto
                .Include(h => h.MedicalCoreAndSpecialities) // Include Medical Core Specialities
                .Include(h => h.OtherSpecialities) // Include Other Specialities
                .FirstOrDefaultAsync(o => o.Id == id); // Use FirstOrDefaultAsync for async operation

            return result; // No need for Task.FromResult, await handles it
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

                if (data.ImageFormFile != null && data.ImageFormFile.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await data.ImageFormFile.CopyToAsync(memoryStream);
                        data.Image = memoryStream.ToArray(); // Convert to byte[]
                    }
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

        #region Contact
        public async Task<bool> ContactDoctor(ContactDoctor contactDoctor)
        {
            try
            {
                AppDbCxt.ContactDoctors.Add(contactDoctor);
                await AppDbCxt.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> ContactHospital(ContactHospital contactHospital)
        {
            try
            {
                AppDbCxt.ContactHospital.Add(contactHospital);
                await AppDbCxt.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        #endregion
    }
}


