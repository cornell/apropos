using Apropos.Domain;
using Xunit;

namespace Apropos.Test.Domain
{
    public class FormationParserTest
    {
        [Fact]
        public void GetMetadonneesTest()
        {
            string contenu = @"---
titre: Le bilan et la rééducation vocale - le timbre en question  
sous-titre:  Niveau 1
axe: formation
layout: post.html
ville: Pontivy
departement: 56
animation:
    - Kristell Bourdin, orthophoniste
    - Natacha Roginski, orthophoniste
date: 2015-11-13
dateAffichage: 11-12 mars 2016
organisateur: organisateur-kb
financement:
    - dpc
    - horsDpc
    - salarie
# infos pour les contrats de formation (pdf)
tarif-adherent-salarie: 300
tarif-non-adherent-salarie: 350
tarif-adherent-liberal: 250
tarif-non-adherent-liberal: 300
ogdpc-reference: 32621500013 session 1
duree: 2 jours en présentiel avec le formateur
datePdf: vendredi 11 et samedi 12 mars 2016
horaire: 9h à 17h30 (repas inclus)
lieu: Auberge de jeunesse - 5 rue de Kerbriant - 29000 Brest
effectif: 20
# photos de suite au déroulement de la formation
photos:
    - 01-formation.jpg
    - 02-formation.jpg
    - 03-formation.jpg
    - 04-formation.jpg          
---
lorem ipsum";

            var sut = new FormationParser(contenu);
            //var s = sut.GetMetadonnee();
        }
    }
}
