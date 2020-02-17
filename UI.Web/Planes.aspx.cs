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
    public partial class Planes : ApplicationPage
    {
        private PlanLogic _PlanLogic;
        private PlanLogic PlanLogic
        {
            get
            {
                if (_PlanLogic == null) _PlanLogic = new PlanLogic();
                return _PlanLogic;
            }
        }
        private EspecialidadLogic EspecialidadLogic { get; set; }


        private Plan PlanActual { get; set; }


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
            PlanActual = PlanLogic.GetOne(id);
            txtdescPlan.Text = PlanActual.Descripcion;
            txtdescEspecialidad.Text = EspecialidadLogic.GetOne(PlanActual.IdEspecialidad).Descripcion;
        }

        private void EnableForm(bool enable)
        {
            txtdescPlan.Enabled = enable;
            txtdescEspecialidad.Enabled = enable;
        }

        private void ClearForm()
        {
            txtdescPlan.Text = string.Empty;
            txtdescEspecialidad.Text = string.Empty;
        }

        private void LoadGrid()
        {
            GridView.DataSource = PlanLogic.GetAllTable();
            GridView.DataBind();
        }

        private void LoadEntity(Plan Plan)
        {
            Plan.Descripcion = txtdescPlan.Text;
            Plan.IdEspecialidad = int.Parse(txtdescEspecialidad.ID);
        }

        private void SaveEntity(Plan Plan)
        {
            PlanLogic.Save(Plan);
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
                    PlanActual = new Plan();
                    PlanActual.ID = SelectedID;
                    PlanActual.State = BusinessEntity.States.Deleted;
                    LoadEntity(PlanActual);
                    SaveEntity(PlanActual);
                    LoadGrid();
                    break;
                case ModoForm.Modificacion:
                    PlanActual = new Plan();
                    PlanActual.ID = SelectedID;
                    PlanActual.State = BusinessEntity.States.Modified;
                    LoadEntity(PlanActual);
                    SaveEntity(PlanActual);
                    LoadGrid();
                    break;
                case ModoForm.Alta:
                    PlanActual = new Plan();
                    LoadEntity(PlanActual);
                    SaveEntity(PlanActual);
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