using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace GC.Plugin.Messaging.Web.Controllers
{
	public class HomeController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}