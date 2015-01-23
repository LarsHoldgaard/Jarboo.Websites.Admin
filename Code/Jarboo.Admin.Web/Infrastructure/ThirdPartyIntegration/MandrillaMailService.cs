using System;
using System.Collections.Generic;
using System.Configuration;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

using Newtonsoft.Json;

namespace Jarboo.Admin.Web.Infrastructure.ThirdPartyIntegration
{
    public class MandrillMailService
    {
        /*static readonly string MandrillKey = ConfigurationManager.AppSettings["MandrillKey"];
        static readonly string MandrillSendUrl = ConfigurationManager.AppSettings["MandrillSendUrl"];
        static readonly string _defaultEmailFrom = ConfigurationManager.AppSettings["defaultEmailFrom"];

        public string Create(NewMail newMail)
        {
            dynamic sendParams = new ExpandoObject();
            sendParams.key = MandrillKey;

            if (!string.IsNullOrEmpty(newMail.TemplateName))
            {
                sendParams.template_name = newMail.TemplateName;
                sendParams.template_content = new List<dynamic>();

                if (newMail.TemplateContent != null && newMail.TemplateContent.Any())
                {
                    foreach (var contentItem in newMail.TemplateContent)
                    {
                        dynamic content = new ExpandoObject();
                        content.name = contentItem.Item1;
                        content.content = contentItem.Item2;
                        sendParams.template_content.Add(content);
                    }
                }
            }

            sendParams.message = new ExpandoObject();
            sendParams.message.html = newMail.Body;
            sendParams.message.subject = newMail.Subject;
            sendParams.message.from_email = string.IsNullOrEmpty(newMail.MailFrom) ? _defaultEmailFrom : newMail.MailFrom;

            sendParams.message.to = new List<dynamic>();
            foreach (var mail in newMail.MailTo)
            {
                dynamic email = new ExpandoObject();
                email.email = mail;
                sendParams.message.to.Add(email);
            }

            MessageSendResult result = null;
            try
            {
                var httpResponse = SendHttpRequest(MandrillSendUrl, sendParams);
                if (httpResponse.StatusCode != HttpStatusCode.OK)
                {
                    throw new Exception("Http request failed. StatusCode:" + httpResponse.StatusCode);
                }

                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    result = JsonConvert.DeserializeObject<List<MessageSendResult>>(streamReader.ReadToEnd()).FirstOrDefault();
                    if (result.Status == "rejected" || result.Status == "invalid")
                    {
                        throw new Exception("Can not send email. The error is " + result.ErrorMessage);
                    }
                    return result.Id;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private HttpWebResponse SendHttpRequest(string url, dynamic sendParams)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.ContentType = "application/json; charset=utf-8";
            httpWebRequest.Method = "POST";
            httpWebRequest.Timeout = 2000;

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = JsonConvert.SerializeObject(sendParams);
                streamWriter.Write(json);
                streamWriter.Flush();
            }

            return (HttpWebResponse)httpWebRequest.GetResponse();

        }

        public NewMail GetMailById(string messageId)
        {
            dynamic sendParams = new ExpandoObject();
            sendParams.key = MandrillKey;
            sendParams.id = messageId;

            NewMail newEmail = new NewMail();
            try
            {
                var httpResponse = SendHttpRequest(MandrillSendUrl, sendParams);
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    MessageInfoResult result = JsonConvert.DeserializeObject<MessageInfoResult>(streamReader.ReadToEnd());
                    newEmail.MailProviderId = result.Id;
                    newEmail.Subject = result.Subject;
                    newEmail.MailFrom = result.SenderEmail;
                    newEmail.MailTo = new List<string>();
                    newEmail.MailTo.Add(result.RecepientEmail);
                    newEmail.DateSent = new DateTime(result.TimeStamp);

                    newEmail.OpensCount = result.OpensCount;
                    if (result.OpenDetails != null)
                    {
                        newEmail.Opens = result.OpenDetails.Select(r => new NewMailStatistic() { 
                            ActionTime = new DateTime(r.TimeStamp),
                            IP = r.IP,
                            Url = r.Url,
                            UserAgent = r.UserAgent,
                            Location = r.Location
                        }).ToList<NewMailStatistic>();
                    }

                    newEmail.ClicksCount = result.ClicksCount;
                    if (result.ClickDetails != null)
                    {
                        newEmail.Clicks = result.ClickDetails.Select(r => new NewMailStatistic()
                        {
                            ActionTime = new DateTime(r.TimeStamp),
                            IP = r.IP,
                            Url = r.Url,
                            UserAgent = r.UserAgent,
                            Location = r.Location
                        }).ToList<NewMailStatistic>();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return newEmail;
        }*/
    }

    public class MessageSendResult
    {
        /// <summary>
        /// Message ID
        /// </summary>
        [JsonProperty("_id")]
        public string Id { get; set; }

        /// <summary>
        /// Message status: "sent", "queued", "scheduled", "rejected", or "invalid"
        /// </summary>
        [JsonProperty("status")]
        public string Status { get; set; }

        public string ErrorMessage { get; set; }
    }

    public class MessageInfoResult
    {
        /// <summary>
        /// Message ID
        /// </summary>
        [JsonProperty("_id")]
        public string Id { get; set; }

        /// <summary>
        /// Message Status: sent, bounced, rejected
        /// </summary>
        [JsonProperty("state")]
        public string Status { get; set; }

        [JsonProperty("ts")]
        public int TimeStamp { get; set; }

        [JsonProperty("sender")]
        public string SenderEmail { get; set; }

        [JsonProperty("subject")]
        public string Subject { get; set; }

        [JsonProperty("email")]
        public string RecepientEmail { get; set; }

        [JsonProperty("opens")]
        public int OpensCount { get; set; }

        [JsonProperty("opens_detail")]
        public MessageStatisticInfo[] OpenDetails { get; set; }

        [JsonProperty("clicks")]
        public int ClicksCount { get; set; }

        [JsonProperty("clicks_detail")]
        public MessageStatisticInfo[] ClickDetails { get; set; }

        public string ErrorMessage { get; set; }
    }

    public class MessageStatisticInfo
    {
        [JsonProperty("ts")]
        public int TimeStamp {get;set;}
        [JsonProperty("url")]
        public string Url {get;set;}
        [JsonProperty("ip")]
        public string IP {get;set;}
        [JsonProperty("location")]
        public string Location {get;set;}
        [JsonProperty("ua")]
        public string UserAgent {get;set;}
    }
}