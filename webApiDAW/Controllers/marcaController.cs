using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webApiDAW.Models;

namespace webApiDAW.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class marcaController : ControllerBase
    {
        private readonly equiposContext _equiposContexto;
        public marcaController(equiposContext equiposContexto)
        {
            _equiposContexto = equiposContexto;
        }

        [HttpGet]
        [Route("GetAll")]
        public IActionResult Get()
        {
            List<marcas> listmarca = (from m in _equiposContexto.marcas select m).ToList();

            if (listmarca.Count == 0)
            {
                return NotFound();
            }

            return Ok(listmarca);
        }

        [HttpPost]
        [Route("Add")]
        public IActionResult addMarca([FromBody] marcas marcas)
        {
            try
            {
                _equiposContexto.marcas.Add(marcas);
                _equiposContexto.SaveChanges();
                return Ok(marcas);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }

        }


        [HttpPut]
        [Route("update/{id}")]
        public IActionResult updateMarca(int id, [FromBody] marcas marcasMod)
        {
            marcas? marcasAct = (from m in _equiposContexto.marcas where m.id_marcas == id select m).FirstOrDefault();


            if (marcasAct == null) return NotFound();


            marcasAct.nombre_marca = marcasMod.nombre_marca;
            marcasAct.estados = marcasMod.estados;

            _equiposContexto.Entry(marcasAct).State = EntityState.Modified;
            _equiposContexto.SaveChanges();
            return Ok(marcasAct);
        }

        [HttpDelete]
        [Route("delete/{id}")]
        public IActionResult DeleteMarca(int id)
        {
            marcas? marcas = (from m in _equiposContexto.marcas where m.id_marcas == id select m).FirstOrDefault();


            if (marcas == null) return NotFound();

            _equiposContexto.marcas.Attach(marcas);
            _equiposContexto.marcas.Remove(marcas);
            _equiposContexto.SaveChanges();
            return Ok(marcas);
        }
    }
}
