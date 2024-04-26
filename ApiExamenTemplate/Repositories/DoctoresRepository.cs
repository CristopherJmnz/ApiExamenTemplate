using ApiExamenTemplate.Data;
using ApiExamenTemplate.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiExamenTemplate.Repositories
{
    public class DoctoresRepository
    {
        private DoctoresContext context;
        public DoctoresRepository(DoctoresContext context)
        {
            this.context = context;
        }

        public async Task<List<Doctor>> GetAllDoctoresAsync()
        {
            return await this.context.Doctores.ToListAsync();
        }

        public async Task<Doctor> FindDoctorByIdAsync(int id)
        {
            return await this.context.Doctores
                .FirstOrDefaultAsync(x => x.IdDoctor == id);
        }

        private async Task<int> GetMaxIdDoctorAsync()
        {
            return await this.context.Doctores
                .MaxAsync(x => x.IdDoctor) + 1;
        }

        public async Task CreateDoctorAsync(Doctor doctor)
        {
            doctor.IdDoctor = await this.GetMaxIdDoctorAsync();
            await this.context.Doctores.AddAsync(doctor);
            await this.context.SaveChangesAsync();
        }

        public async Task UpdateDoctorAsync(Doctor doctorNew)
        {
            Doctor doctorOld = await this.FindDoctorByIdAsync(doctorNew.IdDoctor);
            doctorOld.Salario = doctorNew.Salario;
            doctorOld.Especialidad = doctorNew.Especialidad;
            doctorOld.IdHospital = doctorNew.IdHospital;
            doctorOld.Apellido = doctorNew.Apellido;
            doctorOld.IdHospital = doctorNew.IdHospital;
            await this.context.SaveChangesAsync();
        }

        public async Task DeleteDoctorAsync(int id)
        {
            Doctor doc = await this.FindDoctorByIdAsync(id);
            this.context.Doctores.Remove(doc);
            await this.context.SaveChangesAsync();
        }

        public async Task<List<Doctor>>
            GetDoctoresEspecialidadAsync(string especialidad)
        {
            List<Doctor> doctores = await this.context.Doctores
                .Where(x => x.Especialidad == especialidad).ToListAsync();
            return doctores.Count == 0 ? null : doctores;
        }

        public async Task<List<string>>
            GetEspecialidadesAsync()
        {
            var consulta = from datos in this.context.Doctores
                           select datos.Especialidad;
            List<string> especialidades = await consulta.ToListAsync();
            return especialidades.Count == 0 ? null : especialidades;
        }

        public async Task<Doctor> LogInAsync
            (string username, int password)
        {
            return await this.context.Doctores
                .FirstOrDefaultAsync(x => x.Apellido == username
                && x.IdDoctor == password);
        }
    }
}
