using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ImageGallery.Data;
using ImageGallery.Interface;
using ImageGallery.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ImageGallery.Controllers
{
    [Authorize(Roles ="Admin")]
    public class PanelController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IUploadInterface _upload;
        public PanelController(ApplicationDbContext context, IUploadInterface upload)
        {
            _context = context;
            _upload = upload;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var getDetailPicture = _context.ImageDetail.Include(i => i.Category).ToList().OrderByDescending(a => a.ReleaseDate);
            return View(getDetailPicture);
        }


        [HttpPost]
        public async Task<IActionResult> Index(string searchString)
        {
            var image = from i in _context.ImageDetail.Include(p => p.Category)
                        select i;

            if (!String.IsNullOrEmpty(searchString))
            {
                image = image.Where(s => s.ImageName.Contains(searchString));
            }

            return View(await image.ToListAsync());

        }


        [HttpGet]
        public IActionResult Create()
        {
            // действия Create обрабатывает GET-запрос и выдает представление, передавая в него объект SelectList
            //ViewData представляет словарь из пар ключ-значение
            //Используется для вывода категории в форме
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name");
            return View();
        }

        [HttpPost]
        public IActionResult Create(IList<IFormFile> files, ImageDetail image)
        {
            //Добавляем данные в таблицу
            foreach (var item in files)
            {
                image.PathImage = "~/uploads/" + item.FileName.Trim();
                image.Size = item.Length / 1000;
            }
            _upload.UploadFileMultiple(files);
            _context.ImageDetail.Add(image);
            _context.SaveChanges();

            //TempData, как и ViewData, представляет словарь, хранящий пары ключ-значение
            TempData["Sucess"] = "Save Your File";

            //ViewData представляет словарь из пар ключ-значение
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", image.CategoryId);
            return RedirectToAction("Create", "Panel");
        }


        [HttpGet]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pictureDetail = await _context.ImageDetail.Include(p => p.Category).FirstOrDefaultAsync(m => m.Id == id);
            if (pictureDetail == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", pictureDetail.CategoryId);
            return View(pictureDetail);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //Фильтр ValidateAntiforgeryToken предназначен для противодействия 
        //подделке межсайтовых запросов, производя верификацию токенов при 
        //обращении к методу действия. Наиболее частым случаем является 
        //применение данного фильтра к методам, отвечающим за авторизацию
        public async Task<IActionResult> Edit(Guid id, ImageDetail imageDetail, IList<IFormFile> files)
        {
            if (id != imageDetail.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    foreach (var item in files)
                    {
                        imageDetail.PathImage = "~/uploads/" + item.FileName.Trim();
                        imageDetail.Size = item.Length / 1000;
                    }
                    _upload.UploadFileMultiple(files);
                    _context.ImageDetail.Add(imageDetail);
                    _context.Update(imageDetail);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PictureDetailExists(imageDetail.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", imageDetail.CategoryId);
            return View(imageDetail);
        }

        private bool PictureDetailExists(Guid id)
        {
            return _context.ImageDetail.Any(e => e.Id == id);
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pictureDetail = await _context.ImageDetail
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pictureDetail == null)
            {
                return NotFound();
            }

            return View(pictureDetail);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pictureDetail = await _context.ImageDetail
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pictureDetail == null)
            {
                return NotFound();
            }

            return View(pictureDetail);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        //Фильтр ValidateAntiforgeryToken предназначен для противодействия 
        //подделке межсайтовых запросов, производя верификацию токенов при 
        //обращении к методу действия. Наиболее частым случаем является 
        //применение данного фильтра к методам, отвечающим за авторизацию
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pictureDetail = await _context.ImageDetail.FindAsync(id);
            _context.ImageDetail.Remove(pictureDetail);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}