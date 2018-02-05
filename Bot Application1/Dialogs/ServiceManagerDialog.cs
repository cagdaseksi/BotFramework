using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Builder.Dialogs.Internals;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Linq;

namespace Bot_Application1
{
    [Serializable]
    public class ServiceManagerDialog : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(this.MessageReceivedAsync);
        }

        private async Task WelcomeMessageAsync(IDialogContext context)
        {
            var reply = context.MakeMessage();

            var options = new[]
            {
               "Görüntü ve Ses Problemi Yaşıyorum",
               "Şifre Değişikliği",
               "Hiçbiri",
            };

            reply.AddHeroCard(
                "Borusan Manheim Servis Masasına Hoşgeldiniz.",
                "Dinabot size yardım için burada.",
                options,
                new[] { "http://www.manheimturkiye.com/Images/Banner2.png?74d34eb9-c6d1-492d-ae21-c618f0b1f2a4" });

            await context.PostAsync(reply);

            context.Wait(this.OnOptionSelected);
        }

        private async Task OnOptionSelected(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;
            var activity = await result as Activity;

            var connector = new ConnectorClient(new Uri(message.ServiceUrl));

            if (message.Text == "Şifre Değişikliği")
            {
                var replyMessage = context.MakeMessage();

                Relpy(connector, activity, "Şifreler genellikle unutulur.");

                System.Threading.Thread.Sleep(2000);

                Relpy(connector, activity, "İş yoğunluğundan ve herşeye bir şifre istendiğinden bu durumu olağan karşılıyorum.");

                System.Threading.Thread.Sleep(4000);

                Relpy(connector, activity, "Aşağıdaki link ile şifreni yenileyebilirsin.");

                System.Threading.Thread.Sleep(2000);

                replyMessage.Text = "http://www.manheimturkiye.com/UyelikIslemleri/Sifrehatirlat.aspx";

                await context.PostAsync(replyMessage);

            }
            else if (message.Text == "Görüntü ve Ses Problemi Yaşıyorum")
            {
                var replyMessage = context.MakeMessage();

                Relpy(connector, activity, "Bir link paylaşacağım o link üzerinden sorunlarını çözebileceksin.");

                System.Threading.Thread.Sleep(3000);

                Relpy(connector, activity, "2 saniye daha kaldı.");

                System.Threading.Thread.Sleep(2000);

                Relpy(connector, activity, "Aşağıdaki linke tıklayınız.");

                System.Threading.Thread.Sleep(2000);

                replyMessage.Text = "http://www.manheimturkiye.com/Hizmetler/Images/SubTopic/HizmetTanim_2/manheim_extension.pdf";

                await context.PostAsync(replyMessage);

            }
            else if (message.Text == "OnlineAlSat ile ilgili")
            {
                var replyMessage = context.MakeMessage();

                Relpy(connector, activity, "Az kaldı. Adresi gönderiyorum.");

                System.Threading.Thread.Sleep(2000);

                replyMessage.Text = "Bu linki takip ederek onlinealsat'a ulaşabilirsiniz. http://onlinealsat.com";

                await context.PostAsync(replyMessage);

            }
            else if (message.Text == "Fiziki açık arttırma ile ilgili")
            {
                var replyMessage = context.MakeMessage();

                Relpy(connector, activity, "Az kaldı. Adresi gönderiyorum.");

                System.Threading.Thread.Sleep(2000);

                replyMessage.Text = "Bu linki takip ederek manheimturkiye'ye ulaşabilirsiniz. http://www.manheimturkiye.com/";

                await context.PostAsync(replyMessage);

                System.Threading.Thread.Sleep(2000);

                var reply = context.MakeMessage();

                var options = new[]
                {
                   "Hala sorun devam ediyor",
                   "Sorun devam etmiyor."
                };

                reply.AddHeroCard(
                    "Sorunu anlamaya çalışıyorum.",
                    "Sorunu kaydettim.En kısa zamanda size dönüş yapacağım.",
                    options,
                    new[] { "http://www.manheimturkiye.com/Images/Banner2.png?74d34eb9-c6d1-492d-ae21-c618f0b1f2a4" });

                await context.PostAsync(reply);

                //context.Wait(this.MessageReceivedAsync);

            }
            else if (message.Text == "Sorun devam etmiyor.")
            {
                var replyMessage = context.MakeMessage();

                Relpy(connector, activity, "O zaman çok teşekkürler.İyi günler.");

                await context.PostAsync(replyMessage);

            }
            else if (message.Text == "Hala sorun devam ediyor")
            {
                var replyMessage = context.MakeMessage();

                Relpy(connector, activity, "Çare Hz Google");

                await context.PostAsync(replyMessage);

            }
            else if (message.Text == "Hiçbiri")
            {
                var reply = context.MakeMessage();

                Relpy(connector, activity, "Düşünüyorum. Lütfen bekleyin!");

                System.Threading.Thread.Sleep(2000);

                Relpy(connector, activity, "Biraz daha düşünüyorum.");

                System.Threading.Thread.Sleep(2000);

                Relpy(connector, activity, "Hiçbiri ne olabilir ki.En iyisi şöyle devam edelim.");

                System.Threading.Thread.Sleep(4000);

                var options = new[] { "Fiziki açık arttırma ile ilgili", "OnlineAlSat ile ilgili" };

                reply.AddHeroCard(
                    "Lütfen Talep Tipinizi Seçiniz",
                    "Fiziki ya da onlinealsat ile ilgili seçiminizi yapabilirsiniz.",
                    options,
                    new[] { "http://www.manheimturkiye.com/Images/Banner2.png?74d34eb9-c6d1-492d-ae21-c618f0b1f2a4" });

                await context.PostAsync(reply);

                //context.Wait(this.OnOptionSelected);
            }

        }

        public async Task ProcessSelectedOptionAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {

            var message = await result;
            var replyMessage = context.MakeMessage();

            replyMessage.Text = "http://onlinealsat.com/";

            await context.PostAsync(replyMessage);
            context.Wait(this.GetAttachment);

            context.Reset();
        }

        private async Task GetAttachment(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;

            if (message.Attachments != null && message.Attachments.Any())
            {
                var attachment = message.Attachments.First();
                using (HttpClient httpClient = new HttpClient())
                {
                    // Skype & MS Teams attachment URLs are secured by a JwtToken, so we need to pass the token from our bot.
                    if ((message.ChannelId.Equals("skype", StringComparison.InvariantCultureIgnoreCase) || message.ChannelId.Equals("msteams", StringComparison.InvariantCultureIgnoreCase))
                        && new Uri(attachment.ContentUrl).Host.EndsWith("skype.com"))
                    {
                        var token = await new MicrosoftAppCredentials().GetTokenAsync();
                        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    }

                    var responseMessage = await httpClient.GetAsync(attachment.ContentUrl);

                    var contentLenghtBytes = responseMessage.Content.Headers.ContentLength;

                    await context.PostAsync($"{attachment.Name} isimli formunuz işleme alınmıştır. Teşekkürler");
                }
            }
            else
            {
                await context.PostAsync("Dosyayı göndermeni bekliyorum.");
            }

            context.Wait(this.MessageReceivedAsync);
        }

        public virtual async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;

            //if (this.resumptionCookie == null)
            //{
            //    this.resumptionCookie = new ResumptionCookie(message);
            //}

            await this.WelcomeMessageAsync(context);
        }

        private async Task StartOverAsync(IDialogContext context, string text)
        {
            var message = context.MakeMessage();
            message.Text = text;
            await this.StartOverAsync(context, message);
        }

        private async Task StartOverAsync(IDialogContext context, IMessageActivity message)
        {
            //    await context.PostAsync(message);
            //    this.order = new Models.Order();
            //    await this.WelcomeMessageAsync(context);
        }

        private void Relpy(ConnectorClient connector, Activity activity, string message)
        {
            if (connector != null && activity != null && !string.IsNullOrEmpty(activity.Id))
            {
                var reply = activity.CreateReply(message);
                connector.Conversations.ReplyToActivity(reply);
            }
        }
    }
}
