using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Something.Application;
using Something.Persistence;
using Something.Security;

namespace Something.API.Controllers
{
    [Authorize]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly ISomethingUserManager userManager;
        private readonly ISomethingCreateInteractor createInteractor;
        private readonly ISomethingElseCreateInteractor createElseInteractor;
        private readonly ISomethingReadInteractor readInteractor;
        private readonly ISomethingElseReadInteractor readElseInteractor;

        public HomeController(ISomethingCreateInteractor createInteractor, ISomethingElseCreateInteractor createElseInteractor, ISomethingReadInteractor readInteractor, ISomethingElseReadInteractor readElseInteractor, ISomethingUserManager userManager)
        {
            this.createInteractor = createInteractor;
            this.createElseInteractor = createElseInteractor;
            this.readInteractor = readInteractor;
            this.readElseInteractor = readElseInteractor;
            this.userManager = userManager;
        }

        [AllowAnonymous]
        [Route("home/authenticate")]
        public ActionResult Authenticate()
        {
            var token = userManager.GetUserToken();
            return Ok(new { access_token = token});
        }

        [HttpPost]
        [Route("api/things")]
        public ActionResult Create([FromForm] string name)
        {
            if (name.Length < 1)
                return GetAll();

            createInteractor.CreateSomething(name);
            return GetAll();
        }

        [HttpGet]
        [Route("api/things")]
        public ActionResult GetList()
        {
            return GetAll();
        }

        [HttpPost]
        [Route("api/thingselse")]
        public ActionResult CreateElse([FromForm] string name, [FromForm] string[] othername)
        {
            if (name.Length < 1)
                return GetAllSomethingElseIncludeSomething();

            createElseInteractor.CreateSomethingElse(name, othername);
            return GetAllSomethingElseIncludeSomething();
        }

        [HttpGet]
        [Route("api/thingselse")]
        public ActionResult GetElseList()
        {
            return GetAllSomethingElseIncludeSomething();
        }
        private ActionResult GetAll()
        {
            var result = readInteractor.GetSomethingList();
            return Ok(result);
        }
        private ActionResult GetAllSomethingElseIncludeSomething()
        {
            var result = readElseInteractor.GetSomethingElseIncludingSomethingsList();
            return Ok(result);
        }
    }
}
