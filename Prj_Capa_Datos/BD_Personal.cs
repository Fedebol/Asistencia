using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Prj_Capa_Entidad;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Prj_Capa_Datos
{
    public class BD_Personal : Cls_Conexion 
    {
        public static bool saved = false;
        public static bool edited = false;
        public static bool huella = false;

        public void BD_Registrar_Personal(EN_Persona per)
        {
            SqlConnection cn = new SqlConnection(Conectar());
            SqlCommand cmd = new SqlCommand("Sp_Insert_Personal", cn);
            try
            {
                cmd.CommandTimeout = 20;
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Id_Person", per.Idpersonal);
                cmd.Parameters.AddWithValue("@dni", per.Dni );
                cmd.Parameters.AddWithValue("@nombreCompleto", per.Nombres );
                cmd.Parameters.AddWithValue("@FechaNacimiento", per.anoNacimiento);
                cmd.Parameters.AddWithValue("@Sexo", per.Direccion );
                cmd.Parameters.AddWithValue("@Domicilio", per.Direccion );
                cmd.Parameters.AddWithValue("@Correo", per.Correo);
                cmd.Parameters.AddWithValue("@Celular", per.Celular);
                cmd.Parameters.AddWithValue("@Id_rol", per.IdRol);
                cmd.Parameters.AddWithValue("@Foto", per.xImagen);
                cmd.Parameters.AddWithValue("@Id_Distrito", per.IdDistrito);

                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();
                saved = true;
            }
            catch (Exception ex)
            {
                saved = false;
                if (cn.State == ConnectionState.Open)
                {
                    cn.Close();
                }
                MessageBox.Show("Algo malo paso" + ex.Message, "advertencia de Seguridad", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }


        public void BD_Editar_Personal(EN_Persona per)
        {
            SqlConnection cn = new SqlConnection(Conectar());
            SqlCommand cmd = new SqlCommand("SP_UPDATE_PERSONAL", cn);
            try
            {
                cmd.CommandTimeout = 20;
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Id_Person", per.Idpersonal);
                cmd.Parameters.AddWithValue("@dni", per.Dni);
                cmd.Parameters.AddWithValue("@nombreCompleto", per.Nombres);
                cmd.Parameters.AddWithValue("@FechaNacimiento", per.anoNacimiento);
                cmd.Parameters.AddWithValue("@Sexo", per.Direccion);
                cmd.Parameters.AddWithValue("@Domicilio", per.Direccion);
                cmd.Parameters.AddWithValue("@Correo", per.Correo);
                cmd.Parameters.AddWithValue("@Celular", per.Celular);
                cmd.Parameters.AddWithValue("@Id_rol", per.IdRol);
                cmd.Parameters.AddWithValue("@Foto", per.xImagen);
                cmd.Parameters.AddWithValue("@Id_Distrito", per.IdDistrito);

                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();
                edited = true;
            }
            catch (Exception ex)
            {
                edited = false;
                if (cn.State == ConnectionState.Open)
                {
                    cn.Close();
                }
                MessageBox.Show("Algo malo paso" + ex.Message, "advertencia de Seguridad", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        public void BD_Registrar_Huella_Personal(string idper, object finguer)
        {
            SqlConnection cn = new SqlConnection(Conectar());
            SqlCommand cmd = new SqlCommand("Sp_Actualizar_FinguerPrint", cn);
            try
            {
                cmd.CommandTimeout = 20;
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@IdPersona", idper);
                cmd.Parameters.AddWithValue("@finguerPrint", finguer);

                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();
                huella = true;
            }
            catch (Exception ex)
            {
                huella= false;
                if (cn.State == ConnectionState.Open)
                {
                    cn.Close();
                }
                MessageBox.Show("Erro al ejecutar SP" + ex.Message, "advertencia de Seguridad", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        public DataTable BD_Leer_todoPersonal()
        {
            SqlConnection cn = new SqlConnection();
            try
            {
                cn.ConnectionString= Conectar();
                SqlDataAdapter da = new SqlDataAdapter("SP_Listar_personal", cn);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                DataTable dt = new DataTable();

                da.Fill(dt);
                da = null;
                return dt;
            }
            catch (Exception ex)
            {
                if (cn.State == ConnectionState.Open)
                {
                    cn.Close();
                }
                MessageBox.Show("Erro al ejecutar SP" + ex.Message, "advertencia de Seguridad", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            return null;
        }

        public DataTable BD_Buscar_Personal_xValor(string valor)
        {
            SqlConnection cn = new SqlConnection(Conectar());
            try
            {
                cn.ConnectionString = Conectar();
                SqlDataAdapter da = new SqlDataAdapter("Sp_Cargar_PersonalxDni", cn);
                da.SelectCommand.CommandType= CommandType.StoredProcedure;
                da.SelectCommand.Parameters.AddWithValue("@Dni", valor);
                DataTable dt = new DataTable();

                da.Fill(dt);
                da = null;
                return dt;
            }
            catch (Exception ex)
            {
                if (cn.State == ConnectionState.Open)
                {
                    cn.Close();
                }
                MessageBox.Show("Erro al ejecutar SP" + ex.Message, "advertencia de Seguridad", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            return null;
        }

        public bool BD_Verificar_DNIdePersonal(string dni)
        {
            bool funtionRetornValue = false;
            Int32 xfil = 0;

            SqlConnection Cn = new SqlConnection();
            SqlCommand cmd = new SqlCommand();
            Cn.ConnectionString = Conectar();
            var _with1 = cmd;
            _with1.CommandText = "Sp_Validar_Dni";
            _with1.Connection = Cn;
            _with1.CommandTimeout = 20;
            _with1.CommandType = CommandType.StoredProcedure;
            _with1.Parameters.AddWithValue("@Dni", dni);
            try
            {
                Cn.Open();
                xfil = (Int32)cmd.ExecuteScalar();
                if (xfil > 0)
                {
                    funtionRetornValue = true;
                }
                else
                {
                    funtionRetornValue = false;
                }

                cmd.Parameters.Clear();
                cmd.Dispose();
                cmd = null;
                Cn.Close();
                Cn = null;
            }
            catch (Exception ex)
            {
                if (Cn.State == ConnectionState.Open)
                    Cn.Close();
                cmd.Dispose();
                cmd = null;
                MessageBox.Show("Algo malo paso: " + ex.Message, "Advertencia de seguridad", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return funtionRetornValue;
        }

    }
}
