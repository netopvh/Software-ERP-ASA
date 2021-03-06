﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Linq;
using System.Security.Principal;
using Excel = Microsoft.Office.Interop.Excel;
using System.Reflection;
using System.IO;
using DevExpress.XtraEditors;
using DevExpress.XtraBars;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using ERP.BusinessEntity;
using ERP.BusinessLogic;

namespace ERP.Presentacion.Modulos.Invoices.Maestros
{
    public partial class frmManCurrency : DevExpress.XtraEditors.XtraForm
    {
        #region "Propiedades"

        private List<CurrencyBE> mLista = new List<CurrencyBE>();

        #endregion

        #region "Eventos"

        public frmManCurrency()
        {
            InitializeComponent();
        }

        private void frmManCurrency_Load(object sender, EventArgs e)
        {
            tlbMenu.Ensamblado = this.Tag.ToString();
            Cargar();
        }

        private void tlbMenu_NewClick()
        {
            try
            {
                frmManCurrencyEdit objManCurrency = new frmManCurrencyEdit();
                objManCurrency.lstCurrency = mLista;
                objManCurrency.pOperacion = frmManCurrencyEdit.Operacion.Nuevo;
                objManCurrency.IdCurrency = 0;
                objManCurrency.StartPosition = FormStartPosition.CenterParent;
                objManCurrency.ShowDialog();
                Cargar();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void tlbMenu_EditClick()
        {
            InicializarModificar();
        }

        private void tlbMenu_DeleteClick()
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                if (XtraMessageBox.Show("Be sure to delete the record?", this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if (!ValidarIngreso())
                    {
                        CurrencyBE objE_Currency = new CurrencyBE();
                        objE_Currency.IdCurrency = int.Parse(gvCurrency.GetFocusedRowCellValue("IdCurrency").ToString());
                        objE_Currency.Login = Parametros.strUsuarioLogin;
                        objE_Currency.Machine = WindowsIdentity.GetCurrent().Name.ToString();
                        objE_Currency.IdCompany = Parametros.intEmpresaId;

                        CurrencyBL objBL_Currency = new CurrencyBL();
                        objBL_Currency.Elimina(objE_Currency);
                        XtraMessageBox.Show("The record was successfully deleted.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Cargar();
                    }
                }
                Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor = Cursors.Default;
                XtraMessageBox.Show(ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void tlbMenu_RefreshClick()
        {
            Cargar();
        }

        private void tlbMenu_PrintClick()
        {
            //try
            //{
            //    Cursor = Cursors.WaitCursor;

            //    List<ReporteCurrencyElementoBE> lstReporte = null;
            //    lstReporte = new ReporteCurrencyElementoBL().Listado();

            //    if (lstReporte != null)
            //    {
            //        if (lstReporte.Count > 0)
            //        {
            //            RptVistaReportes objRptCurrencyElemento = new RptVistaReportes();
            //            objRptCurrencyElemento.VerRptCurrencyElemento(lstReporte);
            //            objRptCurrencyElemento.ShowDialog();
            //        }
            //        else
            //            XtraMessageBox.Show("No hay información para el periodo seleccionado", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    }
            //    Cursor = Cursors.Default;
            //}
            //catch (Exception ex)
            //{
            //    Cursor = Cursors.Default;
            //    XtraMessageBox.Show(ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}
        }

        private void tlbMenu_ExportClick()
        {
            ExportarExcel("");
        }

        private void tlbMenu_ExitClick()
        {
            this.Close();
        }

        private void gvCurrency_DoubleClick(object sender, EventArgs e)
        {
            GridView view = (GridView)sender;
            Point pt = view.GridControl.PointToClient(Control.MousePosition);
            FilaDoubleClick(view, pt);
        }

        private void txtDescripcion_KeyUp(object sender, KeyEventArgs e)
        {
            CargarBusqueda();
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            CargarBusqueda();
        }

        private void gvCurrency_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            GridView view = (GridView)sender;
            if (e.RowHandle == view.FocusedRowHandle)
            {
                e.Appearance.Assign(view.GetViewInfo().PaintAppearance.GetAppearance("FocusedRow"));
            }
        }

        #endregion

        #region "Metodos"

        private void Cargar()
        {
            mLista = new CurrencyBL().ListaTodosActivo(Parametros.intEmpresaId);
            gcCurrency.DataSource = mLista;
        }

        private void CargarBusqueda()
        {
            gcCurrency.DataSource = mLista.Where(obj =>
                                                   obj.NameCurrency.ToUpper().Contains(txtDescripcion.Text.ToUpper())).ToList();
        }

        public void InicializarModificar()
        {
            if (gvCurrency.RowCount > 0)
            {
                CurrencyBE objCurrency = new CurrencyBE();
               
                objCurrency.IdCurrency = int.Parse(gvCurrency.GetFocusedRowCellValue("IdCurrency").ToString());

                frmManCurrencyEdit objManCurrencyEdit = new frmManCurrencyEdit();
                objManCurrencyEdit.pOperacion = frmManCurrencyEdit.Operacion.Modificar;
                objManCurrencyEdit.IdCurrency = objCurrency.IdCurrency;
                objManCurrencyEdit.pCurrencyBE = objCurrency;
                objManCurrencyEdit.StartPosition = FormStartPosition.CenterParent;
                objManCurrencyEdit.ShowDialog();

                Cargar();
            }
            else
            {
                MessageBox.Show("No se pudo editar");
            }
        }

        private void FilaDoubleClick(GridView view, Point pt)
        {
            GridHitInfo info = view.CalcHitInfo(pt);
            if (info.InRow || info.InRowCell)
            {
                InicializarModificar();
            }
        }

        private bool ValidarIngreso()
        {
            bool flag = false;

            if (gvCurrency.GetFocusedRowCellValue("IdCurrency").ToString() == "")
            {
                XtraMessageBox.Show("Select a work Currency", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                flag = true;
            }

            Cursor = Cursors.Default;
            return flag;
        }

        void ExportarExcel(string filename)
        {

            Excel._Application xlApp;
            Excel._Workbook xlLibro;
            Excel._Worksheet xlHoja;
            Excel.Sheets xlHojas;
            xlApp = new Excel.Application();
            filename = Path.Combine(Directory.GetCurrentDirectory(), "Excel\\Currency.xlsx");
            xlLibro = xlApp.Workbooks.Open(filename, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);
            xlHojas = xlLibro.Sheets;
            xlHoja = (Excel._Worksheet)xlHojas[1];

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                int Row = 6;
                int Secuencia = 1;

                List<CurrencyBE> lstCurrency = null;
                lstCurrency = new CurrencyBL().ListaTodosActivo(Parametros.intEmpresaId);
                if (lstCurrency.Count > 0)
                {
                    xlHoja.Shapes.AddPicture(Path.Combine(Directory.GetCurrentDirectory(), "Logo.jpg"), Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoCTrue, 1, 1, 80, 60);

                    foreach (var item in lstCurrency)
                    {
                        xlHoja.Cells[Row, 1] = item.IdCurrency;
                        xlHoja.Cells[Row, 2] = item.Abbreviate;
                        xlHoja.Cells[Row, 3] = item.NameCurrency;

                        Row = Row + 1;
                        Secuencia = Secuencia + 1;


                    }

                }

                xlLibro.SaveAs("C:\\Excel\\Currency.xlsx", Excel.XlFileFormat.xlWorkbookDefault, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Excel.XlSaveAsAccessMode.xlExclusive, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);

                xlLibro.Close(true, Missing.Value, Missing.Value);
                xlApp.Quit();

                Cursor.Current = Cursors.Default;
                XtraMessageBox.Show("It was imported correctly \n The file was generated C:\\Excel\\Currency.xlsx", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                xlLibro.Close(false, Missing.Value, Missing.Value);
                xlApp.Quit();
                Cursor.Current = Cursors.Default;
                MessageBox.Show(ex.Message, ex.Source, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        #endregion




    }
}
