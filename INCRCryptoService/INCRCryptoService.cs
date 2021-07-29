
using System.ServiceModel;


namespace INCRCryptoService
{
    /// <summary>
    /// NCR crypto service or degital intarface can be used by the ATM applicaiton to register/connect 
    /// the current ATM user and perfrom transactions like sell users account crypto currency to encash for 
    /// a withdrawal or buy the crypto with convienience like a simple transfer.
    /// </summary>
    [ServiceContract]
    public interface INCRCryptoService
    {
        /// <summary>
        /// Connects the current ATM user to the crypto service.
        /// </summary>
        /// <param name="cardNumber">user card number</param>
        /// <returns></returns>
        [DataContractFormat]
        [OperationContract]
        string ConnectMeToCryptoWorld(string cardNumber);

        /// <summary>
        /// Closes the user connection
        /// </summary>
        [OperationContract]
        void ByeForNow();

        /// <summary>
        /// Registers a new user into the NCR/BANK digital crypto service.
        /// </summary>
        /// <remarks> Need to be called by the banks regiatration webpage.</remarks>
        /// <param name="cardNumber">users card number.</param>
        /// <param name="ApiKey">the apikey for the crypto account.</param>
        /// <param name="secretKey">the secretKey for the users crypto account.</param>
        /// <returns></returns>
        [OperationContract]
        Status NewToCryptoWorld(string cardNumber,string ApiKey, string secretKey);

        
        /// <summary>
        /// Get the current price of the crypto
        /// </summary>
        /// <param name="coinId">coin id.</param>
        /// <returns></returns>
        [OperationContract]
        double FetchCoinValue(string coinId);

        /// <summary>
        /// Buys the crypto currency for the current user.
        /// </summary>
        /// <param name="coinId">crypto currency id (ex : BTCUSD for bitcoin)</param>
        /// <param name="amount">crypto currency in equivalent amount</param>
        /// <returns>success/fail</returns>
        [OperationContract]
        Status GrabSomeCrypto(string coinId, double amount);

        /// <summary>
        /// Sells the crypto currency for the current user.
        /// </summary>
        /// <param name="coinId">crypto currency id (ex : BTCUSD for bitcoin)</param>
        /// <param name="amount">crypto currency in equivalent amount</param>
        /// <returns>success/fail</returns>
        [OperationContract]
        Status EncashCrypto(string coinId, double amount);
    }


    public enum Status
    {
       Success,
       Failed
    }
}
