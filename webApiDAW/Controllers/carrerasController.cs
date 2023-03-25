using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webApiDAW.Models;

namespace webApiDAW.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class carrerasController : ControllerBase
    {
        private readonly equiposContext _equiposContexto;
        public carrerasController(equiposContext equiposContexto)
        {
            _equiposContexto = equiposContexto;
        }


        [HttpGet]
        [Route("GetAll")]
        public IActionResult Get()
        {

            var listaCarreras = (from c in _equiposContexto.carreras
                                 join fac in _equiposContexto.facultades on c.facultad_id equals fac.facultad_id

                                 select new
                                 {
                                     c.carrera_id,
                                     c.nombre_carrera,
                                     c.facultad_id,
                                     fac.nombre_facultad,
                                     c.estado
                                 }).ToList();

            if (listaCarreras.Count == 0)
            {
                return NotFound();
            }
            return Ok(listaCarreras);

        }

        [HttpPost]
        [Route("Add")]
        public IActionResult addCarrera([FromBody] carreras carrera)
        {
            try
            {
                _equiposContexto.carreras.Add(carrera);
                _equiposContexto.SaveChanges();
                return Ok(carrera);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }

        }

        [HttpPut]
        [Route("update/{id}")]
        public IActionResult updateCarrera(int id, [FromBody] carreras carrerasUpdate)
        {


            carreras? carreraActual = (from c in _equiposContexto.carreras where c.carrera_id == id select c).FirstOrDefault();


            if (carreraActual == null) return NotFound();

            carreraActual.nombre_carrera = carrerasUpdate.nombre_carrera;


            _equiposContexto.Entry(carreraActual).State = EntityState.Modified;
            _equiposContexto.SaveChanges();
            return Ok(carreraActual);


        }

        [HttpDelete]
        [Route("delete/{id}")]
        public IActionResult DeleteCarrera(int id)
        {

            carreras? carreraDelete = (from c in _equiposContexto.carreras where c.carrera_id == id select c).FirstOrDefault();


            if (carreraDelete == null) return NotFound();

            _equiposContexto.carreras.Attach(carreraDelete);
            _equiposContexto.carreras.Remove(carreraDelete);
            _equiposContexto.SaveChanges();
            return Ok(carreraDelete);
        }
    }
}
