﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using JapanoriSystem.DAL;
using JapanoriSystem.Models;
using WebGrease.Activities;
using PagedList.Mvc;
using PagedList;

namespace JapanoriSystem.Controllers
{
    public class ComandaController : Controller
    {
        bdJapanoriContext db = new bdJapanoriContext();


        //              Tela Inicial da Comanda
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            //      Cadeia de objetos para definir a "current" Ordem da listagem das comandas
            ViewBag.CurrentSort = sortOrder;
            ViewBag.CodSortParm = String.IsNullOrEmpty(sortOrder) ? "" : "cod_cre"; // objeto que organiza a lista em ordem do código
            ViewBag.SitSortParm = String.IsNullOrEmpty(sortOrder) ? "" : "sit_cre"; // objeto que organiza a lista em ordem da situacao
            ViewBag.PriceSortParm = String.IsNullOrEmpty(sortOrder) ? "" : "preco_decre"; // objeto que organiza a lista em ordem de preço

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var comandas = from s in db.tbComanda
                           select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                comandas = comandas.Where(s => s.ID.ToString().Contains(searchString));
            }
            switch (sortOrder)
            {
                case "cod_cre":
                    comandas = comandas.OrderBy(s => s.ID);
                    break;
                case "sit_cre":
                    comandas = comandas.OrderBy(s => s.Situacao);
                    break;
                case "preco_decre":
                    comandas = comandas.OrderByDescending(s => s.PrecoTotal);
                    break;
                default:
                    comandas = comandas.OrderByDescending(s => s.PrecoTotal);
                    break;
            }
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(comandas.ToPagedList(pageNumber, pageSize));
        }


        //          Tela Detalhes da Comanda
        public ActionResult Details(int? id)
        {
            // Ação de redirecionar para Tela inicial Comandas se não retornar um ID para detalhar
            if (id == null)
            {
                return RedirectToAction("Index","Comanda");
            }
            Comanda comanda = db.tbComanda.Find(id); // Retorna em lista os dados da comanda que possuir o ID retornado ao sistema
            if (comanda == null)
            {
                return RedirectToAction("Index", "Comanda");
            }
            return View(comanda);
        }

        //          Tela Criação de Comanda
        public ActionResult Create()
        {
            return View();
        }

        //      POST Tela Criação de Comanda
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Situacao,Status")] Comanda comanda)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.tbComanda.Add(comanda);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (DataException)
            {
                ModelState.AddModelError("", "Unable to save changes.");
                ViewBag.Message = "Não foi possível Criar a comanda";
            }
            return View(comanda);
        }


        //          Tela Edição de Comanda
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Comanda");
            }
            Comanda comanda = db.tbComanda.Find(id);
            if (comanda == null)
            {
                return RedirectToAction("Index", "Comanda");
            }
            return View(comanda);
        }

        //      POST Tela Edição de Comanda
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Situacao,Status")] Comanda comanda)
        {
            if (ModelState.IsValid)
            {
                db.Entry(comanda).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(comanda);
        }


        //          Tela Excluir dados da Comanda
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Comanda");
            }
            Comanda comanda = db.tbComanda.Find(id);
            if (comanda == null)
            {
                return RedirectToAction("Index", "Comanda");
            }
            return View(comanda);
        }

        //          POST Tela Excluir dados da Comanda
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Comanda comanda = db.tbComanda.Find(id);
            db.tbComanda.Remove(comanda);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //          Ação de se "desconectar" do banco
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
