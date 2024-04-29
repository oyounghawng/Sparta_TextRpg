using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Runtime.Intrinsics.X86;
using System.Xml.Linq;
using System.IO;
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
        static Player player = new Player(1, "르탄", PlayerJob.전사, 50, 5, 100, 1500);
        static List<Item> shopItem = new List<Item>();
        static void Main(string[] args)
        {
            Item item1 = new Item("무쇠갑옷", StatTpye.방어력, 5, "무쇠로 만들어져 튼튼한 갑옷입니다.", 500);
            shopItem.Add(item1);
            player.AddInventory(item1);
            Item item2 = new Item("다이아갑옷", StatTpye.방어력, 8, "다이아로 만들어져 튼튼한 갑옷입니다.", 1000);
            shopItem.Add(item2);
            Item item3 = new Item("스파르타의 창", StatTpye.공격력, 7, "스파르타의 전사들이 사용했다는 전설의 창입니다.", 1000);
            shopItem.Add(item3);
            player.AddInventory(item3);
            Item item4 = new Item("스파르타의 검", StatTpye.공격력, 10, "스파르타의 전사들이 사용한 검입니다.", 1500);
            shopItem.Add(item4);
            GameStart();
        }
        public class Player
        {
            public int _level;
            public int _exp;
            public string _name;
            public PlayerJob _job;
            public float _attack;
            public int _defence;
            public int _Maxhp;
            public int _currnthp;
            public int _gold;

            public List<Item> _inventory;
            public int[] _needlevelexp;
            public Item _weapon;
            public Item _armor;
            public Player(int Level, string Name, PlayerJob Job,
                int Attack, int Defence, int Hp, int Gold)
            {
                _level = Level;
                _exp = 0;
                _name = Name;
                _job = Job;
                _attack = Attack;
                _defence = Defence;
                _Maxhp = Hp;
                _currnthp = Hp;
                _gold = Gold;
                _inventory = new List<Item>();
                _needlevelexp = [1, 2, 3, 4];
            }
            public void AddInventory(Item item)
            {
                _inventory.Add(item);
            }
        }
        public struct Item
        {
            public string _name;
            public StatTpye _stattype;
            public int _statvalue;
            public string _description;

            public bool _isequip;
            public bool _isbuy;
            public int _price;

            public Item(string name, StatTpye stat, int statvalue, string description, int price, bool isequip = false, bool isbuy = false)
            {
                _name = name;
                _stattype = stat;
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
            Console.WriteLine("4. 던전입장");
            Console.WriteLine("5. 휴식하기");
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
                case ConsoleKey.NumPad4:
                    Console.Clear();
                    ShowDungeon();
                    break;
                case ConsoleKey.D5:
                case ConsoleKey.NumPad5:
                    Console.Clear();
                    ShowRest();
                    break;
                default:
                    Console.Clear();
                    Console.WriteLine("잘못된 입력입니다.");
                    GameStart();
                    break;

            }
        }
        public static void ShowStat(Player player)
        {
            string offset = string.Empty;
            Console.Clear();
            Console.WriteLine("상태 보기");
            Console.WriteLine("캐릭터의 정보가 표시됩니다.\n");
            Console.WriteLine("Lv. " + player._level.ToString("D2"));
            Console.WriteLine($"Chad.( {player._job})");
            offset = player._weapon._statvalue != 0 ? $" (+{player._weapon._statvalue})" : "";
            player._attack += player._weapon._statvalue;
            Console.WriteLine("공격력. " + player._attack + offset);
            offset = player._armor._statvalue != 0 ? $" (+{player._armor._statvalue})" : "";
            player._defence += player._armor._statvalue;
            Console.WriteLine("방어력 " + player._defence + offset);
            Console.WriteLine("체력 " + player._currnthp);
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

                if (player._weapon.Equals(item) || player._armor.Equals(item))
                    equip = "[E]";
                Console.WriteLine($"- {equip}{item._name}     | {item._stattype} +{item._statvalue}  | {item._description}");
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
                if (player._weapon.Equals(item) || player._armor.Equals(item))
                    equip = "[E]";
                Console.WriteLine($"-{cnt} {equip}{item._name}     | {item._stattype} +{item._statvalue}  | {item._description}");
                cnt++;
            }
            Console.WriteLine("\n0. 나가기");
            Console.WriteLine("\n원하시는 행동을 입력해주세요");

            var key = Console.ReadKey(true).Key;
            if (key == ConsoleKey.D0 || key == ConsoleKey.NumPad0)
            {
                Console.Clear();
                ShowInventory(inventory);
            }
            else if (key > ConsoleKey.D0 && key <= (ConsoleKey.D0 + inventory.Count))
            {
                Console.Clear();
                Item temp = inventory[(int)(key - 49)];
                switch (temp._stattype)
                {
                    case StatTpye.공격력:
                        if (!player._weapon.Equals(null))
                        {
                            Console.Clear();
                            if (player._weapon.Equals(temp))
                            {
                                Console.WriteLine("이미 장착중인 무기입니다.");
                            }
                            else
                            {
                                player._weapon = temp;
                            }
                        }
                        else
                        {
                            player._weapon = temp;
                        }
                        break;
                    case StatTpye.방어력:
                        if (!player._armor.Equals(null))
                        {
                            Console.Clear();
                            if (player._armor.Equals(temp))
                            {

                                Console.WriteLine("이미 장착중인 방어구입니다.");
                            }
                            else
                            {
                                player._armor = temp;
                            }
                        }
                        else
                        {
                            player._armor = temp;
                        }
                        break;
                }
                ItemEquip(inventory);
            }
            else if ((key > ConsoleKey.NumPad0 && key <= (ConsoleKey.NumPad0 + inventory.Count)))
            {
                Console.Clear();
                Item temp = inventory[(int)(key - 97)];
                switch (temp._stattype)
                {
                    case StatTpye.공격력:
                        if (!player._weapon.Equals(null))
                        {
                            Console.Clear();
                            if (player._weapon.Equals(temp))
                            {
                                Console.WriteLine("이미 장착중인 무기입니다.");
                            }
                            else
                            {
                                player._weapon = temp;
                            }
                        }
                        else
                        {
                            player._weapon = temp;
                        }
                        break;
                    case StatTpye.방어력:
                        if (!player._armor.Equals(null))
                        {
                            Console.Clear();
                            if (player._armor.Equals(temp))
                            {

                                Console.WriteLine("이미 장착중인 방어구입니다.");
                            }
                            else
                            {
                                player._armor = temp;
                            }
                        }
                        else
                        {
                            player._armor = temp;
                        }
                        break;
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

                Console.WriteLine($"- {item._name}     | {item._stattype} +{item._statvalue}  | {item._description}    | {isBuy}");
            }

            Console.WriteLine("\n1. 아이템 구매");
            Console.WriteLine("2. 아이템 판매");
            Console.WriteLine("0. 나가기");
            Console.WriteLine("\n원하시는 행동을 입력해주세요");
            var key = Console.ReadKey(true).Key;
            switch (key)
            {
                case ConsoleKey.D1:
                case ConsoleKey.NumPad1:
                    Console.Clear();
                    BuyShop();
                    break;
                case ConsoleKey.D2:
                case ConsoleKey.NumPad2:
                    Console.Clear();
                    SellShop();
                    break;
                case ConsoleKey.D0:
                case ConsoleKey.NumPad0:
                    Console.Clear();
                    GameStart();
                    break;
                default:
                    Console.Clear();
                    Console.WriteLine("잘못된 입력입니다.");
                    ShowShop();
                    break;
            }
        }
        public static void BuyShop()
        {
            Console.WriteLine("상점 - 아이템 구매");
            Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.\n");
            Console.WriteLine("[보유 골드]");
            Console.WriteLine(player._gold + " G ");
            int cnt = 1;
            foreach (Item item in shopItem)
            {
                string isBuy = string.Empty;
                if (item._isbuy)
                    isBuy = "구매완료";
                else
                    isBuy = item._price.ToString();

                Console.WriteLine($"- {cnt}  {item._name}     | {item._stattype} +{item._statvalue}  | {item._description}    | {isBuy}");
                cnt++;
            }
            Console.WriteLine("\n0. 나가기");
            Console.WriteLine("\n원하시는 행동을 입력해주세요");

            var key = Console.ReadKey(true).Key;
            if (key == ConsoleKey.D0 || key == ConsoleKey.NumPad0)
            {
                Console.Clear();
                ShowShop();
            }
            else if ((key > ConsoleKey.D0 && key <= (ConsoleKey.D0 + shopItem.Count)))
            {
                Console.Clear();
                Item temp = shopItem[(int)(key - 49)];
                if (temp._isbuy)
                {
                    Console.Clear();
                    Console.WriteLine("이미 구매한 아이템입니다.");
                }
                else
                {
                    if (player._gold >= temp._price)
                    {
                        player._gold -= temp._price;
                        temp._isbuy = true;
                        shopItem[(int)(key - 49)] = temp;
                        player.AddInventory(temp);
                        Console.Clear();
                        Console.WriteLine("구매를 완료했습니다.");
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("\nGold가 부족합니다.");
                    }
                }
                BuyShop();
            }
            else if ((key > ConsoleKey.NumPad0 && key <= (ConsoleKey.NumPad0 + shopItem.Count)))
            {
                Item temp = shopItem[(int)(key - 97)];
                if (temp._isbuy)
                {
                    Console.Clear();
                    Console.WriteLine("이미 구매한 아이템입니다.");
                }
                else
                {
                    if (player._gold >= temp._price)
                    {
                        player._gold -= temp._price;
                        temp._isbuy = true;
                        shopItem[(int)(key - 97)] = temp;
                        player.AddInventory(temp);
                        Console.Clear();
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("Gold가 부족합니다.");
                    }
                }
                BuyShop();

            }
            else
            {
                Console.Clear();
                Console.WriteLine("잘못된 입력입니다.");
                BuyShop();
            }
        }
        public static void SellShop()
        {
            Console.WriteLine("상점 - 아이템 판매");
            Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.\n");
            Console.WriteLine("[보유 골드]");
            Console.WriteLine(player._gold + " G ");
            List<Item> inventory = player._inventory;
            int cnt = 1;
            foreach (Item item in inventory)
            {
                string equip = string.Empty;
                if (player._weapon.Equals(item) || player._armor.Equals(item))
                    equip = "[E]";
                Console.WriteLine($"- {cnt} {equip}{item._name}  | {item._stattype} +{item._statvalue}  | {item._description}  | {item._price * 0.85f}");
                cnt++;
            }
            Console.WriteLine("\n0. 나가기");
            Console.WriteLine("\n원하시는 행동을 입력해주세요");

            var key = Console.ReadKey(true).Key;
            if (key == ConsoleKey.D0 || key == ConsoleKey.NumPad0)
            {
                Console.Clear();
                ShowShop();
            }
            else if ((key > ConsoleKey.D0 && key <= (ConsoleKey.D0 + shopItem.Count)))
            {
                Console.Clear();
                Item temp = inventory[(int)(key - 49)];
                int idx = 0;
                foreach (Item item in shopItem)
                {
                    if (item.Equals(temp))
                    {
                        temp._isbuy = false;
                        shopItem[idx] = temp;
                        break;
                    }
                    idx++;
                }
                SellItem(inventory[(int)(key - 49)]);
            }
            else if ((key > ConsoleKey.NumPad0 && key <= (ConsoleKey.NumPad0 + shopItem.Count)))
            {
                Console.Clear();
                Item temp = inventory[(int)(key - 97)];
                int idx = 0;
                foreach (Item item in shopItem)
                {
                    if (item.Equals(temp))
                    {
                        temp._isbuy = false;
                        shopItem[idx] = temp;
                        break;
                    }
                    idx++;
                }
                SellItem(inventory[(int)(key - 97)]);
            }
            else
            {
                Console.Clear();
                Console.WriteLine("잘못된 입력입니다.");
                SellShop();
            }
            void SellItem(Item item)
            {
                Console.Clear();
                string isEquip = string.Empty;
                Item temp = item;
                temp._isbuy = false;
                if (player._weapon.Equals(item))
                {
                    player._weapon = new Item();
                    isEquip = "착용한";
                }
                if (player._armor.Equals(item))
                {
                    player._armor = new Item();
                    isEquip = "착용한";
                }
                player._gold += (int)Math.Round(item._price * 0.85f);
                player._inventory.Remove(item);
                Console.WriteLine($"{isEquip} {item._name}을 판매해 {item._price * 0.85f}G를 획득했습니다.\n");
                SellShop();
            }
        }
        public static void ShowDungeon()
        {
            Console.WriteLine("던전입장");
            Console.WriteLine("이곳에서 던전으로 들어가기전 활동을 할 수 있습니다.\n");
            Console.WriteLine("1.쉬운 던전   | 방어력 5 이상 권장");
            Console.WriteLine("2.일반 던전   | 방어력 11 이상 권장");
            Console.WriteLine("3.어려운 던전 | 방어력 17 이상 권장");
            Console.WriteLine("0. 나가기");
            Console.WriteLine("\n원하시는 행동을 입력해 주세요.");

            var key = Console.ReadKey(true).Key;
            switch (key)
            {
                case ConsoleKey.D1:
                case ConsoleKey.D2:
                case ConsoleKey.D3:
                    DoDungeon((int)(key - 49));
                    break;
                case ConsoleKey.NumPad1:
                case ConsoleKey.NumPad2:
                case ConsoleKey.NumPad3:
                    DoDungeon((int)(key - 97));
                    break;
                case ConsoleKey.D0:
                case ConsoleKey.NumPad0:
                    Console.Clear();
                    GameStart();
                    break;
                default:
                    Console.Clear();
                    Console.WriteLine("잘못된 입력입니다.");
                    ShowRest();
                    break;
            }
        }
        public static void DoDungeon(int level)
        {
            Console.Clear();
            Random random = new Random();
            int[] difficulty = { 5, 11, 17 };
            int[] reward = { 1000, 1700, 2500 };
            int curgold = player._gold;
            int curHp = player._currnthp;
            int offset = difficulty[level] - player._defence;
            int hpCost = random.Next(20 + offset, 36 + offset);
            string[] DungeonString = { "쉬운", "일반", "어려운" };
            if (player._defence < difficulty[level])
            {
                int probablity = random.Next(0, 101);

                if (probablity <= 40)
                {
                    Fail();
                }
                else
                {
                    Clear();
                }

            }
            else
            {
                Clear();
            }
            if (player._currnthp <= 0)
            {
                Console.WriteLine("사망 게임오버");
                return;
            }
            if (player._exp == player._needlevelexp[player._level - 1])
            {
                levelup();
            }

            Console.WriteLine("\n0. 나가기");
            Console.WriteLine("\n원하시는 행동을 입력해 주세요.");
            var key = Console.ReadKey(true).Key;
            switch (key)
            {
                case ConsoleKey.D0:
                case ConsoleKey.NumPad0:
                    Console.Clear();
                    ShowDungeon();
                    break;
                default:
                    Console.WriteLine("잘못된 입력입니다.");
                    ReResult();
                    break;
            }
            //실패
            void Fail()
            {
                hpCost /= 2;
                Console.WriteLine("던전 클리어 실패");
                Console.WriteLine($"{DungeonString[level]} 던전을 클리어에 실패 하였습니다.\n");
                Console.WriteLine("[탐험 결과]");
                Console.WriteLine($"체력 {curHp} -> {curHp - hpCost}");
                player._currnthp -= hpCost;
                Console.WriteLine($"골드 {curgold} -> {curgold}");
            }
            //클리어
            void Clear()
            {
                int rewardGold = (int)Math.Round(reward[level] * (1 + (player._attack * 2 * 0.01f)));
                Console.WriteLine("던전 클리어");
                Console.WriteLine("축하합니다!!");
                Console.WriteLine($"{DungeonString[level]} 던전을 클리어 하였습니다.\n");
                Console.WriteLine("[탐험 결과]");
                Console.WriteLine($"체력 {curHp} -> {curHp - hpCost}");
                player._currnthp -= hpCost;
                Console.WriteLine($"골드 {curgold} -> {curgold + rewardGold}");
                player._gold += rewardGold;
                player._exp++;
            }
            void levelup()
            {
                player._attack += 0.5f;
                player._defence += 1;
                player._exp = 0;
                player._level += 1;
                Console.Write("\nLevel UP\n");
            }
        }
        public static void ReResult()
        {
            var key = Console.ReadKey(true).Key;
            switch (key)
            {
                case ConsoleKey.D0:
                case ConsoleKey.NumPad0:
                    Console.Clear();
                    ShowDungeon();
                    break;
                default:
                    Console.WriteLine("잘못된 입력입니다.");
                    ReResult();
                    break;
            }
        }
        public static void ShowRest()
        {
            Console.WriteLine("휴식하기");
            Console.WriteLine($"500 G 를 내면 체력을 회복할 수 있습니다. (보유 골드 : {player._gold} G)");
            Console.WriteLine($"현재 체력 : {player._currnthp}\n");
            Console.WriteLine("1. 휴식하기");
            Console.WriteLine("0. 나가기");
            Console.WriteLine("\n원하시는 행동을 입력해 주세요.");

            var key = Console.ReadKey(true).Key;
            switch (key)
            {
                case ConsoleKey.D1:
                case ConsoleKey.NumPad1:
                    Console.Clear();
                    TakeRest();
                    ShowRest();
                    break;
                case ConsoleKey.D0:
                case ConsoleKey.NumPad0:
                    Console.Clear();
                    GameStart();
                    break;
                default:
                    Console.Clear();
                    Console.WriteLine("잘못된 입력입니다.");
                    ShowRest();
                    break;
            }
        }
        public static void TakeRest()
        {
            if (player._gold < 500)
            {
                Console.WriteLine("골드가 부족합니다.\n");
                return;
            }
            if (player._currnthp == player._Maxhp)
            {
                Console.WriteLine("hp가 최대입니다.\n");
                return;
            }
            player._currnthp = player._Maxhp;
            Console.WriteLine("휴식을 완료했습니다.\n");
        }
    }
}
