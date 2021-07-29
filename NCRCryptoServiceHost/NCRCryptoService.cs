using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

using INCRCryptoService;
using Newtonsoft.Json;

namespace NCRCryptoServiceHost
{
    [Serializable]
    public class NCRCryptoService : INCRCryptoService.INCRCryptoService
    {

        Dictionary<string, UserCryptoAccountInfo> users = new Dictionary<string, UserCryptoAccountInfo>();

        UserCryptoAccountInfo currentUser = null;

        public Status RegisterUser(string cardNumber, string apiKey, string secretKey)
        {
            Status retVal = Status.Failed;

            if (!users.ContainsKey(cardNumber))
            {
                UserCryptoAccountInfo user = new UserCryptoAccountInfo();
                user.CardNumber = cardNumber;
                user.HashedApiKey = Utilities.EncryptIt(apiKey);
                user.HashedSecretKey = Utilities.EncryptIt(secretKey);
                users.Add(cardNumber, user);

                currentUser = user;
                retVal = Status.Success;
            }
            else
            {
                // do nothig 
                retVal = Status.Failed;
            }

            return retVal;
        }

        public string ConnectUser(string userIdentification)
        {
            string output = string.Empty;

            currentUser = users[userIdentification];

            if(currentUser.CurrencyHoldings!=null)
            {
              IDictionary<string,float> dict = currentUser.CurrencyHoldings;
              output =  JsonConvert.SerializeObject(dict);
            }
            return output;
        }

        public void Close()
        {
            currentUser.SaveDate();
            
            currentUser = null;
        }

        public Status BuyCrypto(string coinId, float amount)
        {
            Utilities.AddHoldings(currentUser, coinId, amount);
            return Status.Success; ;
        }

        public Status SellCrypto(string coinId, float amount)
        {
            Utilities.RemoveHoldings(currentUser, coinId, amount);
            return Status.Success;
        }

        public decimal GetCryptoPrice(string coinId)
        {
            string json;

            using (var web = new System.Net.WebClient())
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                var url = @"https://api.coindesk.com/v1/bpi/currentprice.json";
                json = web.DownloadString(url);
            }

            dynamic obj = JsonConvert.DeserializeObject(json);
            var currentPrice = Convert.ToDecimal(obj.bpi.USD.rate.Value);

            return currentPrice;
        }
    }

    //[DataContract]
    //public class UserCryptoAccountInfo : IUserCryptoAccountInfo
    //{
      
    //    IDictionary<string, decimal> currencyHoldings = new Dictionary<string, decimal>();

    //    //public int this[int index] 
    //    //{ get;set; }
    //    [DataMember]
    //    public IDictionary<string, decimal> CurrencyHoldings
    //    {
    //        get { return currencyHoldings; }
    //    }

    //    [DataMember]
    //    public string CardNumber
    //    { get; set; }

    //    internal void AddHoldings(string coinID, decimal value)
    //    {
    //        if (!currencyHoldings.ContainsKey(coinID))
    //        {
    //            currencyHoldings[coinID] = value;
    //        }
    //        else
    //        {
    //            currencyHoldings[coinID] += value;
    //        }
    //    }
    //    internal void RemoveHoldings(string coinID, decimal value)
    //    {
    //        if (!currencyHoldings.ContainsKey(coinID))
    //        {
    //            //do nothing
    //        }
    //        else
    //        {
    //            currencyHoldings[coinID] -= value;
    //        }
    //    }
    //}
}
