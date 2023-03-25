using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webApiDAW.Models;

namespace webApiDAW.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class tipo_equipoController : ControllerBase
    {
        private readonly equiposContext _equiposContexto;
        public tipo_equipoController(equiposContext equiposContexto)
        {
            _equiposContexto = equiposContexto;
        }

        [HttpGet]
        [Route("GetAll")]
        public IActionResult Get()
        {
            List<tipo_equipo> listTe = (from teq in _equiposContexto.tipo_equipo select teq).ToList();

            if (listTe.Count == 0)
            {
                return NotFound();
            }
            return Ok(listTe);
        }


        [HttpPost]
        [Route("Add")]
        public IActionResult addMark([FromBody] tipo_equipo Tipo_EquipoAdd)
        {
            try
            {
                _equiposContexto.Add(Tipo_EquipoAdd);
                _equiposContexto.SaveChanges();
                return Ok(Tipo_EquipoAdd);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }

        }

        [HttpPut]
        [Route("update/{id}")]
        public IActionResult updateData(int id, [FromBody] tipo_equipo tipo_EquipoMod)
        {
            tipo_equipo? tipoEquipoAct = (from teq in _equiposContexto.tipo_equipo where teq.id_tipo_equipo == id select teq).FirstOrDefault();


            if (tipoEquipoAct == null) return NotFound();


            tipoEquipoAct.descripcion = tipo_EquipoMod.descripcion;
            tipoEquipoAct.estado = tipo_EquipoMod.estado;

            _equiposContexto.Entry(tipoEquipoAct).State = EntityState.Modified;
            _equiposContexto.SaveChanges();
            return Ok(tipoEquipoAct);
        }

        [HttpDelete]
        [Route("delete/{id}")]
        public IActionResult Delete(int id)
        {
            tipo_equipo? tipoE = (from teq in _equiposContexto.tipo_equipo where teq.id_tipo_equipo == id select teq).FirstOrDefault();


            if (tipoE == null) return NotFound();

            _equiposContexto.tipo_equipo.Attach(tipoE);
            _equiposContexto.tipo_equipo.Remove(tipoE);
            _equiposContexto.SaveChanges();
            return Ok(tipoE);
        }
    }
}
