using System;
using System.Collections.Generic;
using System.Linq;
using Marten.AspNetCore.Playground.Model;
using Microsoft.AspNetCore.Mvc;

namespace Marten.AspNetCore.Playground.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class DummyDocumentSessionController : ControllerBase
	{
		private readonly IDocumentSession session;

		public DummyDocumentSessionController(IDocumentSession session)
		{
			this.session = session;
		}

		[HttpGet]
		public ActionResult<IEnumerable<Dummy>> Get()
		{
			return session.Query<Dummy>().Where(x => x.Value != null).ToList();
		}

		[HttpPost]
		public void Post([FromBody] string value)
		{
			var o = new Dummy { Value = value };
			session.Insert(o);			
		}
	}
}