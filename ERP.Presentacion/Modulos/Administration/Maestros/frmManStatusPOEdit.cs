﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Data;
using System.Drawing;
using System.Resources;
using System.Reflection;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Windows.Forms;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using ERP.BusinessEntity;
using ERP.BusinessLogic;
using ERP.Presentacion.Utils;

namespace ERP.Presentacion.Modulos.Administration.Maestros
{
    public partial class frmManStatusPOEdit : DevExpress.XtraEditors.XtraForm
    {
        #region "Propiedades"

        public List<StatusPOBE> lstStatusPO;

        public enum Operacion
        {
            Nuevo = 1,
            Modificar = 2,
            Eliminar = 3,
            Consultar = 4
        }

        public Operacion pOperacion { get; set; }

        public StatusPOBE pStatusPOBE { get; set; }

        
        int _IdStatusPO = 0;

        public int IdStatusPO
        {
            get { return _IdStatusPO; }
            set { _IdStatusPO = value; }
        }

        #endregion

        #region "Eventos"

        public frmManStatusPOEdit()
        {
            InitializeComponent();
        }

        private void frmManStatusPOEdit_Load(object sender, EventArgs e)
        {
            
            if (pOperacion == Operacion.Nuevo)
            {
                this.Text = "StatusPO - New";
            }
            else if (pOperacion == Operacion.Modificar)
            {
                this.Text = "StatusPO - Update";
                StatusPOBE objE_StatusPO = null;
                objE_StatusPO = new StatusPOBL().Selecciona(IdStatusPO);
                if (objE_StatusPO != null)
                {
                    txtDescripcion.Text = objE_StatusPO.NameStatusPO.Trim();
                }

            }

            txtDescripcion.Select();
        }

        

        private void btnGrabar_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                if (!ValidarIngreso())
                {
                    StatusPOBL objBL_StatusPO = new StatusPOBL();
                    StatusPOBE objStatusPO = new StatusPOBE();

                    objStatusPO.IdStatusPO = IdStatusPO;
                    objStatusPO.NameStatusPO = txtDescripcion.Text;
                    objStatusPO.FlagState = true;
                    objStatusPO.Login = Parametros.strUsuarioLogin;
                    objStatusPO.Machine = WindowsIdentity.GetCurrent().Name.ToString();
                    objStatusPO.IdCompany = Parametros.intEmpresaId;

                    if (pOperacion == Operacion.Nuevo)
                        objBL_StatusPO.Inserta(objStatusPO);
                    else
                        objBL_StatusPO.Actualiza(objStatusPO);

                    this.Close();
                }
            }
            catch (Exception ex)
            {
                Cursor = Cursors.Default;
                XtraMessageBox.Show(ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cboUnidadMinera_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)(Keys.Enter))
            {
                e.Handled = true;
                txtDescripcion.Focus();
            }
        }

        private void txtDescripcion_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)(Keys.Enter))
            {
                e.Handled = true;
                btnGrabar.Focus();
            }
        }

        #endregion

        #region "Metodos"

        private bool ValidarIngreso()
        {
            bool flag = false;
            string strMensaje = "Could not register:\n";
            if (txtDescripcion.Text.Trim().ToString() == "")
            {
                strMensaje = strMensaje + "- Enter description.\n";
                flag = true;
            }

            if (pOperacion == Operacion.Nuevo)
            {
                var Buscar = lstStatusPO.Where(oB => oB.NameStatusPO.ToUpper() == txtDescripcion.Text.ToUpper()).ToList();
                if (Buscar.Count > 0)
                {
                    strMensaje = strMensaje + "- Description already exists.\n";
                    flag = true;
                }
            }

            if (flag)
            {
                XtraMessageBox.Show(strMensaje, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                Cursor = Cursors.Default;
            }
            return flag;
        }

        #endregion

        
        
        
    }
}
