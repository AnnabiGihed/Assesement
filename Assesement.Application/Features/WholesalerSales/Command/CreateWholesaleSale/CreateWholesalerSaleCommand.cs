using MediatR;
using AutoMapper;
using Assessment.Shared;
using Assessment.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Assessment.Application.Common.Mappings;
using Assessment.Application.Interfaces.Repositories;
using Assessment.Application.Features.WholesalerStocks.Command.UpdateWholesalerStock;

namespace Assessment.Application.Features.WholesalerSales.Command.CreateWholesaleSale
{
	public record CreateWholesalerSaleCommand : IRequest<Result<CreateWholesalerSaleDto>>, IMapFrom<WholesalerSale>
	{
		[Required(ErrorMessage = "IsQuote is required")]
		public bool IsQuote { get; set; }
        [DataType(DataType.Text)]
		[Required(ErrorMessage = "Client Name is required")]
		[StringLength(255, ErrorMessage = "Client Name Must be between 2 and 255 characters.", MinimumLength = 2)]
		public string ClientName { get; set; }

		[DataType(DataType.Text)]
		[Required(ErrorMessage = "Wholesaler Name is required")]
		[StringLength(255, ErrorMessage = "Wholesaler Name Must be between 2 and 255 characters.", MinimumLength = 2)]
		public string WholesalerName { get; set; }

		[Required(ErrorMessage = "OrderContent is required")]
		public List<KeyValuePair<string,int>> OrderContent { get; set; }
    }
	internal class CreateWholesalerSaleCommandHandler : IRequestHandler<CreateWholesalerSaleCommand, Result<CreateWholesalerSaleDto>>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		
		public CreateWholesalerSaleCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<Result<CreateWholesalerSaleDto>> Handle(CreateWholesalerSaleCommand command, CancellationToken cancellationToken)
		{
			#region First Validation for the order
			if(command.OrderContent.Count == 0)
				return await Result<CreateWholesalerSaleDto>.FailureAsync("The order cannot be empty");
			#endregion

			#region Retrive Wholesaler
			var Wholesaler = await _unitOfWork.Repository<Wholesaler>().Entities.FirstOrDefaultAsync(x => x.Name.ToLower() == command.WholesalerName.ToLower());

			if (Wholesaler == null)
				return await Result<CreateWholesalerSaleDto>.FailureAsync("Wholesaler Not Found.");
			#endregion

			#region Retrive Client
			var Client = await _unitOfWork.Repository<Client>().Entities.FirstOrDefaultAsync(x => x.Name.ToLower() == command.ClientName.ToLower());

			if (Client == null)
				return await Result<CreateWholesalerSaleDto>.FailureAsync("Client Not Found.");
			#endregion

			#region Sort out and organize orders
			int OrderTotalDrinks = 0;
			List<WholesalerSale> Sales = new List<WholesalerSale>();
			List<WholesalerStock> Stocks = new List<WholesalerStock>();
			CreateWholesalerSaleDto SalesDto = new CreateWholesalerSaleDto() { WholeSaler = Wholesaler.Name, Client = Client.Name, OrderContent = new Dictionary<string, int>()};
			SalesDto.TotalPrice = 0;

			foreach (var order in command.OrderContent)
			{
				var Beer = await _unitOfWork.Repository<Beer>().Entities.FirstOrDefaultAsync(x => x.Name.ToLower() == order.Key.ToLower());

				if (Beer == null)
					return await Result<CreateWholesalerSaleDto>.FailureAsync("Beer Not Found.");

				var WholeSalerStock = await _unitOfWork.Repository<WholesalerStock>().Entities.FirstOrDefaultAsync(x => x.BeerId == Beer.Id && x.WholesalerId == Wholesaler.Id);

				if (WholeSalerStock == null)
					return await Result<CreateWholesalerSaleDto>.FailureAsync("WholeSaler doesn't have stock of this beer.");
				
				if(WholeSalerStock.Count < order.Value)
					return await Result<CreateWholesalerSaleDto>.FailureAsync("Wholesaler doesn't have enough stock for this order");

				if (Sales.Count == 0)
				{
					SalesDto.OrderContent.Add(Beer.Name, order.Value);
					Sales.Add(new WholesalerSale() { Quantity = order.Value, ClientId = Client.Id, BeerId = Beer.Id, WholesalerId = Wholesaler.Id, UnitePrice = Beer.Price });
				}
				else if (Sales.Exists(x => x.BeerId == Beer.Id))
				{
					SalesDto.OrderContent[Beer.Name] = SalesDto.OrderContent[Beer.Name] + order.Value;
					Sales.FirstOrDefault(x => x.BeerId == Beer.Id).Quantity += order.Value;
				}

				WholeSalerStock.Count -= order.Value;
				Stocks.Add(WholeSalerStock);

				SalesDto.TotalPrice += order.Value * Beer.Price;
				OrderTotalDrinks += order.Value;
			}
			#endregion

			if(!command.IsQuote)
			{
				int NewId = 0;
				var res = await _unitOfWork.Repository<WholesalerSale>().Entities.CountAsync();

				if (res !=0)
					NewId = await _unitOfWork.Repository<WholesalerSale>().Entities.MaxAsync(wss => wss.Id) + 1;

				Sales.ForEach(x => x.Id = NewId);
				//Create Sale
				await _unitOfWork.Repository<WholesalerSale>().AddRangeAsync(Sales);
				Sales.ForEach(x => x.AddDomainEvent(new WholesaleSaleCreatedEvent(x)));

				await _unitOfWork.Save(cancellationToken);

				//Update Wholesaler Stock
				await _unitOfWork.Repository<WholesalerStock>().UpdateRangeAsync(Stocks);
				Stocks.ForEach(x => x.AddDomainEvent(new WholesalerStockUpdatedEvent(x)));

				await _unitOfWork.Save(cancellationToken);
			}


			return await Result<CreateWholesalerSaleDto>.SuccessAsync(SalesDto, "Wholesaler Sale Created.");
		}
	}
}
