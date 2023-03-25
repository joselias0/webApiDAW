using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webApiDAW.Models;

namespace webApiDAW.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class usuariosController : ControllerBase
    {
        private readonly equiposContext _equiposContexto;

        public usuariosController(equiposContext equiposContexto)
        {
            _equiposContexto = equiposContexto;
        }

        [HttpGet]
        [Route("GetAll")]
        public IActionResult get()
        {

            var listaUsuario = (from usu in _equiposContexto.usuarios
                                join carr in _equiposContexto.carreras on usu.carrera_id equals carr.carrera_id

                                select new
                                {
                                    usu.usuario_id,
                                    usu.nombre,
                                    usu.documento,
                                    usu.tipo,
                                    usu.carnet,
                                    usu.carrera_id,
                                    usu.estado,
                                    carr.nombre_carrera
                                }).ToList();

            if (listaUsuario.Count == 0)
            {
                return NotFound();
            }
            return Ok(listaUsuario);
        }

        [HttpPost]
        [Route("Add")]
        public IActionResult AddUsuario([FromBody] usuarios usuarios)
        {
            try
            {
                _equiposContexto.usuarios.Add(usuarios);
                _equiposContexto.SaveChanges();
                return Ok(usuarios);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }


        [HttpPut]
        [Route("Update/{id}")]
        public IActionResult updateData(int id, [FromBody] usuarios usuarioMod)
        {
            usuarios? usuarioAct = (from usu in _equiposContexto.usuarios where usu.usuario_id == id select usu).FirstOrDefault();


            if (usuarioAct == null) return NotFound();

            usuarioAct.nombre = usuarioMod.nombre;
            usuarioAct.documento = usuarioMod.documento;
            usuarioAct.tipo = usuarioMod.tipo;
            usuarioAct.carnet = usuarioMod.carnet;

            _equiposContexto.Entry(usuarioAct).State = EntityState.Modified;
            _equiposContexto.SaveChanges();
            return Ok(usuarioAct);
        }

        [HttpDelete]
        [Route("Delete/{id}")]
        public IActionResult DeleteUsu(int id)
        {

            usuarios? usuarios = (from usu in _equiposContexto.usuarios where usu.usuario_id == id select usu).FirstOrDefault();


            if (usuarios == null) return NotFound();

            _equiposContexto.usuarios.Attach(usuarios);
            _equiposContexto.usuarios.Remove(usuarios);
            _equiposContexto.SaveChanges();
            return Ok(usuarios);
        }
    }
}
