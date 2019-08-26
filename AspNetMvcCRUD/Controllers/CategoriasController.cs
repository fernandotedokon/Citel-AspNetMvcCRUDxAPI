using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Mvc;
using AspNetMvcCRUD.Models;

namespace AspNetMvcCRUD.Controllers
{
    public class CategoriasController : Controller
    {
        //private LojaContext db = new LojaContext();
        HttpClient client = new HttpClient();

        public CategoriasController()
        {

            client.BaseAddress = new Uri("http://localhost:50089/api/categorias");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }


        // GET: Categorias
        public ActionResult Index()
        {

            List<Categoria> categorias = new List<Categoria>();
            HttpResponseMessage response = client.GetAsync("/api/categorias").Result;

            if (response.IsSuccessStatusCode)
            {
                categorias = response.Content.ReadAsAsync<List<Categoria>>().Result; 
            }
            return View(categorias);

        }


        // GET: Categorias/Details/5
        public ActionResult Details(int? id)
        {
            HttpResponseMessage response = client.GetAsync($"/api/categorias/{id}").Result;
            Categoria categoria = response.Content.ReadAsAsync<Categoria>().Result;

            if (categoria != null)
                return View(categoria);
            else
                return HttpNotFound();
        
        }



        // GET: Categorias/Create
        public ActionResult Create()
        {
            return View();
        }


        // POST: Categorias/Create
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Nome")] Categoria categoria)
        {
            try
            {
                HttpResponseMessage response = client.PostAsJsonAsync<Categoria>("/api/categorias", categoria).Result;
                if (response.StatusCode == System.Net.HttpStatusCode.Created)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Error = "Error na criação da categoria.";
                    return View();
                }
            }
            catch
            {
                return View();
            }
        }


        // GET: Categorias/Edit/5
        public ActionResult Edit(int? id)
        {
            HttpResponseMessage response = client.GetAsync($"/api/categorias/{id}").Result;
            Categoria categoria = response.Content.ReadAsAsync<Categoria>().Result;

            if (categoria != null)
                return View(categoria);
            else
                return HttpNotFound();

        }


        // PUT: Categorias/Edit/5
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Categoria categoria)
        {
            try
            {
                HttpResponseMessage response = client.PutAsJsonAsync<Categoria>($"/api/categorias/{id}", categoria).Result;
                if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Error = "Error na alteração da categoria.";
                    return View();
                }
            }
            catch
            {
                return View();
            }
        }



        // GET: Categorias/Delete/5
        //[ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            HttpResponseMessage response = client.GetAsync($"/api/categorias/{id}").Result;

            Categoria categoria = response.Content.ReadAsAsync<Categoria>().Result;

            if (categoria != null)
                return View(categoria);
            else
                return HttpNotFound();
        }

        // POST: Categorias/Delete/5
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                HttpResponseMessage response = client.DeleteAsync($"/api/categorias/{id}").Result;

                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                    return RedirectToAction("Index");
                else
                {
                    ViewBag.Error = "Error ao deletar categoria.";
                    return View();
                }
            }
            catch
            {
                return View();
            }
        }



    }


}
