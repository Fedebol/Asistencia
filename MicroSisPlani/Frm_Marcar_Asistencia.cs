using DPFP;
using MicroSisPlani.Msm_Forms;
using Prj_Capa_Datos;
using Prj_Capa_Negocio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Layout;



namespace MicroSisPlani
{
    public partial class Frm_Marcar_Asistencia : Form
    {
        public Frm_Marcar_Asistencia()
        {
            InitializeComponent();

        }


   

        private void pnl_titulo_MouseMove(object sender, MouseEventArgs e)
        {
            Complementos move = new Complementos();
            if (e.Button ==MouseButtons.Left)
            {
                move.Mover_formulario(this);
            }
           

        }

        private void btn_Salir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Frm_Marcar_Asistencia_Load(object sender, EventArgs e)
        {
            Verificar = new DPFP.Verification.Verification();
            Resultado = new DPFP.Verification.Verification.Result();
            CargarHorario();
        }

        private DPFP.Verification.Verification Verificar;
        private DPFP.Verification.Verification.Result Resultado;

        private void CargarHorario()
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

        private void xVerificationControl_OnComplete(object Control, FeatureSet FeatureSet, ref DPFP.Gui.EventHandlerStatus EventHandlerStatus)
        {
            DPFP.Template TemplateBD = new DPFP.Template();
            RN_Asistencia objas = new RN_Asistencia();
            RN_Personal objper = new RN_Personal();
            DataTable dtper = new DataTable();
            DataTable dtasis = new DataTable();

            string NroIdPersona = "";
            int xint = 1;
            byte[] fingerByte;
            string rutaFoto;
            bool TerminarBucle = false;
            int totalFila = 0;
            Frm_Filtro fil = new Frm_Filtro();

            int HoLimite= Dtp_Hora_Limite.Value.Hour;
            int MiLimite = Dtp_Hora_Limite.Value.Minute;
            int horaCaptu = DateTime.Now.Hour;
            int minutoCaptu = DateTime.Now.Minute;

            try
            {
                dtper = objper.RN_Leer_todoPersona();
                totalFila = dtper.Rows.Count;
                if (totalFila <= 0) return;

                var datoper = dtper.Rows[0];
                foreach(DataRow xitem in dtper.Rows)
                {
                    fingerByte = (byte[])xitem["FinguerPrint"];
                    NroIdPersona = Convert.ToString(xitem["Id_Pernl"]);
                    TemplateBD.DeSerialize(fingerByte);

                    Verificar.Verify(FeatureSet, TemplateBD, ref Resultado);

                    if (Resultado.Verified == true)
                    {
                        rutaFoto = Convert.ToString(xitem["Foto"]);
                        lbl_nombresocio.Text = Convert.ToString(xitem["Nombre_Completo"]);
                        Lbl_Idperso.Text = Convert.ToString(xitem["Id_pernl"]);
                        lbl_Dni.Text = Convert.ToString(xitem["DNIPR"]);

                        if (File.Exists(rutaFoto) == true) { picSocio.Load(rutaFoto.Trim()); } else { picSocio.Load(Application.StartupPath + @"\user.png"); }

                        if (objas.RN_Checar_Personal_Asistencia(Lbl_Idperso.Text.Trim()) == true)
                        {
                            lbl_msm.BackColor = Color.MistyRose;
                            lbl_msm.ForeColor = Color.Red;
                            lbl_msm.Text = "Ya se encuentra Registrado el dia de hoy";
                            tocar_timbre();
                            lbl_Cont.Text = "10";
                            xVerificationControl.Enabled = true;
                            pnl_Msm.Visible = true;
                            tmr_Conta.Enabled = true;
                            return;
                        }

                        if (objas.RN_Checar_Personal_Yaingreso(Lbl_Idperso.Text.Trim()) == true)
                        {

                            Frm_Sinox sino = new Frm_Sinox();
                            TerminarBucle = true;
                            fil.Show();
                            sino.Lbl_msm1.Text = "El usuario ya ha ingresado, ¿Desea marcar la Salida?";
                            sino.ShowDialog();
                            fil.Hide();

                            if (Convert.ToString(sino.Tag) == "SI")
                            {
                                dtasis = objas.RN_Buscar_Asistencia_deEntrada(Lbl_Idperso.Text);
                                if (dtasis.Rows.Count < 1) return;
                                lbl_IdAsis.Text = Convert.ToString(dtasis.Rows[0]["Id_asis"]);
                                objas.RN_Registrar_Salida_Personal(lbl_IdAsis.Text, Lbl_Idperso.Text, lbl_hora.Text, 8);
                                if (BD_Asistencia.Salida == true)
                                {
                                    lbl_msm.BackColor = Color.YellowGreen;
                                    lbl_msm.ForeColor = Color.White;
                                    lbl_msm.Text = "La Salida del personal fue registrado Exitosamente";
                                    tocar_timbreOK();
                                    xVerificationControl.Enabled = false;
                                    pnl_Msm.Visible = true;

                                    lbl_Cont.Text = "10";
                                    tmr_Conta.Enabled = true;

                                    TerminarBucle = true;
                                }
                            }

                        }
                        else
                        {
                            if (horaCaptu >= HoLimite)
                            {
                                lbl_msm.BackColor = Color.MistyRose;
                                lbl_msm.ForeColor = Color.Red;
                                lbl_msm.Text = "El ingreso ya no esta permitido";
                                tocar_timbre();
                                pnl_Msm.Visible = true;
                                tmr_Conta.Enabled= true;
                                lbl_Cont.Text = "10";
                                xVerificationControl.Enabled = false;
                                return;
                            }

                            calcular_Minutos_Tardanza();
                            lbl_IdAsis.Text = RN_Utilitario.RN_NroDoc(3);
                            objas.RN_Registrar_Entrada_Personal(lbl_IdAsis.Text, Lbl_Idperso.Text, lbl_hora.Text, Convert.ToDouble(lbl_totaltarde.Text), Convert.ToInt32(lbl_TotalHotrabajda.Text), lbl_justifi.Text);

                            if (BD_Asistencia.entrada == true)
                            {
                                RN_Utilitario.RN_Actualiza_tipo_Doc(3);
                                lbl_msm.BackColor = Color.YellowGreen;
                                lbl_msm.ForeColor = Color.White;
                                lbl_msm.Text = "La entrada del personal fue registrada exitosamente";
                                tocar_timbreOK();
                                pnl_Msm.Visible =true;
                                tmr_Conta.Enabled = true;
                                xVerificationControl.Enabled = false;
                                lbl_Cont.Text = "10";
                                TerminarBucle = true;


                            }
                        }
                    }
                }
            }
            catch(Exception ex)
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
        private void tocar_timbreOK()
        {
            string ruta;
            ruta = Application.StartupPath;
            System.Media.SoundPlayer son;
            son = new System.Media.SoundPlayer(ruta + @"\SD_ALERT_43.wav");
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

        private int sec = 10;

        private void tmr_Conta_Tick(object sender, EventArgs e)
        {
            sec -= 1;
            lbl_Cont.Text = sec.ToString();
            lbl_Cont.Refresh();

            if (sec == 0)
            {
                LimpiarFormulario();
                sec = 10;
                tmr_Conta.Stop();
                pnl_Msm.Visible=false;
                xVerificationControl.Enabled = true;
                lbl_Cont.Text = "10";
            }
        }

        private void LimpiarFormulario()
        {
            lbl_nombresocio.Text = "";
            lbl_totaltarde.Text = "0";
            lbl_TotalHotrabajda.Text = "0";
            lbl_Dni.Text = "";
            lbl_Cont.Text = "0";
            lbl_IdAsis.Text = "";
            Lbl_Idperso.Text = "";
            lbl_justifi.Text = "";
            lbl_msm.BackColor = Color.Transparent;
            lbl_msm.Text = "";
            picSocio.Image = null;
            xVerificationControl.Enabled = true;

        }


    }
}
