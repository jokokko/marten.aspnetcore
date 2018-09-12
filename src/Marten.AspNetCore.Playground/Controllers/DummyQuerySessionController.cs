using System.Collections.Generic;
using System.Linq;
using Marten.AspNetCore.Playground.Model;
using Microsoft.AspNetCore.Mvc;

namespace Marten.AspNetCore.Playground.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class DummyQuerySessionController : ControllerBase
	{
		private readonly IQuerySession session;

		public DummyQuerySessionController(IQuerySession session)
		{
			this.session = session;
		}

		[HttpGet]
		public ActionResult<IEnumerable<Dummy>> Get()
		{
			return session.Query<Dummy>().Where(x => x.Value != null).ToList();
		}
	}
}