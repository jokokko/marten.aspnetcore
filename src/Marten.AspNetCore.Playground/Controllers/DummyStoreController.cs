using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Marten.AspNetCore.Playground.Model;
using Microsoft.AspNetCore.Mvc;

namespace Marten.AspNetCore.Playground.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class DummyStoreController : ControllerBase
	{
		private readonly IDocumentStore store;

		public DummyStoreController(IDocumentStore store)
		{
			this.store = store;
		}

		[HttpGet]
		public ActionResult<IEnumerable<Dummy>> Get()
		{
			using (var s = store.OpenSession())
			{
				return s.Query<Dummy>().Where(x => x.Flag).ToList();
			}
		}

		[HttpPost]
		public void Post([FromBody] string value)
		{
		}
	}
}
