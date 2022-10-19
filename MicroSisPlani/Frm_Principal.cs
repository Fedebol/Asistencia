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
            
        }



        private void pnl_titu_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button ==MouseButtons.Left )
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
           
            elTabPage2 .Visible = true;
            elTab1.SelectedTabPageIndex = 1;
            Cargar_todo_Personal();
          

        }

        private void Frm_Principal_FormClosing(object sender, FormClosingEventArgs e)
        {
            
       

        }

        private void btn_nuevoAsis_Click(object sender, EventArgs e)
        {
           
        }

        private void bt_Explo_Asis_Click(object sender, EventArgs e)
        {
           
           

        }

        private void Btn_Cerrar_TabPers_Click(object sender, EventArgs e)
        {
            elTabPage2 .Visible = false ;
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

            if(lsv_person.SelectedIndices.Count == 0)
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
            if(dt.Rows.Count > 0)
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
            if(txt_Buscar.Text.Trim().Length > 2)
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
    }

}
