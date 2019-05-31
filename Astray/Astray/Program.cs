using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;

namespace Astray
{
    public struct Mobs // Mobs
    {
        public string name; // Mob Name
        public string encounter; // Encounter message
        public string beenhit; // BeenHit by mob
        public string hit; // Hit mob
        public string evade; // Successfully escape mob
        public string evadefail; // Failed evading mob
        public string killed; // Death message
    }

    public struct Items // Items
    {

    }

    public struct Grid
    {
        public bool[] searched;
        public bool[] exit;
        public bool[] npc;
    }
    class Program
    {
        static void Main()
        {
            Menu();
        }

        static void Menu()
        {
            Mobs[] Mobs = new Mobs[0];
            Items[] Inventory = new Items[8];
            Grid[] Collum = new Grid[0];
            string choice;
            do // Menu will always appear at the end of any method chosen
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("                   ▄▄▄        ██████ ▄▄▄█████▓ ██▀███   ▄▄▄     ▓██   ██▓");
                Console.WriteLine("                  ▒████▄    ▒██    ▒ ▓  ██▒ ▓▒▓██ ▒ ██▒▒████▄    ▒██  ██▒");
                Console.WriteLine("                  ▒██  ▀█▄  ░ ▓██▄   ▒ ▓██░ ▒░▓██ ░▄█ ▒▒██  ▀█▄   ▒██ ██░");
                Console.WriteLine("                  ░██▄▄▄▄██   ▒   ██▒░ ▓██▓ ░ ▒██▀▀█▄  ░██▄▄▄▄██  ░ ▐██▓░");
                Console.WriteLine("                   ▓█   ▓██▒▒██████▒▒  ▒██▒ ░ ░██▓ ▒██▒ ▓█   ▓██▒ ░ ██▒▓░");
                Console.WriteLine("                   ▒▒   ▓▒█░▒ ▒▓▒ ▒ ░  ▒ ░░   ░ ▒▓ ░▒▓░ ▒▒   ▓▒█░  ██▒▒▒ ");
                Console.WriteLine("                    ▒   ▒▒ ░░ ░▒  ░ ░    ░      ░▒ ░ ▒░  ▒   ▒▒ ░▓██ ░▒░ ");
                Console.WriteLine("                    ░   ▒   ░  ░  ░    ░        ░░   ░   ░   ▒   ▒ ▒ ░░ ");
                Console.WriteLine("                        ░  ░      ░              ░           ░  ░░ ░ ");
                Console.WriteLine("                                                                 ░ ░ ");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("======================================================================================");
                Console.WriteLine("                               |                         |");
                Console.WriteLine("                               |   1   Load              |");
                Console.WriteLine("                               |   2   View Mobs         |");
                Console.WriteLine("                               |   3   Grid Generation   |");
                Console.WriteLine("                               |   0   Exit              |");
                Console.WriteLine("                               |                         |");
                Console.WriteLine("                               |                         |");
                Console.WriteLine("======================================================================================");
                choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        Load(ref Mobs); // Calls Load Method
                        break;
                    case "2":
                        View(Mobs);
                        break;
                    case "3":
                        GenerateGrid(ref Collum);
                        Grid(Collum);
                        Console.ReadLine();

                        break;
                    case "0":
                        break;
                    default:
                        Console.Clear();
                        Console.WriteLine("Make a better choice than that");
                        Thread.Sleep(200);
                        break;
                }
            } while (choice != "0");
        }

        static void Load(ref Mobs[] Mobs)
        {
            // Reading in all mobs
            // Order of read in
            // Encounter msg.Animal hit you$You hit animal$You run and escape$you run and get caught$kill animal$get killed

            StreamReader sr = new StreamReader("mobs.txt");
            int count = 0;

            while (!sr.EndOfStream)
            {
                Array.Resize(ref Mobs, Mobs.Length + 1);
                Mobs[count].name = sr.ReadLine();
                string[] temp = sr.ReadLine().Split('.');
                Mobs[count].encounter = temp[0];
                temp = temp[1].Split('$');
                Mobs[count].beenhit = temp[0];
                Mobs[count].hit = temp[1];
                Mobs[count].evade = temp[2];
                Mobs[count].evadefail = temp[3];
                Mobs[count].killed = temp[4];
                count++;
            }
        }

        static void View(Mobs[] Mobs) // To be deleted later on, for testing purposes
        {
            Console.Clear();
            for (int i = 0; i < Mobs.Length; i++)
            {
                Console.WriteLine("Name: " + Mobs[i].name);
                Console.WriteLine("Encounter: " + Mobs[i].encounter);
                Console.WriteLine("Been Hit: " + Mobs[i].beenhit);
                Console.WriteLine("Hit Mob: " + Mobs[i].hit);
                Console.WriteLine("Evade Mob: " + Mobs[i].evade);
                Console.WriteLine("Evade Fail: " + Mobs[i].evadefail);
                Console.WriteLine("Killed: " + Mobs[i].killed);
                Console.WriteLine();
            }
            Console.ReadLine();
        }

        static void Grid(Grid[] Collum)
        {
            for (int col = 0; col < Collum.Length; col++)
            {
                for (int row = 0; row < Collum[0].exit.Length; row++)
                {
                    if (Collum[col].searched[row] == true)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write("■ ");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else if (Collum[col].exit[row] == true)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write("■ ");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else if (Collum[col].npc[row] == true)
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.Write("■ ");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("■ ");
                        Console.ForegroundColor = ConsoleColor.White;

                    }
                }
                Console.WriteLine();
            }
            Console.WriteLine(Collum.Length);
            Console.WriteLine(Collum[0].npc.Length);
        }

        static void GenerateGrid(ref Grid[] Collum)
        {
            Console.Clear();
            Console.WriteLine("Size of grid to create? (W x H)");
            string[] temp = Console.ReadLine().Split('x');
            Array.Resize(ref Collum, Convert.ToInt32(temp[1].Trim(' ')));
            for (int i = 0; i < Collum.Length; i++)
            {
                Array.Resize(ref Collum[i].searched, Convert.ToInt32(temp[0].Trim(' ')));
                Array.Resize(ref Collum[i].exit, Convert.ToInt32(temp[0].Trim(' ')));
                Array.Resize(ref Collum[i].npc, Convert.ToInt32(temp[0].Trim(' ')));
            }
            int npcRate = 15; //Change chances of sectors being exits or npc zones
            int exitRate = 10;

            int npcCount = 0;
            int exitCount = 0;

            double npcLimit = Math.Sqrt(Collum.Length * Collum[0].npc.Length) * 2; //Change limits of sectors being exits or npc zones
            double exitLimit = Math.Sqrt(Collum.Length * Collum[0].npc.Length) * 2;
            int num;
            Random rand = new Random();

            for (int col = 0; col < Collum.Length; col++)
            {
                for (int row = 0; row < Collum[0].exit.Length; row++)
                {
                    num = rand.Next(1, 1001);
                    if (num <= npcRate && npcCount < npcLimit)
                    {
                        Collum[col].npc[row] = true;
                        npcCount++;
                    }
                    num = rand.Next(1, 1001);
                    if (num <= exitRate && exitCount < exitLimit)
                    {
                        Collum[col].exit[row] = true;
                        exitCount++;
                    }
                }
            }

            bool noexit = true; // Guarentees an exit
            do
            {
                for (int i = 0; i < Collum.Length; i++)
                {
                    if (Collum[i].exit.Contains(true))
                    {
                        noexit = false;
                    }
                }

                if (noexit == true)
                {
                    int num1 = rand.Next(0, Collum.Length - 1), num2 = rand.Next(0, Collum[0].exit.Length - 1);
                    Collum[num1].exit[num2] = true;
                }
            } while (noexit == true);
        }

    }
}
