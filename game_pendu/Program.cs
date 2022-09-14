using System;
using System.Transactions;
using AsciiArt;

namespace game_pendu
{
    internal class Program
    {
        // fonction affiché mot 
        static void AfficherMot(string mot, List<char> lettres)
        {
            for (int i = 0; i < mot.Length; i++)
            {
                char lettre = mot[i];
                if (lettres.Contains(lettre))
                {
                    Console.Write(lettre+" ");
                }else
                {

                Console.Write("_ ");
                }
            }
            Console.WriteLine();
        }

        //fonction si le mot à été trouver 
        static bool MotTrouver(string mot, List<char> lettres)
        {
            foreach(var lettre in lettres)
            {
                mot = mot.Replace(lettre.ToString(), "");
            }

            if (mot == "")
            {
                return true;
            }
            return false;
        }

        //fonction demander une lettre 
        static char DemanderUneLettre(string message = "Donner une lettre ? ")
        {
            // variable de dapart
            string lettre = "";
            string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

            while (true)
            {
                Console.Write(message);
                lettre = Console.ReadLine();

                if (lettre == "")
                {
                    Console.WriteLine("ERREUR: la valeur ne peut être non vide!");
                    Console.WriteLine();

                    return DemanderUneLettre();
                }
                else if (lettre.Count() > 1)
                {
                    Console.WriteLine("ERREUR: la valeur ne peut contenir plus d'un carractéres!");
                    Console.WriteLine();

                    return DemanderUneLettre();
                }
                else if (!alphabet.Contains(lettre.ToUpper()))
                {
                    Console.WriteLine("ERREUR: la valeur ne peut contenir que une lettre entre A et Z! ");
                    Console.WriteLine();

                    return DemanderUneLettre();
                }else
                {
                    return lettre.ToUpper()[0];
                }

            }

            return lettre.ToUpper()[0];
        }

        // fonction demander mot 
        static void DemanderMot(string mot)
        {
            //contante de vie 
            const int NB_VIES = 6;
            int viesRestante = NB_VIES ;
            // variable des listes de lettre
            var goodLettre = new List<char>();
            var noLettre = new List<char>();


            while (true)
            {
                //affichage du art asccii
                Console.WriteLine(Ascii.PENDU[NB_VIES-viesRestante]);
                
                AfficherMot(mot, goodLettre);
                Console.WriteLine();
                char lettre = DemanderUneLettre();

                Console.Clear();
                if (mot.Contains(lettre))
                {
                    if (noLettre.Count() > 0)
                    {
                        Console.WriteLine($"Le mot ne contient pas la lettre {String.Join(", ", noLettre)}");
                    }
                    Console.WriteLine($"le mot contient bien la lettre {lettre}");
                    goodLettre.Add(lettre);
                    MotTrouver(mot, goodLettre);

                    //victoire 
                    if (MotTrouver(mot, goodLettre))
                    {
                        Console.Clear();
                        Console.WriteLine($"VICTOIRE !!! le mot est bien {mot}");
                        return;
                    }
                }
                else
                {
                    if (!noLettre.Contains(lettre))
                    {
                        noLettre.Add(lettre);
                        viesRestante--;
                    }
                    Console.WriteLine($"Le mot ne contient pas la lettre {String.Join(", ", noLettre)}");
                    Console.WriteLine($"Il vous reste {viesRestante}");

                    if (viesRestante == 0)
                    {
                        Console.WriteLine(Ascii.PENDU[NB_VIES - viesRestante]);
                        Console.WriteLine($"PERDU ! Le mot était {mot}");
                        break;
                    }
                }
                Console.WriteLine();
            }
        }

        //fonction de charger les mots 
        static string[] ChargerLesMots(string nameFichier)
        {
            try
            {
                return File.ReadAllLines(nameFichier);
            }catch (Exception ex)
            {
                Console.WriteLine($"ERREUR: probléme de l'ecture du {nameFichier}, {ex.Message}");
            }
            return null;
        }

        //fonction rejouer 
        static bool DemanderDeRejouer()
        {
            
            char reponse = DemanderUneLettre("Voulez vous rejouer (o/n):");
            
            if (reponse == 'O')
            {
                return true;
            }else if (reponse == 'N')
            {
                return false;
            }else
            {
                Console.WriteLine("ERREUR : vous devez repondre o ou n:");
                return DemanderDeRejouer();
            }
        }

        //fonction principal
        static void Main(string[] args)
        {
            var mots = ChargerLesMots("mots.txt");

            if ((mots == null) || (mots.Length ==0))
            {
                Console.WriteLine("ERREUR : La liste de mot est vide!");
            }
            else
            {
                
                while (true)
                {

                    Random rand = new Random();
                    string mot = mots[rand.Next(0,mots.Length-1)].Trim().ToUpper();
                    DemanderMot(mot);
                  
                    if (!DemanderDeRejouer())
                    {
                        break;
                    }
                    Console.Clear();
                }
                Console.Clear();
                Console.WriteLine("Merci et à bientôt !");
            }
        }
    }
}