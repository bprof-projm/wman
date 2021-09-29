using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wman.Data;
using Wman.Data.DB_Models;
using Wman.Logic.Classes;
using Wman.Repository;

namespace Wman.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DemoController : ControllerBase
    {
        private wmanDb db;
        private debugRepo repo;
        private DebugLogic logic;
        public DemoController()
        {
            this.db = new wmanDb();
            this.repo = new debugRepo(db);
            this.logic = new DebugLogic(repo);
        }
        /// <summary>
        /// HTTPGET test method
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public List<WorkEvent> HelloWorld()
        {
            this.logic.testadd();
            return this.logic.testlist();
        }
        /// <summary>
        /// HTTPPost test method
        /// </summary>
        /// <returns>OK</returns>
        [HttpPost]
        public ActionResult test()
        {
            return Ok();
        }
    }
}
