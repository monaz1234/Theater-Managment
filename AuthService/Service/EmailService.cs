﻿using AuthService.Mail;
using AuthService.Service.Interface;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace AuthService.Service;

public class EmailService: IEmailService
{
    private readonly EmailSettings _emailSettings;
    
    public EmailService(IOptions<EmailSettings> emailSettings)
    {
        _emailSettings = emailSettings.Value;
    }
    
    public async Task SendEmailAsync(string toEmail, string subject, string body)
    {
        using (var smtpClient = new SmtpClient(_emailSettings.SmtpServer, _emailSettings.Port))
        {
            smtpClient.Credentials = new NetworkCredential(_emailSettings.SenderEmail, _emailSettings.Password);
            smtpClient.EnableSsl = true;

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_emailSettings.SenderEmail),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            mailMessage.To.Add(toEmail);

            await smtpClient.SendMailAsync(mailMessage);
        }
    }
}