using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using song_lyrics.Models;
using System.Text.RegularExpressions;
using System.IO;
using Rotativa;

namespace song_lyrics.Controllers
{
    public class HomeController : Controller
    {
        private static string lyric;
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Index(string Author, string Song)
        {
            HttpClient client = new HttpClient();
            string Url = "https://api.lyrics.ovh/v1/" + Author + "/" + Song;
            HttpResponseMessage Result = await client.GetAsync(Url);

            if (Result.IsSuccessStatusCode)
            {
                var Json = await client.GetStringAsync(Url);
                API Lyric = JsonConvert.DeserializeObject<API>(Json);

                if (Lyric.Lyrics != "")
                {
                    lyric = Lyric.Lyrics;
                    ViewBag.Title = Song;

                    return View(Lyric);
                }
                else
                {
                    ViewBag.Title = "";
                    return View();
                }
            }

            return View();
        }

        public ActionResult PDF(string Title)
        {
            Song song = new Song() { Title = Title, Lyric = lyric };

            return View(song);
        }

        public ActionResult DownLoad(string Title)
        {
            return new ActionAsPdf("PDF", new { Title = Title }) { FileName = Title + ".pdf" };
        }

    }
}