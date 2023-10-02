using APP_CRUD.Models;
using APP_CRUD.Repositorios.Contrato;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace APP_CRUD.Repositorios.Implementación
{
    public class PersonalRepository : IGenericRepository<Personal>
    {
        private readonly string _cadenaSQL = "";
        public PersonalRepository(IConfiguration configuracion)
        {
            _cadenaSQL = configuracion.GetConnectionString("cadenaSQL");
        }

        public async Task<List<Personal>> Listar()
        {
            List<Personal> _lista = new List<Personal>();

            using (var conexion = new SqlConnection(_cadenaSQL))
            {
                conexion.Open();
                SqlCommand cmd = new SqlCommand("pa_ListaPersonal", conexion);
                cmd.CommandType = CommandType.StoredProcedure;

                using (var dr = await cmd.ExecuteReaderAsync())
                {
                    while (await dr.ReadAsync())
                    {
                        _lista.Add(new Personal
                        {
                            IdPersonal = Convert.ToInt32(dr["IDPERSONAL"]),
                            NombrePerso = dr["NOM_PER"].ToString(),
                            refDepartamento = new Departamento()
                            {
                                IdDepartamento = Convert.ToInt32(dr["IDDEPARTAMENTO"]),
                                NombreDepa = dr["NOM_DEP"].ToString()
                            },
                            Sueldo = Convert.ToDouble((dr["SUELDO"])),
                            FechaContrato = dr["FEC_CONT"].ToString(),
                        });
                    }

                }
            }
            return _lista;
        }



        public async Task<bool> Guardar(Personal modelo)
        {
            using (var conexion = new SqlConnection(_cadenaSQL))
            {
                conexion.Open();
                SqlCommand cmd = new SqlCommand("pa_nuevoPersonal", conexion);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("nombre", modelo.NombrePerso);
                cmd.Parameters.AddWithValue("iddep", modelo.refDepartamento.IdDepartamento);
                cmd.Parameters.AddWithValue("sueldo", modelo.Sueldo);
                cmd.Parameters.AddWithValue("fecha", modelo.FechaContrato);
                cmd.Parameters.AddWithValue("est", "A");
                int fila_afectadas = await cmd.ExecuteNonQueryAsync();
                if (fila_afectadas > 0)
                    return true;
                else
                    return false;
           }
        }

        public async Task<bool> Editar(Personal modelo)
        {
            using (var conexion = new SqlConnection(_cadenaSQL))
            {
                conexion.Open();
                SqlCommand cmd = new SqlCommand("pa_modificaPersonal", conexion);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("idper", modelo.IdPersonal);
                cmd.Parameters.AddWithValue("nombre", modelo.NombrePerso);
                cmd.Parameters.AddWithValue("iddep", modelo.refDepartamento.IdDepartamento);
                cmd.Parameters.AddWithValue("sueldo", modelo.Sueldo);
                cmd.Parameters.AddWithValue("fecha", modelo.FechaContrato);
                cmd.Parameters.AddWithValue("est", "A");
                int fila_afectadas = await cmd.ExecuteNonQueryAsync();
                if (fila_afectadas > 0)
                    return true;
                else
                    return false;
            }
        }

        public async Task<bool> Eliminar(int id)
        {
            using (var conexion = new SqlConnection(_cadenaSQL))
            {
                conexion.Open();
                SqlCommand cmd = new SqlCommand("pa_eliminaPersonal", conexion);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("idper", id);
                int fila_afectadas = await cmd.ExecuteNonQueryAsync();
                if (fila_afectadas > 0)
                    return true;
                else
                    return false;
            }
        }

    
    
    }
}
