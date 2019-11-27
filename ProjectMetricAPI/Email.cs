using System.Net;
using System.Net.Mail;

namespace ProjectMetricAPI
{
    public static class Email
    {
        public static void Enviar(int id)
        {
            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress("engsoft12345@gmail.com");
                mail.To.Add("luciano.fagundes@softplan.com.br");
                mail.Subject = "[ERROR API] Verificar aplicação de API";
                mail.Body = "<h4>O idGit " + id + " não foi encontrado na tabela Projects </h4>";
                mail.IsBodyHtml = true;

                using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                {
                    smtp.Credentials = new NetworkCredential("engsoft12345@gmail.com", "agesune1");
                    smtp.EnableSsl = true;
                    smtp.Send(mail);
                }
            }
        }
    }
}
