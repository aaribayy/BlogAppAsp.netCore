using BlogCoreMVC.Models;
using BlogCoreMVC.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;
//using MyBlogMVCCore.Views.Blog;

namespace BlogCoreMVC.Controllers
{
    public class BlogController : Controller
    {
        private readonly BlogDbContext context;
        private readonly IWebHostEnvironment env;

        public BlogController(BlogDbContext context, IWebHostEnvironment env)
        {
            this.context = context;
            this.env = env;
        }
        public IActionResult Index(string search ="")
        {
            if(search == null|| search == "")
            {
                var data = context.authors.OrderByDescending(x => x.Id).Include(c => c.Blogs).ToList();

                return View(data);
            }
            var find = context.authors.OrderByDescending(x => x.Id).Include(c => c.Blogs.Where(x => x.Title.Contains(search))).ToList();

    
            return View(find);
        }

        public IActionResult Login()
        {
            return View();
        }
        

        [HttpPost]
        public ActionResult Login(AuthorTb a)
        {
            var ad = context.authors.FirstOrDefault(x => x.Username == a.Username && x.Password == a.Password);
            if (ad != null)
            {
                
                HttpContext.Session.SetString("ad_id", ad.Id.ToString());
                HttpContext.Session.SetInt32("ad_x", ad.Id); 
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Error = "Invalid Username or Password";
                return View(a);
            }
        }
        public IActionResult Create()
        {
            var x = HttpContext.Session.GetInt32("ad_x");
            if (x == null || x == 0)
            {
                return RedirectToAction("Login");
            }
            return View();
        }

        [HttpPost]
        public IActionResult Create(BlogDto blogd)
        {
            
            if (blogd.ImageFile == null)
            {
                ModelState.AddModelError("ImageFile", "The image file is required");
            }
            if (!ModelState.IsValid)
            {
                return View(blogd);
            }

            string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            newFileName += Path.GetExtension(blogd.ImageFile!.FileName);
            string imageFullPath = env.WebRootPath + "/blogs/" + newFileName;
            using (var stream = System.IO.File.Create(imageFullPath))
            {
                blogd.ImageFile.CopyTo(stream);
            }


            BlogTb blog = new BlogTb()
            {
                Title = blogd.Title,
                Content = blogd.Content,
                AuthorId = HttpContext.Session.GetInt32("ad_x") ?? 0,
                Description = blogd.Description,
                ImageFileName = newFileName,
                PublishedDate = DateTime.Now
            };

            context.blogs.Add(blog);
            context.SaveChanges();
            return RedirectToAction("Index", "Blog");
        }

        public IActionResult Edit(int id)
        {
            var x = HttpContext.Session.GetInt32("ad_x");
            if (x == null || x == 0)
            {
                return RedirectToAction("Login");
            }
            var blog = context.blogs.Find(id);
            if (blog == null)
            {
                return RedirectToAction("Index", "Blog");
            }

            var blogDto = new BlogDto()
            {
                Title = blog.Title,
                Content = blog.Content,
                Description = blog.Description
            };

            ViewData["BlogId"] = blog.Id;
            ViewData["ImageFileName"] = blog.ImageFileName;
            ViewData["CreatedAt"] = blog.PublishedDate.ToString("MM/dd/yyyy");
            var authorName = query(id);
            ViewData["Author"] = authorName;

            return View(blogDto);
        }

        public string query(int id)
        {
            var authorName = (from b in context.blogs
                              join a in context.authors on b.AuthorId equals a.Id
                              where b.Id == id
                              select a.Name).FirstOrDefault();
            return authorName;
        }

        [HttpPost]
        public IActionResult Edit(int id, BlogDto bdt)
        {
            var blog = context.blogs.Find(id);
            if (blog == null)
            {
                return RedirectToAction("Index", "Products");
            }
            if (!ModelState.IsValid)
            {
                ViewData["BlogId"] = blog.Id;
                ViewData["ImageFileName"] = blog.ImageFileName;
                ViewData["CreatedAt"] = blog.PublishedDate.ToString("MM/dd/yyyy");
                //var authorName = (from b in context.blogs
                //                  join a in context.authors on b.AuthorId equals a.Id
                //                  where b.Id == id // Assuming blogId is the ID of the blog you're querying for
                //                  select a.Name).FirstOrDefault();
                ViewData["Author"] = query(id);
                return View(bdt);
            }

            string newFileName = blog.ImageFileName;
            if (bdt.ImageFile != null)
            {
                newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                newFileName += Path.GetExtension(bdt.ImageFile.FileName);
                string imageFullPath = env.WebRootPath + "/products/" + newFileName;
                using (var stream = System.IO.File.Create(imageFullPath))
                {
                    bdt.ImageFile.CopyTo(stream);
                }
                // delete the old image
                string oldImageFullPath = env.WebRootPath + "/blogs/" + blog.ImageFileName;
                System.IO.File.Delete(oldImageFullPath);
            }

            blog.Title = bdt.Title;
            blog.Description = bdt.Description;
            blog.Content = bdt.Content;
            blog.ImageFileName = newFileName;
            context.SaveChanges();
            return RedirectToAction("Index", "Blog");
        }

        public IActionResult Delete(int id)
        {
            var x = HttpContext.Session.GetInt32("ad_x");
            if (x == null || x == 0)
            {
                return RedirectToAction("Login");
            }
            var blog = context.blogs.Find(id);
            if (blog == null)
            {
                return RedirectToAction("Index", "Products");
            }
            string imageFullPath = env.WebRootPath + "/blogs/" + blog.ImageFileName; System.IO.File.Delete(imageFullPath);
            context.blogs.Remove(blog);
            context.SaveChanges(true);

            return RedirectToAction("Index", "Blog");
        }

        public IActionResult Detail(int id)
        {
            var blog = context.blogs.Find(id);
            if (blog == null)
            {
                return RedirectToAction("Index", "Products");
            }
            ViewData["Author"] = query(id);
            return View(blog);
        }
    }
}
