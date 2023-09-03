namespace Assessment.Shared.Interfaces
{
	public interface IResult<T>
	{
		T Data { get; set; }
		int Code { get; set; }
		bool Succeeded { get; set; }
		Exception Exception { get; set; }
		List<string> Messages { get; set; }
	}
}
