using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using MusicPlatform.Models;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using System.Web.Helpers;
using System.Web.UI;
using System.Net.Http;

namespace MusicPlatform.Controllers
{
    public class SongsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private const string ApiKey = "AIzaSyBKc1f04ZsjmtyHCHIUqFMzn9urIdAmjqg";

        List<string> Genres = new List<string>
            {
                "Alternative", "Ambient", "Anime", "Blues", "Bollywood", "Classical",
                "Contemporary", "Country", "C-pop", "Crunk", "Dance", "Disco",
                "Dubstep", "EDM", "Electronic", "Electronica", "Emo", "Folk",
                "Funk", "Garage", "Gospel", "Grunge", "Heavy Metal", "Hip-Hop",
                "House", "Indie", "Instrumental", "Jazz", "J-pop", "K-pop",
                "Latin", "New Wave", "Opera", "Pop", "R&B", "Rap", "Reggae",
                "Reggaeton", "Rock", "Soul", "Soundtrack", "Swing", "Techno", "Trap",
            };

        List<string> Languages = new List<string>
            {
                "Albanian", "Amharic", "Arabic", "Armenian", "Azerbaijani", "Bambara",
                "Belarusian", "Bengali", "Berber", "Bhojpuri", "Bislama", "Bosnian",
                "Bulgarian", "Burmese", "Cantonese", "Catalan", "Chichewa", "Chinese",
                "Comorian", "Croatian", "Czech", "Danish", "Dhivehi", "Dutch", "Dzongkha",
                "English", "Estonian", "Fiji Hindi", "Fijian", "Filipino", "Finnish",
                "French", "Georgian", "German", "Greek", "Guaraní", "Gujarati", "Haitian Creole",
                "Hausa", "Hebrew", "Hindi", "Hiri", "Hungarian", "Icelandic", "Indonesian",
                "Irish", "Italian", "Japanese", "Kazakh", "Khmer", "Kiribati", "Kirundi",
                "Korean", "Kurdish", "Kyrgyz", "Lao", "Latin", "Latvian", "Lithuanian",
                "Luxembourgish", "Macedonian", "Malagasy", "Malay", "Maltese", "Mandarin",
                "Māori", "Marathi", "Marshallese", "Moldovan", "Mongolian", "Montenegrin",
                "Morisien", "Nauruan", "Nepali", "Niuean", "Norfuk", "Norwegian", "Ossetian",
                "Palauan", "Pashto", "Persian", "Polish", "Portuguese", "Punjabi", "Quechua",
                "Romanian", "Russian", "Rwandan", "Samoan", "Sango", "Serbian", "Shona","Sinhala",
                "Slovak", "Slovenian", "Somali", "Sotho", "Spanish", "Swahili", "Swazi", "Swedish",
                "Tajik", "Tamil", "Telugu", "Tetum", "Thai", "Tigrinya","Tokelauan", "Turkish",
                "Turkmen", "Tuvaluan", "Ukrainian", "Urdu", "Uzbek", "Vietnamese"
            };

        // GET: Songs
        public ActionResult Index()
        {
            var email = User.Identity.GetUserName();
            ViewBag.Email = email;

            if (User.IsInRole("Artist"))
            {
                var songs = db.Songs.Include(s => s.Album).Include(s => s.Artist)
                                               .Where(song => song.Artist.Email == email).ToList();

                if (songs == null)
                {
                    return HttpNotFound();
                }

                return View(songs);
            }

            // For Listeners, show all songs
            var allSongs = db.Songs.ToList();

            return View(allSongs);
        }

        // POST: Songs/Like/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Like(int id)
        {
            var song = db.Songs.Find(id);
            if (song != null)
            {
                var email = User.Identity.GetUserName();

                var user = db.Listeners.FirstOrDefault(l => l.Email == email);

                // Check if the user has a "Liked Songs" playlist, create if not
                var likedSongsPlaylist = user.Playlists.FirstOrDefault(p => p.Title == "Liked Songs");
                if (likedSongsPlaylist == null)
                {
                    likedSongsPlaylist = new Models.Playlist { Title = "Liked Songs", ListenerId = user.Id };
                    db.Playlists.Add(likedSongsPlaylist);
                    user.Playlists.Add(likedSongsPlaylist);
                }

                // Add the liked song to the "Liked Songs" playlist
                likedSongsPlaylist.Songs.Add(song);

                song.LikesFromListeners.Add(user);
                song.Likes++;
                db.SaveChanges();


            }
            if (Request.UrlReferrer != null)
            {
                return Redirect(Request.UrlReferrer.ToString());
            }

            // If no URLReferrer or any other issue, redirect to the default view
            return RedirectToAction("Index", "Songs");
        }

        // POST: Songs/Unlike/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Unlike(int id)
        {
            var song = db.Songs.Find(id);
            if (song != null)
            {
                var email = User.Identity.GetUserName();
                var user = db.Listeners.FirstOrDefault(l => l.Email == email);

                // Find the "Liked Songs" playlist of the user
                var likedSongsPlaylist = user.Playlists.FirstOrDefault(p => p.Title == "Liked Songs");
                if (likedSongsPlaylist != null)
                {
                    // Remove the unliked song from the "Liked Songs" playlist
                    likedSongsPlaylist.Songs.Remove(song);
                }

                song.LikesFromListeners.Remove(user);
                song.Likes--;
                db.SaveChanges();
            }

            if (Request.UrlReferrer != null)
            {
                return Redirect(Request.UrlReferrer.ToString());
            }

            return RedirectToAction("Index", "Songs");
        }

        // GET: Songs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var song = db.Songs.Include(s => s.Comments.Select(c => c.Replies))
                              .FirstOrDefault(s => s.Id == id); if (song == null)
            {
                return HttpNotFound();
            }

            if (!string.IsNullOrEmpty(song.VideoURL) && !string.IsNullOrEmpty(song.AudioURL))
            {
                if (!string.IsNullOrEmpty(song.VideoURL))
                {
                    YouTubeService youtubeService = new YouTubeService(new BaseClientService.Initializer
                    {
                        ApiKey = ApiKey,
                        ApplicationName = "MusicPlatform"
                    });

                    var videoRequest = youtubeService.Videos.List("snippet");
                    videoRequest.Id = song.VideoURL;
                    var videoResponse = videoRequest.Execute();

                    if (videoResponse.Items.Count > 0)
                    {
                        ViewBag.VideoTitle = videoResponse.Items[0].Snippet.Title;
                        ViewBag.ThumbnailUrl = videoResponse.Items[0].Snippet.Thumbnails.Default__.Url;
                    }
                    ViewBag.VideoId = song.VideoURL;
                }
                ViewBag.AudioId = song.AudioURL;
            }

            return View(song);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddComment(int songId, string content)
        {
            var song = db.Songs.Find(songId);
            if (song != null && !string.IsNullOrEmpty(content))
            {
                var email = User.Identity.GetUserName();
                var photo = db.Listeners.FirstOrDefault(l => l.Email == email).PhotoUrl;
                var comment = new Models.Comment
                {
                    Song_Id = songId,
                    Content = content,
                    Email = email,
                    PhotoURL = photo
                };

                song.Comments.Add(comment);
                db.SaveChanges();
            }

            // Redirect back to the song details view
            return RedirectToAction("Details", new { id = songId });
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddReply(int songId, int parentId, string content)
        {
            var song = db.Songs.Find(songId);
            var parentComment = db.Comments.Find(parentId);

            if (song != null && parentComment != null && !string.IsNullOrEmpty(content))
            {
                var email = User.Identity.GetUserName();
                var photo = db.Listeners.FirstOrDefault(l => l.Email == email).PhotoUrl;
                var reply = new Models.Comment
                {
                    Song_Id = songId,
                    ParentCommentId = parentId,
                    Content = content,
                    Email = email,
                    PhotoURL = photo
                };
                parentComment.Replies.Add(reply);
                db.SaveChanges();

                db.Entry(parentComment).Collection(c => c.Replies).Load();
                db.Entry(reply).Collection(r => r.Replies).Load();

                return PartialView("_Comment", reply); // Return the partial view with the newly added reply
            }

            // If there was an error, return an appropriate JSON response (e.g., an empty object)
            return Json(new { });
        }


        [HttpPost]
        public ActionResult IncrementPlays(int songId)
        {
            var song = db.Songs.Find(songId);
            if (song != null)
            {
                var email = User.Identity.GetUserName();
                var listener = db.Listeners.FirstOrDefault(l => l.Email == email);

                song.Plays++;
                db.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }


        private string GetIdFromUrl(string url)
        {
            if (!string.IsNullOrEmpty(url) && Uri.TryCreate(url, UriKind.Absolute, out Uri uriResult))
            {
                if (uriResult.Host.Equals("www.youtube.com", StringComparison.OrdinalIgnoreCase) &&
                    !string.IsNullOrEmpty(uriResult.Query))
                {
                    var queryDictionary = HttpUtility.ParseQueryString(uriResult.Query);
                    var id = queryDictionary["v"];
                    return id;
                }
            }
            return null;
        }

        // GET: Songs/Create
        public ActionResult Create(int artistId, int albumId)
        {
            Song song = new Song();
            song.ArtistId = artistId;
            song.AlbumId = albumId;

            SelectList genreSelectList = new SelectList(Genres);
            SelectList languageSelectList = new SelectList(Languages);

            ViewBag.Genres = genreSelectList;
            ViewBag.Languages = languageSelectList;

            return View(song);
        }

        // POST: Songs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,VideoURL,AudioURL,Lyrics,Language,Genre,ArtistId,AlbumId")] Song song)
        {
            if (ModelState.IsValid)
            {
                var videoId = GetIdFromUrl(song.VideoURL);
                var audioId = GetIdFromUrl(song.AudioURL);

                song.VideoURL = videoId;
                song.AudioURL = audioId;

                db.Songs.Add(song);
                db.SaveChanges();
                return RedirectToAction("Details", "Albums", new { id = song.AlbumId });
            }
            return View(song);
        }


        // GET: Songs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Song song = db.Songs.Find(id);
            if (song == null)
            {
                return HttpNotFound();
            }

            SelectList genreSelectList = new SelectList(Genres);
            SelectList languageSelectList = new SelectList(Languages);

            ViewBag.Genres = genreSelectList;
            ViewBag.Languages = languageSelectList;

            return View(song);
        }

        // POST: Songs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,VideoURL,AudioURL,Lyrics,Language,Genre,ArtistId,AlbumId,Likes")] Song song)
        {
            if (ModelState.IsValid)
            {
                var videoId = GetIdFromUrl(song.VideoURL);
                var audioId = GetIdFromUrl(song.AudioURL);

                song.VideoURL = videoId;
                song.AudioURL = audioId;

                db.Entry(song).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", "Songs", new { id = song.Id });
            }
            return View(song);
        }


        // GET: Songs/Delete/5
        public ActionResult Delete(int id)
        {
            Song song = db.Songs.Find(id);

            db.Songs.Remove(song);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

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
