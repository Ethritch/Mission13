using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Mission13.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Mission13.Controllers
{
    public class HomeController : Controller
    {
        //connect to the repository
        private IBowlersRepository _repo { get; set; }
        
        public HomeController(IBowlersRepository temp)
        {
            _repo = temp;
        }

        //display the needed info from the models for the index page
        public IActionResult Index(string teamName)
        {
            ViewBag.Teams = _repo.Teams.ToList();
            ViewBag.TeamName = teamName;
            var table = _repo.Bowlers
                .Where(x => x.Team.TeamName == teamName || teamName == null)
                .ToList();

            return View(table);
        }
        //get the needed info for the add functionality
        [HttpGet]
        public IActionResult Add()
        {
            ViewBag.Team = _repo.Teams.ToList();

            return View();
        }

        [HttpPost]
        public IActionResult Add(Bowler bowler)
        {
            if(ModelState.IsValid)
            {
                _repo.SaveBowler(bowler);
                return View("Confirmation", bowler);
            }
            else
            {
                ViewBag.Team = _repo.Teams.ToList();

                return View(bowler);
            }
           
        }
        //edit functionality
        [HttpGet]
        public IActionResult Edit (int bowlerid)
        {
            ViewBag.Team = _repo.Teams.ToList();

            var form = _repo.Bowlers.Single(x => x.BowlerID == bowlerid);

            return View("Add", form);
        }

        [HttpPost]
        public IActionResult Edit (Bowler bowler)
        {
            _repo.EditBowler(bowler);

            return RedirectToAction("Index");
        }
        //delete functionality
        [HttpGet]
        public IActionResult Delete (int bowlerid)
        {
            var form = _repo.Bowlers.Single(x => x.BowlerID == bowlerid);

            return View(form);
        }

        [HttpPost]
        public IActionResult Delete (Bowler bowler)
        {
            _repo.DeleteBowler(bowler);

            return RedirectToAction("Index");
        }
    }
}
