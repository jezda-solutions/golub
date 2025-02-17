using Golub.Interfaces.Repositories;
using Golub.Responses.HandlerResponse.EmailProviderResponse;
using Golub.Result;

namespace Golub.Handlers.EmailProvider
{
    public class GetEmailProviderByNameHandler(IEmailProviderRepository emailProviderRepository)
    {
        private readonly IEmailProviderRepository _emailProviderRepository
            = emailProviderRepository ?? throw new ArgumentNullException(nameof(emailProviderRepository));

        public async Task<IResult<GetEmailProviderByNameResponse>> Handle(string providerName)
        {
            var emailProvider = await _emailProviderRepository.GetFirstAsync(x => x.Name == providerName && x.IsActive);

            if (emailProvider == null)
            {
                return Result<GetEmailProviderByNameResponse>.Fail("Email provider not found");
            }

            var data = new GetEmailProviderByNameResponse
            {
                Id = emailProvider.Id,
                Name = emailProvider.Name,
                IsActive = emailProvider.IsActive,
                Configuraiton = emailProvider.Configuration,
                FreePlanQty = emailProvider.FreePlanQty,
                RemainingQty = emailProvider.RemainingQty,
                Period = emailProvider.Period
            };

            return Result<GetEmailProviderByNameResponse>.Success(data);
        }
    }
}
