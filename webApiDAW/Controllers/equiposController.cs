using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webApiDAW.Models;

namespace webApiDAW.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class equiposController : ControllerBase
    {
        private readonly equiposContext _equiposContexto;

        public equiposController(equiposContext equiposContexto)
        {
            _equiposContexto = equiposContexto;
        }


        [HttpGet]
        [Route("GetAll")]
        public IActionResult Get()
        {
            var listadoEquipo = (from e in _equiposContexto.equipos
                                 join m in _equiposContexto.marcas on e.marca_id equals m.id_marcas
                                 join te in _equiposContexto.tipo_equipo on e.tipo_equipo_id equals te.id_tipo_equipo
                                 select new
                                 {
                                     e.id_equipos,
                                     e.descripcion,
                                     e.tipo_equipo_id,
                                     e.marca_id,
                                     m.nombre_marca,
                                     te.id_tipo_equipo,
                                     tipo_equipo = te.descripcion
                                 }).ToList();

            if (listadoEquipo.Count() == 0)
            {
                return NotFound();
            }
            return Ok(listadoEquipo);
        }

        [HttpGet]
        [Route("getbyid/{id}")]

        public IActionResult GetID(int id)
        {
            equipos? equipo = (from e in _equiposContexto.equipos where e.id_equipos == id select e).FirstOrDefault();

            if (equipo == null)
            {
                return NotFound();
            }
            return Ok(equipo);
        }

        [HttpGet]
        [Route("find/{descripcion}")]

        public IActionResult GetByName(string descripcion)
        {
            List<equipos> equipo = (from e in _equiposContexto.equipos where e.descripcion.Contains(descripcion) select e).ToList();

            if (equipo == null)
            {
                return NotFound();
            }
            return Ok(equipo);
        }

        [HttpPost]
        [Route("add")]

        public IActionResult Post([FromBody] equipos equipo)
        {
            try
            {
                _equiposContexto.equipos.Add(equipo);
                _equiposContexto.SaveChanges();
                return Ok(equipo);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("update/{id}")]

        public IActionResult update(int id, [FromBody] equipos equiposActualizar)
        {
            equipos? equipo = (from e in _equiposContexto.equipos where e.id_equipos == id select e).FirstOrDefault();

            if (equipo == null)
            {
                return NotFound();
            }

            equipo.nombre = equiposActualizar.nombre;
            equipo.descripcion = equiposActualizar.descripcion;
            _equiposContexto.Entry(equipo).State = EntityState.Modified;
            _equiposContexto.SaveChanges();
            return Ok(equipo);
        }

        [HttpDelete]
        [Route("delete/{id}")]

        public IActionResult delete(int id)
        {
            equipos? equipo = (from e in _equiposContexto.equipos where e.id_equipos == id select e).FirstOrDefault();

            if (equipo == null)
            {
                return NotFound();
            }

            _equiposContexto.equipos.Attach(equipo);
            _equiposContexto.equipos.Remove(equipo);
            _equiposContexto.SaveChanges();
            return Ok(equipo);

        }
    }
}
