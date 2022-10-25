using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Prj_Capa_Datos;
using Prj_Capa_Entidad;
using Prj_Capa_Negocio;
using MicroSisPlani.Msm_Forms;
using System.IO;





namespace MicroSisPlani
{
    public partial class Frm_Marcar_Asis_Manual : Form
    {
        public Frm_Marcar_Asis_Manual()
        {
            InitializeComponent();
        }

        private void Frm_Marcar_Asis_Manual_Load(object sender, EventArgs e)
        {
            CargarHorarios();
            txt_dni_Buscar.Focus();

        }

        private void CargarHorarios()
        {
            RN_Horario obj = new RN_Horario();
            DataTable dt = new DataTable();

            dt = obj.RN_Leer_Horarios();
            if (dt.Rows.Count == 0) return;
            dtp_horaIngre.Value = Convert.ToDateTime(dt.Rows[0]["HoEntrada"]);
            Lbl_HoraEntrada.Text = dtp_horaIngre.Value.Hour.ToString() + ":" + dtp_horaIngre.Value.Minute.ToString();
            dtp_horaSalida.Value = Convert.ToDateTime(dt.Rows[0]["HoSalida"]);
            dtp_hora_tolercia.Value = Convert.ToDateTime(dt.Rows[0]["MiTolrncia"]);
            Dtp_Hora_Limite.Value = Convert.ToDateTime(dt.Rows[0]["HoLimite"]);
        }

        private void btn_buscar_Click(object sender, EventArgs e)
        {
            RN_Personal obj = new RN_Personal();
            RN_Asistencia objas = new RN_Asistencia();
            DataTable dataper = new DataTable();
            DataTable dataAsis = new DataTable();
            Frm_Filtro fil = new Frm_Filtro();
            Frm_Advertencia ver = new Frm_Advertencia();

            string NroIdPer;
            int cont = 1;
            string rutaFoto;

            int HoLimite = Dtp_Hora_Limite.Value.Hour;
            int MiLimite = Dtp_Hora_Limite.Value.Minute;

            int horaCaptu = DateTime.Now.Hour;
            int minutoCaptu = DateTime.Now.Minute;


            try
            {
                dataper = obj.RN_Buscar_Personal_xValor(txt_dni_Buscar.Text.Trim());
                if (dataper.Rows.Count <= 0)
                {
                    lbl_msm.BackColor = Color.MistyRose;
                    lbl_msm.ForeColor = Color.Red;
                    lbl_msm.Text = "No se encontro el Nro de DNI";
                    tocar_timbre();
                    lbl_Cont.Text = "10";
                    pnl_Msm.Visible = true;
                    tmr_Conta.Enabled = true;
                    return;

                }
                else
                {
                    var dt = dataper.Rows[0];
                    rutaFoto = Convert.ToString(dt["Foto"]);
                    lbl_nombresocio.Text = Convert.ToString(dt["Nombre_Completo"]);
                    lbl_Dni.Text = Convert.ToString(dt["DNIPR"]);
                    NroIdPer = Convert.ToString(dt["Id_Pernl"]);
                    Lbl_Idperso.Text = Convert.ToString(dt["Id_Pernl"]);

                    if (File.Exists(rutaFoto) == true)
                    {
                        picSocio.Load(rutaFoto.Trim());

                    }
                    else
                    {
                        picSocio.Load(Application.StartupPath + @"\user.png");
                    }

                    if (objas.RN_Checar_Personal_Asistencia(Lbl_Idperso.Text) == true)
                    {
                        lbl_msm.BackColor = Color.MistyRose;
                        lbl_msm.ForeColor = Color.Red;
                        lbl_msm.Text = "El sistema verifico que el personal ya marco su ingreso y egreso";
                        tocar_timbre();
                        lbl_Cont.Text = "10";
                        pnl_Msm.Visible = true;
                        tmr_Conta.Enabled = true;
                        return;
                    }

                    if (objas.RN_Checar_Personal_Yaingreso(Lbl_Idperso.Text.Trim()) == true)
                    {

                    }
                    else
                    {
                        if (horaCaptu > HoLimite)
                        {
                            lbl_msm.BackColor = Color.MistyRose;
                            lbl_msm.ForeColor = Color.Red;
                            lbl_msm.Text = "No puede ingresar en este Horario!!!";
                            tocar_timbre();
                            lbl_Cont.Text = "10";
                            pnl_Msm.Visible = true;
                            tmr_Conta.Enabled = true;
                            return;
                        }

                        calcular_Minutos_Tardanza();
                        lbl_IdAsis.Text = RN_Utilitario.RN_NroDoc(3);
                        objas.RN_Registrar_Entrada_Personal(lbl_IdAsis.Text, Lbl_Idperso.Text, lbl_hora.Text,Convert.ToDouble(lbl_totaltarde.Text), 8, lbl_justifi.Text);

                        if (BD_Asistencia.entrada == true)
                        {
                            RN_Utilitario.RN_Actualizar_Tipo_Doc(3);
                            lbl_msm.BackColor = Color.YellowGreen;
                            lbl_msm.ForeColor = Color.White;
                            lbl_msm.Text = "La entrade del personal fue registrada exitosamente";
                            tocar_timbre();
                            lbl_Cont.Text = "10";
                            pnl_Msm.Visible = true;
                            tmr_Conta.Enabled = true;
                            return;
                        }
                    }

                }
            }
            catch (Exception ex)
            {

            }
        }

        private void tocar_timbre()
        {
            string ruta;
            ruta = Application.StartupPath;
            System.Media.SoundPlayer son;
            son = new System.Media.SoundPlayer(ruta + @"\timbre1.wav");
            son.Play();

        }

        void calcular_Minutos_Tardanza()
        {
            RN_Asistencia obj = new RN_Asistencia();

            int horaCaptu = DateTime.Now.Hour;
            int minutoCaptu = DateTime.Now.Minute;

            int horaIn = dtp_horaIngre.Value.Hour;
            int MinuIn = dtp_horaIngre.Value.Minute;

            int Mintole = dtp_hora_tolercia.Value.Minute;

            int totalMinutofijo;
            int totaltardnza;

            if (obj.RN_Verificar_justificacion(Lbl_Idperso.Text) == true)
            {
                lbl_justifi.Text = "Tardanza Justificada";
            }
            else
            {
                lbl_justifi.Text = "Tardanza no justificada";
                totalMinutofijo = MinuIn + Mintole;

                if (horaCaptu == horaIn & minutoCaptu > totalMinutofijo)
                {
                    totaltardnza = minutoCaptu - totalMinutofijo;
                    lbl_totaltarde.Text = Convert.ToString(totaltardnza);
                }
                else if (horaCaptu > horaIn)
                {
                    int horaTarde;
                    horaTarde = horaCaptu - horaIn;
                    int HoraenMinuto;
                    HoraenMinuto = horaTarde * 60;
                    int totaltardexx = HoraenMinuto - totalMinutofijo;

                    totaltardnza = minutoCaptu + totaltardexx;
                    lbl_totaltarde.Text = Convert.ToString(totaltardnza);

                }
            }
        }
    }
}
