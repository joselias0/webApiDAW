using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webApiDAW.Models;

namespace webApiDAW.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class facultadesController : ControllerBase
    {
        private readonly equiposContext _equiposContexto;
        public facultadesController(equiposContext equiposContexto)
        {
            _equiposContexto = equiposContexto;
        }

        [HttpGet]
        [Route("GetAll")]
        public IActionResult Get()
        {

            List<facultades> listaFac = (from facu in _equiposContexto.facultades select facu).ToList();

            if (listaFac.Count == 0)
            {
                return NotFound();
            }
            return Ok(listaFac);
        }

        [HttpPost]
        [Route("Add")]
        public IActionResult addFac([FromBody] facultades fac)
        {
            try
            {
                _equiposContexto.facultades.Add(fac);
                _equiposContexto.SaveChanges();
                return Ok(fac);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPut]
        [Route("update/{id}")]
        public IActionResult updateFacultad(int id, [FromBody] facultades facultadModificar)
        {


            facultades? fac = (from facu in _equiposContexto.facultades where facu.facultad_id == id select facu).FirstOrDefault();


            if (fac == null) return NotFound();

            fac.nombre_facultad = facultadModificar.nombre_facultad;


            _equiposContexto.Entry(fac).State = EntityState.Modified;
            _equiposContexto.SaveChanges();
            return Ok(fac);
        }

        [HttpDelete]
        [Route("delete/{id}")]
        public IActionResult DeleteFAC(int id)
        {
            facultades? facul = (from facu in _equiposContexto.facultades where facu.facultad_id == id select facu).FirstOrDefault();


            if (facul == null) return NotFound();

            _equiposContexto.facultades.Attach(facul);
            _equiposContexto.facultades.Remove(facul);
            _equiposContexto.SaveChanges();
            return Ok(facul);
        }
    }
}
