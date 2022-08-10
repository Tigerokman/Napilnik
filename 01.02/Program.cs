using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _01._02
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Good iPhone12 = new Good("IPhone 12");
            Good iPhone11 = new Good("IPhone 11");

            Warehouse warehouse = new Warehouse();

            Shop shop = new Shop(warehouse);

            warehouse.Delive(iPhone12, 10);
            warehouse.Delive(iPhone11, 1);

            warehouse.Show(); //Вывод всех товаров на складе с их остатком

            Cart cart = shop.Cart();
            cart.Add(iPhone12, 4);
            cart.Add(iPhone11, 3); //при такой ситуации возникает ошибка так, как нет нужного количества товара на складе

            cart.Show();//Вывод всех товаров в корзине

            Console.WriteLine(cart.Order().Paylink);

            cart.Add(iPhone12, 9); //Ошибка, после заказа со склада убираются заказанные товары
        }
    }

    public class Warehouse
    {
        private List<Box> _boxes = new List<Box>();

        public void Delive(Good good, int count)
        {
            var tempBox = _boxes.FirstOrDefault(box => box.Good == good);

            if (tempBox == null)
            {
                Box box = new Box(good, count);
                _boxes.Add(box);
                return;
            }

            tempBox.AddGood(count);
        }

        public void Show()
        {
            if (_boxes.Count == 0)
                throw new ArgumentException("Нет товаров на складе.");

            for (int i = 0; i < _boxes.Count; i++)
                Console.WriteLine("Товар - " + _boxes[i].Good.Name + " в количестве - " + _boxes[i].Count + " штук.");
        }

        public bool TryBuy(Good good, int count)
        {
            var tempBox = _boxes.FirstOrDefault(box => box.Good == good);

            if (tempBox != null)
            {
                if (tempBox.Count >= count)
                {
                    tempBox.TakeGood(count);
                    return true;
                }
                else
                {
                    Console.WriteLine("Недостаточное количество товара на складе.");
                    return false;
                }
            }
            else
            {
                Console.WriteLine("Товар не найден.");
                return false;
            }
        }
    }

    public class Good
    {
        public string Name { get; private set; }

        public Good(string name)
        {
            if (name.Length == 0)
                throw new ArgumentException("Введите название товара.");

            Name = name;
        }
    }

    public class Shop
    {
        private Cart _cart;

        public string Paylink { get; private set; } = "Товары заказаны";

        public Cart Cart() => _cart;

        public Shop(Warehouse warehouse = null)
        {
            if (warehouse != null)
            {
                Shop shop = new Shop();
                _cart = new Cart(shop, warehouse);
            }
        }
    }

    public class Cart
    {
        private List<Good> _goods = new List<Good>();
        private Warehouse _warehouse;
        public Shop Shop { get; private set; }
        public Shop Order() => Shop;

        public Cart(Shop shop, Warehouse warehouse)
        {
            Shop = shop;
            _warehouse = warehouse;
        }

        public void Add(Good good, int count)
        {
            bool canBuy = _warehouse.TryBuy(good, count);

            if (canBuy)
            {
                for (int i = 0; i < count; i++)
                    _goods.Add(good);
            }
        }

        public void Show()
        {
            if (_goods.Count > 0)
            {
                Console.WriteLine("Список ваших товаров;");

                foreach (Good good in _goods)
                {
                    Console.WriteLine(good.Name);
                }
            }
            else
                Console.WriteLine("Пусто");
        }
    }

    public class Box
    {
        public readonly Good Good;
        public int Count { get; private set; }

        public Box(Good good, int count)
        {
            Good = good;
            Count = count;
        }

        public void AddGood(int count)
        {
            if (count > 0)
                Count += count;
            else
                Console.WriteLine("Нечего добавлять.");
        }

        public void TakeGood(int count)
        {
            if (count > 0 && count <= Count)
                Count -= count;
        }
    }
}