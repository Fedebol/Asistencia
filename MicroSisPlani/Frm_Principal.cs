using MicroSisPlani.Personal;
using Prj_Capa_Negocio;
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
using MicroSisPlani.Msm_Forms;
using MicroSisPlani.Informes;
using System.IO;


namespace MicroSisPlani
{
    public partial class Frm_Principal : Form
    {
        public Frm_Principal()
        {
            InitializeComponent();
        }

        private void Frm_Principal_Load(object sender, EventArgs e)
        {

            ConfiguraListview();
            ConfiguraListview_Asis();
            ConfiguraListview_Justifi();
            CargarHorarios();
            Verificar_Robot_de_Faltas();

        }

        private void Verificar_Robot_de_Faltas()
        {
            string tipo;
            tipo = RN_Utilitario.RN_Listar_TipoFalta(5);
            if (tipo.Trim() == "SI")
            {
                timerFalta.Start();
                rdb_ActivarRobot.Checked = true;
            }
            else if (tipo.Trim() == "NO")
            {
                timerFalta.Stop();
                rdb_Desact_Robot.Checked = true;
            }
        }

        public void Cargar_Datos_usuario()
        {
            try
            {
                Frm_Filtro xfil = new Frm_Filtro();
                xfil.Show();
                MessageBox.Show("Bienvenid@: " + Cls_Libreria.Apellidos, "al sistema", MessageBoxButtons.OK, MessageBoxIcon.Information);
                xfil.Hide();

                Lbl_NomUsu.Text = Cls_Libreria.Apellidos;
                lbl_rolNom.Text = Cls_Libreria.Rol;

                if (Cls_Libreria.Foto.Trim().Length == 0 | Cls_Libreria.Foto == null) return;

                if (File.Exists(Cls_Libreria.Foto) == true)
                {
                    pic_user.Load(Cls_Libreria.Foto);
                }
                else
                {
                    pic_user.Image = Properties.Resources.user;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Algo malo paso: " + ex.Message, "Advertencia de seguridad", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void pnl_titu_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Utilitarios u = new Utilitarios();
                u.Mover_formulario(this);
            }
        }

        private void btn_Salir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_mini_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Frm_Explorador_Personal pers = new Frm_Explorador_Personal();
            pers.MdiParent = this;
            pers.Show();

        }

        private void bt_personal_Click(object sender, EventArgs e)
        {

            elTabPage2.Visible = true;
            elTab1.SelectedTabPageIndex = 1;
            Cargar_todo_Personal();


        }

        private void Frm_Principal_FormClosing(object sender, FormClosingEventArgs e)
        {
            Frm_Filtro fil = new Frm_Filtro();
            Frm_Sino sino = new Frm_Sino();

            fil.Show();
            sino.Lbl_msm1.Text = "¿Estas Seguro de salir y abandonar el Sistema?";
            sino.ShowDialog();
            fil.Hide();

            if (Convert.ToString(sino.Tag) == "Si")
            {
                Application.ExitThread();
            }
            else
            {
                e.Cancel = true;
            }


        }

        private void btn_nuevoAsis_Click(object sender, EventArgs e)
        {
            Frm_Filtro fil = new Frm_Filtro();
            Frm_Marcar_Asis_Manual asis = new Frm_Marcar_Asis_Manual();

            fil.Show();
            asis.ShowDialog();
            fil.Hide();
        }

        private void bt_Explo_Asis_Click(object sender, EventArgs e)
        {
            elTabPage3.Visible = true;
            elTab1.SelectedTabPageIndex = 2;
            Cargar_Asistencias_delDia(dtp_fechadeldia.Value);


        }

        private void Btn_Cerrar_TabPers_Click(object sender, EventArgs e)
        {
            elTabPage2.Visible = false;
            elTab1.SelectedTabPageIndex = 0;
        }

        private void btn_cerrarEx_Asis_Click(object sender, EventArgs e)
        {
            elTabPage3.Visible = false;
            elTab1.SelectedTabPageIndex = 0;
        }

        private void bt_copiarNroDNI_Click(object sender, EventArgs e)
        {
            Frm_Advertencia adver = new Frm_Advertencia();
            Frm_Filtro fis = new Frm_Filtro();

            if (lsv_person.SelectedIndices.Count == 0)
            {
                fis.Show();
                adver.Lbl_Msm1.Text = "Seleccione el Item que desea Copiar";
                adver.ShowDialog();
                fis.Hide();
            }
            else
            {
                var lsv = lsv_person.SelectedItems[0];
                string xdni = lsv.SubItems[1].Text;

                Clipboard.Clear();
                Clipboard.SetText(xdni.Trim());
            }
        }


        #region "Persona"

        private void ConfiguraListview()
        {
            var lis = lsv_person;
            lis.Columns.Clear();
            lis.Items.Clear();
            lis.View = View.Details;
            lis.GridLines = false;
            lis.FullRowSelect = true;
            lis.Scrollable = true;
            lis.HideSelection = false;

            lis.Columns.Add("Id Persona", 0, HorizontalAlignment.Left);
            lis.Columns.Add("Dni", 95, HorizontalAlignment.Left);
            lis.Columns.Add("Nombre del Socio", 316, HorizontalAlignment.Left);
            lis.Columns.Add("Direccion", 0, HorizontalAlignment.Left);
            lis.Columns.Add("Correo", 0, HorizontalAlignment.Left);
            lis.Columns.Add("Sex", 0, HorizontalAlignment.Left);
            lis.Columns.Add("Fe Nacim", 110, HorizontalAlignment.Center);
            lis.Columns.Add("Nro Celular", 120, HorizontalAlignment.Left);
            lis.Columns.Add("Rol", 100, HorizontalAlignment.Left);
            lis.Columns.Add("Distrito", 0, HorizontalAlignment.Left);
            lis.Columns.Add("Estado", 100, HorizontalAlignment.Left);
        }

        private void Cargar_todo_Personal()
        {
            RN_Personal obj = new RN_Personal();
            DataTable dt = new DataTable();

            dt = obj.RN_Leer_todoPersona();
            if (dt.Rows.Count > 0)
            {
                LlenarListview(dt);
            }
        }

        private void Buscar_Personal_PorValor(string xvalor)
        {
            RN_Personal obj = new RN_Personal();
            DataTable dt = new DataTable();

            dt = obj.RN_Buscar_Personal_xValor(xvalor);
            if (dt.Rows.Count > 0)
            {
                LlenarListview(dt);
            }
            else
            {
                lsv_person.Items.Clear();

            }
        }






        private void LlenarListview(DataTable data)
        {
            lsv_person.Items.Clear();

            for (int i = 0; i < data.Rows.Count; i++)
            {
                DataRow dr = data.Rows[i];
                ListViewItem list = new ListViewItem(dr["Id_Pernl"].ToString());
                list.SubItems.Add(dr["DNIPR"].ToString());
                list.SubItems.Add(dr["Nombre_Completo"].ToString());
                list.SubItems.Add(dr["Domicilio"].ToString());
                list.SubItems.Add(dr["Correo"].ToString());
                list.SubItems.Add(dr["Sexo"].ToString());
                list.SubItems.Add(dr["Fec_Naci"].ToString());
                list.SubItems.Add(dr["Celular"].ToString());
                list.SubItems.Add(dr["NomRol"].ToString());

                list.SubItems.Add(dr["Distrito"].ToString());
                list.SubItems.Add(dr["Estado_Per"].ToString());
                lsv_person.Items.Add(list);
            }
            Lbl_total.Text = Convert.ToString(lsv_person.Items.Count);
        }

        private void txt_Buscar_OnValueChanged(object sender, EventArgs e)
        {
            if (txt_Buscar.Text.Trim().Length > 2)
            {
                Buscar_Personal_PorValor(txt_Buscar.Text.Trim());
            }
        }

        private void txt_Buscar_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Buscar_Personal_PorValor(txt_Buscar.Text.Trim());
            }
        }

        private void bt_mostrarTodoElPersonal_Click(object sender, EventArgs e)
        {
            Cargar_todo_Personal();
        }

        #endregion

        private void bt_nuevoPersonal_Click(object sender, EventArgs e)
        {
            Frm_Filtro fil = new Frm_Filtro();
            Frm_Registro_Personal per = new Frm_Registro_Personal();

            fil.Show();
            per.xedit = false;
            per.ShowDialog();

            fil.Hide();
            if (Convert.ToString(per.Tag) == "")
                return;
            {
                Cargar_todo_Personal();
            }
        }

        private void bt_editarPersonal_Click(object sender, EventArgs e)
        {
            Frm_Filtro fil = new Frm_Filtro();
            Frm_Registro_Personal per = new Frm_Registro_Personal();

            if (lsv_person.SelectedIndices.Count > 0)
            {
                var lsv = lsv_person.SelectedItems[0];
                string Idpersona = lsv.SubItems[0].Text;

                fil.Show();
                per.editPerso = true;
                per.Buscar_Personal_ParaEditar(Idpersona);
                per.ShowDialog();
                fil.Hide();

                if (Convert.ToString(per.Tag) == "A")
                {
                    Cargar_todo_Personal();
                }
            }

        }

        private void btn_SaveHorario_Click(object sender, EventArgs e)
        {
            try
            {
                RN_Horario hor = new RN_Horario();
                EN_Horario por = new EN_Horario();
                Frm_Filtro fil = new Frm_Filtro();
                Frm_Msm_Bueno ok = new Frm_Msm_Bueno();
                Frm_Advertencia adver = new Frm_Advertencia();


                por.Idhora = lbl_idHorario.Text;
                por.HoEntrada = dtp_horaIngre.Value;
                por.HoTole = dtp_hora_tolercia.Value;
                por.HoLimite = Dtp_Hora_Limite.Value;
                por.HoSalida = dtp_horaSalida.Value;

                hor.RN_Actualizar_Horario(por);

                if (BD_Horario.saved == true)
                {
                    fil.Show();
                    ok.Lbl_msm1.Text = "El horario fue actualizado";
                    ok.ShowDialog();
                    fil.Hide();

                    elTabPage4.Visible = false;
                    elTab1.SelectedTabPageIndex = 0;

                }
            }
            catch
            {

            }
        }

        private void bt_Config_Click(object sender, EventArgs e)
        {
            elTabPage4.Visible = true;
            elTab1.SelectedTabPageIndex = 3;
            CargarHorarios();

            // string tipo;
            // tipo = RN_Utilitario.RN_Listar_TipoFalta(5);
            //  if (tipo.Trim() == "SI")
            //  {
            //     rdb_ActivarRobot.Checked = true;
            //      rdb_Desact_Robot.Checked = false;
            //  }
            //  else if(tipo.Trim() == "NO")
            //  {
            //      rdb_Desact_Robot.Checked = true;
            //      rdb_ActivarRobot.Checked = false;
            //  }
        }

        private void CargarHorarios()
        {
            RN_Horario obj = new RN_Horario();
            DataTable dt = new DataTable();

            dt = obj.RN_Leer_Horarios();
            if (dt.Rows.Count == 0) return;

            lbl_idHorario.Text = Convert.ToString(dt.Rows[0]["Id_Hor"]);
            dtp_horaIngre.Value = Convert.ToDateTime(dt.Rows[0]["HoEntrada"]);
            dtp_horaSalida.Value = Convert.ToDateTime(dt.Rows[0]["HoSalida"]);
            dtp_hora_tolercia.Value = Convert.ToDateTime(dt.Rows[0]["MiTolrncia"]);
            Dtp_Hora_Limite.Value = Convert.ToDateTime(dt.Rows[0]["HoLimite"]);

        }


        #region "Asistencia"

        private void ConfiguraListview_Asis()
        {
            var lis = lsv_asis;
            lis.Columns.Clear();
            lis.Items.Clear();
            lis.View = View.Details;
            lis.GridLines = false;
            lis.FullRowSelect = true;
            lis.Scrollable = true;
            lis.HideSelection = false;


            lis.Columns.Add("Id Asis", 0, HorizontalAlignment.Left);
            lis.Columns.Add("Dni", 80, HorizontalAlignment.Left);
            lis.Columns.Add("Nombres del Personal", 316, HorizontalAlignment.Left);
            lis.Columns.Add("Fecha", 90, HorizontalAlignment.Left);
            lis.Columns.Add("Dia", 80, HorizontalAlignment.Left);
            lis.Columns.Add("Hs Ingreso", 90, HorizontalAlignment.Left);
            lis.Columns.Add("Tardnza", 70, HorizontalAlignment.Center);
            lis.Columns.Add("Hs Salida", 90, HorizontalAlignment.Left);
            lis.Columns.Add("Adelanto", 90, HorizontalAlignment.Left);
            lis.Columns.Add("Justificacion", 0, HorizontalAlignment.Left);
            lis.Columns.Add("Estado", 100, HorizontalAlignment.Left);
        }

        private void LlenarListview_Asis(DataTable data)
        {
            lsv_asis.Items.Clear();
            for (int i = 0; i < data.Rows.Count; i++)
            {
                DataRow dr = data.Rows[i];
                ListViewItem list = new ListViewItem(dr["Id_asis"].ToString());
                list.SubItems.Add(dr["DNIPR"].ToString());
                list.SubItems.Add(dr["Nombre_Completo"].ToString());
                list.SubItems.Add(dr["FechaAsis"].ToString());
                list.SubItems.Add(dr["Nombre_dia"].ToString());
                list.SubItems.Add(dr["HoIngreso"].ToString());
                list.SubItems.Add(dr["Tardanzas"].ToString());
                list.SubItems.Add(dr["HoSalida"].ToString());
                list.SubItems.Add(dr["Adelanto"].ToString());
                list.SubItems.Add(dr["Justificacion"].ToString());
                list.SubItems.Add(dr["EstadoAsis"].ToString());
                lsv_asis.Items.Add(list);
            }
            Lbl_total.Text = Convert.ToString(lsv_asis.Items.Count);
        }

        private void Cargar_todas_asistencias()
        {
            RN_Asistencia obj = new RN_Asistencia();
            DataTable dt = new DataTable();

            dt = obj.RN_Ver_Todas_Asistencia();
            if (dt.Rows.Count > 0)
            {
                LlenarListview_Asis(dt);
            }

        }

        private void Cargar_Asistencias_delDia(DateTime fechas)
        {
            RN_Asistencia obj = new RN_Asistencia();
            DataTable dt = new DataTable();

            dt = obj.RN_Ver_Todas_Asistencia_Deldia(fechas);
            if (dt.Rows.Count > 0)
            {
                LlenarListview_Asis(dt);
            }

        }

        private void Cargar_Asistencias_delMes(DateTime fechas)
        {
            RN_Asistencia obj = new RN_Asistencia();
            DataTable dt = new DataTable();

            dt = obj.RN_Ver_Todas_Asistencia_DelMes(fechas);
            if (dt.Rows.Count > 0)
            {
                LlenarListview_Asis(dt);
            }

        }


        private void Cargar_Asistencias_xvalor(String xvalor)
        {
            RN_Asistencia obj = new RN_Asistencia();
            DataTable dt = new DataTable();

            dt = obj.RN_Ver_Todas_Asistencia_ParaExplorador(xvalor);
            if (dt.Rows.Count > 0)
            {
                LlenarListview_Asis(dt);
            }

        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            Cargar_todas_asistencias();
        }



        private void txt_buscarAsis_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Cargar_Asistencias_xvalor(txt_buscarAsis.Text);
            }
        }

        private void lbl_lupaAsis_Click(object sender, EventArgs e)
        {
            Cargar_todas_asistencias();
        }


        private void verAsistenciasDelDíaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Frm_Filtro fil = new Frm_Filtro();
            Frm_Solo_Fecha solo = new Frm_Solo_Fecha();

            fil.Show();
            solo.ShowDialog();
            fil.Hide();

            if (Convert.ToString(solo.Tag) == "") return;

            DateTime xfecha = solo.dtp_fecha.Value;

            Cargar_Asistencias_delDia(xfecha);

        }
        #endregion

        private void bt_registrarHuellaDigital_Click(object sender, EventArgs e)
        {

            Frm_Filtro fil = new Frm_Filtro();
            Frm_Regis_Huella per = new Frm_Regis_Huella();

            if (lsv_person.SelectedIndices.Count == 0)
            {
                MessageBox.Show("Selecciona el trabajador para editar sus datos", "advertencia de Seguridad", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                var lsv = lsv_person.SelectedItems[0];
                string xidSocio = lsv.SubItems[0].Text;

                fil.Show();
                per.Buscar_Personal_ParaEditar(xidSocio);
                per.ShowDialog();
                fil.Hide();

                if (Convert.ToString(per.Tag) == "") return;
                {
                    Cargar_todo_Personal();
                }
            }
        }

        private void btn_Asis_With_Huella_Click(object sender, EventArgs e)
        {
            Frm_Filtro fil = new Frm_Filtro();
            Frm_Marcar_Asistencia asis = new Frm_Marcar_Asistencia();
            fil.Show();
            asis.ShowDialog();
            fil.Hide();
        }

        private void btn_Savedrobot_Click(object sender, EventArgs e)
        {
            RN_Utilitario uti = new RN_Utilitario();

            if (rdb_ActivarRobot.Checked == true)
            {
                uti.BD_Actualizar_RobotFalta(5, "SI");
                if (BD_Utilitario.falta == true)
                {
                    Frm_Msm_Bueno ok = new Frm_Msm_Bueno();
                    ok.Lbl_msm1.Text = "El Robot fue actualizado";
                    ok.ShowDialog();

                    elTab1.SelectedTabPageIndex = 0;
                    elTabPage4.Visible = false;
                }
            }
            else if (rdb_Desact_Robot.Checked == true)
            {
                uti.BD_Actualizar_RobotFalta(5, "NO");
                if (BD_Utilitario.falta == true)
                {
                    Frm_Msm_Bueno ok = new Frm_Msm_Bueno();
                    ok.Lbl_msm1.Text = "El Robot fue Actualizado";
                    ok.ShowDialog();

                    elTab1.SelectedTabPageIndex = 0;
                    elTabPage4.Visible = false;
                }
            }
        }

        private void timerFalta_Tick(object sender, EventArgs e)
        {
            RN_Asistencia obj = new RN_Asistencia();
            Frm_Filtro fil = new Frm_Filtro();
            Frm_Advertencia adver = new Frm_Advertencia();
            Frm_Msm_Bueno ok = new Frm_Msm_Bueno();
            DataTable dt = new DataTable();
            RN_Personal objper = new RN_Personal();

            int HoLimite = Dtp_Hora_Limite.Value.Hour;
            int MiLimite = Dtp_Hora_Limite.Value.Minute;

            int horaCaptu = DateTime.Now.Hour;
            int minutocaptu = DateTime.Now.Minute;

            string Dniper = "";
            int Cant = 0;
            int TotalItem = 0;
            string xidpersona = "";
            string IdAsistencia = "";
            string xjustificacion = "";

            if (horaCaptu >= HoLimite)
            {
                if (minutocaptu > MiLimite)
                {
                    dt = objper.RN_Leer_todoPersona();
                    if (dt.Rows.Count <= 0) return;
                    TotalItem = dt.Rows.Count;

                    foreach (DataRow Registro in dt.Rows)
                    {
                        Dniper = Convert.ToString(Registro["DNIPR"]);
                        xidpersona = Convert.ToString(Registro["Id_pernl"]);
                        if (obj.RN_Checar_Personal_Asistencia(xidpersona.Trim()) == false)
                        {
                            if (obj.RN_Checar_Personal_Yaingreso(xidpersona.Trim()) == false)
                            {
                                RN_Asistencia objA = new RN_Asistencia();
                                EN_Asistencia asi = new EN_Asistencia();
                                IdAsistencia = RN_Utilitario.RN_NroDoc(3);

                                if (objA.RN_Verificar_justificacion(xidpersona) == true)
                                {
                                    xjustificacion = "Falta tiene Justificacion";
                                }
                                else
                                {
                                    xjustificacion = "No tiene justificacion";
                                }

                                obj.RN_Registrar_Falta_Personal(IdAsistencia, xidpersona, xjustificacion);
                                if (BD_Asistencia.falta == true)
                                {
                                    RN_Utilitario.RN_Actualizar_Tipo_Doc(3);
                                    Cant += 1;
                                }
                            }
                        }
                    }

                    if (Cant > 1)
                    {
                        timerFalta.Stop();
                        fil.Show();
                        ok.Lbl_msm1.Text = "Un total de: " + Cant.ToString() + "/" + TotalItem + "Faltas se han registrado exitosamente";
                        ok.ShowDialog();
                        fil.Hide();
                    }
                    else
                    {
                        timerFalta.Stop();
                        fil.Show();
                        ok.Lbl_msm1.Text = "El dia de hoy no falto nadie al trabajo, las " + TotalItem + "personas marcaron Asistencia";
                        ok.ShowDialog();
                        fil.Hide();
                    }
                }
            }

        }

        #region "Justificacion"


        private void ConfiguraListview_Justifi()
        {
            var lis = lsv_justifi;
            lis.Columns.Clear();
            lis.Items.Clear();
            lis.View = View.Details;
            lis.GridLines = false;
            lis.FullRowSelect = true;
            lis.Scrollable = true;
            lis.HideSelection = false;

            lis.Columns.Add("IdJusti", 0, HorizontalAlignment.Left);
            lis.Columns.Add("IdPerso", 0, HorizontalAlignment.Left);
            lis.Columns.Add("Nombres del Personal", 316, HorizontalAlignment.Left);
            lis.Columns.Add("Motivo", 110, HorizontalAlignment.Left);
            lis.Columns.Add("Fecha", 120, HorizontalAlignment.Left);
            lis.Columns.Add("Estado", 120, HorizontalAlignment.Left);
            lis.Columns.Add("Detalle Justifi", 0, HorizontalAlignment.Left);

        }

        private void LlenarListview_Justi(DataTable data)
        {
            lsv_justifi.Items.Clear();
            for (int i = 0; i < data.Rows.Count; i++)
            {
                DataRow dr = data.Rows[i];
                ListViewItem list = new ListViewItem(dr["Id_Justi"].ToString());
                list.SubItems.Add(dr["Id_Pernl"].ToString());
                list.SubItems.Add(dr["Nombre_Completo"].ToString());
                list.SubItems.Add(dr["PrincipalMotivo"].ToString());
                list.SubItems.Add(dr["FechaEmi"].ToString());
                list.SubItems.Add(dr["FechaJusti"].ToString());
                list.SubItems.Add(dr["EstadoJus"].ToString());
                list.SubItems.Add(dr["Detalle_Justi"].ToString());

                lsv_justifi.Items.Add(list);
            }
            lbl_totaljusti.Text = Convert.ToString(lsv_justifi.Items.Count);
        }

        private void Cargar_todas_Justificaciones()
        {
            RN_Justificacion obj = new RN_Justificacion();
            DataTable dt = new DataTable();

            dt = obj.RN_Cargar_todos_Justificacion();
            if (dt.Rows.Count > 0)
            {
                LlenarListview_Justi(dt);
            }
            else
            {
                lsv_justifi.Items.Clear();
            }
        }

        private void Buscar_Justificacion_porValor(string xvalor)
        {
            RN_Justificacion obj = new RN_Justificacion();
            DataTable dt = new DataTable(xvalor);

            dt = obj.RN_Cargar_todos_Justificacion();
            if (dt.Rows.Count > 0)
            {
                LlenarListview_Justi(dt);
            }
            else { lsv_justifi.Items.Clear(); }
        }

        # endregion
        private void bt_mostrarJusti_Click(object sender, EventArgs e)
        {
            Cargar_todas_Justificaciones();
        }

        private void bt_editJusti_Click(object sender, EventArgs e)
        {
            Frm_Filtro fil = new Frm_Filtro();
            Frm_Reg_Justificacion per = new Frm_Reg_Justificacion();

            if (lsv_justifi.SelectedIndices.Count == 0)
            {
                fil.Show();
                MessageBox.Show("Seleccione un Item por favor", "advertencia de Seguridad", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                fil.Hide();
            }
            else
            {
                var lsv = lsv_justifi.SelectedItems[0];
                string xidsocio = lsv.SubItems[1].Text;
                string xidJusti = lsv.SubItems[0].Text;
                string xnombre = lsv.SubItems[2].Text;

                fil.Show();
                per.xedit = false;
                per.txt_IdPersona.Text = xidsocio;
                per.txt_nompersona.Text = xnombre;
                per.txt_idjusti.Text = xidJusti;
                per.BuscarJustificacion(xidJusti);
                per.ShowDialog();
                fil.Hide();

                if (Convert.ToString(per.Tag) == "") return;
                {
                    Cargar_todas_Justificaciones();
                    elTab1.SelectedTabPageIndex = 4;
                    elTabPage5.Visible = true;
                }

            }
        }

        private void bt_solicitarJustificacion_Click(object sender, EventArgs e)
        {
            Frm_Filtro fil = new Frm_Filtro();
            Frm_Reg_Justificacion per = new Frm_Reg_Justificacion();

            if (lsv_person.SelectedIndices.Count == 0)
            {
                fil.Show();
                MessageBox.Show("Seleccione el personal por favor", "advertencia de Seguridad", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                fil.Hide();
            }
            else
            {
                var lsv = lsv_person.SelectedItems[0];
                string xidsocio = lsv.SubItems[1].Text;
                string xnombre = lsv.SubItems[2].Text;

                fil.Show();
                per.xedit = false;
                per.txt_IdPersona.Text = xidsocio;
                per.txt_nompersona.Text = xnombre;
                per.txt_idjusti.Text = RN_Utilitario.RN_NroDoc(4);
                per.ShowDialog();
                fil.Hide();

                if (Convert.ToString(per.Tag) == "") return;
                {
                    Cargar_todas_Justificaciones();
                    elTab1.SelectedTabPageIndex = 4;
                    elTabPage5.Visible = true;
                }

            }
        }

        private void bt_aprobarJustificacion_Click(object sender, EventArgs e)
        {
            Frm_Advertencia adver = new Frm_Advertencia();
            Frm_Sino sino = new Frm_Sino();
            Frm_Msm_Bueno ok = new Frm_Msm_Bueno();
            Frm_Filtro fil = new Frm_Filtro();
            RN_Justificacion obj = new RN_Justificacion();

            if (lsv_justifi.SelectedIndices.Count == 0)
            {
                fil.Show();
                adver.Lbl_Msm1.Text = "Seleccione el Item que desea aoobar por favor";
                adver.ShowDialog();
                fil.Hide();
                return;
            }
            else
            {
                var lsv = lsv_justifi.SelectedItems[0];
                string xidper = lsv.SubItems[1].Text;
                string xidJusti = lsv.SubItems[0].Text;
                string xstadojus = lsv.SubItems[6].Text;

                if (xstadojus.Trim() == "Aprobado") { fil.Show(); adver.Lbl_Msm1.Text = " La Justificacion seleccionada, ya fue aprobada"; adver.ShowDialog(); fil.Hide(); return; }

                sino.Lbl_msm1.Text = "Esta seguro de aprobar la justificacion??";
                fil.Show();
                sino.ShowDialog();
                fil.Hide();

                if (Convert.ToString(sino.Tag) == "SI")
                {
                    obj.RN_Aprobar_Justificacion(xidJusti, xidper);
                    if (BD_Justificacion.tryed == true)
                    {
                        fil.Show();
                        ok.Lbl_msm1.Text = "Justificacion aprobada";
                        ok.ShowDialog();
                        fil.Hide();
                        Buscar_Justificacion_porValor(xidJusti);
                    }
                }

            }
        }

        private void bt_desaprobarJustificacion_Click(object sender, EventArgs e)
        {
            Frm_Advertencia adver = new Frm_Advertencia();
            Frm_Sino sino = new Frm_Sino();
            Frm_Msm_Bueno ok = new Frm_Msm_Bueno();
            Frm_Filtro fil = new Frm_Filtro();
            RN_Justificacion obj = new RN_Justificacion();

            if (lsv_justifi.SelectedIndices.Count == 0)
            {
                fil.Show();
                adver.Lbl_Msm1.Text = "Seleccione el Item que desea desaprobar por favor";
                adver.ShowDialog();
                fil.Hide();
                return;
            }
            else
            {
                var lsv = lsv_justifi.SelectedItems[0];
                string xidper = lsv.SubItems[1].Text;
                string xidJusti = lsv.SubItems[0].Text;
                string xstadojus = lsv.SubItems[6].Text;

                if (xstadojus.Trim() == "Falta Aprobar") { fil.Show(); adver.Lbl_Msm1.Text = " La Justificacion seleccionada, aun no fue aprobada"; adver.ShowDialog(); fil.Hide(); return; }

                sino.Lbl_msm1.Text = "Esta seguro de desaprobar la justificacion??";
                fil.Show();
                sino.ShowDialog();
                fil.Hide();

                if (Convert.ToString(sino.Tag) == "SI")
                {
                    obj.RN_Aprobar_Justificacion(xidJusti, xidper);
                    if (BD_Justificacion.tryed == true)
                    {
                        fil.Show();
                        ok.Lbl_msm1.Text = "Justificacion pendiente de aprobacion";
                        ok.ShowDialog();
                        fil.Hide();
                        Buscar_Justificacion_porValor(xidJusti);
                    }
                }

            }
        }

        private void bt_ElimiJusti_Click(object sender, EventArgs e)
        {
            Frm_Advertencia adver = new Frm_Advertencia();
            Frm_Sino sino = new Frm_Sino();
            Frm_Msm_Bueno ok = new Frm_Msm_Bueno();
            Frm_Filtro fil = new Frm_Filtro();
            RN_Justificacion obj = new RN_Justificacion();

            if (lsv_justifi.SelectedIndices.Count == 0)
            {
                fil.Show();
                adver.Lbl_Msm1.Text = "Seleccione el Item que desea Eliminar por favor";
                adver.ShowDialog();
                fil.Hide();
                return;
            }
            else
            {
                var lsv = lsv_justifi.SelectedItems[0];
                string xidper = lsv.SubItems[1].Text;


                sino.Lbl_msm1.Text = "Esta seguro de desaprobar la justificacion??";
                fil.Show();
                sino.ShowDialog();
                fil.Hide();

                if (Convert.ToString(sino.Tag) == "SI")
                {
                    obj.RN_Eliminar_Justificacion(xidper);
                    if (BD_Justificacion.tryed == true)
                    {
                        fil.Show();
                        ok.Lbl_msm1.Text = "Justificacion pendiente de aprobacion";
                        ok.ShowDialog();
                        fil.Hide();
                        Buscar_Justificacion_porValor(xidper);
                    }
                }

            }
        }

        private void bt_CopiarNroJusti_Click(object sender, EventArgs e)
        {
            Frm_Advertencia adver = new Frm_Advertencia();
            Frm_Filtro fil = new Frm_Filtro();

            if (lsv_justifi.SelectedIndices.Count == 0)
            {
                fil.Show();
                adver.Lbl_Msm1.Text = "Seleccione el Item que desea Copiar por favor";
                adver.ShowDialog();
                fil.Hide();
                return;
            }
            else
            {
                var lsv = lsv_justifi.SelectedItems[0];
                string xidper = lsv.SubItems[0].Text;

                Clipboard.Clear();
                Clipboard.SetText(xidper.Trim());
            }
        }

        private void txt_buscarjusti_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (txt_buscarjusti.Text.Trim().Length > 2)
                {
                Buscar_Justificacion_porValor(txt_buscarjusti.Text);
                }
                else
                {
                    Cargar_todas_Justificaciones();
                }
            }
        }

        private void bt_cerrarjusti_Click(object sender, EventArgs e)
        {
            elTabPage5.Visible = false;
            elTabPage1.Visible = true;
            elTab1.SelectedTabPageIndex = 0;
        }

        private void lsv_justifi_MouseClick(object sender, MouseEventArgs e)
        {
            var lsv = lsv_justifi.SelectedItems[0];
            string xnombre = lsv.SubItems[7].Text;

            lbl_Detalle.Text = xnombre.Trim();
        }

        private void bt_exploJusti_Click(object sender, EventArgs e)
        {
            elTab1.SelectedTabPageIndex = 4;
            elTabPage5.Visible = true;
            Cargar_todas_Justificaciones();
        }
    }

}
