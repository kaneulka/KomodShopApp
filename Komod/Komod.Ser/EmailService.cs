using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Komod.Ser
{
    public class EmailService
    {
        public async Task SendEmailAsync(string email, string subject, string message, string title = "Администрация сайта Набитый комод", string emailFrom = "info@komod-tlt.ru")
        {
            var header = $"<table cellspacing='50' width='100%' align='center'>" +
                                $"<tbody>" +
                                    $"<tr>" +
                                        $"<td style='PADDING-BOTTOM:0px;PADDING-TOP:0px;PADDING-LEFT:0px;MARGIN:0px;PADDING-RIGHT:0px;' bgcolor='#ffffff' align='center'>" +
                                            $"<table style='BORDER-TOP:#707070 1px solid;BORDER-RIGHT:#707070 1px solid;BACKGROUND:#f5f5f5;BORDER-BOTTOM:#707070 1px solid;border-radius:25px;MARGIN:0px 25px;BORDER-LEFT:#707070 1px solid;' cellspacing='0' cellpadding='0' width='900' align='center' border='0'>" +
                                                $"<tbody>" +
                                                    $"<tr>" +
                                                        $"<td style='BORDER-COLLAPSE:collapse;BACKGROUND:#0BDFCA;border-top-left-radius:25px;border-top-right-radius:25px;' align='center'>" +
                                                            $"<table cellspacing='0' cellpadding='0' width='820' align='center' border='0'>" +
                                                                $"<tbody>" +
                                                                    $"<tr>" +
                                                                        $"<td style='PADDING-BOTTOM:22px;PADDING-TOP:22px;PADDING-LEFT:0px;PADDING-RIGHT:0px;'>" +
                                                                        $"<span style='FONT-SIZE:32px;FONT-WEIGTH:700;FONT-FAMILY:Trebuchet MS;COLOR:#ffffff!important;'>Набитый комод</span>" +
                                                                        $"</td>" +
                                                                        $"<td style='FONT-SIZE:25px;FONT-FAMILY:Trebuchet MS;WIDTH:204px;COLOR:#ffffff!important;PADDING-BOTTOM:33px;TEXT-ALIGN:center;PADDING-TOP:34px;PADDING-LEFT:300px;PADDING-RIGHT:0px;'>" +
                                                                            $"8 (937) 202-09-93" +
                                                                        $"</td>" +
                                                                    $"</tr>" +
                                                                $"</tbody>" +
                                                            $"</table>" +
                                                        $"</td>" +
                                                    $"</tr>";
                                                    
            var footer =                            $"<tr>" +
                                                       $"<td style='BORDER-COLLAPSE:collapse;BACKGROUND:#0BDFCA;border-bottom-left-radius:25px;border-bottom-right-radius:25px;' align='center'>" +
                                                           $"<table cellspacing='0' cellpadding='0' width='820' align='center' border='0'>" +
                                                               $"<tbody>" +
                                                                   $"<tr>" +
                                                                       $"<td style='PADDING-BOTTOM:38px;PADDING-TOP:38px;PADDING-LEFT:0px;PADDING-RIGHT:0px;COLOR:#ffffff!important;'>" +
                                                                           $"© 2018 - " + DateTime.Now.Year.ToString() + " г. Набитый комод." +
                                                                       $"</td>" +
                                                                       $"<td style='FONT-SIZE:18px;FONT-FAMILY:Trebuchet MS;WIDTH:430px;COLOR:#ffffff!important;PADDING-BOTTOM:16px;TEXT-ALIGN:center;PADDING-TOP:16px;PADDING-LEFT:150px;PADDING-RIGHT:0px;text-align:left;'>" +
                                                                           $"Самарская обл., г. Тольятти, ул. Ворошилова, 24<br />Nabitiy.Komod.tlt@yandex.ru<br />8 (937) 202-09-93" +
                                                                       $"</td>" +
                                                                   $"</tr>" +
                                                               $"</tbody>" +
                                                           $"</table>" +
                                                       $"</td>" +
                                                   $"</tr>" +
                                               $"</tbody>" +
                                           $"</table>" +
                                        $"</td>" +
                                     $"</tr>" +
                                  $"<tbody>" +
                               $"</table>";
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress(title, emailFrom));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = header + message + footer
            };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync("smtp.yandex.ru", 465, true);//mail.komod-tlt.ru
                await client.AuthenticateAsync("info@komod-tlt.ru", "mkddquxdjlbjblhe");
                //client.ServerCertificateValidationCallback = (s, c, h, e) => true;//Исправить, не рекомендуют
                //await client.
                await client.SendAsync(emailMessage);

                await client.DisconnectAsync(true);
            }
        }
    }
}
