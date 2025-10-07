using System;
using System.Collections.Generic;
namespace VendingMachineApp
{
    class Prd
    {
        public int Id;
        public string N;
        public int Pr;
        public int Q;

        public string S()
        {
            int r = Pr / 100;
            int c = Pr % 100;
            return r + "." + c.ToString("D2");
        }
    }
    class V
    {
        private List<Prd> L = new List<Prd>();
        private int F = 0;
        private int C = 0;
        private int[] Cn = new int[] { 500, 200, 100, 50, 20, 10, 5 };
        public V()
        {
            L.Add(new Prd { Id = 1, N = "Добрый-кола", Pr = 120, Q = 5 });
            L.Add(new Prd { Id = 2, N = "Кириешки",    Pr = 85,  Q = 3 });
            L.Add(new Prd { Id = 3, N = "Птичье-молоко",Pr = 50,  Q = 10 });
        }
        public void Sh()
        {
            Console.WriteLine("Список товаров:");
            foreach (var x in L)
            {
                Console.WriteLine($"{x.Id}. {x.N} — {x.S()} — шт: {x.Q}");
            }
        }
        public bool Ins(int coin)
        {
            bool ok = false;
            for (int i = 0; i < Cn.Length; i++)
            {
                if (Cn[i] == coin) { ok = true; break; }
            }
            if (!ok)
            {
                Console.WriteLine($"Монета {coin} не принимается.");
                return false;
            }
            C += coin;
            Console.WriteLine($"Вставлено {Fmt(coin)}.");
            return true;
        }
        public void Can()
        {
            if (C == 0)
            {
                Console.WriteLine("Деньги не вставлялись.");
                return;
            }
            Console.WriteLine("Возврат: " + Fmt(C));
            C = 0;
        }
        public void Sel(int id)
        {
            Prd s = null;
            foreach (var x in L) if (x.Id == id) { s = x; break; }

            if (s == null)
            {
                Console.WriteLine("Товар не найден.");
                return;
            }
            if (s.Q <= 0)
            {
                Console.WriteLine("Товар закончился.");
                return;
            }
            if (C < s.Pr)
            {
                Console.WriteLine($"Недостаточно средств. Цена: {s.S()}, у вас: {Fmt(C)}");
                return;
            }
            s.Q -= 1;
            C -= s.Pr;
            F += s.Pr;
            Console.WriteLine($"Выдано: {s.N}. Спасибо!");
            if (C > 0)
            {
                var ch = Chg(C);
                Console.WriteLine($"Сдача {Fmt(C)} в монетах:");
                foreach (var c in ch) Console.WriteLine($" - {Fmt(c)}");
                C = 0;
            }
        }
        private List<int> Chg(int amt)
        {
            var res = new List<int>();
            int m = amt;
            for (int i = 0; i < Cn.Length; i++)
            {
                int c = Cn[i];
                while (m >= c)
                {
                    m -= c;
                    res.Add(c);
                }
            }
            if (m > 0) res.Add(m);
            return res;
        }
        public void AdmSt()
        {
            Console.WriteLine("=== Статус ===");
            Console.WriteLine("Собрано: " + Fmt(F));
            Console.WriteLine("Товары:");
            foreach (var x in L)
                Console.WriteLine($" {x.Id}. {x.N} — {x.S()} — шт: {x.Q}");
        }
        public void AdmCl()
        {
            Console.WriteLine("Снимаю: " + Fmt(F));
            F = 0;
        }
        public void Ref(int id, int q)
        {
            Prd s = null;
            foreach (var x in L) if (x.Id == id) { s = x; break; }
            if (s == null) { Console.WriteLine("Товар не найден."); return; }
            s.Q += q;
            Console.WriteLine($"Пополнено {s.N}. Новое кол-во: {s.Q}");
        }
        public void Add(string name, int price, int qty)
        {
            int nid = 1;
            if (L.Count > 0)
            {
                int mx = L[0].Id;
                for (int i = 1; i < L.Count; i++) if (L[i].Id > mx) mx = L[i].Id;
                nid = mx + 1;
            }
            L.Add(new Prd { Id = nid, N = name, Pr = price, Q = qty });
            Console.WriteLine($"Добавлен {name} id={nid} price={price} qty={qty}");
        }
        private string Fmt(int c)
        {
            int r = c / 100;
            int m = c % 100;
            return r + "." + m.ToString("D2");
        }
    }
    class Program
    {
        private const string PW = "admin";
        static void Main()
        {
            var v = new V();
            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("Режим: 1-пользователь 2-админ 0-выход");
                Console.Write("> ");
                var r = Console.ReadLine();
                if (r == null) r = "";
                r = r.Trim();
                if (r == "0") break;
                if (r == "1") UL(v);
                else if (r == "2")
                {
                    Console.Write("Пароль: ");
                    var p = RP();
                    if (p != PW) Console.WriteLine("Неверный пароль.");
                    else AL(v);
                }
            }
            Console.WriteLine("Пока!");
        }
        static void UL(V vm)
        {
            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("Пользователь:");
                Console.WriteLine("1 - Показать товары");
                Console.WriteLine("2 - Вставить монету");
                Console.WriteLine("3 - Выбрать товар");
                Console.WriteLine("4 - Отмена и возврат");
                Console.WriteLine("0 - Назад");
                Console.Write("> ");
                var c = Console.ReadLine();
                if (c == null) c = "";
                c = c.Trim();
                if (c == "0") return;
                if (c == "1") vm.Sh();
                else if (c == "2")
                {
                    Console.WriteLine("Введите монету в копейках (10,20,50,100,200,500):");
                    Console.Write("> ");
                    var s = Console.ReadLine();
                    if (int.TryParse(s, out int coin)) vm.Ins(coin);
                    else Console.WriteLine("Неверно.");
                }
                else if (c == "3")
                {
                    vm.Sh();
                    Console.Write("id: ");
                    var s = Console.ReadLine();
                    if (int.TryParse(s, out int id)) vm.Sel(id);
                    else Console.WriteLine("Неверный id.");
                }
                else if (c == "4") vm.Can();
                else Console.WriteLine("Неизвестно.");
            }
        }
        static void AL(V vm)
        {
            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("Админ:");
                Console.WriteLine("1 - Показать статус");
                Console.WriteLine("2 - Снять средства");
                Console.WriteLine("3 - Пополнить товар");
                Console.WriteLine("4 - Добавить товар");
                Console.WriteLine("0 - Назад");
                Console.Write("> ");
                var c = Console.ReadLine();
                if (c == null) c = "";
                c = c.Trim();
                if (c == "0") return;
                if (c == "1") vm.AdmSt();
                else if (c == "2") vm.AdmCl();
                else if (c == "3")
                {
                    vm.Sh();
                    Console.Write("id: ");
                    var s1 = Console.ReadLine();
                    if (!int.TryParse(s1, out int id)) { Console.WriteLine("Неверный id."); continue; }
                    Console.Write("qty: ");
                    var s2 = Console.ReadLine();
                    if (!int.TryParse(s2, out int q)) { Console.WriteLine("Неверное число."); continue; }
                    vm.Ref(id, q);
                }
                else if (c == "4")
                {
                    Console.Write("name: ");
                    var nm = Console.ReadLine();
                    if (nm == null) nm = "";
                    Console.Write("price в коп.: ");
                    var sp = Console.ReadLine();
                    if (!int.TryParse(sp, out int price)) { Console.WriteLine("Неверно."); continue; }
                    Console.Write("qty: ");
                    var sq = Console.ReadLine();
                    if (!int.TryParse(sq, out int qty)) { Console.WriteLine("Неверно."); continue; }
                    vm.Add(nm, price, qty);
                }
                else Console.WriteLine("Неизвестно.");
            }
        }
        static string RP()
        {
            string s = "";
            ConsoleKeyInfo k;
            while ((k = Console.ReadKey(true)).Key != ConsoleKey.Enter)
            {
                if (k.Key == ConsoleKey.Backspace && s.Length > 0)
                {
                    s = s.Substring(0, s.Length - 1);
                    Console.Write("\b \b");
                }
                else if (!char.IsControl(k.KeyChar))
                {
                    s += k.KeyChar;
                    Console.Write("*");
                }
            }
            Console.WriteLine();
            return s;
        }
    }
}
