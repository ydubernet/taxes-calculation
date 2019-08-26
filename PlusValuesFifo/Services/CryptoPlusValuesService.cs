using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using PlusValuesFifo.Models;

namespace PlusValuesFifo.Services
{
    public class CryptoPlusValuesService : IPlusValuesService
    {
        private readonly ILogger<CryptoPlusValuesService> _logger;

        public CryptoPlusValuesService(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<CryptoPlusValuesService>();
        }

        // https://www.legifrance.gouv.fr/affichCodeArticle.do?cidTexte=LEGITEXT000006069577&idArticle=LEGIARTI000037943236&dateTexte=&categorieLien=cid
        public IList<OutputEvent> ComputePlusValues(IEnumerable<IEvent> events)
        {
            // prix de cession = montant en legal currency cédé
            // PV = prix de cession - prix d'acquisition du porfeteuille d'actifs numériques * prix de cession / valeur du portefeuille juste avant la cession
            // prix d'acquisition = somme des prix effectivement acquittés en monnaie ayant cours légal avant la cession et de la valeur de chacun des services et des biens,
            // autres que actifs numériques faisant l'objet du sursis d'imposition de 305€, tout en incluant au débit les frais d'acquisition.
            // --> Si acquisition à titre gratuit (fork, etc...), le prix d'acquisition est la valeur légale de l'actif au moment de l'entrée dans le patrimoine de l'utilisateur
            // --> On retire du prix d'acquisition les valeurs des portions d'assets qui ont fait l'objet du sursis d'imposition de 305€
            // *** On retire du prix d'acquisition l'ensemble des prix de cessions antérieurement réalisés (hors échanges ayant bénéficiés du sursis d'imposition de 305€)
            // Rappel : B. – Les personnes réalisant des cessions dont la somme des prix, tels que définis au A du III, n'excède pas 305 € au cours de l'année d'imposition hors opérations mentionnées au A du présent II, sont exonérées.
            //             --> Cela veut dire qu'il faut retirer 305€ des prix de cession par an



            // Nécessite de connaître :
            // 1. Prix de cession
            // 2. Prix d'acquisition du portefeuille d'actifs numériques
            // 3. Valeur globale du portefeuille lors de la cession
            // 4. la somme des valeurs déjà cédées

            // Les frais sont déductibles du prix de cession




            throw new NotImplementedException();
        }
    }
}
