﻿using Golub.Entities;
using Golub.Entities.ProviderConfiguration;
using Golub.Requests;
using Golub.Responses;

namespace Golub.Services.Interfaces
{
    public interface IEmailProvider
    {
        /// <summary>
        /// Email provider name for example: SendGrid, Mandrill, etc.
        /// </summary>
        string ProviderName { get; }

        /// <summary>
        /// Gets the priority level of the item.
        /// </summary>
        int Priority { get; }

        /// <summary>
        /// Method for sending emails
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<IEmailResponse> SendEmailAsync(SendEmailRequest request, EmailProvider provider, BaseEmailProviderConfiguration configuration);
    }
}
