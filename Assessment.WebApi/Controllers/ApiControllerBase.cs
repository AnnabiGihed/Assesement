using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Assessment.WebApi.Controllers
{
	[ApiController]
	[Route("api/[controller]/[action]")]
	public abstract class ApiControllerBase : ControllerBase
	{ 
	}
}
