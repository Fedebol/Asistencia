using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using Prj_Capa_Entidad;

namespace Prj_Capa_Datos
{
   public class BD_Asistencia: Cls_Conexion 
    {
        public DataTable BD_Ver_Todas_Asistencia()
        {
            SqlConnection xcn = new SqlConnection();
            try
            {
                xcn.ConnectionString = Conectar();
                SqlDataAdapter da = new SqlDataAdapter("Sp_Cargar_Todas_Asistencias", xcn);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
               // da.SelectCommand.Parameters.AddWithValue("@fecha", xdia);
                DataTable dato = new DataTable();
                da.Fill(dato);
                return dato;
            }
            catch (Exception ex)
            {
                if (xcn.State == ConnectionState.Open)
                {
                    xcn.Close();
                    throw new Exception("Error" + ex.Message, ex);
                }
            }
            return null;
        }


        public DataTable BD_Ver_Todas_Asistencia_Deldia(DateTime xfecha)
        {
            SqlConnection xcn = new SqlConnection();
            try
            {
                xcn.ConnectionString = Conectar();
                SqlDataAdapter da = new SqlDataAdapter("Sp_Cargar_Asistencia_deldia", xcn);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.Parameters.AddWithValue("@fecha", xfecha);
                DataTable dato = new DataTable();
                da.Fill(dato);
                return dato;
            }
            catch (Exception ex)
            {
                if (xcn.State == ConnectionState.Open)
                {
                    xcn.Close();
                    throw new Exception("Error" + ex.Message, ex);
                }
            }
            return null;
        }

        public DataTable BD_Ver_Todas_Asistencia_DelMes(DateTime xfecha)
        {
            SqlConnection xcn = new SqlConnection();
            try
            {
                xcn.ConnectionString = Conectar();
                SqlDataAdapter da = new SqlDataAdapter("Sp_Cargar_Asistencia_xfecha", xcn);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.Parameters.AddWithValue("@fecha", xfecha);
                DataTable dato = new DataTable();
                da.Fill(dato);
                return dato;
            }
            catch (Exception ex)
            {
                if (xcn.State == ConnectionState.Open)
                {
                    xcn.Close();
                    throw new Exception("Error" + ex.Message, ex);
                }
            }
            return null;
        }

        public DataTable BD_Ver_Todas_Asistencia_ParaExplorador(String xvalor)
        {
            SqlConnection xcn = new SqlConnection();
            try
            {
                xcn.ConnectionString = Conectar();
                SqlDataAdapter da = new SqlDataAdapter("Sp_Buscar_Asistencia_paraExplorador", xcn);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.Parameters.AddWithValue("@Id_Asis", xvalor);
                DataTable dato = new DataTable();
                da.Fill(dato);
                return dato;
            }
            catch (Exception ex)
            {
                if (xcn.State == ConnectionState.Open)
                {
                    xcn.Close();
                    throw new Exception("Error" + ex.Message, ex);
                }
            }
            return null;
        }

        //registro

        public static bool entrada = false;

        public void BD_Registrar_Entrada_Personal(string idAsis, string idPerso, string HoIngreso, double tarde, int totalHora, string justificado)
        {
            SqlConnection cn = new SqlConnection();
            SqlCommand cmd = new SqlCommand("Sp_Registrar_Entrada", cn);

            try
            {
                cmd.CommandTimeout = 20;
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@IdAsis", idAsis);
                cmd.Parameters.AddWithValue("@Id_Persol", idPerso);
                cmd.Parameters.AddWithValue("@Hoingre", HoIngreso);
                cmd.Parameters.AddWithValue("@tardanza", tarde);
                cmd.Parameters.AddWithValue("@TotalHora", totalHora);
                cmd.Parameters.AddWithValue("@justificado", justificado);
               
                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();

                entrada = true;
            }
            catch (Exception ex)
            {
                entrada = false;
                if (cn.State == ConnectionState.Open)
                {
                    cn.Close();
                }
                throw new Exception("Erro al guardar" + ex.Message);    
            }
        }

        // salida
        public static bool Salida = false;

        public void BD_Registrar_Salida_Personal(string idAsis, string idPerso, string HoSalida, double Totalhora)
        {
            SqlConnection cn = new SqlConnection();
            SqlCommand cmd = new SqlCommand("Sp_Registrar_Salida", cn);

            try
            {
                cmd.CommandTimeout = 20;
                cmd.CommandType= CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@IdAsis", idAsis);
                cmd.Parameters.AddWithValue("@Id_Persol", idPerso);
                cmd.Parameters.AddWithValue("@HoSalida", HoSalida);
                cmd.Parameters.AddWithValue("@TotalHora", Totalhora);

                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();

                Salida = true;
            }
            catch (Exception ex)
            {
                Salida = false;
                if (cn.State == ConnectionState.Open)
                {
                    cn.Close();
                }
                throw new Exception("Erro al guardar" + ex.Message);

            }

        }

        //verificar

        public bool BD_Checar_Personal_Asistencia(string xidPerso)
        {
            bool funcionReturnValue = false;
            Int32 xfil = 0;
            SqlConnection cn = new SqlConnection();
            SqlCommand cmd = new SqlCommand();

            cn.ConnectionString = Conectar();
            var _with1 = cmd;
            _with1.CommandText = "Sp_Validar_RegistroAsistencia";
            _with1.Connection = cn;
            _with1.CommandTimeout = 20;
            _with1.CommandType = CommandType.StoredProcedure;
            _with1.Parameters.AddWithValue("@Id_Personal", xidPerso);
            try
            {
                cn.Open();
                xfil = (Int32)cmd.ExecuteScalar();
                if (xfil > 0)
                {
                    funcionReturnValue = true;
                }
                else
                {
                    funcionReturnValue = false;
                }
                cmd.Parameters.Clear();
                cmd.Dispose();
                cmd = null;
                cn.Close();
                cn = null;
            }
            catch (Exception ex)
            {
                if (cn.State == ConnectionState.Open)
                {
                    cn.Close();
                }
                cmd.Dispose();
                cmd = null;
                cn.Close();
                cn = null;
                MessageBox.Show("Algo salio mal: " + ex.Message, "advertencia de Seguridad", MessageBoxButtons.OK,MessageBoxIcon.Error);

            }
            return funcionReturnValue;
        }



        public bool BD_Checar_Personal_Yaingreso(string xidPerso)
        {
            bool funcionReturnValue = false;
            Int32 xfil = 0;
            SqlConnection cn = new SqlConnection();
            SqlCommand cmd = new SqlCommand();

            cn.ConnectionString = Conectar();
            var _with1 = cmd;
            _with1.CommandText = "Sp_Verificar_IngresoAsis";
            _with1.Connection = cn;
            _with1.CommandTimeout = 20;
            _with1.CommandType = CommandType.StoredProcedure;
            _with1.Parameters.AddWithValue("@Id_Personal", xidPerso);
            try
            {
                cn.Open();
                xfil = (Int32)cmd.ExecuteScalar();
                if (xfil > 0)
                {
                    funcionReturnValue = true;
                }
                else
                {
                    funcionReturnValue = false;
                }
                cmd.Parameters.Clear();
                cmd.Dispose();
                cmd = null;
                cn.Close();
                cn = null;
            }
            catch (Exception ex)
            {
                if (cn.State == ConnectionState.Open)
                {
                    cn.Close();
                }
                cmd.Dispose();
                cmd = null;
                cn.Close();
                cn = null;
                MessageBox.Show("Algo salio mal: " + ex.Message, "advertencia de Seguridad", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            return funcionReturnValue;
        }



        //Justificacion

        public bool BD_Verificar_justificacion(string xidPerso)
        {
            bool funcionReturnValue = false;
            Int32 xfil = 0;
            SqlConnection cn = new SqlConnection();
            SqlCommand cmd = new SqlCommand();

            cn.ConnectionString = Conectar();
            var _with1 = cmd;
            _with1.CommandText = "Sp_VerificarJustificacion_Aprobada";
            _with1.Connection = cn;
            _with1.CommandTimeout = 20;
            _with1.CommandType = CommandType.StoredProcedure;
            _with1.Parameters.AddWithValue("@Id_Personal", xidPerso);
            try
            {
                cn.Open();
                xfil = (Int32)cmd.ExecuteScalar();
                if (xfil > 0)
                {
                    funcionReturnValue = true;
                }
                else
                {
                    funcionReturnValue = false;
                }
                cmd.Parameters.Clear();
                cmd.Dispose();
                cmd = null;
                cn.Close();
                cn = null;
            }
            catch (Exception ex)
            {
                if (cn.State == ConnectionState.Open)
                {
                    cn.Close();
                }
                cmd.Dispose();
                cmd = null;
                cn.Close();
                cn = null;
                MessageBox.Show("Algo salio mal: " + ex.Message, "advertencia de Seguridad", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            return funcionReturnValue;
        }



    }
}
