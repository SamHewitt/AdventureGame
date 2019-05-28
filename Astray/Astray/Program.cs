using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;

namespace Astray
{
    public struct Mobs
    {
        public string name; // Mob Name
        public string encounter; // Encounter message
        public string beenhit; // BeenHit by mob
        public string hit; // Hit mob
        public string evade; // Successfully escape mob
        public string evadefail; // Failed evading mob
        public string killed; // Death message
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
            string choice;
            do // Menu will always appear at the end of any method chosen
            {
                Console.Clear();
                Console.WriteLine("dev menu\n1. load\n2. view mobs loaded\n0. exit");
                choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        Load(ref Mobs); // Calls Load Method
                        break;
                    case "2":
                        View(Mobs);
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
                Console.WriteLine("Evade Mob: "+ Mobs[i].evade);
                Console.WriteLine("Evade Fail: " +Mobs[i].evadefail);
                Console.WriteLine("Killed: " + Mobs[i].killed);
                Console.WriteLine();
            }
            Console.ReadLine();
        }

    }
}
