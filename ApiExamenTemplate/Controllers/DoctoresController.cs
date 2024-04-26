using ApiExamenTemplate.Models;
using ApiExamenTemplate.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiExamenTemplate.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctoresController : ControllerBase
    {
        private DoctoresRepository repo;
        public DoctoresController(DoctoresRepository repo)
        {
            this.repo = repo;
        }

        [HttpGet]
        public async Task<ActionResult<List<Doctor>>> GetDoctores()
        {
            return await this.repo.GetAllDoctoresAsync();
        }

        [HttpGet("[action]/{especialidad}")]
        public async Task<ActionResult<List<Doctor>>>
            GetDoctoresByEspecialidad(string especialidad)
        {
            return await this.repo.GetDoctoresEspecialidadAsync(especialidad);
        }
        [HttpGet("[action]")]
        public async Task<ActionResult<List<string>>> GetEspecialidades()
        {
            return await this.repo.GetEspecialidadesAsync();
        }

        [Authorize]
        [HttpGet("[action]/{id}")]
        public async Task<ActionResult<Doctor>> FindDoctor(int id)
        {
            Doctor doc = await this.repo.FindDoctorByIdAsync(id);
            return doc == null ? NotFound() : doc;
        }

        [HttpDelete("[action]/{id}")]
        public async Task<ActionResult> DeleteDoctor(int id)
        {
            await this.repo.DeleteDoctorAsync(id);
            return Ok();
        }

        [HttpPut("[action]")]
        public async Task<ActionResult> UpdateDoctor(Doctor doctor)
        {
            await this.repo.UpdateDoctorAsync(doctor);
            return Ok();
        }

        [HttpPost("[action]")]
        public async Task<ActionResult> CreateDoctor(Doctor doctor)
        {
            await this.repo.CreateDoctorAsync(doctor);
            return Ok();
        }
    }
}
