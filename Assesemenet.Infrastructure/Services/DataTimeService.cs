using Assessment.Application.Interfaces;

namespace Assessment.Infrastructure.Services
{
	public class DateTimeService : IDateTimeService
	{
		public DateTime NowUtc => DateTime.UtcNow;
	}
}
