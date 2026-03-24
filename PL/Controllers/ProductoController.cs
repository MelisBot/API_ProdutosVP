using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;


namespace PL.Controllers
{
    public class ProductoController : Controller
    {
        
        [HttpGet]
        public ActionResult GetAll()
        {
            ML.Producto producto = new ML.Producto();
            ML.Result result = GetAllREST(producto);

            if (result.Correct)
            {
                producto.Productos = result.Objects;
            }
            else
            {
                ViewBag.Error = $"Error al obtener productos: {result.ErrorMessage}";
            }
            return View(producto);
        }

        [HttpGet]
        public ActionResult Formulario(int? idProducto)
        {
            ML.Producto producto = new ML.Producto();

            if (idProducto.HasValue)
            {
                ML.Result result = GetByIdREST(idProducto.Value);
                if (result.Correct)
                {
                    producto = (ML.Producto)result.Object;
                }
            }
            return View(producto);
        }

        [HttpPost]
        public ActionResult Formulario(ML.Producto producto)
        {
            ML.Result result;
            if (producto.IdProducto > 0)
            {
                result = UpdateREST(producto.IdProducto, producto);
            }
            else
            {
                result = AddREST(producto);
            }

            if (result.Correct)
            {
                return RedirectToAction("GetAll");
            }
            else
            {
                ViewBag.ErrorMessage = result.ErrorMessage;
                return View(producto);
            }
        }

        [HttpPost]
        public ActionResult Delete(int idProducto)
        {
            ML.Result result = DeleteREST(idProducto);
            return RedirectToAction("GetAll");
        }


        // Métodos REST 
        public ML.Result GetAllREST(ML.Producto producto)
        {
            ML.Result result = new ML.Result();
            try
            {
                string urlAPI = ConfigurationManager.AppSettings["URLapi"];

                using (var client = new HttpClient())
                {
                    //RecuperarBaseAddress de AppSettings 
                    client.BaseAddress = new Uri(urlAPI);

                    var responseTask = client.GetAsync("producto");
                    responseTask.Wait(); //abrir otro hilo
                    var resultServicio = responseTask.Result;

                    if (resultServicio.IsSuccessStatusCode) //200 - 299
                    {
                        var readTask = resultServicio.Content.ReadAsAsync<ML.Result>();
                        readTask.Wait();
                        result.Objects = new List<object>();
                        foreach (var resultItem in readTask.Result.Objects)
                        {
                            ML.Producto resultItemList = Newtonsoft.Json.JsonConvert.DeserializeObject<ML.Producto>(resultItem.ToString());
                            result.Objects.Add(resultItemList);
                        }
                        result.Correct = true;
                    }
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.Ex = ex;
                result.ErrorMessage = ex.Message;
            }
            return result;
        }
        public ML.Result AddREST(ML.Producto producto)
        {
            ML.Result result = new ML.Result();
            try
            {
                string urlAPI = ConfigurationManager.AppSettings["URLapi"];

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(urlAPI);

                    var postTask = client.PostAsJsonAsync("producto", producto);
                    postTask.Wait();

                    var response = postTask.Result;

                    if (response.IsSuccessStatusCode)
                    {
                        var readTask = response.Content.ReadAsAsync<ML.Result>();
                        readTask.Wait();
                        result = readTask.Result;
                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = $"Error: {response.StatusCode}";
                    }
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
                result.Ex = ex;
            }

            return result;
        }

        public ML.Result UpdateREST(int idProducto, ML.Producto producto)
        {
            ML.Result result = new ML.Result();

            try
            {
                string urlAPI = ConfigurationManager.AppSettings["URLapi"];

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(urlAPI);

                    var responseTask = client.PutAsJsonAsync("producto/" + idProducto, producto);
                    responseTask.Wait();

                    var resultServicio = responseTask.Result;

                    if (resultServicio.IsSuccessStatusCode)
                    {
                        var readTask = resultServicio.Content.ReadAsAsync<ML.Result>();
                        readTask.Wait();

                        result.Correct = readTask.Result.Correct;
                        result.ErrorMessage = readTask.Result.ErrorMessage;
                        result.Objects = readTask.Result.Objects;
                        result.Object = readTask.Result.Object;
                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = $"Error en la respuesta del servicio: {resultServicio.ReasonPhrase}";
                    }
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
                result.Ex = ex;
            }

            return result;
        }

        public ML.Result DeleteREST(int idProducto)
        {
            ML.Result result = new ML.Result();
            try
            {
                string urlAPI = ConfigurationManager.AppSettings["URLapi"];
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(urlAPI);

                    var responseTask = client.DeleteAsync("producto/" + idProducto);
                    responseTask.Wait();

                    var resultServicio = responseTask.Result;

                    if (resultServicio.IsSuccessStatusCode)
                    {
                        var readTask = resultServicio.Content.ReadAsAsync<ML.Result>();
                        readTask.Wait();

                        result.Correct = readTask.Result.Correct;
                        result.ErrorMessage = readTask.Result.ErrorMessage;
                        result.Objects = readTask.Result.Objects;
                        result.Object = readTask.Result.Object;
                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = $"Error en la respuesta del servicio: {resultServicio.ReasonPhrase}";
                    }
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
                result.Ex = ex;
            }
            return result;

        }
        public static ML.Result GetByIdREST(int idProducto)
        {
            ML.Result result = new ML.Result();
            try
            {
                string urlAPI = System.Configuration.ConfigurationManager.AppSettings["URLapi"];
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(urlAPI);
                    var responseTask = client.GetAsync("producto/" + idProducto);
                    responseTask.Wait();
                    var resultAPI = responseTask.Result;
                    if (resultAPI.IsSuccessStatusCode)
                    {
                        var readTask = resultAPI.Content.ReadAsAsync<ML.Result>();
                        readTask.Wait();
                        ML.Producto resultItemList = new ML.Producto();
                        resultItemList = Newtonsoft.Json.JsonConvert.DeserializeObject<ML.Producto>(readTask.Result.Object.ToString());
                        result.Object = resultItemList;

                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "No existen registros en la tabla Departamento";
                    }
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;

            }
            return result;
        }

    }
}
