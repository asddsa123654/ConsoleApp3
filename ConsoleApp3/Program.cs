using System;
using System.IO;

namespace Person1
{
    class Person
    {
        string name;
        int birth_year;
        double pay;

        public Person()  // Конструктор без параметрів
        {
            name = "Anonimous";
            birth_year = 0;
            pay = 0;
        }

        public Person(string s) // Конструктор з параметром - рядок
        {
            string[] parts = s.Split(','); // Розбиття рядка за комами

            if (parts.Length != 3)
                throw new FormatException("Неправильний формат даних");

            name = parts[0].Trim(); // Прізвище та ініціали
            birth_year = Convert.ToInt32(parts[1].Trim()); // Рік народження
            pay = Convert.ToDouble(parts[2].Trim()); // Оклад

            if (birth_year < 0) throw new FormatException("Неприпустимий рік народження");
            if (pay < 0) throw new FormatException("Неприпустимий оклад");
        }

        public override string ToString()
        {
            return string.Format("Name: {0,30} Birth Year: {1} Pay: {2:F2}", name, birth_year, pay);
        }

        public int Compare(string name)
        {
            return string.Compare(this.name, name, StringComparison.OrdinalIgnoreCase);
        }

        // Властивості класу
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public int Birth_year
        {
            get { return birth_year; }
            set
            {
                if (value > 0) birth_year = value;
                else throw new FormatException("Неприпустимий рік народження");
            }
        }

        public double Pay
        {
            get { return pay; }
            set
            {
                if (value > 0) pay = value;
                else throw new FormatException("Неприпустимий оклад");
            }
        }

        // Оператори класу
        public static double operator +(Person pers, double a)
        {
            pers.pay += a;
            return pers.pay;
        }

        public static double operator -(Person pers, double a)
        {
            pers.pay -= a;
            if (pers.pay < 0) throw new FormatException("Неприпустимий оклад");
            return pers.pay;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Person[] dbase = new Person[100];
            int n = 0;

            try
            {
                using (StreamReader f = new StreamReader("d:\\Persons.txt")) // Відкриваємо файл
                {
                    string s;
                    int i = 0;

                    while ((s = f.ReadLine()) != null) // Читаємо рядки
                    {
                        dbase[i] = new Person(s); // Створюємо об'єкт на основі рядка
                        Console.WriteLine(dbase[i]); // Виводимо інформацію про персону
                        ++i;
                    }

                    n = i;
                }
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("Перевірте правильність імені і шляху до файлу!");
                return;
            }
            catch (FormatException e)
            {
                Console.WriteLine("Помилка формату: " + e.Message);
                return;
            }
            catch (Exception e)
            {
                Console.WriteLine("Помилка: " + e.Message);
                return;
            }

            int n_pers = 0;
            double mean_pay = 0;
            Console.WriteLine("Введіть прізвище співробітника");

            string name;
            while ((name = Console.ReadLine()) != "") // Пошук за прізвищем
            {
                bool not_found = true;
                for (int k = 0; k < n; ++k)
                {
                    Person pers = dbase[k];
                    if (pers.Compare(name) == 0)
                    {
                        Console.WriteLine(pers);
                        ++n_pers;
                        mean_pay += pers.Pay;
                        not_found = false;
                    }
                }
                if (not_found)
                    Console.WriteLine("Такого співробітника немає");
                Console.WriteLine("Введіть прізвище співробітника або Enter для завершення");
            }

            if (n_pers > 0)
                Console.WriteLine("Середній оклад: {0:F2}", mean_pay / n_pers);

            Console.ReadKey();
        }
    }
}
