using ML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SL_REST.Controllers
{
    public class ProductoController : ApiController
    {
        // GET /api/producto
        [HttpGet]
        public IHttpActionResult GetAll()
        {
            ML.Result result = BL.Producto.GetAllSPEF();
            if (result.Correct)
            {
                return Ok(result); // devuelve el objeto completo
            }
            else
            {
                return Content(HttpStatusCode.BadRequest, result);
            }
        }

        // GET /api/producto/{id}
        [HttpGet]
        public IHttpActionResult GetById(int id)
        {
            ML.Result result = BL.Producto.GetByIdSPEF(id);
            if (result.Correct)
            {
                return Ok(result);
            }
            else
            {
                return Content(HttpStatusCode.NotFound, result);
            }
        }

        // POST /api/producto
        [HttpPost]
        public IHttpActionResult Add([FromBody] ML.Producto producto)
        {
            ML.Result result = BL.Producto.AddSPEF(producto);
            if (result.Correct)
            {
                return Ok(result);
            }
            else
            {
                return Content(HttpStatusCode.BadRequest, result);
            }
        }

        // PUT /api/producto/{id}
        [HttpPut]
        public IHttpActionResult Update(int id, [FromBody] ML.Producto producto)
        {
            producto.IdProducto = id;
            ML.Result result = BL.Producto.UpdateSPEF(producto);
            if (result.Correct)
            {
                return Ok(result);
            }
            else
            {
                return Content(HttpStatusCode.BadRequest, result);
            }
        }

        // DELETE /api/producto/{id}
        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            ML.Result result = BL.Producto.DeleteSPEF(id);
            if (result.Correct)
            {
                return Ok(result);
            }
            else
            {
                return Content(HttpStatusCode.BadRequest, result);
            }
        }
    }
}

