using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static ef_core.Models.Model;

namespace ef_core.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        private readonly BloggingContext _context;
        public ValuesController(BloggingContext context)
        {
            _context = context;
        }

        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            var categories = _context.Categories    
                .OrderBy(c => EF.Property<DateTime>(c, "LastUpdated"))
                .ToList();
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
            Category category = new Category() 
            {
                Name = "Assessories"
            };
            _context.Entry(category).Property("LastUpdated").CurrentValue = DateTime.Now;
            _context.Categories.Add(category);
            _context.SaveChanges();
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
