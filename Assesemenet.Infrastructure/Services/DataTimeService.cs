using Assesement.Application.Interfaces;

namespace Assesemenet.Infrastructure.Services
{
	public class DateTimeService : IDateTimeService
	{
		public DateTime NowUtc => DateTime.UtcNow;
	}
}
