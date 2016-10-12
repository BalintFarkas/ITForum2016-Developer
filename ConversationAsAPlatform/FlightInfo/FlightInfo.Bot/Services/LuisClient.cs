﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace FlightInfo.Bot.Services
{
    public static class LuisClient
    {
        public static async Task<LuisMessage> ParseMessage(string userMessage)
        {
            string escapedMessage = Uri.EscapeDataString(userMessage);
            using (var client = new HttpClient())
            {
                string uri = "https://api.projectoxford.ai/luis/v1/application?id=85d056d1-fb42-4dc4-b974-e4a1f3b0f5e7&subscription-key=95ddeffc516c4f6394606f98b8fa096a&q=" + escapedMessage;
                HttpResponseMessage luisMessage = await client.GetAsync(uri);
                if (luisMessage.IsSuccessStatusCode)
                {
                    var jsonResponse = await luisMessage.Content.ReadAsStringAsync();
                    var responseMessage = JsonConvert.DeserializeObject<LuisMessage>(jsonResponse);
                    return responseMessage;
                }
            }
            return null;
        }
    }

    public class LuisMessage
    {
        public string query { get; set; }
        public Intent[] intents { get; set; }
        public Entity[] entities { get; set; }
    }

    public class Intent
    {
        public string intent { get; set; }
        public float score { get; set; }
    }

    public class Entity
    {
        public string entity { get; set; }
        public string type { get; set; }
        public int startIndex { get; set; }
        public int endIndex { get; set; }
        public float score { get; set; }
    }
}