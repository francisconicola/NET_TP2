﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Business.Entities;
using Business.Logic;

namespace UI.Web
{
    public partial class Comisiones : ApplicationPage
    {
        private ComisionLogic _ComisionLogic;
        private ComisionLogic ComisionLogic
        {
            get
            {
                if (_ComisionLogic == null) _ComisionLogic = new ComisionLogic();
                return _ComisionLogic;
            }
        }
        private PlanLogic PlanLogic { get;  }
        private Comision ComisionActual { get; set; }
       
        private int SelectedID
        {
            get
            {
                return ViewState["SelectedID"] != null ?
                    (int)ViewState["SelectedID"] : 0;
            }
            set
            {
                ViewState["SelectedID"] = value;
            }
        }

        private bool IsEntitySelected => SelectedID != 0;

        private void LoadForm(int id)
        {
            ComisionActual = ComisionLogic.GetOne(id);
            txtDescripcion.Text = ComisionActual.Descripcion;
            txtAñoEspecialidad.Text = ComisionActual.AñoEspecialidad.ToString();
         //   DropDownList1.SelectedValue=int.Parse( PlanLogic.GetOne(ComisionActual.IdPlan).ID);



        }

        private void EnableForm(bool enable)
        {
            txtDescripcion.Enabled = enable;
            txtAñoEspecialidad.Enabled = enable;
            DropDownList1.Enabled = enable;

        }

        private void ClearForm()
        {
            txtAñoEspecialidad.Text = string.Empty;
            txtDescripcion.Text = string.Empty;
            DropDownList1.Text = string.Empty;
        }

        private void LoadGrid()
        {
            GridView.DataSource = ComisionLogic.GetAllTable();
            GridView.DataBind();
        }

        private void LoadEntity(Comision Comision)
        {
            Comision.Descripcion = txtDescripcion.Text;
            Comision.AñoEspecialidad =int.Parse( txtAñoEspecialidad.Text);
           // Comision.IdPlan = ((Plan)DropDownList1.SelectedItem).ID;

        }

        private void SaveEntity(Comision Comision)
        {
            ComisionLogic.Save(Comision);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            LoadGrid();
        }

        protected void GridView_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectedID = (int)GridView.SelectedValue;
        }

        protected void btnEditarLink_Click(object sender, EventArgs e)
        {
            if (IsEntitySelected)
            {
                btnAceptarLink.Visible = true;
                btnCancelarLink.Visible = true;
                formPanel.Visible = true;
                Modo = ModoForm.Modificacion;
                LoadForm(SelectedID);
            }
        }

        protected void btnAceptarLink_Click(object sender, EventArgs e)
        {
            switch (Modo)
            {
                case ModoForm.Baja:
                    ComisionActual = new Comision();
                    ComisionActual.ID = SelectedID;
                    ComisionActual.State = BusinessEntity.States.Deleted;
                    LoadEntity(ComisionActual);
                    SaveEntity(ComisionActual);
                    LoadGrid();
                    break;
                case ModoForm.Modificacion:
                    ComisionActual = new Comision();
                    ComisionActual.ID = SelectedID;
                    ComisionActual.State = BusinessEntity.States.Modified;
                    LoadEntity(ComisionActual);
                    SaveEntity(ComisionActual);
                    LoadGrid();
                    break;
                case ModoForm.Alta:
                    ComisionActual = new Comision();
                    LoadEntity(ComisionActual);
                    SaveEntity(ComisionActual);
                    LoadGrid();
                    break;
                default:
                    break;
            }
            formPanel.Visible = false;
        }

        protected void btnEliminarLink_Click(object sender, EventArgs e)
        {
            if (IsEntitySelected)
            {
                btnAceptarLink.Visible = true;
                btnCancelarLink.Visible = true;
                formPanel.Visible = true;
                Modo = ModoForm.Baja;
                EnableForm(false);
                LoadForm(SelectedID);

            }
        }

        protected void btnNuevoLink_Click(object sender, EventArgs e)
        {
            btnAceptarLink.Visible = true;
            btnCancelarLink.Visible = true;
            formPanel.Visible = true;
            Modo = ModoForm.Alta;
            ClearForm();
            EnableForm(true);

        }

        protected void btnCancelarLink_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/MainMenu.aspx");
        }
    }
}
