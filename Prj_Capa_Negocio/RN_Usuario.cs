using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Prj_Capa_Datos;
using Prj_Capa_Entidad;


namespace Prj_Capa_Negocio
{
    public  class RN_Usuario
    {
        public bool RN_Verificar_Acceso(string Usuario, string Contraseña)
        {
            BD_Usuario obj = new BD_Usuario();
            return obj.BD_Verificar_Acceso(Usuario, Contraseña);

        }

        public DataTable RN_Leer_Datos_Usuario(string Usuario)
        {
            BD_Usuario obj = new BD_Usuario();
            return obj.BD_Leer_Datos_Usuario(Usuario);
        }


    }
}
