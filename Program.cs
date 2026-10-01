Console.WriteLine("Witaj!");
Console.WriteLine("Введите ваш возраст:");
string input = Console.ReadLine();
int age = int.Parse(input);

Console.WriteLine("Через 10 лет вам будет: " + (age + 10));