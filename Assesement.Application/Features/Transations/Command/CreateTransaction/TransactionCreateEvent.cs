using Assessment.Domain.Common;
using Assessment.Domain.Entities;

namespace Assessment.Application.Features.Transations.Command.CreateTransaction
{
	public class TransactionCreateEvent : BaseEvent
	{
        public Transaction Transaction { get; set; }
        public TransactionCreateEvent(Transaction transaction)
        {
            Transaction = transaction;
        }
    }
}
