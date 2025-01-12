using GAPPLE.Client.Extensiones;
using GAPPLE.Server.Data;
using GAPPLE.Shared.Model;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Security.Claims;

namespace GAPPLE.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeguridadController : ControllerBase
    {
        private IConfiguration Configuration { get; }
        private Usuario Usuario { get; }

        public SeguridadController(IConfiguration configuration)
        {
            Configuration = configuration;
            Usuario = new Usuario();
        }

        [HttpGet("usuarios")]
        public List<Usuario> GetUsuarios(int? idUsuario, string? nombreUsuario, string? apellidoYNombre, int? idPerfil, bool? habilitado)
        {
            DA_Seguridad daS = new(Configuration.GetConnectionString("DefaultConnection"));
            List<Usuario> usuarios = new();
            using (DataTable dt = daS.GetUsuarios(idUsuario, nombreUsuario, apellidoYNombre, idPerfil, habilitado))
            {
                foreach (DataRow row in dt.Rows)
                {
                    Usuario usuario = new Usuario();
                    usuario.IdUsuario = int.Parse(row["IdUsuario"].ToString()!);
                    usuario.NombreUsuario = row["NombreUsuario"].ToString();
                    usuario.ApellidoYNombre = row["ApellidoYNombre"].ToString();
                    usuario.Perfil = int.Parse(row["IdPerfil"].ToString());
                    usuario.PerfilCompleto = new(usuario.Perfil, row["DescripcionPerfil"].ToString());
                    usuario.Email = row["Correo"].ToString();
                    usuario.Provincia = row["Provincia"].ToString();
                    usuario.Habilitado = bool.Parse(row["Habilitado"].ToString());
                    usuarios.Add(usuario);
                }
            }
            return usuarios;
        }

        [HttpPost("usuario")]
        public IActionResult PostUsuario(Usuario usuario)
        {
            DA_Seguridad daS = new(Configuration.GetConnectionString("DefaultConnection"));
            daS.PostUsuario(usuario.NombreUsuario, usuario.ApellidoYNombre, usuario.Perfil, usuario.Email, usuario.Provincia, usuario.Habilitado, usuario.Contraseña);
            return Ok();
        }

        [HttpPut("usuario")]
        public IActionResult PutUsuario(Usuario usuario)
        {
            DA_Seguridad daS = new(Configuration.GetConnectionString("DefaultConnection"));
            daS.PutUsuario(usuario.IdUsuario, usuario.NombreUsuario, usuario.ApellidoYNombre, usuario.Perfil, usuario.Email, usuario.Provincia, usuario.Habilitado, usuario.Contraseña);
            return Ok();
        }

        [HttpGet("usuarios/perfiles")]
        public List<PerfilUsuario> GetUsuariosPerfiles(int? idPerfil, string? descripcion)
        {
            DA_Seguridad daS = new(Configuration.GetConnectionString("DefaultConnection"));
            List<PerfilUsuario> perfiles = new();
            using (DataTable dt = daS.GetUsuariosPerfiles(idPerfil, descripcion))
            {
                foreach (DataRow row in dt.Rows)
                {
                    PerfilUsuario perfil = new PerfilUsuario();
                    perfil.IdPerfil = int.Parse(row["IdPerfil"].ToString());
                    perfil.DescripcionPerfil = row["Descripcion"].ToString();
                    perfiles.Add(perfil);
                }
            }
            return perfiles;
        }

        [HttpGet("validaracceso")]
        public bool? ValidarAcceso(string href)
        {
            DA_Parametro daP = new(Configuration.GetConnectionString("DefaultConnection"));
            if (Usuario != null)
                return daP.ObtenerPermisos(Usuario.IdUsuario, 'M', "/" + href, null, null).Rows.Count > 0;
            else return null;
        }

        [HttpGet("permisos/componente")]
        public List<string> ObtenerPermisos(string nombre, char tipoPermiso)
        {
            List<string> list = new();

            if (Usuario != null)
            {
                DA_Parametro daP = new(Configuration.GetConnectionString("DefaultConnection"));
                int idPermiso = (int)daP.ObtenerPermisos(Usuario.IdUsuario, null, null, null, nombre).Rows[0]["IdPermiso"];
                foreach (DataRow row in daP.ObtenerPermisos(Usuario.IdUsuario, tipoPermiso, null, idPermiso, null).Rows)
                {
                    if (row["HRef"] != DBNull.Value)
                        list.Add((string)row["HRef"]);
                    else
                        list.Add((string)row["Nombre"]);
                }
            }

            return list;
        }

        [HttpPost("permiso")]
        public IActionResult PostPermiso(Menu permiso)
        {
            try
            {
                DA_Seguridad da = new(Configuration.GetConnectionString("DefaultConnection"));
                int idPermiso = da.InsertarPermiso((int)permiso.IdPadre, permiso.Nombre, permiso.Tipo, permiso.Url, permiso.Icono, (int)permiso.Orden);
                permiso.IdPermiso = idPermiso;
                return Ok(permiso);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.ToString());
            }
        }

        [HttpPut("permiso")]
        public IActionResult PutPermiso(Menu permiso)
        {
            try
            {
                DA_Seguridad da = new(Configuration.GetConnectionString("DefaultConnection"));
                da.ActualizarPermiso(permiso.IdPermiso, (int)permiso.IdPadre, permiso.Nombre, permiso.Tipo, permiso.Url, permiso.Icono, (int)permiso.Orden);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.ToString());
            }
        }

        [HttpDelete("permiso/{idPermiso:int}")]
        public IActionResult DeletePermiso(int idPermiso)
        {
            DA_Seguridad da = new(Configuration.GetConnectionString("DefaultConnection"));
            using (SqlConnection connection = new(Configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();
                try
                {
                    da.EliminarPermisoPorPerfil(null, idPermiso, transaction);
                    da.EliminarPermisoPorUsuario(null, idPermiso, transaction);
                    da.EliminarPermiso(idPermiso, transaction);
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction?.Rollback();
                    return StatusCode(500, ex.ToString());
                }
            }
            return Ok();
        }

        [HttpGet("permisos")]
        public List<Menu> GetPermisos()
        {
            List<Menu> list = new();
            DA_Seguridad da = new(Configuration.GetConnectionString("DefaultConnection"));

            foreach (DataRow row in da.ObtenerPermisos().Rows)
            {
                Menu menu = new()
                {
                    IdPermiso = (int)row["IdPermiso"],
                    Tipo = char.Parse(row["Tipo"].ToString()),
                    TieneHijos = (bool)row["TieneHijos"]
                };
                if (row["Nombre"] != DBNull.Value) menu.Nombre = (string)row["Nombre"];
                if (row["IdPadre"] != DBNull.Value) menu.IdPadre = (int)row["IdPadre"];
                if (row["HRef"] != DBNull.Value) menu.Url = (string)row["HRef"];
                if (row["Icono"] != DBNull.Value) menu.Icono = (string)row["Icono"];
                if (row["Orden"] != DBNull.Value) menu.Orden = (int)row["Orden"];

                list.Add(menu);
            }

            return list;
        }

        [HttpGet("usuariosporperfil")]
        public List<string> GetUsuariosPorPerfil(int idPerfil)
        {
            List<string> usuarios = new();
            DA_Usuario daU = new(Configuration.GetConnectionString("DefaultConnection"));
            using (DataTable dt = daU.ObtenerUsuariosPorPerfil(idPerfil))
            {
                foreach (DataRow row in dt.Rows)
                {
                    string name;
                    name = row["ApellidoYNombre"].ToString();
                    usuarios.Add(name);
                }
            }
            return usuarios;
        }

        [HttpGet("usuario/{id:int}")]
        public Usuario GetUsuario(int id)
        {
            Usuario usuario = null;
            DA_Usuario da = new(Configuration.GetConnectionString("DefaultConnection"));
            DataTable dt = da.ObtenerUsuario(idUsuario: id);
            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                usuario = new()
                {
                    IdUsuario = (int)row["IdUsuario"],
                    NombreUsuario = (string)row["NombreUsuario"],
                    ApellidoYNombre = (string)row["ApellidoYNombre"],
                    Perfil = (int)row["IdPerfil"],
                    Provincia = (string)row["Provincia"]
                };

                if (row["Provincia"] != DBNull.Value) usuario.Provincia = (string)row["Provincia"];
                if (row["Correo"] != DBNull.Value) usuario.Email = (string)row["Correo"];
            }
            return usuario;
        }

        [HttpPost("permisosUsuario")]
        public IActionResult PostPermisosPorUsuario(List<Permiso> lstCambios)
        {
            DA_Usuario daU = new(Configuration.GetConnectionString("DefaultConnection"));
            List<Permiso> Habilitar = new();
            List<Permiso> Eliminar = new();
            try
            {
                foreach (var item in lstCambios)
                {
                    if (item.Eliminar)
                    {
                        Eliminar.Add(item);
                    }
                    else
                    {
                        Habilitar.Add(item);
                    }
                }

                foreach (var item in Habilitar)
                {
                    daU.PostPermisoPorUsuario((int)item.IdPerfilOUsuario, item.IdPermiso, (bool)item.HabilitadoPorUsuario, null);
                }
                foreach (var item in Eliminar)
                {
                    daU.DeletePermisoPorUsuario((int)item.IdPerfilOUsuario, item.IdPermiso, null);
                }
                return Ok("Los datos se han guardado");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("totalpermisos")]
        public List<Permiso> GetPermisosTotales(int? idUsuario, int? idPerfil)
        {
            DA_Parametro daP = new(Configuration.GetConnectionString("DefaultConnection"));
            List<Permiso> permisos = new();
            using (DataTable dt = daP.GetPermisosTotal(idUsuario, idPerfil))
            {
                foreach (DataRow dr in dt.Rows)
                {
                    Permiso p = new()
                    {
                        IdPermiso = (int)dr["IdPermiso"],
                        TieneHijos = (bool)dr["TieneHijos"],
                        Descripcion = (string)dr["Nombre"],
                        TipoPermiso = (string)dr["Tipo"],
                        IdPadre = (dr["IdPadre"] != DBNull.Value) ? int.Parse(dr["IdPadre"].ToString()) : null,
                        HabilitadoPorPerfil = (dr["Habilitado"].ToString() == "1") ? true : false
                    };
                    if (dr["HabilitadoPorusuario"] != DBNull.Value)
                    {
                        p.HabilitadoPorUsuario = (dr["HabilitadoPorusuario"].ToString() == "1") ? true : false;
                    }

                    permisos.Add(p);
                }

            }
            return permisos;
        }

        [HttpGet("perfiles")]
        public List<PerfilUsuario> GetPerfiles(int? idPerfil, string? descripcion)
        {
            List<PerfilUsuario> perfiles = new();
            DA_Usuario da = new(Configuration.GetConnectionString("DefaultConnection"));
            foreach (DataRow row in da.ObtenerUsuarioPerfiles(idPerfil, descripcion).Rows)
                perfiles.Add(new PerfilUsuario((int)row["IdPerfil"], (string)row["Descripcion"]));

            return perfiles;
        }

        [HttpPost("permisosPerfil")]
        public IActionResult PostPermisoPorPerfil(List<Permiso> lstCambios)
        {
            DA_Usuario daU = new(Configuration.GetConnectionString("DefaultConnection"));

            try
            {
                foreach (var item in lstCambios)
                {
                    if (item.HabilitadoPorPerfil)
                    {
                        daU.PostPermisoPorPerfil((int)item.IdPerfilOUsuario, item.IdPermiso, null);
                    }
                    else
                    {
                        daU.DeletePermisoPorPerfil((int)item.IdPerfilOUsuario, item.IdPermiso, null);
                    }
                }
                return Ok("Los datos se han guardado");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("menu")]
        public List<MenuNew> GetMenu(int idUsuario)
        {
            List<MenuNew> menu = new();
            DA_Parametro daP = new(Configuration.GetConnectionString("DefaultConnection"));
            using (DataTable dt = daP.ObtenerPermisos(idUsuario, 'M', null, null, null))
            {
                foreach (DataRow dr in dt.Rows)
                {
                    MenuNew m = new()
                    {
                        Id = (int)dr["IdPermiso"],
                        Text = (string)dr["Nombre"]
                    };
                    if (dr["Href"] != DBNull.Value) m.Path = (string)dr["Href"];
                    if (dr["Icono"] != DBNull.Value) m.Icon = (string)dr["Icono"];
                    if (dr["IdPadre"] == DBNull.Value)
                    {
                        menu.Add(m);
                    }
                    else
                    {
                        m.IdPadre = (int)dr["IdPadre"];
                        var mn = SearchMenu(menu, (int)dr["IdPadre"]);
                        mn.Expanded = true;
                        mn.Items ??= new();
                        mn.Items.Add(m);
                    }
                }
            }

            return menu;
        }

        private MenuNew SearchMenu(List<MenuNew> menu, int idPadre)
        {
            MenuNew m = menu.Find(x => x.Id == idPadre);
            if (m == null)
            {
                foreach (var item in menu)
                {
                    if (item.Items != null)
                    {
                        m = SearchMenu(item.Items, idPadre);
                        if (m != null)
                        {
                            return m;
                        }
                    }
                }
            }
            return m;
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO login)
        {
            DA_Usuario daU = new(Configuration.GetConnectionString("DefaultConnection"));
            SesionDTO sesionDTO = new SesionDTO();
            using (DataTable dt = daU.ObtenerUsuarioLogin(login.Correo, login.Clave))
            {
                if (dt.Rows.Count > 0)
                {
                    sesionDTO.IdUsuario = int.Parse(dt.Rows[0]["IdUsuario"].ToString()!);
                    sesionDTO.Nombre = dt.Rows[0]["NombreUsuario"].ToString();
                    sesionDTO.Correo = dt.Rows[0]["Correo"].ToString();
                    sesionDTO.Rol = dt.Rows[0]["IdPerfil"].ToString();
                }
                else
                {
                    return Unauthorized();
                }
            }

            return StatusCode(StatusCodes.Status200OK, sesionDTO);
        }

        [HttpPost("Perfilusuario")]
        public IActionResult PostPerfil(PerfilUsuario perfil)
        {
            DA_Usuario daU = new(Configuration.GetConnectionString("DefaultConnection"));

            try
            {
                int id = daU.PostPerfil(perfil.DescripcionPerfil);
                return Ok(id);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("Perfilusuario")]
        public IActionResult PutPerfil(PerfilUsuario perfil)
        {
            DA_Usuario daU = new(Configuration.GetConnectionString("DefaultConnection"));

            try
            {
                daU.PutPerfil((int)perfil.IdPerfil!, perfil.DescripcionPerfil);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
