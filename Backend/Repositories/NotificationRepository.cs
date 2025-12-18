using Entities;
using Repositories.Abstract;

namespace Repositories
{
	public class NotificationRepository : GenericRepository<Notification>, INotificationRepository
	{
		public NotificationRepository(AppDbContext context) : base(context) { }
	}
}
