using Golub.Interfaces.Repositories;
using Golub.Result;

namespace Golub.Handlers.EmailProvider
{
    public class SoftDeleteEmailProviderHandler(IEmailProviderRepository emailProviderRepository)
    {
        private readonly IEmailProviderRepository _emailProviderRepository
            = emailProviderRepository ?? throw new ArgumentNullException(nameof(emailProviderRepository));

        public async Task<IResult<Guid>> Handle(Guid emailProviderId)
        {
            var emailProvider = await _emailProviderRepository.GetById(emailProviderId);

            emailProvider.DeletedOnUtc = DateTimeOffset.UtcNow;

            await _emailProviderRepository.UpdateAsync(emailProvider);

            return Result<Guid>.Success(emailProvider.Id);
        }
    }
}
