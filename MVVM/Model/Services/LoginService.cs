using QuanLiCoffeeShop.MVVM.View.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace QuanLiCoffeeShop.MVVM.Model.Services
{
    public class LoginService
    {
        private static LoginService _ins;

        public static LoginService Ins
        {
            get
            {
                if (_ins == null)
                    _ins = new LoginService();
                return _ins;
            }
            set { _ins = value; }
        }
        public async Task sendEmail(string email, string pass, string username)
        {
            try
            {
                var smtpClient = new SmtpClient("smtp.gmail.com");
                smtpClient.EnableSsl = true;
                smtpClient.Port = 587;
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential("coffeeshophello@gmail.com", "klciusqavysaiorf");

                MailMessage mail = new MailMessage();
                mail.IsBodyHtml = true;
                mail.From = new MailAddress("coffeeshophello@gmail.com");
                mail.To.Add(email);
                mail.Subject = "Coffee Shop xin thông báo";
                mail.IsBodyHtml = false;
                mail.Body = "Tên đăng nhập: " + username + "\n" + "Mật khẩu mới: " + pass;
                await smtpClient.SendMailAsync(mail);
                MessageBoxCustom.Show(MessageBoxCustom.Success, "Đã gửi email thành công");
            }
            catch 
            {
                MessageBoxCustom.Show(MessageBoxCustom.Error, "Có lỗi xảy ra, vui lòng nhập lại!");
            }
        }
    }
}
