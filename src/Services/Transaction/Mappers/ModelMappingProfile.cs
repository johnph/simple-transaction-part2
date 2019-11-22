namespace Transaction.WebApi.Mappers
{
    using AutoMapper;
    using Transaction.Framework.Domain;
    using Transaction.WebApi.Models;

    public class ModelMappingProfile : Profile
    {
        public ModelMappingProfile()
        {
            CreateMap<TransactionModel, AccountTransaction>()
                .ForMember(dest => dest.Date, opt => opt.MapFrom(o => o.Date))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(o => o.Description))
                 .AfterMap<SetIdentityAction>()
                 .ForAllOtherMembers(opts => opts.Ignore());
               
            CreateMap<TransactionResult, TransactionResultModel>()
                .ForMember(dest => dest.Balance, opt => opt.MapFrom(o => o.Balance.Amount.ToString("N")))
                .ForMember(dest => dest.Currency, opt => opt.MapFrom(o => o.Balance.Currency.ToString()));

            CreateMap<StatementTransaction, StatementTransactionModel>()
                .ForMember(dest => dest.Amount, opt => opt.MapFrom(o => o.Amount.Amount.ToString("N")))
                .ForMember(dest => dest.CurrentBalance, opt => opt.MapFrom(o => o.CurrentBalance.Amount.ToString("N")));

            CreateMap<AccountStatement, StatementResultModel>()
                .ForMember(dest => dest.Date, opt => opt.MapFrom(o => o.Date.StartDate.ToString("MMM-yyyy")));
        }
    }
}
