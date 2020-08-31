using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;
using Ajka.BL.Services.Base.Interfaces;
using Ajka.BL.Services.Order.Interfaces;
using RazorLight;
using Microsoft.AspNetCore.Hosting;
using Ajka.BL.Models.Order;
using Ajka.Common.Helpers;
using Microsoft.Extensions.Options;
using Ajka.Common.Constants.Service;

namespace Ajka.BL.Services.Order
{
    public class OrderEmailService : IOrderEmailService, IAjkaShopService
    {
        private readonly IWebHostEnvironment _environment;
        private readonly AppSettings _appSettings;

        public OrderEmailService(IWebHostEnvironment environment,
                                 IOptions<AppSettings> appSettings)
        {
            _environment = environment;
            _appSettings = appSettings.Value;
        }

        public async Task SendAsync(string customersEmail, string emailBody, bool isRequestedCopyOfOrder, CancellationToken cancellationToken)
        {
            MailMessage message = new MailMessage();
            SmtpClient smtp = new SmtpClient();
            message.From = new MailAddress(_appSettings.MailCredentialsName, "Ajčin obchůdek");
            message.To.Add(new MailAddress(_appSettings.MailAddress));
            message.To.Add(new MailAddress(OrderEmailConstants.administratorEmail));
            if (isRequestedCopyOfOrder)
            {
                message.To.Add(new MailAddress(customersEmail));
            }
            message.Subject = $"Ajčin obchůdek - informace o objednávce";
            message.IsBodyHtml = true;
            message.Body = emailBody;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential
            {
                UserName = @_appSettings.MailCredentialsName,
                Password = _appSettings.MailCredentialsPassword
            };
            smtp.Port = Convert.ToInt32(_appSettings.SmtpPort);
            smtp.Host = _appSettings.SmtpHost;
            smtp.EnableSsl = true;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            await smtp.SendMailAsync(message);
        }

        public async Task<string> RenderViewToStringAsync(IList<OrderBasketItemDto> itemCardsInBasket, string email, string orderNumber, CancellationToken cancellationToken)
        {
            var engine = new RazorLightEngineBuilder()
                .UseFileSystemProject(_environment.ContentRootPath)
                .UseMemoryCachingProvider()
                .Build();

            var model = new OrderEmailSummaryModel {
                Email = email,
                OrderNumber = orderNumber,
                OrderBasketItems = itemCardsInBasket
            };
            return await engine.CompileRenderAsync("Views/OrderEmailTemplate.cshtml", model).ConfigureAwait(false);
        }
    }
}
