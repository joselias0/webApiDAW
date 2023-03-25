using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webApiDAW.Models;

namespace webApiDAW.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class estados_equipoController : ControllerBase
    {
        private readonly equiposContext _equiposContexto;

        public estados_equipoController(equiposContext equiposContexto)
        {
            _equiposContexto = equiposContexto;
        }


        [HttpGet]
        [Route("GetAll")]
        public ActionResult Get()
        {

            List<estados_equipo> estadoEq = (from seq in _equiposContexto.estados_equipos select seq).ToList();

            if (estadoEq.Count == 0)
            {
                return NotFound();
            }
            return Ok(estadoEq);
        }

        [HttpPost]
        [Route("Add")]
        public IActionResult AddEstadoEQ([FromBody] estados_equipo estadoEq)
        {
            try
            {
                _equiposContexto.estados_equipos.Add(estadoEq);
                _equiposContexto.SaveChanges();
                return Ok(estadoEq);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }


        [HttpPut]
        [Route("update/{id}")]
        public IActionResult updateEstadoEQ(int id, [FromBody] estados_equipo stateEquipModificar)
        {
            estados_equipo? estadosEQ = (from seq in _equiposContexto.estados_equipos where seq.id_estados_equipo == id select seq).FirstOrDefault();

            if (estadosEQ == null) return NotFound();

            estadosEQ.descripcion = stateEquipModificar.descripcion;
            estadosEQ.estado = stateEquipModificar.estado;

            _equiposContexto.Entry(estadosEQ).State = EntityState.Modified;
            _equiposContexto.SaveChanges();
            return Ok(estadosEQ);
        }


        [HttpDelete]
        [Route("delete/{id}")]
        public IActionResult deleteEstadoEQ(int id)
        {
            estados_equipo? estadosEQ = (from seq in _equiposContexto.estados_equipos where seq.id_estados_equipo == id select seq).FirstOrDefault();

            if (estadosEQ == null) return NotFound();

            _equiposContexto.estados_equipos.Attach(estadosEQ);
            _equiposContexto.estados_equipos.Remove(estadosEQ);
            _equiposContexto.SaveChanges();
            return Ok(estadosEQ);
        }
    }
}
