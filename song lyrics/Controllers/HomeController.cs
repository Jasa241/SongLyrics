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
                    //Lyric.Lyrics = "I found a love for me\r\nDarling, just dive right in, follow my lead\r\nWell, I found a girl, beautiful and sweet\r\nOh, I never knew you were the someone waiting for me\r\n'Cause we were just kids when we fell in love\r\nNot knowing what it was\n\nI will not give you up this time\n\nBut darling, just kiss me slow, your heart is all I own\n\nAnd in your eyes you're holding mine\n\n\n\nBaby, I'm dancing in the dark with you between my arms\n\nBarefoot on the grass, listening to our favorite song\n\nWhen you said you looked a mess, I whispered underneath my breath\n\nBut you heard it, darling, you look perfect tonight\n\n\n\nWell I found a woman, stronger than anyone I know\n\nShe shares my dreams, I hope that someday I'll share her home\n\nI found a love, to carry more than just my secrets\n\nTo carry love, to carry children of our own\n\nWe are still kids, but we're so in love\n\nFighting against all odds\n\nI know we'll be alright this time\n\nDarling, just hold my hand\n\nBe my girl, I'll be your man\n\nI see my future in your eyes\n\n\n\nBaby, I'm dancing in the dark, with you between my arms\n\nBarefoot on the grass, listening to our favorite song\n\nWhen I saw you in that dress, looking so beautiful\n\nI don't deserve this, darling, you look perfect tonight\n\n\n\nBaby, I'm dancing in the dark, with you between my arms\n\nBarefoot on the grass, listening to our favorite song\n\nI have faith in what I see\n\nNow I know I have met an angel in person\n\nAnd she looks perfect\n\nI don't deserve this\n\nYou look perfect tonight";
                    lyric = Lyric.Lyrics;
                    ViewBag.Title = Song;

                    return View(Lyric);
                }
                else {
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