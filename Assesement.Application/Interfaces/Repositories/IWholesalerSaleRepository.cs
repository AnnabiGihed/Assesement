namespace Assessment.Application.Interfaces.Repositories
{
	public interface IWholesalerSaleRepository
	{
		Task<int> GetLastUsedIndex();
	}
}
