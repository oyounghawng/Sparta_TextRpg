using System.Collections.Generic;
using System.Xml.Linq;
using static ConsoleApp2.Program;

namespace ConsoleApp2
{
    enum PlayerJob
    {
        전사,
        마법사,
        궁수,
    }
    enum StatTpye
    {
        공격력,
        방어력
    }
    internal class Program
    {
        static Player player = new Player(1, "르탄", PlayerJob.전사, 10, 5, 100, 1500);
        static List<Item> shopItem = new List<Item>();
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");

            Item item1 = new Item("무쇠값옷", StatTpye.방어력, 5, "무쇠로 만들어져 튼튼한 갑옷입니다.", 1500);
            player.AddInventory(item1);
            Item item2 = new Item("스파르타의 창", StatTpye.공격력, 7, "스파르타의 전사들이 사용했다는 전설의 창입니다.", 2000);
            player.AddInventory(item2);

            GameStart();
        }
        public class Player
        {
            public int _level;
            public string _name;
            public PlayerJob _job;
            public int _attack;
            public int _defence;
            public int _hp;
            public int _gold;
            public List<Item> _inventory;

            public Player(int Level, string Name, PlayerJob Job, 
                int Attack, int Defence, int Hp, int Gold)
            {
                _level = Level;
                _name = Name;
                _job = Job;
                _attack = Attack;
                _defence = Defence;
                _hp = Hp;
                _gold = Gold;
                _inventory = new List<Item>();
            }

            public void AddInventory(Item item)
            {
                _inventory.Add(item);
            }
        }
        public struct Item
        {
            public string _name;
            public StatTpye _stat;
            public int _statvalue;
            public string _description;
            public bool _isequip;
            public bool _isbuy;
            public int _price;

            public Item(string name, StatTpye stat, int statvalue, string description, int price, bool isequip = false, bool isbuy = false)
            {
                _name = name;
                _stat = stat;
                _statvalue = statvalue;
                _description = description;
                _isequip = isequip;
                _price = price;
            }
        }
        public static void GameStart()
        {
            Console.WriteLine("스파르타 마을에 오신 여러분 환영합니다");
            Console.WriteLine("이곳에서 던전으로 들어가기전 활동을 할 수 있습니다.");
            Console.WriteLine("");
            Console.WriteLine("1. 상태 보기");
            Console.WriteLine("2. 인벤토리");
            Console.WriteLine("3. 상점");
            Console.WriteLine("");
            Console.WriteLine("원하시는 행동을 입력해주세요.");

            var key = Console.ReadKey(true).Key;
            switch (key)
            {
                case ConsoleKey.D1:
                case ConsoleKey.NumPad1:
                    Console.Clear();
                    ShowStat(player);
                    break;
                case ConsoleKey.D2:
                case ConsoleKey.NumPad2:
                    Console.Clear();
                    ShowInventory(player._inventory);
                    break;
                case ConsoleKey.D3:
                case ConsoleKey.NumPad3:
                    Console.Clear();
                    ShowShop();
                    break;
                case ConsoleKey.D4:
                    break;
                case ConsoleKey.D5:
                    break;
                default:
                    Console.WriteLine("잘못된 입력입니다.");
                    break;
            }
        }
        public static void ShowStat(Player player)
        {
            Console.Clear();
            Console.WriteLine("상태 보기");
            Console.WriteLine("캐릭터의 정보가 표시됩니다.\n");
            Console.WriteLine("Lv. " + player._level.ToString("D2"));
            Console.WriteLine($"Chad.( {player._job})");
            Console.WriteLine("공격력. " + player._attack);
            Console.WriteLine("방어력 " + player._defence);
            Console.WriteLine("체력 " + player._hp);
            Console.WriteLine("골드 " + player._gold + "\n");
            Console.WriteLine("0. 나가기\n");
            Console.WriteLine("원하시는 행동을 입력해 주세요");

            var key = Console.ReadKey(true).Key;
            switch (key)
            {
                case ConsoleKey.D0:
                case ConsoleKey.NumPad0:
                    Console.Clear();
                    GameStart();
                    break;
                default:
                    Console.WriteLine("잘못된 입력입니다.");
                    break;
            }
        }
        public static void ShowInventory(List<Item> inventory)
        {
            Console.WriteLine("인벤토리");
            Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.\n");
            Console.WriteLine("[아이템목록]");
            foreach (Item item in inventory)
            {
                string equip = string.Empty;
                if (item._isequip)
                    equip = "[E]";

                Console.WriteLine($"- {equip}{item._name}     | {item._stat} +{item._statvalue}  | {item._description}");
            }

            Console.WriteLine("\n1. 장착 관리");
            Console.WriteLine("0. 나가기");
            var key = Console.ReadKey(true).Key;
            switch (key)
            {
                case ConsoleKey.D1:
                case ConsoleKey.NumPad1:
                    Console.Clear();
                    ItemEquip(inventory);
                    break;
                case ConsoleKey.D0:
                case ConsoleKey.NumPad0:
                    Console.Clear();
                    GameStart();
                    break;
                default:
                    Console.WriteLine("잘못된 입력입니다.");
                    break;
            }
        }
        public static void ItemEquip(List<Item> inventory)
        {
            Console.WriteLine("인벤토리 - 장착 관리");
            Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.\n");
            Console.WriteLine("[아이템목록]");
            int cnt = 1;
            foreach (Item item in inventory)
            {
                string equip = string.Empty;
                if (item._isequip)
                    equip = "[E]";
                Console.WriteLine($"-{cnt} {equip}{item._name}     | {item._stat} +{item._statvalue}  | {item._description}");
                cnt++;
            }
            Console.WriteLine("\n0. 나가기");

            var key = Console.ReadKey(true).Key;
            if (key == ConsoleKey.D0 || key == ConsoleKey.NumPad0)
            {
                Console.Clear();
                ShowInventory(inventory);
            }
            else if (key > ConsoleKey.D0 || key < (ConsoleKey.D0 + inventory.Count))
            {
                Console.Clear();
                Item temp = inventory[(int)(key - 49)];
                if (temp._isequip)
                {
                    Console.WriteLine("이미 장착중입니다.");
                }
                else
                {
                    temp._isequip = true;
                    inventory[(int)(key - 49)] = temp;
                }
                ItemEquip(inventory);
                
            }
            else
            {
                Console.WriteLine("잘못된 입력입니다.");
                ItemEquip((inventory));
            }
        }
        public static void ShowShop()
        {
            Console.WriteLine("상점");
            Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.\n");
            Console.WriteLine("[보유 골드]");
            Console.WriteLine(player._gold + " G ");
            foreach (Item item in shopItem)
            {
                string isBuy = string.Empty;
                if (item._isbuy)
                    isBuy = "구매완료";
                else
                    isBuy = item._price.ToString();

                Console.WriteLine($"- {item._name}     | {item._stat} +{item._statvalue}  | {item._description}    | {isBuy}");
            }

            Console.WriteLine("\n1. 아이템 구매");
            Console.WriteLine("0. 나가기");
            var key = Console.ReadKey(true).Key;
            switch (key)
            {
                case ConsoleKey.D1:
                case ConsoleKey.NumPad1:
                    Console.Clear();
                    break;
                case ConsoleKey.D0:
                case ConsoleKey.NumPad0:
                    Console.Clear();
                    GameStart();
                    break;
                default:
                    Console.WriteLine("잘못된 입력입니다.");
                    break;
            }
        }
        public static void BuyShop()
        { 
            Console.WriteLine("상점");
            Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.\n");
            Console.WriteLine("[보유 골드]");
            Console.WriteLine(player._gold + " G ");
            foreach (Item item in shopItem)
            {
                string isBuy = string.Empty;
                if (item._isbuy)
                    isBuy = "구매완료";
                else
                    isBuy = item._price.ToString();

                Console.WriteLine($"- {item._name}     | {item._stat} +{item._statvalue}  | {item._description}    | {isBuy}");
            }

            Console.WriteLine("\n1. 아이템 구매");
            Console.WriteLine("0. 나가기");
            var key = Console.ReadKey(true).Key;
            switch (key)
            {
                case ConsoleKey.D1:
                case ConsoleKey.NumPad1:
                    Console.Clear();
                    break;
                case ConsoleKey.D0:
                case ConsoleKey.NumPad0:
                    Console.Clear();
                    break;
                default:
                    Console.WriteLine("잘못된 입력입니다.");
                    break;
            }
        }
    }
}
