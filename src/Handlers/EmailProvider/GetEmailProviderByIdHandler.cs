using Golub.Interfaces.Repositories;
using Golub.Responses.HandlerResponse.EmailProviderResponse;
using Golub.Result;

namespace Golub.Handlers.EmailProvider
{
    public class GetEmailProviderByIdHandler(IEmailProviderRepository emailProviderRepository)
    {
        private readonly IEmailProviderRepository _emailProviderRepository
            = emailProviderRepository ?? throw new ArgumentNullException(nameof(emailProviderRepository));

        public async Task<IResult<GetEmailProviderByIdQueryResponse>> Handle(Guid providerId)
        {
            var emailProvider = await _emailProviderRepository.GetFirstAsync(x => x.IsActive && x.Id == providerId);

            var data = new GetEmailProviderByIdQueryResponse
            {
                Id = emailProvider.Id,
                Name = emailProvider.Name,
                IsActive = emailProvider.IsActive,
                Configuration = emailProvider.Configuration,
                FreePlanQty = emailProvider.FreePlanQty,
                RemainingQty = emailProvider.RemainingQty,
                Period = emailProvider.Period
            };

            return Result<GetEmailProviderByIdQueryResponse>.Success(data);
        }
    }
}
