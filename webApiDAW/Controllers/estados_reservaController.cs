using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webApiDAW.Models;

namespace webApiDAW.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class estados_reservaController : ControllerBase
    {
        private readonly equiposContext _equiposContexto;
        public estados_reservaController(equiposContext equiposContext)
        {
            _equiposContexto = equiposContext;
        }

        [HttpGet]
        [Route("GetAll")]
        public ActionResult Get()
        {
            List<estados_reserva> estadoRe = (from sre in _equiposContexto.estados_reserva select sre).ToList();

            if (estadoRe.Count == 0)
            {
                return NotFound();
            }
            return Ok(estadoRe);
        }

        [HttpPost]
        [Route("Add")]
        public IActionResult AddER([FromBody] estados_reserva estadoRES)
        {
            try
            {
                _equiposContexto.estados_reserva.Add(estadoRES);
                _equiposContexto.SaveChanges();
                return Ok(estadoRES);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }


        [HttpPut]
        [Route("update/{id}")]
        public IActionResult updateER(int id, [FromBody] estados_equipo estadoREMOD)
        {
            estados_reserva? estadoRE = (from sre in _equiposContexto.estados_reserva where sre.estado_res_id == id select sre).FirstOrDefault();

            if (estadoRE == null) return NotFound();

            estadoRE.estado = estadoREMOD.estado;

            _equiposContexto.Entry(estadoREMOD).State = EntityState.Modified;
            _equiposContexto.SaveChanges();
            return Ok(estadoREMOD);
        }


        [HttpDelete]
        [Route("delete/{id}")]
        public IActionResult deleteER(int id)
        {
            try
            {
                estados_reserva? estadoRE = (from sre in _equiposContexto.estados_reserva where sre.estado_res_id == id select sre).FirstOrDefault();

                if (estadoRE == null) return NotFound();

                _equiposContexto.estados_reserva.Attach(estadoRE);
                _equiposContexto.estados_reserva.Remove(estadoRE);
                _equiposContexto.SaveChanges();
                return Ok(estadoRE);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
    }
}
