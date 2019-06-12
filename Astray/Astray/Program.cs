using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;

namespace Astray
{
    public struct Npc
    {
        public string name;
        public Items[] reward;
        public Items[] find;
    }
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
    struct Characters // Base assigned stats for chosen character
    {
        public string Name;
        public double Attack;
        public double Health;
        public double Spelldamage; // Aoe (area off effect) damage 
        public double Speed; // (rate of attack)
        public double Escapechance;
        public double Dodgechance;
        public double Bleedresistance;
        public double Weaknessresistance;
        public double Poisonresistance;
        public double Criticalchance; //chance to hit for 2x/3x/4x damage. (lower chance as u get higher damage times multiplyer)
    }

    public struct Items // Items
    {
        public string name;
        public int damage;
        public int durabillity;
        public int gain;
        public int duration;
    }

    public struct Grid
    {
        public bool[] searched;
        public bool[] exit;
        public bool[] npc;
        public bool[] weapon;
        public bool[] mob;
        public bool[] food;
    }
    class Program
    {
        static void Main()
        {
            int foodstart = 0;
            Mobs[] Mobs = new Mobs[0];
            Items[] Items = new Items[0];
            Items[] Inventory = new Items[8];
            for (int i = 0; i < Inventory.Length; i++)
            {
                Inventory[i].name = "";
                Inventory[i].damage = 0;
                Inventory[i].durabillity = 0;
            }
            Grid[] Collum = new Grid[0];
            Characters[] Selection = new Characters[0];
            Load(ref Mobs, ref Items, ref Selection, ref foodstart);

            bool debug = true;
            if (debug == true)
            {
                DebugMenu(Collum, Mobs, Items, Selection, foodstart);
            }
            Story();
            Menu(Mobs, Items, Inventory, Collum, Selection, foodstart);
        }

        static void Menu(Mobs[] Mobs, Items[] Items, Items[] Inventory, Grid[] Collum, Characters[] Selection, int foodstart)
        {
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
                Console.WriteLine("                               |   1   Play              |");
                Console.WriteLine("                               |   0   Exit              |");
                Console.WriteLine("                               |                         |");
                Console.WriteLine("======================================================================================");
                choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        Game(Mobs, Items, Collum, Inventory, Selection, foodstart); // Calls Load Method
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

        static void Load(ref Mobs[] Mobs, ref Items[] items, ref Characters[] Selection, ref int foodstart) // Loads All Data
        {
            // Reading in all mobs
            // Order of read in
            // Encounter msg.Animal hit you$You hit animal$You run and escape$you run and get caught$kill animal$get killed

            StreamReader sr = new StreamReader("mobs.txt");
            int count = 0;

            Array.Resize(ref Mobs, 0);

            while (!sr.EndOfStream) // READING IN MOBS
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
            sr.Close();

            StreamReader wr = new StreamReader("Weapons.txt");
            count = 0;

            while (!wr.EndOfStream) // READING IN WEAPONS
            {
                Array.Resize(ref items, items.Length + 1);
                items[count].name = wr.ReadLine();
                items[count].damage = Convert.ToInt32(wr.ReadLine());
                items[count].durabillity = Convert.ToInt32(wr.ReadLine());
                count++;
            }
            wr.Close();

            StreamReader cr = new StreamReader("Characters.txt"); // Reading in characters
            count = 0;
            while (!cr.EndOfStream)
            {
                Array.Resize(ref Selection, Selection.Length + 1);
                Selection[count].Name = cr.ReadLine();
                Selection[count].Attack = Convert.ToInt32(cr.ReadLine());
                Selection[count].Health = Convert.ToInt32(cr.ReadLine());
                Selection[count].Spelldamage = Convert.ToInt32(cr.ReadLine()); // Aoe (area off effect) damage 
                Selection[count].Speed = Convert.ToInt32(cr.ReadLine()); // (rate of attack)
                Selection[count].Escapechance = Convert.ToInt32(cr.ReadLine());
                Selection[count].Dodgechance = Convert.ToInt32(cr.ReadLine());
                Selection[count].Bleedresistance = Convert.ToInt32(cr.ReadLine());
                Selection[count].Weaknessresistance = Convert.ToInt32(cr.ReadLine());
                Selection[count].Poisonresistance = Convert.ToInt32(cr.ReadLine());
                Selection[count].Criticalchance = Convert.ToInt32(cr.ReadLine());
                count++;
            }
            cr.Close();

            StreamReader fr = new StreamReader("Food.txt");
            count = items.Length;
            foodstart = items.Length;
            while (!fr.EndOfStream)
            {
                Array.Resize(ref items, items.Length + 1);
                items[count].name = fr.ReadLine();
                items[count].gain = Convert.ToInt32(fr.ReadLine());
                items[count].duration = Convert.ToInt32(fr.ReadLine());
                count++;
            }
            fr.Close();


        }

        // GAME LOOP

        static void Game(Mobs[] Mobs, Items[] Items, Grid[] Collum, Items[] Inventory, Characters[] Selection, int foodstart)
        {
            Characters[] Character = new Characters[1];
            ChooseCharacter(Character, Selection);
            GenerateGrid(ref Collum);

            bool win = false;
            string choice;
            int x = Collum.Length / 2, y = Collum[1].searched.Length / 2;

            while (x > 0 || x < Collum.Length || y > 0 || y < Collum[1].searched.Length || win == false)
            {
                Console.Clear();
                Message(Collum, x, y, win, Inventory, Items, foodstart);
                Collum[x].searched[y] = true;
                Gui(Collum, Inventory, Character, x, y);

                choice = Console.ReadLine();
                switch (choice.ToLower())
                {
                    case "w":
                        if (x <= 0)
                        {
                        }
                        else
                        {
                            x--;
                        }
                        break;
                    case "a":
                        if (y <= 0)
                        {
                        }
                        else
                        {
                            y--;
                        }
                        break;
                    case "s":
                        if (x >= Collum.Length - 1)
                        {
                        }
                        else
                        {
                            x++;
                        }
                        break;
                    case "d":
                        if (y >= Collum[1].searched.Length - 1)
                        {
                        }
                        else
                        {
                            y++;
                        }
                        break;

                    case "m":
                    case "map":
                        Map(Collum);
                        break;

                    // commands that will be used to call the methods
                    // each method will have an error check

                    // so if someone said fight, the fight method would do an if statement saying if the current sector.animal = false,
                    // meaning there is no animal, it will return, there is nothing to swing at


                    case "fight":
                    case "attack":
                        break;

                    case "run":
                    case "leave":
                    case "evade":
                    case "sneak":
                        break;

                }
            }
        }

        static void Message(Grid[] Collum, int x, int y, bool win, Items[] Inventory, Items[] Items, int foodstart)
        {
            Random rand = new Random();
            string choice,choice2;
            int ChatHeight = 10;
            int count = 0;
            int num = 0;
            bool exit = false;
            if (Collum[x].exit[y] == true)
            {
                Console.WriteLine("You have found an exit!");
                win = true;
                Console.WriteLine("Would you like to leave Astray?(Y/N)");
                choice2 = Console.ReadLine();
                if ((choice2 == "Y") || (choice2 == "y"))
                {
                    Outro();
                }
                else
                {
                   
                }
                count++;
            }
            else if (Collum[x].npc[y] == true)
            {
                Console.WriteLine("You have found a NPC");
                // NPC CODE NEEDS TO GO HERE
                count++;
            }
            else
            {
                if (Collum[x].food[y] == true)
                {
                    num = rand.Next(foodstart, Items.Length);
                    do
                    {
                        Console.WriteLine($"You have found {Items[num].name}");
                        Console.WriteLine("Would you like to pick it up?");
                        choice = Console.ReadLine();
                        switch (choice.ToLower())
                        {
                            case "y":
                            case "yes":
                                int inv = FindFirstEmpty(Inventory);
                                Inventory[inv] = Items[num];
                                exit = true;
                                break;
                            case "n":
                            case "no":
                                exit = true;
                                break;                         
                        }

                    } while (exit == false);
                    count++;
                }

                if (Collum[x].weapon[y] == true)
                {
                    num = rand.Next(0, Items.Length);
                    do
                    {
                        Console.WriteLine($"You have found a {Items[num].name}");
                        Console.WriteLine("Would you like to pick it up?");
                        choice = Console.ReadLine();
                        switch (choice.ToLower())
                        {
                            case "y":
                            case "yes":
                                int inv = FindFirstEmpty(Inventory);
                                Inventory[inv] = Items[num];
                                exit = true;
                                break;
                            case "n":
                            case "no":
                                exit = true;
                                break;
                        }
                    } while (exit == false);
                    count++;
                } //WEAPONS FIND CODE
                if (Collum[x].mob[y] == true)
                {
                    Console.WriteLine("Chosen Mob Appeared Message");
                    //FIGHT CODE NEEDS TO GO HERE
                    count++;
                }
            }

            for (int i = 0; i < ChatHeight - count; i++)
            {
                Console.WriteLine();
            }
        }

        static int FindFirstEmpty(Items[] Inventory) // FINDS LOWEST VALUE EMPTY SLOT
        {
            int lowestempty = 0;
            for (int i = Inventory.Length - 1; i >= 0; i--)
            {
                if (Inventory[i].name == "")
                {
                    lowestempty = i;
                }
            }
            return lowestempty;
        }

        static void ChooseCharacter(Characters[] Choice, Characters[] Character)
        {
            Console.Clear();
            for (int i = 0; i < Character.Length; i++)
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine($"==========================================================================");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"{i + 1} {Character[i].Name}");
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine($"Attack:           {Character[i].Attack.ToString().PadLeft(3)}    Health:             {Character[i].Health.ToString().PadLeft(3)}");
                Console.WriteLine($"Spell Damage:     {Character[i].Spelldamage.ToString().PadLeft(3)}    Speed:              {Character[i].Speed.ToString().PadLeft(3)}");
                Console.WriteLine($"Escape Chance:    {Character[i].Escapechance.ToString().PadLeft(3)}    Dodge Chance:       {Character[i].Dodgechance.ToString().PadLeft(3)}");
                Console.WriteLine($"Bleed Resistance: {Character[i].Bleedresistance.ToString().PadLeft(3)}    Weakness Resistance:{Character[i].Dodgechance.ToString().PadLeft(3)}");
                Console.WriteLine($"Critical Chance:  {Character[i].Escapechance.ToString().PadLeft(3)}");
                Console.ForegroundColor = ConsoleColor.DarkGray;
            }
            Console.WriteLine($"==========================================================================");
            int temp = Convert.ToInt32(Console.ReadLine());
            Choice[0] = Character[temp - 1];
        }

        static void Gui(Grid[] Collum, Items[] Inventory, Characters[] Character, int x, int y)
        {
            int count = 0;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\n================================================================================================");
            for (int col = x - 5; col < x + 5; col++)
            {
                Console.Write("| ");
                for (int row = y - 5; row < y + 5; row++)
                {
                    if (col < 0 || col > Collum.Length - 1 || row < 0 || row > Collum[1].searched.Length - 1)
                    {
                        Console.Write("  ");
                    }
                    else
                    {
                        if (Collum[col].searched[row] == true)
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.Write("■ ");
                            Console.ForegroundColor = ConsoleColor.Red;
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.DarkGray;
                            Console.Write("■ ");
                            Console.ForegroundColor = ConsoleColor.Red;
                        }
                    }

                }
                int padding = 15;
                int padding2 = padding;
                switch (count)
                {
                    case 0:
                        Console.WriteLine($"| Inventory                             | {Character[0].Name}                          |");
                        break;
                    case 1:
                        Console.WriteLine($"| 1. {Inventory[0].name.PadLeft(padding)} 2. {Inventory[1].name.PadLeft(padding)} | Health:   {Character[0].Health.ToString().PadLeft(padding2)} |");
                        break;
                    case 2:
                        Console.WriteLine($"| 3. {Inventory[2].name.PadLeft(padding)} 4. {Inventory[3].name.PadLeft(padding)} | Attack:   {Character[0].Attack.ToString().PadLeft(padding2)} |");
                        break;
                    case 3:
                        Console.WriteLine($"| 5. {Inventory[4].name.PadLeft(padding)} 6. {Inventory[5].name.PadLeft(padding)} | Speed:    {Character[0].Speed.ToString().PadLeft(padding2)} |");
                        break;
                    case 4:
                        Console.WriteLine($"| 7. {Inventory[6].name.PadLeft(padding)} 8. {Inventory[7].name.PadLeft(padding)} | Escape %: {Character[0].Escapechance.ToString().PadLeft(padding2)} |");
                        break;
                    default:
                        Console.WriteLine("|");
                        break;
                }
                count++;
            }
        }

        static void GenerateGrid(ref Grid[] Collum)
        {
            bool exit = false;
            int rate = 0;
            do
            {
                Console.Clear();
                Console.WriteLine("Choose Difficulty\n1. Easy\n2. Doable\n3. Impossible");
                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        rate = 50;
                        exit = true;
                        break;
                    case "2":
                        rate = 10;
                        exit = true;
                        break;
                    case "3":
                        rate = 3;
                        exit = true;
                        break;
                    default:
                        Console.Clear();
                        Console.WriteLine("Error. Try Again");
                        Console.ReadLine();
                        break;
                }
            } while (exit == false);



            int[] temp = { 30, 30 };
            int tmp = temp[1];
            Array.Resize(ref Collum, temp[0]);
            for (int i = 0; i < Collum.Length; i++)
            {
                Array.Resize(ref Collum[i].searched, tmp);
                Array.Resize(ref Collum[i].exit, tmp);
                Array.Resize(ref Collum[i].npc, tmp);
                Array.Resize(ref Collum[i].weapon, tmp);
                Array.Resize(ref Collum[i].mob, tmp);
                Array.Resize(ref Collum[i].food, tmp);
            }
            int npcRate = rate * 1; //Change chances of sectors
            int exitRate = rate * 2;
            int weaponRate = rate * 4;
            int mobRate = rate * 3;
            int foodRate = rate * 8;

            int npcCount = 0; //Count of weapons ammount
            int exitCount = 0;
            int weaponCount = 0;
            int mobCount = 0;
            int foodCount = 0;


            double npcLimit = Math.Sqrt(Collum.Length * Collum[0].npc.Length) * 2; //Change limits of sectors
            double exitLimit = Math.Sqrt(Collum.Length * Collum[0].npc.Length) * 2;
            double weaponLimit = Math.Sqrt(Collum.Length * Collum[0].npc.Length) * 10;
            double mobLimit = Math.Sqrt(Collum.Length * Collum[0].npc.Length) * 8;
            double foodLimit = Math.Sqrt(Collum.Length * Collum[0].npc.Length) * 4;

            int num;
            Random rand = new Random();

            for (int col = 0; col < Collum.Length; col++)
            {
                for (int row = 0; row < Collum[0].exit.Length; row++)
                {
                    num = rand.Next(1, 1001);
                    if (num <= exitRate && exitCount < exitLimit)
                    {
                        Collum[col].exit[row] = true;
                        exitCount++;
                    }

                    num = rand.Next(1, 1001);
                    if (num <= npcRate && npcCount < npcLimit && Collum[col].exit[row] == false)
                    {
                        Collum[col].npc[row] = true;
                        npcCount++;
                    }

                    num = rand.Next(1, 1001);
                    if (num <= weaponRate && weaponCount < weaponLimit && Collum[col].exit[row] == false && Collum[col].npc[row] == false)
                    {
                        Collum[col].weapon[row] = true;
                        weaponCount++;
                    }

                    num = rand.Next(1, 1001);
                    if (num <= mobRate && mobCount < mobLimit && Collum[col].exit[row] == false && Collum[col].npc[row] == false)
                    {
                        Collum[col].mob[row] = true;
                        mobCount++;
                    }

                    num = rand.Next(1, 1001);
                    if (num <= foodRate && foodCount < foodLimit && Collum[col].exit[row] == false && Collum[col].npc[row] == false && Collum[col].weapon[row] == false && Collum[col].mob[row] == false)
                    {
                        Collum[col].food[row] = true;
                        foodCount++;
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

        static void Map(Grid[] Collum)
        {
            Console.Clear();
            for (int col = 0; col < Collum.Length; col++)
            {
                for (int row = 0; row < Collum[0].exit.Length; row++)
                {
                    if (Collum[col].searched[row] == true)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write("■ ");
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("■ ");
                    }
                }
                Console.WriteLine();
            }
            Console.ReadKey();
        }

        public static void Story()
        {
            int time = 5;
            for (int i = 0; i < 30; i++)
            {
                Console.WriteLine();
            }
            StreamReader sr = new StreamReader("MainStory.txt");
            while (!sr.EndOfStream)
            {
                Console.WriteLine(sr.ReadLine());
                Thread.Sleep(time);
            }
            for (int i = 0; i < 28; i++)
            {
                Console.WriteLine();
                Thread.Sleep(time);
            }
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("                   ▄▄▄        ██████ ▄▄▄█████▓ ██▀███   ▄▄▄     ▓██   ██▓");
            Thread.Sleep(time);
            Console.WriteLine("                  ▒████▄    ▒██    ▒ ▓  ██▒ ▓▒▓██ ▒ ██▒▒████▄    ▒██  ██▒");
            Thread.Sleep(time);
            Console.WriteLine("                  ▒██  ▀█▄  ░ ▓██▄   ▒ ▓██░ ▒░▓██ ░▄█ ▒▒██  ▀█▄   ▒██ ██░");
            Thread.Sleep(time);
            Console.WriteLine("                  ░██▄▄▄▄██   ▒   ██▒░ ▓██▓ ░ ▒██▀▀█▄  ░██▄▄▄▄██  ░ ▐██▓░");
            Thread.Sleep(time);
            Console.WriteLine("                   ▓█   ▓██▒▒██████▒▒  ▒██▒ ░ ░██▓ ▒██▒ ▓█   ▓██▒ ░ ██▒▓░");
            Thread.Sleep(time);
            Console.WriteLine("                   ▒▒   ▓▒█░▒ ▒▓▒ ▒ ░  ▒ ░░   ░ ▒▓ ░▒▓░ ▒▒   ▓▒█░  ██▒▒▒ ");
            Thread.Sleep(time);
            Console.WriteLine("                    ▒   ▒▒ ░░ ░▒  ░ ░    ░      ░▒ ░ ▒░  ▒   ▒▒ ░▓██ ░▒░ ");
            Thread.Sleep(time);
            Console.WriteLine("                    ░   ▒   ░  ░  ░    ░        ░░   ░   ░   ▒   ▒ ▒ ░░ ");
            Thread.Sleep(time);
            Console.WriteLine("                        ░  ░      ░              ░           ░  ░░ ░ ");
            Thread.Sleep(time);
            Console.WriteLine("                                                                 ░ ░ ");
            Thread.Sleep(time);
            for (int i = 0; i < 16; i++)
            {
                Console.WriteLine();
                Thread.Sleep(time);
            }
            sr.Close();
        }

        public static void Outro()
        {
            int time = 5;
            for (int i = 0; i < 30; i++)
            {
                Console.WriteLine();
            }
            StreamReader sr = new StreamReader("Outro.txt");
            while (!sr.EndOfStream)
            {
                Console.WriteLine(sr.ReadLine());
                Thread.Sleep(time);
            }
            for (int i = 0; i < 28; i++)
            {
                Console.WriteLine();
                Thread.Sleep(time);
            }
        }


        // DEBUG STUFF !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!


        static void DebugMenu(Grid[] Collum, Mobs[] Mobs, Items[] items, Characters[] Selection, int foodstart)
        {
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
                Console.WriteLine("                               |   0   TEST GAME         |");
                Console.WriteLine("                               |   4   Story Line        |");
                Console.WriteLine("                               |   5   View Items        |");
                Console.WriteLine("                               |                         |");
                Console.WriteLine("======================================================================================");
                choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        Load(ref Mobs, ref items, ref Selection, ref foodstart); // Calls Load Method
                        break;
                    case "2":
                        View(Mobs);
                        break;

                    case "3":
                        GenerateGrid(ref Collum);
                        Grid(Collum);
                        Console.ReadLine();

                        break;

                    case "4":
                        Console.Clear();
                        Story();
                        break;

                    case "0":
                        break;
                    case "5":
                        ViewItems(items);
                        break;
                    default:
                        Console.Clear();
                        Console.WriteLine("Make a better choice than that");
                        Thread.Sleep(200);
                        break;
                }
            } while (choice != "0");
        }
        static void View(Mobs[] Mobs) // To be deleted later on, for testing purposes
        {
            Console.Clear();
            for (int i = 0; i < Mobs.Length - 1; i++)
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
                    }
                    else if (Collum[col].exit[row] == true)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write("■ ");
                    }
                    else if (Collum[col].npc[row] == true)
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.Write("■ ");
                    }
                    else if (Collum[col].weapon[row] == true)
                    {
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        Console.Write("■ ");
                    }
                    else if (Collum[col].mob[row] == true)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("■ ");
                    }
                    else if (Collum[col].food[row] == true)
                    {
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.Write("■ ");
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("■ ");
                    }
                }
                Console.WriteLine();
            }
            Console.WriteLine(Collum.Length);
            Console.WriteLine(Collum[0].npc.Length);
        }

        static void ViewItems(Items[] Items)
        {
            for (int i = 0; i < Items.Length;i++)
            {
                Console.WriteLine($"Name: {Items[i].name}");
                Console.WriteLine($"Damage: {Items[i].damage}");
                Console.WriteLine($"Durabil: {Items[i].durabillity}");
                Console.WriteLine($"Gain: {Items[i].gain}");
                Console.WriteLine($"Duration: {Items[i].duration}\n");
            }
            Console.ReadLine();
        }
    }
}
