using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webApiDAW.Models;

namespace webApiDAW.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class reservasController : ControllerBase
    {
        private readonly equiposContext _equiposContexto;
        public reservasController(equiposContext equiposContexto)
        {
            _equiposContexto = equiposContexto;
        }




        [HttpGet]
        [Route("GetAll")]
        public IActionResult Get()
        {
            var listaRes = (from res in _equiposContexto.reservas
                            join equip in _equiposContexto.equipos on res.equipo_id equals equip.id_equipos
                            join usu in _equiposContexto.usuarios on res.usuario_id equals usu.usuario_id
                            join eres in _equiposContexto.estados_reserva on res.reserva_id equals eres.estado_res_id
                            select new
                            {
                                res.reserva_id,
                                res.equipo_id,
                                equip.nombre,
                                equip.descripcion,
                                equip.costo,
                                res.usuario_id,
                                res.fecha_salida,
                                res.fecha_retorno,
                                res.tiempo_reserva,
                                res.estado_reserva_id,
                                nUsuario = usu.nombre,
                                usu.documento,
                                usu.carnet,
                                estadoRes = eres.estado,
                                res.estado
                            }).ToList();

            if (listaRes.Count == 0)
            {
                return NotFound();
            }
            return Ok(listaRes);

        }

        [HttpPost]
        [Route("Add")]
        public IActionResult AddRes([FromBody] reservas reservaAdd)
        {
            try
            {
                _equiposContexto.reservas.Add(reservaAdd);
                _equiposContexto.SaveChanges();
                return Ok(reservaAdd);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }

        }

        //Update
        [HttpPut]
        [Route("update/{id}")]
        public IActionResult updateData(int id, [FromBody] reservas reservasMod)
        {
            reservas? reservaAct = (from res in _equiposContexto.reservas where res.reserva_id == id select res).FirstOrDefault();


            if (reservaAct == null) return NotFound();

            reservaAct.fecha_salida = reservasMod.fecha_salida;
            reservaAct.hora_salida = reservasMod.hora_salida;
            reservaAct.tiempo_reserva = reservasMod.tiempo_reserva;
            reservaAct.fecha_retorno = reservasMod.fecha_retorno;
            reservaAct.hora_retorno = reservasMod.hora_retorno;

            _equiposContexto.Entry(reservaAct).State = EntityState.Modified;
            _equiposContexto.SaveChanges();
            return Ok(reservaAct);

        }

        [HttpDelete]
        [Route("delete/{id}")]
        public IActionResult DeleteRes(int id)
        {
            reservas? reservaDelete = (from res in _equiposContexto.reservas where res.reserva_id == id select res).FirstOrDefault();


            if (reservaDelete == null) return NotFound();

            _equiposContexto.reservas.Attach(reservaDelete);
            _equiposContexto.reservas.Remove(reservaDelete);
            _equiposContexto.SaveChanges();
            return Ok(reservaDelete);
        }
    }
}
