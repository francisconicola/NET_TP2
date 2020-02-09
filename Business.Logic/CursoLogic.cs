﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Entities;
using Data.Database;

namespace Business.Logic
{
    public class CursoLogic
    {
        private CursoAdapter CursoData { get; set; }

        public CursoLogic()
        {
            CursoData = new CursoAdapter();
        }

        public List<Curso> GetAll()
        {
            return CursoData.GetAll();
        }

        public Curso GetOne(int id)
        {
            return CursoData.GetOne(id);
        }

        public DataTable GetAllTable()
        {
            MateriaAdapter materiaData = new MateriaAdapter();
            ComisionAdapter comisionData = new ComisionAdapter();

            DataTable table = new DataTable();
            table.Columns.Add("ID", typeof(int));
            table.Columns.Add("descMateria", typeof(string));
            table.Columns.Add("descComision", typeof(string));
            table.Columns.Add("anioCalendario", typeof(int));
            table.Columns.Add("cupo", typeof(int));

            List<Curso> cursos = GetAll();
            DataRow row;

            foreach (Curso curso in cursos)
            {
                row = table.NewRow();
                row["ID"] = curso.ID;
                row["descMateria"] = materiaData.GetOne(curso.IdMateria).Descripcion;
                row["descComision"] = comisionData.GetOne(curso.IdComision).Descripcion;
                row["anioCalendario"] = curso.AñoCalendario;
                row["cupo"] = curso.Cupo;
                table.Rows.Add(row);
            }

            return table;
        }

        public void Save(Curso curso)
        {
            CursoData.Save(curso);
        }
    }
}