using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Mensajeria.MVC.Controllers
{
    public class MensajeLecturasController : Controller
    {
        // GET: MensajeLecturasController
        public ActionResult Index()
        {
            return View();
        }

        // GET: MensajeLecturasController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: MensajeLecturasController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MensajeLecturasController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: MensajeLecturasController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: MensajeLecturasController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: MensajeLecturasController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: MensajeLecturasController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
