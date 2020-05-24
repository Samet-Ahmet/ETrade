using System;
using System.Collections.Generic;
using System.Text;
using Entities.Concrete;

namespace Business.Constants
{
    public static class Messages
    {
        public static string UserAlreadyExists = "Bu kullanıcı zaten mevcut";
        public static string UserNotFound = "Kullanıcı bulunamadı!";
        public static string PasswordError = "Şifre Hatalı!";
        public static string Error = "Bozuk";
        public static string MustBeFilled = "Zorunlu alan *";
        public static string InvalidEmail = "Geçerli bir e-posta adresi giriniz *";
        public static string ErrorWhileAddingEntity = "Veri tabanına eklerken bir hata oluştu.";
        public static string PasswordConfirmError = "Şifreler uyuşmuyor, lütfen tekrar deneyiniz.";
        public static string SamePassword = "Yeni şifreniz eskisi ile aynı olamaz.";

        public static string OldPasswordIncorrect =
            "Girmiş olduğunuz şifre, kayıtlarımızdaki şifre ile eşleşmiyor. Lütfen eski şifrenizi doğru girdiğinizden emin olunuz.";

        public static string PasswordChanged = "Şifreniz başarılı bir şekilde değiştirildi.";
        public static string ErrorWhileUpdatingEntity = "Veri tabanında güncelleme yaparken bir hata oluştu.";
        public static string IncorrectEmailOrPassword = "E-posta adresi veya şifre yanlış.";
        public static string UserUpdatedSuccessfully = "Bilgileriniz başarıyla güncellendi.";
    }
}
