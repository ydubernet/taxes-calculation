using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PlusValuesFifo.Models;
using PlusValuesFifo.Models.Cryptos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace PlusValuesFifo.Services
{
    public class CryptoPlusValuesService : IPlusValuesService
    {
        private readonly ILogger<CryptoPlusValuesService> _logger;
        private readonly HttpClient _httpClient = new HttpClient();
        private readonly IDictionary<(long timestamp, string assetName), CryptoPriceModel> _cryptoAssetsPricesCache;


        public CryptoPlusValuesService(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<CryptoPlusValuesService>();
            _cryptoAssetsPricesCache = new Dictionary<(long, string), CryptoPriceModel>();
        }

        // https://www.legifrance.gouv.fr/affichCodeArticle.do?cidTexte=LEGITEXT000006069577&idArticle=LEGIARTI000037943236&dateTexte=&categorieLien=cid
        public IEnumerable<IOutputEvent> ComputePlusValues(IEnumerable<IInputEvent> events)
        {
            // prix de cession = montant en legal currency cédé
            // PV = prix de cession - prix d'acquisition du porfeteuille d'actifs numériques * prix de cession / valeur du portefeuille juste avant la cession
            // prix d'acquisition = somme des prix effectivement acquittés en monnaie ayant cours légal avant la cession et de la valeur de chacun des services et des biens,
            // autres que actifs numériques faisant l'objet du sursis d'imposition de 305€, tout en incluant au débit les frais d'acquisition.
            // --> Si acquisition à titre gratuit (fork, etc...), le prix d'acquisition est la valeur légale de l'actif au moment de l'entrée dans le patrimoine de l'utilisateur
            // --> On retire du prix d'acquisition les valeurs des portions d'assets qui ont fait l'objet du sursis d'imposition de 305€
            // *** On retire du prix d'acquisition l'ensemble des prix de cessions antérieurement réalisés (hors échanges ayant bénéficiés du sursis d'imposition de 305€)
            // Rappel : B. – Les personnes réalisant des cessions dont la somme des prix, tels que définis au A du III, n'excède pas 305 € au cours de l'année d'imposition hors opérations mentionnées au A du présent II, sont exonérées.
            // D'autre part, il y a un abattement de 305€ par an, mais cela ne rentre pas dans la formule de calcul des impots. C'est les impôts eux-mêmes qui prennent en compte cet abattement.


            // Nécessite de connaître :
            // 1. Prix de cession
            // 2. Prix d'acquisition du portefeuille d'actifs numériques
            // 3. Valeur globale du portefeuille lors de la cession
            // 4. la somme des valeurs déjà cédées

            // Les frais sont déductibles du prix de cession



            // Buy data
            List<IInputEvent> buyEvents = events.Where(e => e.ActionEvent == BuySell.Buy).OrderBy(e => e.Date).ToList();
            // Sell data
            List<IInputEvent> sellEvents = events.Where(e => e.ActionEvent == BuySell.Sell).OrderBy(e => e.Date).ToList();

            var outputs = new List<IOutputEvent>();

            foreach (var sellEvent in sellEvents)
            {



                // TODO : Take into account fees
                // TODO : Optim that
                var cessionPrice = sellEvent.Price * sellEvent.Amount;
                var totalAcquisitionPrice = buyEvents.Where(e => e.Date <= sellEvent.Date) // TODO : + price of assets which got for free in the wallet (forks, etc...)
                                                     .Sum(e => e.Amount * e.Price);
                var totalCessionPriceBeforeCurrentSell = sellEvents.Where(e => e.Date < sellEvent.Date)
                                                                   .Sum(e => e.Amount * e.Price);

                decimal currentSellEventAcquisitionPrice = totalAcquisitionPrice - totalCessionPriceBeforeCurrentSell;

                decimal totalPortfolioValue = 1m; // TODO : We need to have at any given time the total portfolio value.

                decimal pv = cessionPrice - currentSellEventAcquisitionPrice * cessionPrice / totalPortfolioValue;

                outputs.Add(new CryptoOutputEvent(/*0m, pv, sellEvent*/));
            }

            return outputs;
        }

        public async Task<CryptoPriceModel> GetCryptoPrice(string cryptoAssetName, DateTime dateTime)
        {
            var timestamp = dateTime.Ticks;
            var response = await _httpClient.GetAsync($"https://localhost:5000/api/binance/price?symbol={cryptoAssetName}&timestamp={timestamp}");

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Issue while contacting the Crypto Data service.");
                return null;
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            var cryptoPrice = JsonConvert.DeserializeObject<CryptoPriceModel>(responseContent);

            if (cryptoPrice == null)
            {
                _logger.LogError("Issue while deserializing Json Response from the Crypto Data service.");
                return null;
            }

            _cryptoAssetsPricesCache.Add((timestamp, cryptoAssetName), cryptoPrice);
            return cryptoPrice;
        }
    }
}
