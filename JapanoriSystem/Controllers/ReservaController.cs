﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JapanoriSystem.Controllers
{
    public class ReservaController : Controller
    {
        // GET: Reserva
        public ActionResult Cadastro()
        {
            return View();
        }

        public ActionResult Consulta()
        {
            return View();
        }
    }
}