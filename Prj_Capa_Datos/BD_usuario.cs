using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
using System.Data.SqlClient;
using Prj_Capa_Entidad;
using System.Windows.Forms;

namespace Prj_Capa_Datos
{
   public  class BD_Usuario : Cls_Conexion 
    {
        public bool BD_Verificar_Acceso(string Usuario, string Contraseña)
        {
            bool funtionRetornValue = false;
            Int32 xfil = 0;

            SqlConnection Cn = new SqlConnection();
            SqlCommand cmd = new SqlCommand();
            Cn.ConnectionString = Conectar();
            var _with1 = cmd;
            _with1.CommandText = "Sp_Login";
            _with1.Connection = Cn;
            _with1.CommandTimeout = 20;
            _with1.CommandType = CommandType.StoredProcedure;
            _with1.Parameters.AddWithValue("@Usuario", Usuario);
            _with1.Parameters.AddWithValue("@Contraseña", Contraseña);
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
