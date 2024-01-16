using System;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Reflection.PortableExecutable;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Linq;

namespace Hangman
{
    class Program
    {
        public static void Main(string[] args)
        {
            

            // Odczytaj każdą linię pliku i zapisz ją w tablicy.
            // Każdy element to osobna linia pliku
            string[] lines = new string[1];
            if (System.IO.File.Exists("dane_wisielec.csv"))
            {
                // Powitanie
                Console.WriteLine("Witaj w konsolowej grze HangMan!");
                Console.WriteLine("(Wciśnij \"ENTER\", aby rozpocząć)");
                lines = System.IO.File.ReadAllLines("dane_wisielec.csv");
            }
            else
            {
                Console.WriteLine("Brakuje pliku z ciągami");
            }

            // Wylosuj jeden element z tablicy lines
            // random.Next losuje liczbę całkowitą w danym przedziale -> wykorzystuję ją jako index tablicy lines
            Random random = new Random();
            int start = random.Next(0, lines.Length);
            string secretWord = lines[start];
            Console.ReadLine();


            // Czyści konsolę. Dodaje wrażenia wejścia w grę po wciśnięciu ENTER
            Console.Clear();

            int counter = -1;
            int lives = 5;
            secretWord = secretWord.ToUpper();
            int wordLength = secretWord.Length;
            char[] secretArray = secretWord.ToCharArray();
            char[] hiddenChars = new char[wordLength];
            
            char[] guessedLetters = new char[26];
            int numberStore = 0;
            int allGuesses = 0; // wszystkie trafienia
            int allLetters = 0; //wszystkie znalezione litery przez usera
            int bullseye = 0; // wszystkie jego celne strzały
            bool victory = false;


            // przypisujemy zasłonięte litery do tablicy hiddenChars
            foreach (char letter in hiddenChars)
            {
                counter++;
                hiddenChars[counter] = '-';
            }

            Console.WriteLine("\t");

            // Tworzymy stoper
            Stopwatch stopw = new Stopwatch();

            while (lives > 0)
            {
                
                stopw.Start();
                for (int i = 0; i < 100; i++)
                {
                    Thread.Sleep(2);
                }
                
                counter = -1;
                string printProgress = String.Concat(hiddenChars);
                bool letterFound = false;
                int multiples = 0; // ilość znalezionych liter

                if (printProgress == secretWord)
                {
                    victory = true;
                    break;
                }


                Console.WriteLine("Postęp: " + printProgress);
                Console.WriteLine("\t");
                Console.Write("Zgadnij literę: ");
                string playerGuess = Console.ReadLine();

                // przetestuj czy input użytkownika jest jednym znakiem
                // nie można przejść dalej, jeśli pojawi się odstęp między wyrazami, np. Palestinian Territory -> to debug
                bool guessTest = playerGuess.All(Char.IsLetter);

                while (guessTest == false || playerGuess.Length != 1)
                {
                    Console.WriteLine("Wprowadź proszę tylko jedną literę!");
                    Console.Write("Zgadnij literę: ");
                    playerGuess = Console.ReadLine();
                    guessTest = playerGuess.All(Char.IsLetter);
                }

                playerGuess = playerGuess.ToUpper();
                char playerChar = Convert.ToChar(playerGuess);

                if (guessedLetters.Contains(playerChar) == false)
                {

                    guessedLetters[numberStore] = playerChar;
                    numberStore++;
                    allGuesses++;

                    foreach (char letter in secretArray)
                    {
                        counter++;
                        if (letter == playerChar)
                        {
                            hiddenChars[counter] = playerChar;
                            letterFound = true;
                            multiples++;
                            allLetters++;
                        }

                    }

                    if (letterFound)
                    {
                        bullseye++;
                        if(multiples >= 2 && multiples < 5)
                        {
                            Console.WriteLine("Znalzałeś {0} litery {1}!", multiples, playerChar);

                        }
                        else
                        {
                            Console.WriteLine("Znalazałeś {0} literę {1}!", multiples, playerChar);

                        }
                    }
                    else
                    {
                        Console.WriteLine("Nie ma litery {0} w sekretnym słowie!", playerChar);
                        lives--;
                    }
                    Console.WriteLine(GallowView(lives));
                }
                else
                {
                    Console.WriteLine("Już wprowadzałeś literę {0}!", playerChar);
                    allGuesses++;
                }
            }
            stopw.Stop();
            string time = stopw.Elapsed.ToString();
            
            //Console.WriteLine(" Time elapsed: {0} ", stopw.Elapsed);
            //File.WriteAllText(@"E:\PROJEKTY\Programowanie\repos\Hangman\Files" + time + "test.txt", "Hello World");

            if (victory)
            {
                Console.WriteLine("\n\nSzukane słowo to: {0}", secretWord);
                Console.WriteLine("\n\nWYGRAŁEŚ!");
                SaveData(time, allGuesses, bullseye, allLetters);

            }
            else
            {
                Console.WriteLine("\n\nSzukane słowo to: {0}", secretWord);
                Console.WriteLine("\n\nPRZEGRAŁEŚ!");
                SaveData(time, allGuesses, bullseye, allLetters);

            }

            // Zachowaj otwarte okno w trybie debugowania
            //Console.WriteLine("Press any key to exit.");
            //Console.ReadKey();

        }


        //metoda rysująca wisielca
        private static string GallowView(int livesLeft)
        {

            string drawHangman = "";

            if (livesLeft < 5)
            {
                drawHangman += "--------\n";
            }

            if (livesLeft < 4)
            {
                drawHangman += "       |\n";
            }

            if (livesLeft < 3)
            {
                drawHangman += "       O\n";
            }

            if (livesLeft < 2)
            {
                drawHangman += "      /|\\ \n";
            }

            if (livesLeft == 0)
            {
                drawHangman += "      / \\ \n";
            }

            return drawHangman;

        }

        //zapis danych do pliku tekstowego
        public static async Task SaveData(string time, int allGuesses, int bullseye, int allLetters)
        {
            string[] lines = {"Gratulacje! Wygrałeś!", "\t", "Twój czas:", time, "Twoje wszystkie próby:", allGuesses.ToString(),"Twoje celne strzały:", bullseye.ToString(), "Znalezionych liter:", allLetters.ToString() };

            await File.WriteAllLinesAsync("dane_wisielec_wygrana.txt", lines);
        }

    }



}
