using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using profjulioguimaraes.Data;
using System.Text;
using Amizade.Domain.Model.Entities;
using Amizade.Infrastructure.Services.Blob;
using Amizade.Infrastructure.Services.Queue;

namespace profjulioguimaraes.Controllers
{
    public class AmigoController : Controller
    {
        private readonly AmizadeDbContext _context;
        private readonly IBlobService _blobService;
        private readonly IQueueService _queueService;
        private readonly IConfiguration _configuration;

        public AmigoController(
            AmizadeDbContext context, 
            IBlobService blobService, 
            IQueueService queueService,
            IConfiguration configuration)
        {
            _context = context;
            _blobService = blobService;
            _queueService = queueService;
            _configuration = configuration;
        }

        // GET: Amigo
        public async Task<IActionResult> Index()
        {
              return _context.Amigo != null ? 
                          View(await _context.Amigo.ToListAsync()) :
                          Problem("Entity set 'AmizadeDbContext.Amigo'  is null.");
        }

        // GET: Amigo/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Amigo == null)
            {
                return NotFound();
            }

            var amigo = await _context.Amigo
                .FirstOrDefaultAsync(m => m.Id == id);
            if (amigo == null)
            {
                return NotFound();
            }

            ///*************************************************
            //INVOCANDO UMA FUNÇÃO ASSINCRONAMENTE */
            //var httpClient = new HttpClient();
            //var json = JsonConvert.SerializeObject(new { amigoId = id });
            //var requestData = new StringContent(json, Encoding.UTF8, "application/json");
            //var baseAddressFunction = _configuration.GetValue<string>("FunctionBaseAddress");
            //_ = await httpClient.PostAsync(baseAddressFunction, requestData);
            ///**************************************************/


            //primeira forma de serializar objeto json/base64 (usando package Newtonsoft.Json)
            var jsonAmigo = JsonConvert.SerializeObject(amigo);
            var bytesJsonAmigo = UTF8Encoding.UTF8.GetBytes(jsonAmigo);
            string jsonAmigoBase64 = Convert.ToBase64String(bytesJsonAmigo);

            //segunda forma de serializar objeto json/base64 (usando package System.Text.Json)
            //var jsonBytes = JsonSerializer.SerializeToUtf8Bytes(amigoEntity);
            //string jsonAmigoBase64 = Convert.ToBase64String(jsonBytes);

            //enviando objeto serializado em json e base64 como msg para a fila
            await _queueService.SendAsync(jsonAmigoBase64);


            return View(amigo);
        }

        // GET: Amigo/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Amigo/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Amigo amigo)
        {
            if (ModelState.IsValid)
            {
                var file = Request.Form.Files.SingleOrDefault();
                var stream = file?.OpenReadStream();
                var newUri = await _blobService.UploadAsync(stream);
                amigo.ImagemUrl = newUri;

                _context.Add(amigo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(amigo);
        }

        // GET: Amigo/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Amigo == null)
            {
                return NotFound();
            }

            var amigo = await _context.Amigo.FindAsync(id);
            if (amigo == null)
            {
                return NotFound();
            }
            return View(amigo);
        }

        // POST: Amigo/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PrimeiroNome,Sobrenome,DataNascimento,QuantidadeFilhos,PossuiParentesco")] Amigo amigo)
        {
            if (id != amigo.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(amigo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AmigoExists(amigo.Id))
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
            return View(amigo);
        }

        // GET: Amigo/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Amigo == null)
            {
                return NotFound();
            }

            var amigo = await _context.Amigo
                .FirstOrDefaultAsync(m => m.Id == id);
            if (amigo == null)
            {
                return NotFound();
            }

            return View(amigo);
        }

        // POST: Amigo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Amigo == null)
            {
                return Problem("Entity set 'AmizadeDbContext.Amigo'  is null.");
            }
            var amigo = await _context.Amigo.FindAsync(id);
            if (amigo != null)
            {
                _context.Amigo.Remove(amigo);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AmigoExists(int id)
        {
          return (_context.Amigo?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
