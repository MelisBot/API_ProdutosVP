using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class Producto
    {
        //Add
        public static ML.Result AddSPEF(ML.Producto producto) 
        {
            ML.Result result = new ML.Result();
            try 
            {
                // Validaciones 
                if (producto.Nombre == null)
                {
                    result.Correct = false;
                    result.ErrorMessage = "El nombre del producto es obligatorio.";
                    return result;
                }

                if (producto.Precio <= 0)
                {
                    result.Correct = false;
                    result.ErrorMessage = "El precio debe ser mayor a 0.";
                    return result;
                }

                if (producto.Stock < 0)
                {
                    result.Correct = false;
                    result.ErrorMessage = "El stock no puede ser negativo.";
                    return result;
                }
                using (DL.MJProductoEntities context = new DL.MJProductoEntities()) 
                {
                    int filasAfectadas = context.ProductoAdd
                        (
                            producto.Nombre,
                            producto.Descripcion,
                            producto.Precio,
                            producto.Stock,
                            producto.Categoria
                        );

                    if (filasAfectadas > 0)
                    {
                        result.Correct = true;
                    }
                    else 
                    {
                        result.Correct = false;
                        result.ErrorMessage = "Producto no agregado";
                    }
                }
            }
            catch (Exception ex)
            {
                result.Correct = true;
                result.Ex = ex;
                result.ErrorMessage = ex.Message;

            }
            return result;
        }
        //GetAll
        public static ML.Result GetAllSPEF()
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.MJProductoEntities context = new DL.MJProductoEntities())
                {
                    var listaProducto = context.PRODUCTOGetAll().ToList();

                    if (listaProducto.Count > 0)
                    {
                        result.Objects = new List<object>();
                        foreach (var productoObj in listaProducto)
                        {
                            ML.Producto producto = new ML.Producto
                            {
                               IdProducto = productoObj.IdProducto,
                               Nombre = productoObj.Nombre,
                               Descripcion = productoObj.Descripcion,
                               Precio  = productoObj.Precio.Value,
                               Stock =productoObj.Stock.Value,
                               Categoria = productoObj.Categoria,
                               FechaRegistro = productoObj.FechaRegistro.Value
                            };
                            result.Objects.Add(producto);
                        }

                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "Elementos no encontrados";
                    }
                }
                result.Correct = true;
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
                result.Ex = ex;
            }
            return result;
        }
        //Update
        public static ML.Result UpdateSPEF(ML.Producto producto)
        {
            ML.Result result = new ML.Result();
            try
            {
                // Validaciones 
                if (producto.Nombre == null)
                {
                    result.Correct = false;
                    result.ErrorMessage = "El nombre del producto es obligatorio.";
                    return result;
                }

                if (producto.Precio <= 0)
                {
                    result.Correct = false;
                    result.ErrorMessage = "El precio debe ser mayor a 0.";
                    return result;
                }

                if (producto.Stock < 0)
                {
                    result.Correct = false;
                    result.ErrorMessage = "El stock no puede ser negativo.";
                    return result;
                }

                using (DL.MJProductoEntities context = new DL.MJProductoEntities())
                {
                    int filasAfectadas = context.ProductoUpdate(Convert.ToInt32(producto.IdProducto), producto.Nombre, producto.Descripcion,
                        producto.Precio, producto.Stock, producto.Categoria);

                    if (filasAfectadas > 0)
                    {
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "No se pudo editar al cliente";
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

        //Delete
        public static ML.Result DeleteSPEF(int idProducto)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.MJProductoEntities context = new DL.MJProductoEntities())
                {
                    int filasAfectadas = context.ProductoDelete(idProducto);

                    if (filasAfectadas > 0)
                    {
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "No se pudo eliminar el producto";
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

        //GetById
        public static ML.Result GetByIdSPEF(int idProducto)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.MJProductoEntities context = new DL.MJProductoEntities())
                {
                    var productoObj = context.ProductoGetById(idProducto).FirstOrDefault();


                    if (productoObj != null)
                    {
                        ML.Producto producto = new ML.Producto
                        {
                            IdProducto = productoObj.IdProducto,
                            Nombre = productoObj.Nombre,
                            Descripcion = productoObj.Descripcion,
                            Precio = productoObj.Precio.Value,
                            Stock = productoObj.Stock.Value,
                            Categoria = productoObj.Categoria,
                            FechaRegistro = productoObj.FechaRegistro.Value
                        };
                        result.Object = producto;
                        result.Correct = true;

                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "No se enccontro al cliente";
                    }
                }
                result.Correct = true;
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
                result.Ex = ex;
            }
            return result;
        }
    }
}
