using System;
using System.Linq;
using System.Web.Mvc;
using AspNetMvcCRUD.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Collections.Generic;

namespace AspNetMvcCRUD.Controllers
{
    public class ProdutosController : Controller
    {
        //private LojaContext db = new LojaContext();
        HttpClient client = new HttpClient();

        public ProdutosController()
        {

            client.BaseAddress = new Uri("http://localhost:50089/api/produtos");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }


        // GET: Produtos
        public ActionResult Index(string busca = null)
        {
            HttpResponseMessage response = client.GetAsync("/api/produtos").Result;
            List<Produto> produtos = new List<Produto>();

            if (response.IsSuccessStatusCode)
            {
                produtos = response.Content.ReadAsAsync<List<Produto>>().Result;
            }

            if (busca != null)
                return View(produtos.Where(a => a.Nome.ToUpper().Contains(busca.ToUpper())).ToList());
            return View(produtos.ToList());

        }




        // GET: Produtos/Details/5
        public ActionResult Details(int? id)
        {
            HttpResponseMessage response = client.GetAsync($"/api/produtos/{id}").Result;
            Produto produto = response.Content.ReadAsAsync<Produto>().Result;

            if (produto != null)

                return View(produto);
            else
                return HttpNotFound();

        }


        // GET: Produtos/Create
        public ActionResult Create()
        {
            HttpResponseMessage response = client.GetAsync("/api/categorias").Result;
            List<Categoria> categorias = new List<Categoria>();

            if (response.IsSuccessStatusCode)
            {
                categorias = response.Content.ReadAsAsync<List<Categoria>>().Result;
            }

            ViewBag.CategoriaId = new SelectList(categorias, "Id", "Nome");
            return View();
        }




        //// POST: Produtos/Create
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Ativo,CategoriaId,DataCadastro,Nome,Preco")] Produto produto)
        {
            try
            {
                HttpResponseMessage response = client.PostAsJsonAsync<Produto>("/api/produtos", produto).Result;

                if (response.StatusCode == System.Net.HttpStatusCode.Created)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Error = "Error na criação do produto.";
                    return View();
                }
            }
            catch
            {
                return View();
            }
        }



        // GET: Produtos/Edit/5
        public ActionResult Edit(int? id)
        {
            HttpResponseMessage response = client.GetAsync($"/api/produtos/{id}").Result;
            Produto produto = response.Content.ReadAsAsync<Produto>().Result;

            HttpResponseMessage responses = client.GetAsync("/api/categorias").Result;
            List<Categoria> categorias = new List<Categoria>();

            if (produto != null)
            {
                if (responses.IsSuccessStatusCode)
                {
                    categorias = responses.Content.ReadAsAsync<List<Categoria>>().Result;
                    ViewBag.CategoriaId = new SelectList(categorias, "Id", "Nome", produto.CategoriaId);
                }
                return View(produto);
            }

            else
            { 
                return HttpNotFound();
            }

        }




        // PUT: Produtos/Edit/5
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Nome,Preco,DataCadastro,CategoriaId,Ativo")] int id, Produto produto)
        {
            try
            {
                HttpResponseMessage response = client.PutAsJsonAsync<Produto>($"/api/produtos/{id}", produto).Result;
                if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Error = "Error na alteração da produto.";
                    return View();
                }
            }
            catch
            {
                return View();
            }
        }



        // GET: Produtos/Delete/5
        //[ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            HttpResponseMessage response = client.GetAsync($"/api/produtos/{id}").Result;

            Produto produto = response.Content.ReadAsAsync<Produto>().Result;

            if (produto != null)
                return View(produto);
            else
                return HttpNotFound();
        }


        // POST: Produtos/Delete/5
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                HttpResponseMessage response = client.DeleteAsync($"/api/produtos/{id}").Result;

                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                    return RedirectToAction("Index");
                else
                {
                    ViewBag.Error = "Error ao deletar produto.";
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
