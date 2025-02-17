using Golub.Interfaces.Repositories;
using Golub.Result;

namespace Golub.Handlers.EmailProvider
{
    public class UpdateEmailProviderIsActiveCommand
    {
        public Guid Id { get; set; }

        public bool IsActive { get; set; }
    }

    public class UpdateEmailProviderIsActiveHandler(IEmailProviderRepository emailProviderRepository)
    {
        private readonly IEmailProviderRepository _emailProviderRepository
            = emailProviderRepository ?? throw new ArgumentNullException(nameof(emailProviderRepository));

        public async Task<IResult<Guid>> Handle(UpdateEmailProviderIsActiveCommand request)
        {
            var emailProvider = await _emailProviderRepository.GetById(request.Id);

            emailProvider.IsActive = request.IsActive;

            await _emailProviderRepository.UpdateAsync(emailProvider);

            return Result<Guid>.Success(emailProvider.Id);
        }
    }
}
