using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prj_Capa_Datos;
using System.Data;

namespace Prj_Capa_Negocio
{
  public  class RN_Asistencia
    {

        public DataTable RN_Ver_Todas_Asistencia()
        {
            BD_Asistencia obj = new BD_Asistencia();
            return obj.BD_Ver_Todas_Asistencia();
        }

        public DataTable RN_Ver_Todas_Asistencia_Deldia(DateTime xfecha)
        {
            BD_Asistencia obj = new BD_Asistencia();
            return obj.BD_Ver_Todas_Asistencia_Deldia(xfecha);

        }

        public DataTable RN_Ver_Todas_Asistencia_DelMes(DateTime xfecha)
        {
            BD_Asistencia obj = new BD_Asistencia();
            return obj.BD_Ver_Todas_Asistencia_DelMes(xfecha);
        }

        public DataTable RN_Ver_Todas_Asistencia_ParaExplorador(String xvalor)
        {
            BD_Asistencia obj = new BD_Asistencia();
            return obj.BD_Ver_Todas_Asistencia_ParaExplorador(xvalor);
        }

    }
}
