using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MVC_CRUD_app.Data;
using MVC_CRUD_app.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace MVC_CRUD_app.Controllers
{
    public class BookController : Controller
    {
        private readonly IConfiguration _configuration;

        public BookController(IConfiguration configuration)
        {
            this._configuration = configuration;
            //ezzel behívjuk az adatbázist, kapcsolódunk
        }

        // GET: Book
        public IActionResult Index()
        {
            DataTable dtbl = new DataTable();
            using (var sqlConnection = new SqlConnection(_configuration.GetConnectionString("DevConnection")))
            {

                sqlConnection.Open();
                SqlDataAdapter sqlDa = new SqlDataAdapter("BookViewAll", sqlConnection);
                sqlDa.SelectCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlDa.Fill(dtbl);
                
            }
            return View(dtbl);
        }

        // GET: Book/Details/5
        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var bookViewModel = await _context.BookViewModel
        //        .FirstOrDefaultAsync(m => m.BookId == id);
        //    if (bookViewModel == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(bookViewModel);
        //}

        //// GET: Book/Create
        //public IActionResult Create()
        //{
        //    return View();
        //}

        //// POST: Book/Create
        //// To protect from overposting attacks, enable the specific properties you want to bind to, for 
        //// more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("BookId,Author,Title,Price")] BookViewModel bookViewModel)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(bookViewModel);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(bookViewModel);
        //}

        // GET: Book/AddOrEdit/5
        public IActionResult AddOrEdit(int? id)
        {
            BookViewModel bookViewModel = new BookViewModel();
            
            if (id != null)
            {
                bookViewModel = FetchBookByID(id);
            }
            
            return View(bookViewModel);
        }

        // POST: Book/AddOrEdit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddOrEdit(int id, [Bind("BookId,Author,Title,Price")] BookViewModel bookViewModel)
        {
            
            if (ModelState.IsValid)
            {
                // ha nincs hiba a formban, akkor meghívjuk a tárolt eljárást, hogy mentse az adatokat a db-be
                using (var sqlConnection = new SqlConnection(_configuration.GetConnectionString("DevConnection")))
                {
                    
                    sqlConnection.Open();
                    SqlCommand sqlCmd = new SqlCommand("BookAddOrEdit", sqlConnection);
                    sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlCmd.Parameters.AddWithValue("BookId", bookViewModel.BookId);
                    sqlCmd.Parameters.AddWithValue("Title", bookViewModel.Title);
                    sqlCmd.Parameters.AddWithValue("Author", bookViewModel.Author);
                    sqlCmd.Parameters.AddWithValue("Price", bookViewModel.Price);
                    sqlCmd.ExecuteNonQuery();
                }
                return RedirectToAction(nameof(Index));
            }
            return View(bookViewModel);
        }

        // GET: Book/Delete/5
        public IActionResult Delete(int? id)
        {
            BookViewModel bookViewModel = FetchBookByID(id);
            return View(bookViewModel);
        }

        // POST: Book/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            using (var sqlConnection = new SqlConnection(_configuration.GetConnectionString("DevConnection")))
            {

                sqlConnection.Open();
                SqlCommand sqlCmd = new SqlCommand("BookDeleteById", sqlConnection);
                sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCmd.Parameters.AddWithValue("BookId", id);
                sqlCmd.ExecuteNonQuery();
            }
            return RedirectToAction(nameof(Index));
        }

        [NonAction]
        public BookViewModel FetchBookByID(int? id)
        {
            BookViewModel bookViewModel = new BookViewModel();

            
            using (var sqlConnection = new SqlConnection(_configuration.GetConnectionString("DevConnection")))
            {
                DataTable dtbl = new DataTable();
                sqlConnection.Open();
                SqlDataAdapter sqlDa = new SqlDataAdapter("BookViewById", sqlConnection);
                sqlDa.SelectCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlDa.SelectCommand.Parameters.AddWithValue("BookID", id);
                sqlDa.Fill(dtbl);

                if(dtbl.Rows.Count == 1)
                {
                    bookViewModel.BookId = Convert.ToInt32(dtbl.Rows[0]["BookId"].ToString());
                    bookViewModel.Title = dtbl.Rows[0]["Title"].ToString();
                    bookViewModel.Author = dtbl.Rows[0]["Author"].ToString();
                    bookViewModel.Price = Convert.ToInt32(dtbl.Rows[0]["Price"].ToString());
                }
                return bookViewModel;
            }
        }
    }
}
