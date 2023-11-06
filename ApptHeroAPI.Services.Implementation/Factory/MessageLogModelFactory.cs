using ApptHeroAPI.Services.Abstraction.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApptHeroAPI.Services.Implementation.Factory
{
    public interface IMessageLogModelFactory
    {
        MessageLogModel Create(long messageLogId, DateTime dateTimeMessageSent, long companyId, long personId, bool isEmail, string subject, string body, bool isAdminEmail, string personName);
    }

    public class MessageLogModelFactory : IMessageLogModelFactory
    {
        public MessageLogModel Create(long messageLogId, DateTime dateTimeMessageSent, long companyId, long personId, bool isEmail, string subject, string body, bool isAdminEmail, string personName)
        {
            return new MessageLogModel
            {
                MessageLogId = messageLogId,
                DateTimeMessageSent = dateTimeMessageSent,
                CompanyId = companyId,
                PersonId = personId,
                IsEmail = isEmail,
                Subject = subject,
                Body = body,
                IsAdminEmail = isAdminEmail,
                personName = personName
            };
        }
    }

}
