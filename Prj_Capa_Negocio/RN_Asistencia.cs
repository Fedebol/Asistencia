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

        public DataTable RN_Buscar_Asistencia_deEntrada(String idperso)
        {
            BD_Asistencia obj = new BD_Asistencia();
            return obj.BD_Buscar_Asistencia_deEntrada(idperso);
        }


        public void RN_Registrar_Entrada_Personal(string idAsis, string idPerso, string HoIngreso, double tarde, int totalHora, string justificado)
        {
            BD_Asistencia obj = new BD_Asistencia();
             obj.BD_Registrar_Entrada_Personal(idAsis, idPerso, HoIngreso, tarde, totalHora, justificado);
        }

        public void RN_Registrar_Salida_Personal(string idAsis, string idPerso, string HoSalida, double Totalhora)
        {
            BD_Asistencia obj = new BD_Asistencia();
            obj.BD_Registrar_Salida_Personal(idAsis,idPerso,HoSalida,Totalhora);
        }

        public bool RN_Checar_Personal_Asistencia(string xidPerso)
        {
            BD_Asistencia obj = new BD_Asistencia();
            return obj.BD_Checar_Personal_Asistencia(xidPerso);
        }

        public bool RN_Checar_Personal_Yaingreso(string xidPerso)
        {
            BD_Asistencia obj = new BD_Asistencia();
            return (obj.BD_Checar_Personal_Yaingreso(xidPerso));
        }

        public bool RN_Verificar_justificacion(string xidPerso)
        {
            BD_Asistencia obj = new BD_Asistencia();
            return obj.BD_Verificar_justificacion(xidPerso);
        }
        public DataTable BD_Ver_Todas_Asistencia_DelDia(DateTime xdia)
        {
            BD_Asistencia obj = new BD_Asistencia();
            return obj.BD_Ver_Todas_Asistencia_Deldia(xdia);
        }
        public void RN_Registrar_Falta_Personal(string idAsis, string idPerso, string justifi)
        {
            BD_Asistencia obj = new BD_Asistencia();
            obj.BD_Registrar_Falta_Personal(idAsis, idPerso, justifi);
        }
    }
}
